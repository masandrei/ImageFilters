using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class InverseFilter : AbstractFunctionalFilter
{
    public InverseFilter(Bitmap originalImage): base(originalImage) {}

    public override unsafe Bitmap Apply()
    {
        return Apply((r, g, b) => ((byte)(255 - r), (byte)(255 - g), (byte)(255 - b)));
    }
}
