using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class EdgeDetectionFilter : IFilter
{
        private readonly Bitmap _originalImage;
        private readonly double _threshold;
        private readonly bool _grayscale;
        private readonly int _kernelSize;

        public EdgeDetectionFilter(Bitmap originalImage, int kernelSize = 3, double threshold = 0.0, bool grayscale = true)
        {
            ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));

            // Ensure kernel size is odd
            if (kernelSize % 2 == 0)
                kernelSize += 1;

            _originalImage = originalImage;
            _kernelSize = kernelSize;
            _threshold = threshold;
            _grayscale = grayscale;
        }

        public unsafe Bitmap Apply()
        {
            Bitmap image = new Bitmap(_originalImage);
            int radius = _kernelSize / 2;

            // Create dynamic Sobel-like operators based on kernel size
            float[,] kernelX = CreateSobelKernelX(_kernelSize);
            float[,] kernelY = CreateSobelKernelY(_kernelSize);

            BitmapData imageData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format32bppArgb);

            try
            {
                // Create temporary buffer to store results
                double[,] gradientMagnitude = new double[imageData.Height, imageData.Width];

                byte* ptr = (byte*)imageData.Scan0.ToPointer();

                // First pass: calculate gradients
                for (int y = 0; y < imageData.Height; y++)
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        double gx = 0;
                        double gy = 0;

                        // Apply kernels
                        for (int ky = -radius; ky <= radius; ky++)
                        {
                            for (int kx = -radius; kx <= radius; kx++)
                            {
                                int px = Math.Clamp(x + kx, 0, imageData.Width - 1);
                                int py = Math.Clamp(y + ky, 0, imageData.Height - 1);

                                byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                                byte b = pixel[0];
                                byte g = pixel[1];
                                byte r = pixel[2];

                                // Convert to grayscale intensity
                                double intensity = 0.299 * r + 0.587 * g + 0.114 * b;

                                gx += intensity * kernelX[ky + radius, kx + radius];
                                gy += intensity * kernelY[ky + radius, kx + radius];
                            }
                        }

                        // Calculate gradient magnitude
                        double magnitude = Math.Sqrt(gx * gx + gy * gy);

                        // Store in temporary buffer
                        gradientMagnitude[y, x] = magnitude;
                    }
                }

                // Find maximum gradient for normalization
                double maxGradient = 0;
                for (int y = 0; y < imageData.Height; y++)
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        maxGradient = Math.Max(maxGradient, gradientMagnitude[y, x]);
                    }
                }

                // Second pass: apply results
                for (int y = 0; y < imageData.Height; y++)
                {
                    for (int x = 0; x < imageData.Width; x++)
                    {
                        byte* pixel = ptr + (y * imageData.Stride) + (x * 4);

                        // Normalize and apply threshold
                        double normalizedMagnitude = gradientMagnitude[y, x] / maxGradient * 255.0;

                        if (normalizedMagnitude < _threshold)
                            normalizedMagnitude = 0;

                        byte edge = (byte)Math.Min(255, normalizedMagnitude);

                        if (_grayscale)
                        {
                            // Grayscale output
                            pixel[0] = edge;  // B
                            pixel[1] = edge;  // G
                            pixel[2] = edge;  // R
                        }
                        else
                        {
                            // Colored edges - modulate original color by edge strength
                            byte originalB = pixel[0];
                            byte originalG = pixel[1];
                            byte originalR = pixel[2];

                            double factor = edge / 255.0;
                            pixel[0] = (byte)(originalB * factor);  // B
                            pixel[1] = (byte)(originalG * factor);  // G
                            pixel[2] = (byte)(originalR * factor);  // R
                        }
                        // Alpha channel remains unchanged
                    }
                }
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

        private float[,] CreateSobelKernelX(int size)
        {
            float[,] kernel = new float[size, size];
            int radius = size / 2;

            // Create a Sobel-like X gradient kernel of the specified size
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // The x coordinate determines the gradient direction
                    // Scale based on distance from center for larger kernels
                    float scale = (float)(radius - Math.Abs(y)) / radius;
                    kernel[y + radius, x + radius] = x * scale;
                }
            }

            // Normalize the kernel
            float sum = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sum += Math.Abs(kernel[y, x]);
                }
            }

            if (sum > 0)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        kernel[y, x] /= sum;
                    }
                }
            }

            return kernel;
        }

        private float[,] CreateSobelKernelY(int size)
        {
            float[,] kernel = new float[size, size];
            int radius = size / 2;

            // Create a Sobel-like Y gradient kernel of the specified size
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    // The y coordinate determines the gradient direction
                    // Scale based on distance from center for larger kernels
                    float scale = (float)(radius - Math.Abs(x)) / radius;
                    kernel[y + radius, x + radius] = y * scale;
                }
            }

            // Normalize the kernel
            float sum = 0;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sum += Math.Abs(kernel[y, x]);
                }
            }

            if (sum > 0)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        kernel[y, x] /= sum;
                    }
                }
            }

            return kernel;
        }
    }
