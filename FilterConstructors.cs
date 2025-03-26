namespace ImageFilters.Filters;

public static class FilterConstructors
{
    private static Dictionary<FilterEnum, Func<int, Bitmap, AbstractFilter>> _filtersConstructors =
        new Dictionary<FilterEnum, Func<int, Bitmap, AbstractFilter>>()
        {
            { FilterEnum.Inverse, (args, image) => new InverseFilter(image)},
            { FilterEnum.EmbossFilter, (args, image) => new EmbossFilter(image)},
            { FilterEnum.EdgeDetectionFilter, (args, image) => new EdgeDetectionFilter(image)},
            { FilterEnum.SharpenFilter, (args, image) => new SharpenFilter(image)},
            { FilterEnum.BlurFilter, (args, image) => new BlurFilter(image)},
            { FilterEnum.GaussianBlurFilter, (args, image) => new GaussianBlurFilter(image)},
            { FilterEnum.BrightnessCorrection, (args, image) => new BrightnessCorrectionFilter(image)},
            { FilterEnum.ContrastEnhancement, (args, image) => new ContrastEnhancementFilter(image)},
            { FilterEnum.GammaCorrection, (args, image) => new GammaCorrectionFilter(image)},
            { FilterEnum.OrderedDilthering, (args, image) => new OrderedDithering(image, args) },
            { FilterEnum.UniformColorQuantization, (args, image) => new UniformColorQuantization(image, args) }

        };
    public static AbstractFilter GetFilter(Bitmap image, FilterEnum filterType, int args)
    {
        return _filtersConstructors[filterType](args, image);
    }
}
