namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public sealed class Palette
{
    private readonly Dictionary<int, RgbColor> colorWheel;
    private readonly ShadeMap shadeMap;

    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public PaletteKind Kind { get; set; } = PaletteKind.MonochromaticComplementary;

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

    public void Reset()
    {
        this.PrimaryWheel = 90.0;
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
            this.Primary.UpdateAllShadeColors(this.shadeMap);
        }
    }

    public void UpdatePrimaryWheelMonochromaticComplementary(double primaryWheel)
    {
        this.UpdatePrimaryWheelMonochromatic(primaryWheel); 
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

    public int PrimaryAngle() => (int)Math.Round(this.PrimaryWheel * 10.0);

    public int ComplementaryAngle()
    {
        double oppposite = (this.PrimaryWheel + 180.0).NormalizeAngleDegrees();
        return (int)Math.Round(oppposite * 10.0);
    }
}
