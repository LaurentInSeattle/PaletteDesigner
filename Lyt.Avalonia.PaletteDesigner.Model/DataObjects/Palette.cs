namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public sealed class Palette
{
    private readonly Dictionary<int, RgbColor> colorWheel; 
    private readonly ShadeMap shadeMap ; 

    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public PaletteKind Kind { get; set; } = PaletteKind.Monochromatic;

    [JsonRequired]
    public double PrimaryWheel { get; set; } 

    [JsonRequired]
    public double ComplementaryWheel { get; set; }

    [JsonRequired]
    public double Secondary1Wheel { get; set; } = new();

    [JsonRequired]
    public double Secondary2Wheel { get; set; } = new();

    [JsonRequired]
    public double SecondaryWheelDistance { get; set; } = new();

    [JsonRequired]
    public Shades Primary { get; set; } = new();

    [JsonRequired]
    public Shades Secondary1 { get; set; } = new();

    [JsonRequired]
    public Shades Secondary2 { get; set; } = new();

    [JsonRequired]
    public Shades Complementary { get; set; } = new();

#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Palette() { /* needed for serialization */ }
#pragma warning restore CS8618 

    public Palette(Dictionary<int, RgbColor> colorWheel, ShadeMap shadeMap)
    { 
        this.colorWheel = colorWheel;
        this.shadeMap = shadeMap;
    }

    public void UpdatePrimaryWheelMonochromatic(
        double primaryWheel )
    {
        this.PrimaryWheel = primaryWheel;
        if (this.colorWheel.TryGetValue(this.PrimaryAngle(), out RgbColor? rgbColorPrimary))
        {
            if (rgbColorPrimary is null) 
            {
                throw new Exception("No such angle"); 
            }

            var hsvColorPrimary = rgbColorPrimary.ToHsv();
            var color = this.Primary.Base.Color;
            hsvColorPrimary.S = color.S;
            hsvColorPrimary.V = color.V;
            this.Primary.Base.Color = hsvColorPrimary;
        }

        var position = this.Primary.Base.Position; 
        this.Primary.Update(this.shadeMap, position.X, position.Y);
    }

    public void UpdatePrimaryShadeMonochromatic(int pixelX, int pixelY)
    {
        this.Primary.Update(this.shadeMap, pixelX, pixelY);
    }


    public void UpdateMonochromaticComplementary(double huePrimary)
    {

    }

    //public void Update (
    //    string name,
    //    PaletteKind kind,
    //    double hueComplementary,
    //    double saturation, double brightness,
    //    double saturationFactor, double brightnessFactor)
    //{
    //    this.Name = name;
    //    this.Kind = kind;
    //    switch (kind)
    //    {
    //        default:
    //        case PaletteKind.Unknown:
    //            throw new Exception("Missing kind");

    //        case PaletteKind.Monochromatic:
    //            this.GenerateMonochromatic(huePrimary, saturation, brightness, saturationFactor, brightnessFactor);
    //            return;

    //        case PaletteKind.Duochromatic:
    //            break;

    //        case PaletteKind.Trichromatic:
    //            break;

    //        case PaletteKind.Quadrichromatic:
    //            break;

    //        case PaletteKind.MonochromaticComplementary:
    //            this.GenerateMonochromaticComplementary(huePrimary, hueComplementary, saturation, brightness, saturationFactor, brightnessFactor);
    //            return;

    //        case PaletteKind.Triad:
    //            break;

    //        case PaletteKind.TriadComplementary:
    //            break;

    //        case PaletteKind.Square:
    //            break;
    //    }

    //    throw new NotImplementedException("Later...");
    //}

    public int PrimaryAngle () => (int)Math.Round(this.PrimaryWheel * 10.0);

    public int ComplementaryAngle()
    {
        double oppposite = (this.PrimaryWheel + 180.0).NormalizeAngleDegrees();
        return (int)Math.Round(oppposite * 10.0);
    }
}
