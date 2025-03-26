using ImageFilters.Filters;

unsafe class UniformColorQuantization : AbstractFunctionalFilter
{
    private readonly byte[] _quantizationMap = new byte[256];

    public UniformColorQuantization(Bitmap image, int intervals = 3) : base(image)
    {
        if (intervals < 2 || intervals > 256)
            throw new ArgumentException("Intervals must be between 2 and 256.");
        InitializeQuantizationMap(intervals);
    }

    private void InitializeQuantizationMap(int intervals)
    {
        int step = 255 / (intervals - 1);

        for (int i = 0; i < 256; i++)
        {
            int closest = (int)Math.Round((double)i / step) * step;
            _quantizationMap[i] = (byte)Math.Clamp(closest, 0, 255);
        }
        Console.WriteLine(string.Join(" ", _quantizationMap));
    }

    public override unsafe Bitmap Apply()
    {
        return Apply((r, g, b) => (_quantizationMap[r], _quantizationMap[g], _quantizationMap[b]));
    }
}
