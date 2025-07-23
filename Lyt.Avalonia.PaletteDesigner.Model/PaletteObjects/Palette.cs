namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class Palette
{
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

    private readonly Dictionary<int, RgbColor> colorWheel;
    private readonly ShadeMap shadeMap;

#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Palette() { /* needed for serialization */ }
#pragma warning restore CS8618 

    public Palette(Dictionary<int, RgbColor> colorWheel, ShadeMap shadeMap)
    {
        this.colorWheel = colorWheel;
        this.shadeMap = shadeMap;
    }

    public Shades FromWheel (WheelKind wheelKind)
        => wheelKind switch
        {
            WheelKind.Primary => this.Primary,
            WheelKind.Complementary => this.Complementary,
            WheelKind.Secondary1 => this.Secondary1,
            WheelKind.Secondary2 => this.Secondary2,
            _ => this.Primary,
        };

    public void Reset()
    {
        this.Primary.Wheel = 90.0;
        this.ResetShades();
    }

    public void ResetShades()
    {
        this.Primary.Reset(this.shadeMap);
        this.Complementary.Reset(this.shadeMap);
        this.Secondary1.Reset(this.shadeMap);
        this.Secondary2.Reset(this.shadeMap);
    }

    public void UpdatePrimaryWheelMonochromatic(double primaryWheel)
    {
        this.Primary.Wheel = primaryWheel;
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
            this.Primary.UpdateAllShadeColors(this.shadeMap);
        }
    }

    public void UpdateShadesWheel(Shades shades, double wheel)
    {
        shades.Wheel = wheel;
        if (this.colorWheel.TryGetValue(this.ToAngle(wheel), out RgbColor? rgbColor))
        {
            if (rgbColor is null)
            {
                throw new Exception("No such angle");
            }

            var hsvColor = rgbColor.ToHsv();
            var color = shades.Base.Color;
            hsvColor.S = color.S;
            hsvColor.V = color.V;
            shades.Base.Color = hsvColor;
            shades.UpdateAllShadeColors(this.shadeMap);
        }
    }

    public void UpdatePrimaryWheelMonochromaticComplementary(double primaryWheel)
    {
        this.Complementary.Wheel = this.ComplementaryWheel();
        if (this.colorWheel.TryGetValue(this.ComplementaryAngle(), out RgbColor? rgbColor))
        {
            if (rgbColor is null)
            {
                throw new Exception("No such angle");
            }

            var hsvColor = rgbColor.ToHsv();
            var color = this.Complementary.Base.Color;
            hsvColor.S = color.S;
            hsvColor.V = color.V;
            this.Complementary.Base.Color = hsvColor;
            this.Complementary.UpdateAllShadeColors(this.shadeMap);
        }
    }

    public void UpdateAllShades(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(this.shadeMap, pixelX, pixelY);
        this.Complementary.UpdateAll(this.shadeMap, pixelX, pixelY);
        this.Secondary1.UpdateAll(this.shadeMap, pixelX, pixelY);
        this.Secondary2.UpdateAll(this.shadeMap, pixelX, pixelY);
    }

    public void UpdateAllPrimaryShadeMonochromatic(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(this.shadeMap, pixelX, pixelY);
    }

    public void UpdateAllPrimaryShadeMonochromaticComplementary(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(this.shadeMap, pixelX, pixelY);
        this.Complementary.UpdateAll(this.shadeMap, pixelX, pixelY);
    }

    public void UpdateOnePrimaryShadeMonochromatic(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        this.Primary.UpdateOne(this.shadeMap, shadeKind, pixelX, pixelY);
    }

    public void UpdateOnePrimaryShadeMonochromaticComplementary(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        this.Primary.UpdateOne(this.shadeMap, shadeKind, pixelX, pixelY);
        this.Complementary.UpdateOne(this.shadeMap, shadeKind, pixelX, pixelY);
    }

    public int PrimaryAngle() => (int)Math.Round(this.Primary.Wheel * 10.0);

    public double ComplementaryWheel()
        => (this.Primary.Wheel + 180.0).NormalizeAngleDegrees();

    public int ComplementaryAngle()
    {
        double oppposite = (this.Primary.Wheel + 180.0).NormalizeAngleDegrees();
        return (int)Math.Round(oppposite * 10.0);
    }

    public int ToAngle(double wheel) => (int)Math.Round(wheel * 10.0);

    [Conditional("DEBUG")]
    public void Dump()
    {
        this.Primary.Dump("Primary");
        this.Complementary.Dump("Complementary");
        this.Secondary1.Dump("Secondary1");
        this.Secondary2.Dump("Secondary2");
    }
}
