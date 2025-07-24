namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

// See: https://en.wikipedia.org/wiki/HSL_and_HSV 
// http://paletton.com/
// https://color.adobe.com/create/color-wheel/


public enum PaletteKind
{
    Unknown,

    Monochromatic,                          // One free color
    Duochromatic,                           // Two free colors
    Trichromatic,                           // Three free colors
    Quadrichromatic,                        // Four free colors

    MonochromaticComplementary,             // One free color and complementary

    Triad,                                  // Two free colors, one symmetrical (Aka Adjacent or Triad) 
    TriadComplementary,                     // Two free colors, one symmetrical and complementary

    Square,                                 // Two free colors, two complementaries
}

// Identify the wheel marker 
public enum WheelKind
{
    Unknown,

    Primary,
    Complementary,  
    Secondary1, 
    Secondary2, 
}

public static class PaletteKindExtensions
{
    public static int ColorCount(this PaletteKind kind)
        => kind switch
        {
            PaletteKind.Monochromatic => 1,
            PaletteKind.MonochromaticComplementary or PaletteKind.Duochromatic => 2,
            PaletteKind.Triad or PaletteKind.Trichromatic => 3,
            PaletteKind.TriadComplementary or PaletteKind.Quadrichromatic or PaletteKind.Square => 4,
            _ => 0, // Unknown 
        };

    // All except Triad and Monochromatic
    public static bool HasComplementaryMarker(this PaletteKind kind)
        => kind == PaletteKind.MonochromaticComplementary ||
           kind == PaletteKind.Duochromatic ||
           kind == PaletteKind.Quadrichromatic ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Square;

    // True when complementary is automatically set 
    public static bool HasComplementary(this PaletteKind kind)
        => kind == PaletteKind.MonochromaticComplementary ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Square;

    // Only for free color models 
    public static bool CanMoveComplementaryMarker(this PaletteKind kind)
        => kind == PaletteKind.Duochromatic ||
           kind == PaletteKind.Quadrichromatic;

    public static bool HasSecondary1Marker(this PaletteKind kind)
        => kind == PaletteKind.Trichromatic ||
           kind == PaletteKind.Quadrichromatic ||
           kind == PaletteKind.Triad ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Square;

    public static bool CanMoveSecondary1Marker(this PaletteKind kind)
        => kind == PaletteKind.Trichromatic ||
           kind == PaletteKind.Quadrichromatic ||
           kind == PaletteKind.Triad ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Square;

    public static bool HasSecondary2Marker(this PaletteKind kind)
        => kind == PaletteKind.Trichromatic ||
           kind == PaletteKind.Quadrichromatic ||
           kind == PaletteKind.Triad ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Square;

    public static bool CanMoveSecondary2Marker(this PaletteKind kind)
        => kind == PaletteKind.Trichromatic ||
           kind == PaletteKind.Triad ||
           kind == PaletteKind.TriadComplementary ||
           kind == PaletteKind.Quadrichromatic;

}

// 