using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class ContrastEnhancementFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly double _alpha;

    public ContrastEnhancementFilter(Bitmap originalImage, double alpha = 1.5)
    {
        ArgumentNullException.ThrowIfNull(originalImage);
        _originalImage = originalImage;
        _alpha = alpha;
    }

    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        BitmapData imageData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb );

        try
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            for(int i = 0; i < imageData.Height; i++)
            {
                byte* row = ptr + i * imageData.Stride;
                for(int j = 0; j < imageData.Width; j++)
                {
                    int idx = j * 4;
                    row[idx] = ApplyContrast(row[idx]);
                    row[idx + 1] = ApplyContrast(row[idx + 1]);
                    row[idx + 2] = ApplyContrast(row[idx + 2]);
                }
            }
        }
        finally
        {
            image.UnlockBits( imageData );
        }
        return image;
    }

    public Bitmap Restore()
    {
        return _originalImage;
    }
    private byte ApplyContrast(byte channel)
    {
        double normalizedValue = channel / 255.0;
        double contrastedValue = Math.Pow(normalizedValue, _alpha);
        return (byte)Math.Min(255, Math.Max(0, contrastedValue * 255));
    }
}
