using System.Drawing.Imaging;

namespace ImageFilters.Filters;

public static class FilterExtensions
{
    public static unsafe Bitmap ApplyMapping(this Bitmap _imageToApply, byte[] colorMapping)
    {
        Bitmap newImage = new Bitmap(_imageToApply);

        BitmapData imageData = newImage.LockBits(
            new Rectangle(0, 0, newImage.Width, newImage.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb
            );

        try
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();
            for (int i = 0; i < imageData.Height; i++)
            {
                for (int j = 0; j < imageData.Width; j++)
                {
                    byte* resultPixel = ptr + (i * imageData.Stride) + j * 4;
                    resultPixel[0] = colorMapping[resultPixel[0]];
                    resultPixel[1] = colorMapping[resultPixel[1]];
                    resultPixel[2] = colorMapping[resultPixel[2]];
                    resultPixel[3] = *(ptr + (i * imageData.Stride) + (j * 4) + 3);
                }
            }

        }
        finally
        {
            newImage.UnlockBits(imageData);
        }

        return newImage;
    }
}
