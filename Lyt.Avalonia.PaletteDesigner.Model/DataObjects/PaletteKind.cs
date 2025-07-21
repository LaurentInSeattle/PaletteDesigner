namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

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
           kind == PaletteKind.Trichromatic ||
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
           kind == PaletteKind.Trichromatic ||
           kind == PaletteKind.Quadrichromatic;
}

//public class PaletteKindComboBoxItem
//{
//    public PaletteKindComboBoxItem(PaletteKind kind, string displayString, string tip)
//    {
//        this.Kind = kind;
//        this.DisplayString = displayString;
//        this.Tip = tip;
//    }

//    public string DisplayString { get; private set; }
//    public PaletteKind Kind { get; private set; }
//    public string Tip { get; private set; }

//    public static List<PaletteKindComboBoxItem> All()
//    {
//        var list = new List<PaletteKindComboBoxItem>(10)
//        {
//            new PaletteKindComboBoxItem ( PaletteKind.Monochromatic,                        "Monochromatic",    "One free color." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.MonochromaticComplementary,           "Monochromatic with Complementary", "One free color and its complementary." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.Duochromatic,                         "Duochromatic", "Two free colors." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.DuochromaticComplementary,            "Duochromatic with Complementary", "Two free colors and one complementary." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.TriChromatic,                         "Free Triad", "Three free colors." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.TriChromaticSymmetrical,               "Symmetrical Triad", "Two free colors, one symetrical." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.TriChromaticSymmetricalComplementary,  "Symmetrical Triad with Complementary", "Two free colors, one symetrical and one complementary." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.TriChromaticComplementary,            "Free Triad with Complementary", "Three free colors, one complementary" ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.Quadrichromatic,                      "Free Quadrichromatic", "Four free colors." ) ,
//            new PaletteKindComboBoxItem ( PaletteKind.QuadrichromaticComplementary,         "Quadrichromatic with Complementaries", "Two free colors, two complementary colors." ) ,
//        };

//        return list;
//    }
//}