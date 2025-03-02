using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class EmbossFilter : IFilter
{
        private readonly Bitmap _originalImage;
        private readonly EmbossDirection _direction;
        private readonly float _strength;
        private readonly bool _preserveColors;

        public enum EmbossDirection
        {
            TopLeft,    // 315 degrees
            Top,        // 0 degrees
            TopRight,   // 45 degrees
            Right,      // 90 degrees
            BottomRight,// 135 degrees
            Bottom,     // 180 degrees
            BottomLeft, // 225 degrees
            Left        // 270 degrees
        }

        public EmbossFilter(Bitmap originalImage, EmbossDirection direction = EmbossDirection.TopLeft,
                            float strength = 1.0f, bool preserveColors = false)
        {
            ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));
            _originalImage = originalImage;
            _direction = direction;
            _strength = Math.Clamp(strength, 0.1f, 5.0f);
            _preserveColors = preserveColors;
        }

        public unsafe Bitmap Apply()
        {
            Bitmap image = new Bitmap(_originalImage);
            float[,] kernel = CreateEmbossKernel(_direction, _strength);

            BitmapData imageData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try
            {
                byte* ptr = (byte*)imageData.Scan0.ToPointer();

                // Create a temporary array to store the modified pixel values
                // This prevents the filter from affecting its own input during processing
                int[,] resultR = new int[imageData.Height, imageData.Width];
                int[,] resultG = new int[imageData.Height, imageData.Width];
                int[,] resultB = new int[imageData.Height, imageData.Width];

                // Apply the kernel to each pixel
                Parallel.For(0, imageData.Height, y =>
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        float sumR = 0, sumG = 0, sumB = 0;

                        // Apply convolution with the emboss kernel
                        for (int ky = -1; ky <= 1; ky++)
                        {
                            for (int kx = -1; kx <= 1; kx++)
                            {
                                int px = Math.Clamp(x + kx, 0, imageData.Width - 1);
                                int py = Math.Clamp(y + ky, 0, imageData.Height - 1);

                                byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                                float kernelValue = kernel[ky + 1, kx + 1];

                                sumB += pixel[0] * kernelValue;
                                sumG += pixel[1] * kernelValue;
                                sumR += pixel[2] * kernelValue;
                            }
                        }

                        // Add bias to get neutral gray for unchanged areas (128)
                        sumR += 128;
                        sumG += 128;
                        sumB += 128;

                        // Store the result in temporary array
                        resultR[y, x] = (int)Math.Clamp(sumR, 0, 255);
                        resultG[y, x] = (int)Math.Clamp(sumG, 0, 255);
                        resultB[y, x] = (int)Math.Clamp(sumB, 0, 255);
                    }
                });

                // Apply the preserved colors if requested
                if (_preserveColors)
                {
                    Parallel.For(0, imageData.Height, y =>
                    {
                        for (int x = 0; x < imageData.Width; x++)
                        {
                            byte* originalPixel = ptr + (y * imageData.Stride) + (x * 4);
                            byte originalB = originalPixel[0];
                            byte originalG = originalPixel[1];
                            byte originalR = originalPixel[2];

                            // Calculate luminance of embossed pixel
                            float luminance = 0.299f * resultR[y, x] + 0.587f * resultG[y, x] + 0.114f * resultB[y, x];

                            // Calculate luminance of original pixel
                            float originalLuminance = 0.299f * originalR + 0.587f * originalG + 0.114f * originalB;

                            // Calculate luminance ratio
                            float ratio = (originalLuminance > 0) ? luminance / originalLuminance : 1.0f;

                            // Apply luminance change to original colors
                            resultR[y, x] = (int)Math.Clamp(originalR * ratio, 0, 255);
                            resultG[y, x] = (int)Math.Clamp(originalG * ratio, 0, 255);
                            resultB[y, x] = (int)Math.Clamp(originalB * ratio, 0, 255);
                        }
                    });
                }

                // Write the results back to the image
                Parallel.For(0, imageData.Height, y =>
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        byte* resultPixel = ptr + (y * imageData.Stride) + (x * 4);
                        resultPixel[0] = (byte)resultB[y, x];
                        resultPixel[1] = (byte)resultG[y, x];
                        resultPixel[2] = (byte)resultR[y, x];
                        // Alpha channel remains unchanged
                    }
                });
            }
            finally
            {
                image.UnlockBits(imageData);
            }

            return image;
        }

        public Bitmap Restore()
        {
            return _originalImage;
        }

        private float[,] CreateEmbossKernel(EmbossDirection direction, float strength)
        {
            float[,] kernel = new float[3, 3];

            // Define the kernel based on the direction
            switch (direction)
            {
                case EmbossDirection.TopLeft:
                    kernel[0, 0] = strength; kernel[0, 1] = strength / 2; kernel[0, 2] = 0;
                    kernel[1, 0] = strength / 2; kernel[1, 1] = 0; kernel[1, 2] = -strength / 2;
                    kernel[2, 0] = 0; kernel[2, 1] = -strength / 2; kernel[2, 2] = -strength;
                    break;
                case EmbossDirection.Top:
                    kernel[0, 0] = strength / 2; kernel[0, 1] = strength; kernel[0, 2] = strength / 2;
                    kernel[1, 0] = 0; kernel[1, 1] = 0; kernel[1, 2] = 0;
                    kernel[2, 0] = -strength / 2; kernel[2, 1] = -strength; kernel[2, 2] = -strength / 2;
                    break;
                case EmbossDirection.TopRight:
                    kernel[0, 0] = 0; kernel[0, 1] = strength / 2; kernel[0, 2] = strength;
                    kernel[1, 0] = -strength / 2; kernel[1, 1] = 0; kernel[1, 2] = strength / 2;
                    kernel[2, 0] = -strength; kernel[2, 1] = -strength / 2; kernel[2, 2] = 0;
                    break;
                case EmbossDirection.Right:
                    kernel[0, 0] = -strength / 2; kernel[0, 1] = 0; kernel[0, 2] = strength / 2;
                    kernel[1, 0] = -strength; kernel[1, 1] = 0; kernel[1, 2] = strength;
                    kernel[2, 0] = -strength / 2; kernel[2, 1] = 0; kernel[2, 2] = strength / 2;
                    break;
                case EmbossDirection.BottomRight:
                    kernel[0, 0] = -strength; kernel[0, 1] = -strength / 2; kernel[0, 2] = 0;
                    kernel[1, 0] = -strength / 2; kernel[1, 1] = 0; kernel[1, 2] = strength / 2;
                    kernel[2, 0] = 0; kernel[2, 1] = strength / 2; kernel[2, 2] = strength;
                    break;
                case EmbossDirection.Bottom:
                    kernel[0, 0] = -strength / 2; kernel[0, 1] = -strength; kernel[0, 2] = -strength / 2;
                    kernel[1, 0] = 0; kernel[1, 1] = 0; kernel[1, 2] = 0;
                    kernel[2, 0] = strength / 2; kernel[2, 1] = strength; kernel[2, 2] = strength / 2;
                    break;
                case EmbossDirection.BottomLeft:
                    kernel[0, 0] = 0; kernel[0, 1] = -strength / 2; kernel[0, 2] = -strength;
                    kernel[1, 0] = strength / 2; kernel[1, 1] = 0; kernel[1, 2] = -strength / 2;
                    kernel[2, 0] = strength; kernel[2, 1] = strength / 2; kernel[2, 2] = 0;
                    break;
                case EmbossDirection.Left:
                    kernel[0, 0] = strength / 2; kernel[0, 1] = 0; kernel[0, 2] = -strength / 2;
                    kernel[1, 0] = strength; kernel[1, 1] = 0; kernel[1, 2] = -strength;
                    kernel[2, 0] = strength / 2; kernel[2, 1] = 0; kernel[2, 2] = -strength / 2;
                    break;
                default:
                    // Default to top-left emboss
                    kernel[0, 0] = strength; kernel[0, 1] = strength / 2; kernel[0, 2] = 0;
                    kernel[1, 0] = strength / 2; kernel[1, 1] = 0; kernel[1, 2] = -strength / 2;
                    kernel[2, 0] = 0; kernel[2, 1] = -strength / 2; kernel[2, 2] = -strength;
                    break;
            }

            return kernel;
        }
    }