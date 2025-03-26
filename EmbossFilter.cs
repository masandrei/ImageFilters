using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class EmbossFilter : AbstractConvolutionalFilter
{
    private readonly EmbossDirection _direction;
    private readonly float _strength;

    public enum EmbossDirection
    {
        TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left
    }

    public EmbossFilter(Bitmap originalImage, EmbossDirection direction = EmbossDirection.TopLeft, float strength = 1.0f)
        : base(originalImage, 3)
    {
        _direction = direction;
        _strength = Math.Clamp(strength, 0.1f, 5.0f);
    }

    protected override unsafe void ApplyKernel(byte* ptr, BitmapData imageData, int x, int y, float[,] kernel, int radius)
    {
        float sumR = 0, sumG = 0, sumB = 0;

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

        sumR = Math.Clamp(sumR + 128, 0, 255);
        sumG = Math.Clamp(sumG + 128, 0, 255);
        sumB = Math.Clamp(sumB + 128, 0, 255);

        byte* resultPixel = ptr + (y * imageData.Stride) + (x * 4);
        resultPixel[0] = (byte)sumB;
        resultPixel[1] = (byte)sumG;
        resultPixel[2] = (byte)sumR;
    }

    protected override float[,] GetKernelMatrix()
    {
        return CreateEmbossKernel(_direction, _strength);
    }

    private float[,] CreateEmbossKernel(EmbossDirection direction, float strength)
    {
        return direction switch
        {
            EmbossDirection.TopLeft => new float[,] { { strength, strength / 2, 0 }, { strength / 2, 0, -strength / 2 }, { 0, -strength / 2, -strength } },
            EmbossDirection.Top => new float[,] { { strength / 2, strength, strength / 2 }, { 0, 0, 0 }, { -strength / 2, -strength, -strength / 2 } },
            EmbossDirection.TopRight => new float[,] { { 0, strength / 2, strength }, { -strength / 2, 0, strength / 2 }, { -strength, -strength / 2, 0 } },
            EmbossDirection.Right => new float[,] { { -strength / 2, 0, strength / 2 }, { -strength, 0, strength }, { -strength / 2, 0, strength / 2 } },
            EmbossDirection.BottomRight => new float[,] { { -strength, -strength / 2, 0 }, { -strength / 2, 0, strength / 2 }, { 0, strength / 2, strength } },
            EmbossDirection.Bottom => new float[,] { { -strength / 2, -strength, -strength / 2 }, { 0, 0, 0 }, { strength / 2, strength, strength / 2 } },
            EmbossDirection.BottomLeft => new float[,] { { 0, -strength / 2, -strength }, { strength / 2, 0, -strength / 2 }, { strength, strength / 2, 0 } },
            EmbossDirection.Left => new float[,] { { strength / 2, 0, -strength / 2 }, { strength, 0, -strength }, { strength / 2, 0, -strength / 2 } },
            _ => new float[,] { { strength, strength / 2, 0 }, { strength / 2, 0, -strength / 2 }, { 0, -strength / 2, -strength } }
        };
    }
}
