namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public sealed class Palette
{
    public string Name { get; set; } = string.Empty;

    public PaletteKind Kind { get; set; } = PaletteKind.MonochromaticComplementary;

    public double PrimaryWheel { get; set; }

    public double ComplementaryWheel { get; set; }

    public double Secondary1Wheel { get; set; } = new();

    public double Secondary2Wheel { get; set; } = new();

    public double SecondaryWheelDistance { get; set; } = new();

    public List<SvColor> SvColors { get; set; } = new();
        
    public Shades Primary { get; set; } = new();

    public Shades Secondary1 { get; set; } = new();

    public Shades Secondary2 { get; set; } = new();

    public Shades Complementary { get; set; } = new();

    public Palette() { /* needed for serialization */ }

    public Palette(
        string name,
        PaletteKind kind,
        double huePrimary,
        double hueComplementary,
        double saturation, double brightness,
        double saturationFactor, double brightnessFactor)
    {
        this.Name = name;
        this.Kind = kind;
        switch (kind)
        {
            default:
            case PaletteKind.Unknown:
                throw new Exception("Missing kind");

            case PaletteKind.Monochromatic:
                this.GenerateMonochromatic(huePrimary, saturation, brightness, saturationFactor, brightnessFactor);
                return;

            case PaletteKind.Duochromatic:
                break;

            case PaletteKind.Trichromatic:
                break;

            case PaletteKind.Quadrichromatic:
                break;

            case PaletteKind.MonochromaticComplementary:
                this.GenerateMonochromaticComplementary(huePrimary, hueComplementary, saturation, brightness, saturationFactor, brightnessFactor);
                return;

            case PaletteKind.Triad:
                break;

            case PaletteKind.TriadComplementary:
                break;

            case PaletteKind.Square:
                break;
        }

        throw new NotImplementedException("Later...");
    }

    private void GenerateMonochromatic(
        double hue, double saturation, double brightness,
        double saturationFactor, double brightnessFactor)
    {
        HsvColor primary = new(hue, saturation, brightness);
        this.Primary = new Shades(primary, saturationFactor, brightnessFactor);
    }

    private void GenerateMonochromaticComplementary(
        double huePrimary, double hueComplementary, 
        double saturation, double brightness,
        double saturationFactor, double brightnessFactor)
    {
        HsvColor primary = new(huePrimary, saturation, brightness);
        HsvColor complementary = new(hueComplementary, saturation, brightness);

        // TODO !
        this.Primary = new Shades(primary, saturationFactor, brightnessFactor);
        this.Complementary = new Shades(complementary, saturationFactor, brightnessFactor);
    }
}
