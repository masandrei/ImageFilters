using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class ContrastEnhancementFilter : AbstractFunctionalFilter
{
    private readonly double _alpha;

    public ContrastEnhancementFilter(Bitmap originalImage, double alpha = 1.5): base(originalImage)
    {
        _alpha = alpha;
    }

    public override unsafe Bitmap Apply()
    {
        return Apply((r, g, b) => (ApplyContrast(r), ApplyContrast(g), ApplyContrast(b)));
    }

    private byte ApplyContrast(byte channel)
    {
        double normalizedValue = channel / 255.0;
        double contrastedValue = Math.Pow(normalizedValue, _alpha);
        return (byte)Math.Min(255, Math.Max(0, contrastedValue * 255));
    }
}
