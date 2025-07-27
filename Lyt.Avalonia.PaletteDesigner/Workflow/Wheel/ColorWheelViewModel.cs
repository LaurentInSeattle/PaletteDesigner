namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

using Lyt.Avalonia.PaletteDesigner.Model;
using HsvColor = Model.PaletteObjects.HsvColor;

public sealed partial class ColorWheelViewModel : ViewModel<ColorWheelView>
{
    private const int width = 256;
    private const int height = 256;
    private const int bpp = 4;
    private const byte alpha = 255;

    private readonly PaletteDesignerModel paletteDesignerModel;
    private double hue;

    [ObservableProperty]
    private WriteableBitmap shades;

    [ObservableProperty]
    private bool hasComplementaryMarker;

    [ObservableProperty]
    private bool hasSecondary1Marker;

    [ObservableProperty]
    private bool hasSecondary2Marker;

    [ObservableProperty]
    private bool canMoveComplementary;

    [ObservableProperty]
    private bool canMoveSecondary1;

    [ObservableProperty]
    private bool canMoveSecondary2;

    public ColorWheelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;

        var pixelSize = new PixelSize(width, height);
        var dpi = new Vector(96, 96);
        var bitmap = new WriteableBitmap(pixelSize, dpi, PixelFormat.Bgra8888, AlphaFormat.Opaque);
        this.shades = bitmap;
        this.hue = double.NaN;
        this.HasComplementaryMarker = false;
        this.CanMoveComplementary = false;
        this.HasSecondary1Marker = false;
        this.CanMoveSecondary1 = false;
        this.HasSecondary2Marker = false;
        this.CanMoveSecondary2 = false;
    }

    public void OnResetShadesClick() 
        => this.paletteDesignerModel.ResetShades();

    public void OnWheelAngleChanged(WheelKind wheelKind, double wheelAngle) 
        => this.paletteDesignerModel.OnWheelAngleChanged(wheelKind, wheelAngle);

    public void OnShadeMarkerPositionChanged(ShadeKind shadeKind, int pixelX, int pixelY)
        => this.paletteDesignerModel.OnShadeMarkerPositionChanged(shadeKind, pixelX, pixelY);

    public void Update(Palette palette)
    {
        this.HasComplementaryMarker = palette.Kind.HasComplementaryMarker();
        this.HasSecondary1Marker = palette.Kind.HasSecondary1Marker();
        this.HasSecondary2Marker = palette.Kind.HasSecondary2Marker();
        this.CanMoveComplementary = palette.Kind.CanMoveComplementaryMarker();
        this.CanMoveSecondary1 = palette.Kind.CanMoveSecondary1Marker();
        this.CanMoveSecondary2 = palette.Kind.CanMoveSecondary2Marker();
        this.View.PrimaryMarker.MoveWheelMarker(palette.Primary.Wheel);
        this.View.ComplementaryMarker.MoveWheelMarker(palette.Complementary.Wheel);
        this.View.Secondary1Marker.MoveWheelMarker(palette.Secondary1.Wheel);
        this.View.Secondary2Marker.MoveWheelMarker(palette.Secondary2.Wheel);

        var shades =
            palette.AreShadesLocked ?
                palette.Primary :
                palette.SelectedWheel.ToShadesFrom(palette);

        var position = shades.Base.Position;
        this.View.BaseShadeMarker.MoveShadeMarker(position.X, position.Y);
        position = shades.Lighter.Position;
        this.View.LighterShadeMarker.MoveShadeMarker(position.X, position.Y);
        position = shades.Light.Position;
        this.View.LightShadeMarker.MoveShadeMarker(position.X, position.Y);
        position = shades.Dark.Position;
        this.View.DarkShadeMarker.MoveShadeMarker(position.X, position.Y);
        position = shades.Darker.Position;
        this.View.DarkerShadeMarker.MoveShadeMarker(position.X, position.Y);

        HsvColor hsv = shades.Base.Color;
        this.UpdateShadesBitmap(hsv.H);
    }

    public unsafe void UpdateShadesBitmap(double newHue)
    {
        if (Math.Abs(this.hue - newHue) < 0.000_001)
        {
            return;
        }

        var map = 
            Palette.ShadeMap ?? 
            throw new ArgumentException("Palette has not been setup.");
        this.hue = newHue;
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

                    HsvColor.ToRgb(
                        this.hue, mapColor.S, mapColor.V,
                        out byte red, out byte green, out byte blue);
                    *p++ = blue;
                    *p++ = green;
                    *p++ = red;
                    *p++ = alpha;
                }
            }
        }

        this.View.Shades.InvalidateVisual();
    }

    [RelayCommand]
    public void OnRotateClockwise()
    {
    }

    [RelayCommand]
    public void OnRotateCounterClockwise()
    {
    }

    [RelayCommand]
    public void OnSwap()
    {
    }

    [RelayCommand]
    public void OnResetShades() => this.paletteDesignerModel.ResetShades();

    #region Color Wheel from resources
    /* 
     
    ResourcesUtilities.SetResourcesPath("Lyt.Avalonia.PaletteDesigner.Resources");
        ResourcesUtilities.SetExecutingAssembly(Assembly.GetExecutingAssembly());
        byte[] imageBytes = ResourcesUtilities.LoadEmbeddedBinaryResource(
            "wheel.png", out string? _);
        this.colorWheel = WriteableBitmap.Decode(new MemoryStream(imageBytes));

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

    */

    #endregion Color Wheel from resources
}
