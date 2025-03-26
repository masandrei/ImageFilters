namespace ImageFilters.Filters;

public class FilterParameters
{
    private static Dictionary<FilterEnum, List<int>> _filterOptionsAvailable =
        new Dictionary<FilterEnum, List<int>>()
        {
            { FilterEnum.OrderedDilthering, new List<int> { 2, 3, 4, 6, 8, 16, 32, 64 } },
            { FilterEnum.UniformColorQuantization, Enumerable.Range(2, 255).ToList()}
        };

    public static List<int> GetFilterParamsValues(FilterEnum filterEnum)
    {
        if (_filterOptionsAvailable.TryGetValue(filterEnum, out var values))
        {
            return values;
        }
        return Enumerable.Empty<int>().ToList();
    }
}
