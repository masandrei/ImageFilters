namespace ImageFilters.Filters;

internal class BrightnessCorrectionFilter : AbstractFunctionalFilter
{
    private readonly int _brightness;

    public BrightnessCorrectionFilter(Bitmap originalImage, int brightness = 30): base(originalImage)
    {
        _brightness = brightness;
    }

    public override unsafe Bitmap Apply()
    {
       return base.Apply((r, g, b) => (ApplyBrightness(r), ApplyBrightness(g), ApplyBrightness(b)));
    }

    private byte ApplyBrightness(byte channel)
    {
        int newValue = channel + _brightness;
        return (byte)Math.Min(255, Math.Max(0, newValue));
    }
}
