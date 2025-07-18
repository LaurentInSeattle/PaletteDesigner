namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

using Lyt.Utilities.DataStructures;
using HsvColor = Model.DataObjects.HsvColor;

public sealed partial class ColorWheelViewModel : ViewModel<ColorWheelView>
{
    private const int width = 256;
    private const int height = 256;
    private const int bpp = 4;
    private const byte alpha = 255;

    private readonly PaletteDesignerModel paletteDesignerModel;
    private double hue;

    [ObservableProperty]
    private WriteableBitmap colorWheel;

    [ObservableProperty]
    private WriteableBitmap shades;

    [ObservableProperty]
    private bool hasComplementary;

    [ObservableProperty]
    private bool canMoveComplementary;

    public ColorWheelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;

        ResourcesUtilities.SetResourcesPath("Lyt.Avalonia.PaletteDesigner.Resources");
        ResourcesUtilities.SetExecutingAssembly(Assembly.GetExecutingAssembly());
        byte[] imageBytes = ResourcesUtilities.LoadEmbeddedBinaryResource(
            "wheel.png", out string? _);
        this.colorWheel = WriteableBitmap.Decode(new MemoryStream(imageBytes));

        var pixelSize = new PixelSize(width, height);
        var dpi = new Vector(96, 96);
        var bitmap = new WriteableBitmap(pixelSize, dpi, PixelFormat.Bgra8888, AlphaFormat.Opaque);
        this.shades = bitmap;
        this.hue = double.NaN;
    }

    public NestedDictionary<int, int, HsvColor> Map => this.paletteDesignerModel.ShadeColorMap;

    public void OnAngleChanged(double wheelAngle)
    {
        this.paletteDesignerModel.UpdatePalettePrimaryWheel(wheelAngle);
    }

    public void OnShadeChanged(double saturation, double brightness)
    {
        this.paletteDesignerModel.UpdatePalettePrimaryShade(saturation, brightness);
    }

    public void Update(Palette palette)
    {
        this.HasComplementary = palette.Kind.HasComplementary();
        this.CanMoveComplementary = palette.Kind.CanMoveComplementary();
        this.View.PrimaryMarker.MoveWheelMarker(palette.PrimaryWheel);
        var hsv = palette.Primary.Base;
        this.View.PrimaryShadeMarker.MoveShadeMarker(hsv.S, hsv.V);
        this.UpdateShadesBitmap(palette.Primary.Base.H);
        if (this.HasComplementary)
        {
            double complementaryWheel = (palette.PrimaryWheel + 180.0).NormalizeAngleDegrees();
            this.View.ComplementaryMarker.MoveWheelMarker(complementaryWheel);
        }
    }

    public unsafe void UpdateShadesBitmap(double newHue)
    {
        if (Math.Abs(this.hue - newHue) < 0.000_001)
        {
            return;
        }

        var map = this.paletteDesignerModel.ShadeColorMap;
        this.hue = newHue;
        HsvColor hsvColor = new(this.hue, 0.0, 0.0);
        using var frameBuffer = this.Shades.Lock();
        {
            byte* p = (byte*)frameBuffer.Address;
            for (int row = 0; row < height; ++row)
            {
                for (int col = 0; col < width; ++col)
                {
                    if (!map.TryGetValue(row, col, out HsvColor? mapColor) || (mapColor is null))
                    {
                        Debugger.Break();

                        *p++ = 0;
                        *p++ = 0;
                        *p++ = 0;
                        *p++ = 0;
                        continue;
                    }

                    if (mapColor.H < 0.0)
                    {
                        *p++ = 0;
                        *p++ = 0;
                        *p++ = 0;
                        *p++ = 0;
                        continue;
                    }

                    hsvColor.Set(this.hue, mapColor.S, mapColor.V);
                    var rgb = hsvColor.ToRgb();
                    byte blu = (byte)rgb.B;
                    byte gre = (byte)rgb.G;
                    byte red = (byte)rgb.R;
                    *p++ = blu;
                    *p++ = gre;
                    *p++ = red;
                    *p++ = alpha;
                }
            }
        }

        this.View.Shades.InvalidateVisual();
    }

    private void CreateColorLookupTable()
    {
        Dictionary<int, RgbColor> colorLookupTable = new(360 * 10);

        using ILockedFramebuffer lockedFramebuffer = this.ColorWheel.Lock();
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
                int pixelX = (int)(x + 320.0);
                int pixelY = (int)(320.0 - y);

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
