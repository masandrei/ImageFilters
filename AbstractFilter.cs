namespace ImageFilters.Filters;

public abstract class AbstractFilter
{
    protected readonly Bitmap _originalImage;
    public AbstractFilter(Bitmap originalImage)
    {
        ArgumentNullException.ThrowIfNull(originalImage, nameof(originalImage));
        _originalImage = originalImage;
    }

    public abstract Bitmap Apply();
    public Bitmap Restore() => _originalImage;
}
