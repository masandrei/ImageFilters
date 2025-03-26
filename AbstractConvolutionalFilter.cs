using System.Drawing.Imaging;

namespace ImageFilters.Filters;

internal abstract class AbstractConvolutionalFilter : AbstractFilter
{
    protected readonly int _kernelSize;
    public AbstractConvolutionalFilter(Bitmap originalImage, int kernelSize) : base(originalImage) 
    {
        _kernelSize = kernelSize % 2 == 0 ? kernelSize + 1 : kernelSize; 
    }
    public override unsafe Bitmap Apply()
    {
        Bitmap image = new Bitmap(_originalImage);
        float[,] kernel = GetKernelMatrix();
        int radius = _kernelSize / 2;

        BitmapData imageData = image.LockBits(
            new Rectangle(0, 0, image.Width, image.Height),
            ImageLockMode.ReadWrite,
            PixelFormat.Format32bppArgb
        );

        try
        {
            byte* ptr = (byte*)imageData.Scan0.ToPointer();
            for (int y = 0; y < imageData.Height; y++)
            {
                for (int x = 0; x < imageData.Width; x++)
                {
                    ApplyKernel(ptr, imageData, x, y, kernel, radius);
                }
            }
        }
        finally
        {
            image.UnlockBits(imageData);
        }

        return image;
    }

    protected abstract unsafe void ApplyKernel(byte* ptr, BitmapData imageData, int x, int y, float[,] kernel, int radius);

    protected abstract float[,] GetKernelMatrix();
}
