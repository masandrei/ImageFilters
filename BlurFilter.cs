using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class BlurFilter : AbstractConvolutionalFilter
{

    public BlurFilter(Bitmap originalImage, int blurRadius = 7) : base(originalImage, blurRadius) { }

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

    protected override float[,] GetKernelMatrix()
    {
        float[,] kernelMatrix = new float[_kernelSize, _kernelSize];
        float value = 1.0f / (_kernelSize * _kernelSize);

        for(int i = 0; i < _kernelSize; i++)
        {
            for(int j = 0; j < _kernelSize; j++)
            {
                kernelMatrix[i, j] = value;
            }
        }
        return kernelMatrix;
    }
}
