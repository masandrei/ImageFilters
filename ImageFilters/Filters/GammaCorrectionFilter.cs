using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class GammaCorrectionFilter : IFilter
{
    private readonly Bitmap _originalImage;
    private readonly double _gamma;

    public GammaCorrectionFilter(Bitmap originalImage, double gamma = 2.2)
    {
        ArgumentNullException.ThrowIfNull(originalImage);
        _originalImage = originalImage;
        _gamma = gamma;
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
            int stride = imageData.Stride;
            int width = imageData.Width;
            int height = imageData.Height;

            for (int i = 0; i < height; i++)
            {
                byte* row = ptr + i * stride;
                for (int j = 0; j < width; j++)
                {
                    int idx = j * 4;
                    row[idx] = (byte)ApplyGamma(row[idx]);
                    row[idx + 1] = (byte)ApplyGamma(row[idx + 1]);
                    row[idx + 2] = (byte)ApplyGamma(row[idx + 2]);
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

    private byte ApplyGamma(byte channel)
    {
        return (byte)(255 * Math.Pow(channel / 255.0, 1 / _gamma));
    }
}
