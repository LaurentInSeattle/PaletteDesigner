namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

public partial class ColorWheelView : View
{
    public ColorWheelView() : base()
    {
    }

        //using ILockedFramebuffer lockedFramebuffer = bitmap.Lock();
        //unsafe
        //{
        //    byte* p = (byte*)lockedFramebuffer.Address.ToPointer();
        //    int bytesPerPixel = lockedFramebuffer.Format.BitsPerPixel / 8;
        //    int stride = lockedFramebuffer.RowBytes;

        //    // Calculate the offset to the desired pixel
        //    int pixelOffset = (pixelY * stride) + (pixelX * bytesPerPixel);

        //    // Access individual color components (assuming BGRA format for example)
        //    byte blue = p[pixelOffset];
        //    byte green = p[pixelOffset + 1];
        //    byte red = p[pixelOffset + 2];
        //    byte alpha = p[pixelOffset + 3];

        //    //Debug.WriteLine(
        //    //    string.Format("A: {0:X2}  R: {1:X2}  G: {2:X2}  B: {3:X2}  ", alpha, red, green, blue));
        //}
}
