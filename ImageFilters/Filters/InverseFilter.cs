using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal class InverseFilter : IFilter
{
    private readonly Bitmap _originalImage;

    public InverseFilter(Bitmap originalImage)
    {
        ArgumentNullException.ThrowIfNull(originalImage);
        _originalImage = originalImage;
    }
    public unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        BitmapData imageData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb);

        try 
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();
            int stride = imageData.Stride;
            int width = imageData.Width;
            int height = imageData.Height;

            for(int i = 0; i < height; i++)
            {
                byte* row = ptr + i * stride;
                for(int j = 0; j < width; j++)
                {
                    int idx = j * 4;
                    row[idx] = (byte)(255 - row[idx]);
                    row[idx + 1] = (byte)(255 - row[idx + 1]);
                    row[idx + 2] = (byte)(255 - row[idx + 2]);
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
}
