namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

// using global::Avalonia.Media.Imaging;

public sealed partial class ColorWheelViewModel : ViewModel<ColorWheelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private WriteableBitmap imageSource;

    public ColorWheelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;

        SerializationUtilities.SetResourcesPath("Lyt.Avalonia.PaletteDesigner.Resources");
        SerializationUtilities.SetExecutingAssembly(Assembly.GetExecutingAssembly());        
        byte[] imageBytes = SerializationUtilities.LoadEmbeddedBinaryResource(
            "wheel.png", out string? _);
        var bitmap = WriteableBitmap.Decode(new MemoryStream(imageBytes));
        this.imageSource = bitmap;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        // this.CreateColorLookupTable();
    }

    private void CreateColorLookupTable()
    {
        Dictionary<int, RgbColor> colorLookupTable = new(360 * 10);

        using ILockedFramebuffer lockedFramebuffer = this.ImageSource.Lock();
        unsafe
        {
            byte* p = (byte*)lockedFramebuffer.Address.ToPointer();
            int bytesPerPixel = lockedFramebuffer.Format.BitsPerPixel / 8;
            int stride = lockedFramebuffer.RowBytes;

            // angle in tenth of degree 
            double radius = 210.0; 
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

                colorLookupTable.Add(angle, new RgbColor(red, green, blue)); 
            }
        }

        this.paletteDesignerModel.ColorLookupTable = colorLookupTable;

        var fileManager = App.GetRequiredService<FileManagerModel>();
        string colorLookupTableSeralized = fileManager.Serialize(colorLookupTable);
        fileManager.Save(
            FileManagerModel.Area.User, FileManagerModel.Kind.Json, "ColorWheel.json", colorLookupTable); 
    }
}
