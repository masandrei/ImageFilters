using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal abstract class AbstractFunctionalFilter : AbstractFilter
{
    public AbstractFunctionalFilter(Bitmap originalImage) : base(originalImage) {}
    protected unsafe Bitmap Apply(Func<byte, byte, byte, (byte, byte, byte)> func)
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

            for (int i = 0; i < height; i++)
            {
                byte* row = ptr + i * stride;
                for (int j = 0; j < width; j++)
                {
                    int idx = j * 4;
                    (row[idx], row[idx + 1], row[idx + 2]) = func(row[idx], row[idx + 1], row[idx + 2]);
                }
            }
        }
        finally
        {
            image.UnlockBits(imageData);
        }
        return image;
    }
}
