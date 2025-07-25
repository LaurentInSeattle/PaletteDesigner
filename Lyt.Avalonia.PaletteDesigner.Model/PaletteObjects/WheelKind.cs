namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

// Identify the wheel marker 
public enum WheelKind
{
    Unknown,

    Primary,
    Complementary,
    Secondary1,
    Secondary2,
}

public static class WheelKindExtensions
{
    public static Shades ToShadesFrom(this WheelKind wheelKind, Palette palette)
        => wheelKind switch
        {
            WheelKind.Primary => palette.Primary,
            WheelKind.Complementary => palette.Complementary,
            WheelKind.Secondary1 => palette.Secondary1,
            WheelKind.Secondary2 => palette.Secondary2,
            _ => throw new ArgumentException("Wheel Kind is unkown"),
        };
}