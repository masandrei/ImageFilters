using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class EdgeDetectionFilter : AbstractConvolutionalFilter
{
    private readonly double _threshold;
    private readonly bool _grayscale;

    public EdgeDetectionFilter(Bitmap originalImage, int kernelSize = 3, double threshold = 0.0, bool grayscale = true)
        : base(originalImage, kernelSize)
    {
        ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));
        _threshold = threshold;
        _grayscale = grayscale;
    }

    protected override unsafe void ApplyKernel(byte* ptr, BitmapData imageData, int x, int y, float[,] kernel, int radius)
    {
        double gx = 0, gy = 0;

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

                double intensity = 0.299 * r + 0.587 * g + 0.114 * b;
                gx += intensity * kernel[ky + radius, kx + radius];
                gy += intensity * kernel[ky + radius, kx + radius];
            }
        }

        double magnitude = Math.Sqrt(gx * gx + gy * gy);
        magnitude = (magnitude > _threshold) ? Math.Min(255, magnitude) : 0;

        byte edge = (byte)magnitude;

        byte* targetPixel = ptr + (y * imageData.Stride) + (x * 4);
        if (_grayscale)
        {
            targetPixel[0] = edge;
            targetPixel[1] = edge;
            targetPixel[2] = edge;
        }
        else
        {
            targetPixel[0] = (byte)(targetPixel[0] * edge / 255);
            targetPixel[1] = (byte)(targetPixel[1] * edge / 255);
            targetPixel[2] = (byte)(targetPixel[2] * edge / 255);
        }
    }

    protected override float[,] GetKernelMatrix()
    {
        return CreateSobelKernelX(_kernelSize);
    }

    private float[,] CreateSobelKernelX(int size)
    {
        float[,] kernel = new float[size, size];
        int radius = size / 2;

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                float scale = (float)(radius - Math.Abs(y)) / radius;
                kernel[y + radius, x + radius] = x * scale;
            }
        }

        NormalizeKernel(kernel);
        return kernel;
    }

    private void NormalizeKernel(float[,] kernel)
    {
        float sum = 0;
        foreach (var value in kernel) sum += Math.Abs(value);
        if (sum > 0)
        {
            for (int i = 0; i < kernel.GetLength(0); i++)
                for (int j = 0; j < kernel.GetLength(1); j++)
                    kernel[i, j] /= sum;
        }
    }
}
