namespace ImageFilters.Filters;

internal interface IFilter
{
    Bitmap Apply();
    Bitmap Restore();
}
