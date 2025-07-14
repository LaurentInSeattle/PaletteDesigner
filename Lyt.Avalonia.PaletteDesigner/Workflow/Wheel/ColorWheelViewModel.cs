namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

using global::Avalonia.Media.Imaging;

public sealed partial class ColorWheelViewModel : ViewModel<ColorWheelView>
{
    private readonly Dictionary<int, RgbColor> colorLookupTable;

    [ObservableProperty]
    private WriteableBitmap imageSource;

    public ColorWheelViewModel()
    {
        byte[] imageBytes = SerializationUtilities.LoadEmbeddedBinaryResource(
            "wheel.png", out string? _);
        var bitmap = WriteableBitmap.Decode(new MemoryStream(imageBytes));
        this.imageSource = bitmap;
        this.colorLookupTable = new(360 * 10);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        this.CreateColorLookupTable();
    }

    private void CreateColorLookupTable()
    {
        using ILockedFramebuffer lockedFramebuffer = this.ImageSource.Lock();
        unsafe
        {
            byte* p = (byte*)lockedFramebuffer.Address.ToPointer();
            int bytesPerPixel = lockedFramebuffer.Format.BitsPerPixel / 8;
            int stride = lockedFramebuffer.RowBytes;

            // angle in tenth of degree 
            double radius = 170.0; 
            for (int angle = 0; angle < 3600; ++angle)
            {
                // convert to radians
                double radian = 2.0 * Math.PI * angle / 3600.0;
                
                // Coordinates at wheel center 
                double x = radius * Math.Cos(radian);
                double y = radius * Math.Sin(radian);

                // Translate to top left 
                int pixelX = (int )( x + 320.0) ;
                int pixelY = (int) ( 320.0 - y) ;

                // Calculate the offset to the desired pixel
                int pixelOffset = (pixelY * stride) + (pixelX * bytesPerPixel);

                // Access individual color components (assuming BGRA format)
                byte blue = p[pixelOffset++];
                byte green = p[pixelOffset++];
                byte red = p[pixelOffset];

                this.colorLookupTable.Add(angle, new RgbColor(red, green, blue)); 
            }
        }
    }
}
