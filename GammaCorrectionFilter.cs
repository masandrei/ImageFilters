namespace ImageFilters.Filters;

internal class GammaCorrectionFilter : AbstractFunctionalFilter
{
    private readonly double _gamma;

    public GammaCorrectionFilter(Bitmap originalImage, double gamma = 2.2):base(originalImage)
    {
        _gamma = gamma;
    }

    public override unsafe Bitmap Apply()
    {
        return Apply((r, g, b) => (ApplyGamma(r), ApplyGamma(g), ApplyGamma(b)));
    }

    private byte ApplyGamma(byte channel)
    {
        return (byte)(255 * Math.Pow(channel / 255.0, 1 / _gamma));
    }
}
