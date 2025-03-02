using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class BlurFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly int _blurRadius;

    public BlurFilter(Bitmap originalImage, int blurRadius = 7)
    {
        ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));
        _originalImage = originalImage;
        _blurRadius = (blurRadius & 1) == 0 ? blurRadius + 1 : blurRadius;
    }

    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        float[,] BlurMatrix = CreateBlurMatrix();
        int radius = _blurRadius / 2;

        BitmapData imageData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb
            );

        try
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();
            for (int i = 0; i < imageData.Height; i++)
            {
                for(int j = 0; j < imageData.Width; j++)
                {
                    float sumB = 0, sumG = 0, sumR = 0;

                    for(int ky = -radius; ky <= radius; ky++)
                    {
                        for(int kx = -radius; kx <= radius; kx++)
                        {
                            int px = Math.Clamp(j + kx, 0, imageData.Width - 1);
                            int py = Math.Clamp(i + ky, 0, imageData.Height - 1);

                            byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                            float blurValue = BlurMatrix[ky + radius, kx + radius];

                            sumB += pixel[0] * blurValue;
                            sumG += pixel[1] * blurValue;
                            sumR += pixel[2] * blurValue;
                        }
                    }
                    byte* resultPixel = ptr + (i * imageData.Stride) + j * 4;
                    resultPixel[0] = (byte)Math.Clamp(sumB, 0, 255);
                    resultPixel[1] = (byte)Math.Clamp(sumG, 0, 255);
                    resultPixel[2] = (byte)Math.Clamp(sumR, 0, 255);
                    resultPixel[3] = *(ptr + (i * imageData.Stride) + (j * 4) + 3);
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

    private float[,] CreateBlurMatrix()
    {
        float[,] kernelMatrix = new float[_blurRadius, _blurRadius];
        float value = 1.0f / (_blurRadius * _blurRadius);

        for(int i = 0; i < _blurRadius; i++)
        {
            for(int j = 0; j < _blurRadius; j++)
            {
                kernelMatrix[i, j] = value;
            }
        }
        return kernelMatrix;
    }
}
