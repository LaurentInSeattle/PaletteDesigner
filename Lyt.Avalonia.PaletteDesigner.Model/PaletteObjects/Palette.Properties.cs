namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette
{
#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor.
#pragma warning disable CA2211 
    // Non-constant fields should not be visible

    public static PaletteDesignerModel Model;

    public static Dictionary<int, RgbColor> ColorWheel;

    public static ShadeMap ShadeMap;

    public static void Setup(
        PaletteDesignerModel model, Dictionary<int, RgbColor> colorWheel, ShadeMap shadeMap)
    {
        Palette.Model = model;
        Palette.ColorWheel = colorWheel;
        Palette.ShadeMap = shadeMap;
    }

#pragma warning restore CA2211 
#pragma warning restore CS8618 

    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public PaletteKind Kind { get; set; } = PaletteKind.MonochromaticComplementary;

    // for both Triad and Square, otherwise ignored 
    // Degrees on the wheel 
    [JsonRequired]
    public double SecondaryWheelDistance { get; set; } = 27.0;

    [JsonRequired]
    public bool AreShadesLocked { get; set; } = true;

    // If shades are unlocked, the shades user wants to edit  
    [JsonRequired]
    public WheelKind SelectedWheel { get; set; } = WheelKind.Unknown;

    [JsonRequired]
    public Shades Primary { get; set; } = new();

    [JsonRequired]
    public Shades Secondary1 { get; set; } = new();

    [JsonRequired]
    public Shades Secondary2 { get; set; } = new();

    [JsonRequired]
    public Shades Complementary { get; set; } = new();

    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Palette() { /* needed for serialization */ }
}
