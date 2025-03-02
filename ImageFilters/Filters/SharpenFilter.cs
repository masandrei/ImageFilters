using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class SharpenFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly int _kernelSize;
    private readonly float _sigma;

    public SharpenFilter(Bitmap originalImage, int kernelSize = 3, float sigma = 10.0f)
    {
        ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));
        _originalImage = originalImage;
        _kernelSize = kernelSize;
        _sigma = sigma;
    }

    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        float[,] LoGMatrix = CreateKernelMatrix();
        int radius = _kernelSize / 2;


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
                    byte* originalPixel = ptr + (i * imageData.Stride) + (j * 4);
                    byte originalB = originalPixel[0];
                    byte originalG = originalPixel[1];
                    byte originalR = originalPixel[2];

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = Math.Clamp(j + kx, 0, imageData.Width - 1);
                            int py = Math.Clamp(i + ky, 0, imageData.Height - 1);

                            byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                            float blurValue = LoGMatrix[ky + radius, kx + radius];

                            sumB += pixel[0] * blurValue;
                            sumG += pixel[1] * blurValue;
                            sumR += pixel[2] * blurValue;
                        }
                    }
                    byte* resultPixel = ptr + (i * imageData.Stride) + j * 4;
                    resultPixel[0] = (byte)Math.Clamp(originalB + sumB, 0, 255);
                    resultPixel[1] = (byte)Math.Clamp(originalG + sumG, 0, 255);
                    resultPixel[2] = (byte)Math.Clamp(originalR + sumR, 0, 255);
                    resultPixel[3] = originalPixel[3];
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

    private float[,] CreateKernelMatrix()
    {
        float[,] kernel = new float[_kernelSize, _kernelSize];
        int radius = _kernelSize / 2;
        float sigma2 = _sigma * _sigma;

        float sum = 0;
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                float distance2 = x * x + y * y;
                float value = (float)((distance2 - 2 * sigma2) / (Math.Pow(sigma2, 2)) *
                               Math.Exp(-distance2 / (2 * sigma2)));
                kernel[y + radius, x + radius] = value;
                sum += value;
            }
        }

        for (int y = 0; y < _kernelSize; y++)
        {
            for (int x = 0; x < _kernelSize; x++)
            {
                kernel[y, x] /= sum;
            }
        }

        return kernel;
    }
}
