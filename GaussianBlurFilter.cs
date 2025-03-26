using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class GaussianBlurFilter : AbstractConvolutionalFilter
{
    private readonly double _sigma;

    public GaussianBlurFilter(Bitmap originalImage, int blurRadius = 5, double sigma = 10.0f) : base(originalImage, blurRadius)
    {
        _sigma = sigma;
    }

    protected override float[,] GetKernelMatrix()
    {
        float[,] kernel = new float[_kernelSize, _kernelSize];
        int radius = _kernelSize / 2;
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

        for(int i = 0; i < _kernelSize; i++)
        {
            for(int j = 0; j < _kernelSize; j++)
            {
                kernel[i, j] /= sum;
            }
        }
        return kernel;
    }

    protected override unsafe void ApplyKernel(byte* ptr, BitmapData imageData, int x, int y, float[,] kernel, int radius)
    {
        float sumB = 0, sumG = 0, sumR = 0;

        for (int ky = -radius; ky <= radius; ky++)
        {
            for (int kx = -radius; kx <= radius; kx++)
            {
                int px = Math.Clamp(x + kx, 0, imageData.Width - 1);
                int py = Math.Clamp(y + ky, 0, imageData.Height - 1);

                byte* pixel = ptr + (py * imageData.Stride) + (px * 4);
                float blurValue = kernel[ky + radius, kx + radius];

                sumB += pixel[0] * blurValue;
                sumG += pixel[1] * blurValue;
                sumR += pixel[2] * blurValue;
            }
        }
        byte* resultPixel = ptr + (y * imageData.Stride) + x * 4;
        resultPixel[0] = (byte)Math.Clamp(sumB, 0, 255);
        resultPixel[1] = (byte)Math.Clamp(sumG, 0, 255);
        resultPixel[2] = (byte)Math.Clamp(sumR, 0, 255);
        resultPixel[3] = *(ptr + (y * imageData.Stride) + (x * 4) + 3);
    }
}
