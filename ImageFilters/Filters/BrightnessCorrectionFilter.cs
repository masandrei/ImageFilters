using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class BrightnessCorrectionFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly int _brightness;

    public BrightnessCorrectionFilter(Bitmap originalImage, int brightness = 30)
    {
        ArgumentNullException.ThrowIfNull(originalImage);
        _originalImage = originalImage;
        _brightness = brightness;
    }

    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);

        BitmapData imageData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppRgb);

        try
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();

            for (int i = 0; i < imageData.Height; i++)
            {
                byte* row = ptr + i * imageData.Stride;
                for(int j = 0; j < imageData.Width; j++)
                {
                    int idx = j * 4;
                    row[idx] = ApplyBrightness(row[idx]);
                    row[idx + 1] = ApplyBrightness(row[idx + 1]);
                    row[idx + 2] = ApplyBrightness(row[idx + 2]);
                }
            }
        }
        finally
        {
            image.UnlockBits(imageData);
        }
        return image;
    }

    public Bitmap Restore()
    {
        return _originalImage;
    }

    private byte ApplyBrightness(byte channel)
    {
        int newValue = channel + _brightness;
        return (byte)Math.Min(255, Math.Max(0, newValue));
    }
}
