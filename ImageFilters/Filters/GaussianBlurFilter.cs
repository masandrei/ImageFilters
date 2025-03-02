using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class GaussianBlurFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly int _blurRadius;
    private readonly double _sigma;

    public GaussianBlurFilter(Bitmap originalImage, int blurRadius = 5, double sigma = 10.0f)
    {
        ArgumentNullException.ThrowIfNull(originalImage);
        _originalImage = originalImage;
        _blurRadius = (blurRadius & 1) == 0 ? blurRadius + 1 : blurRadius;
        _sigma = sigma;
    }

    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        float[,] GaussianBlurMatrix = CreateKernelMatrix();
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
                for (int j = 0; j < imageData.Width; j++)
                {
                    float sumB = 0, sumG = 0, sumR = 0;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = Math.Clamp(j + kx, 0, imageData.Width - 1);
                            int py = Math.Clamp(i + ky, 0, imageData.Height - 1);

                            byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                            float blurValue = GaussianBlurMatrix[ky + radius, kx + radius];

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

    private float[,] CreateKernelMatrix()
    {
        float[,] kernel = new float[_blurRadius, _blurRadius];
        int radius = _blurRadius / 2;
        float sum = 0;

        for(int y = -radius; y <= radius; y++)
        {
            for(int x = -radius; x <= radius; x++)
            {
                float value = (float)(Math.Exp(-(x * x + y * y) / (2 * _sigma * _sigma)) / (2 * Math.PI * _sigma * _sigma));
                kernel[y + radius, x + radius] = value;

                sum += value;
            }
        }

        for(int i = 0; i < _blurRadius; i++)
        {
            for(int j = 0; j < _blurRadius; j++)
            {
                kernel[i, j] /= sum;
            }
        }
        return kernel;
    }

    public Bitmap Restore()
    {
        return _originalImage;
    }
}
