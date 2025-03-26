using ImageFilters.Filters;
using System.Drawing.Imaging;

unsafe class OrderedDithering : AbstractConvolutionalFilter
{
    private readonly int _matrixSize;

    private static readonly HashSet<int> _allowedSizes = new() { 2, 3, 4, 6, 8, 16, 32, 64 };

    public OrderedDithering(Bitmap image, int size = 2) : base(image, size)
    {
        if (!_allowedSizes.Contains(size))
            throw new ArgumentException($"Matrix size must be one of: {string.Join(", ", _allowedSizes)}");
        _matrixSize = size;
    }

    private float[,] GetKernelMatrix(int size)
    {
        if (size == 2)
        {
            return new float[,] { { 1, 3 }, { 4, 2 } };
        }
        if (size == 3)
        {
            return new float[,] { { 3, 7, 4 }, { 6, 1, 9 }, { 2, 8, 5 } };
        }

        int prevSize = size / 2;
        float[,] prevMatrix = GetKernelMatrix(prevSize);
        float[,] newMatrix = new float[size, size];

        for (int y = 0; y < prevSize; y++)
        {
            for (int x = 0; x < prevSize; x++)
            {
                int baseValue = (int)prevMatrix[y, x] * 4;
                newMatrix[y, x] = baseValue;
                newMatrix[y, x + prevSize] = baseValue + 2;
                newMatrix[y + prevSize, x] = baseValue + 3;
                newMatrix[y + prevSize, x + prevSize] = baseValue + 1;
            }
        }

        return newMatrix;
    }

    protected override unsafe void ApplyKernel(byte* ptr, BitmapData imageData, int x, int y, float[,] kernel, int radius)
    {
        int factor = 256 / (_matrixSize * _matrixSize + 1);
        int matrixX = x % _matrixSize;
        int matrixY = y % _matrixSize;
        int threshold = (int)kernel[matrixY, matrixX] * factor;

        byte* pixel = ptr + (y * imageData.Stride) + (x * 3);

        int gray = (int)(0.299 * pixel[2] + 0.587 * pixel[1] + 0.114 * pixel[0]);
        byte newColor = (gray > threshold) ? (byte)255 : (byte)0;

        pixel[0] = pixel[1] = pixel[2] = newColor;
    }

    protected override float[,] GetKernelMatrix()
    {
        return GetKernelMatrix(_matrixSize);
    }
}
