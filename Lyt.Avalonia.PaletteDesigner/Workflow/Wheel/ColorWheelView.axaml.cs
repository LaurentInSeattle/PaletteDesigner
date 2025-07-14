namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

using global::Avalonia.Media.Imaging;

public partial class ColorWheelView : View
{
    public ColorWheelView() : base()
    {
        this.Image.PointerPressed += this.OnPointerPressed;
        this.Image.PointerMoved += this.OnPointerMoved;
        this.Image.PointerReleased += this.OnPointerReleased;
    }

    ~ColorWheelView()
    {
        this.Image.PointerPressed -= this.OnPointerPressed;
        this.Image.PointerMoved -= this.OnPointerMoved;
        this.Image.PointerReleased -= this.OnPointerReleased;
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    { 
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if ( sender is not Image image)
        {
            Debug.WriteLine("Not an image");
            return;
        }

        var mousePosition = e.GetPosition(image);
        var source = image.Source;
        if (source is not WriteableBitmap bitmap)
        {
            Debug.WriteLine("Not a WriteableBitmap");
            return; 
        }

        // Image is square, side is 640 pix, gradient begins at 150
        Rect bounds = image.Bounds;
        int pixelX = (int)(mousePosition.X / bounds.Width * bitmap.Size.Width);
        int pixelY = (int)(mousePosition.Y / bounds.Height * bitmap.Size.Height);
        Debug.WriteLine(string.Format("X: {0}  Y: {1}", pixelX, pixelY));

        using ILockedFramebuffer lockedFramebuffer = bitmap.Lock();
        unsafe
        {
            byte* p = (byte*)lockedFramebuffer.Address.ToPointer();
            int bytesPerPixel = lockedFramebuffer.Format.BitsPerPixel / 8;
            int stride = lockedFramebuffer.RowBytes;

            // Calculate the offset to the desired pixel
            int pixelOffset = (pixelY * stride) + (pixelX * bytesPerPixel);

            // Access individual color components (assuming BGRA format for example)
            byte blue = p[pixelOffset];
            byte green = p[pixelOffset + 1];
            byte red = p[pixelOffset + 2];
            byte alpha = p[pixelOffset + 3];

            Debug.WriteLine(
                string.Format("A: {0:X2}  R: {1:X2}  G: {2:X2}  B: {3:X2}  ", alpha, red, green, blue));
        }
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    { 
    } 

}
