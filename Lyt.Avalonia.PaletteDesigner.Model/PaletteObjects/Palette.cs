namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class Palette
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

#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Palette() { /* needed for serialization */ }
#pragma warning restore CS8618 

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
        this.Primary.Reset();
        this.Complementary.Reset();
        this.Secondary1.Reset();
        this.Secondary2.Reset();
    }

    public void UpdatePrimaryWheelMonochromatic(double primaryWheel)
    {
        this.Primary.UpdateFromWheel(primaryWheel);
    }

    public void UpdateShadesWheel(Shades shades, double wheel)
    {
        shades.Wheel = wheel;
        if (Palette.ColorWheel.TryGetValue(Palette.ToAngle(wheel), out RgbColor? rgbColor))
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
            shades.UpdateAllShadeColors(Palette.ShadeMap);
        }
    }

    public void UpdatePrimaryWheelTriad(double primaryWheel)
    {
        this.Secondary1.UpdateFromWheel(this.TriadWheel1());
        this.Secondary2.UpdateFromWheel(this.TriadWheel2());
        if (this.Kind.HasComplementary())
        {
            this.Complementary.UpdateFromWheel(this.ComplementaryWheel());
        } 
    }

    public void UpdatePrimaryWheelComplementary(double primaryWheel)
    {
        this.Complementary.UpdateFromWheel(this.ComplementaryWheel());
    }

    public void UpdatePrimaryWheelSquare(double primaryWheel)
    {
        this.UpdatePrimaryWheelComplementary(primaryWheel);
        double secondary1Wheel = 
            (primaryWheel + this.SecondaryWheelDistance).NormalizeAngleDegrees();
        this.Secondary1.UpdateFromWheel(secondary1Wheel);
        double secondary2Wheel =
            (this.Secondary1.Wheel + 180.0).NormalizeAngleDegrees();
        this.Secondary2.UpdateFromWheel(secondary2Wheel);
    }

    public void UpdateSecondaryWheelTriad(double wheel)
    {
        double delta = this.Primary.Wheel - wheel;
        this.SecondaryWheelDistance = Math.Abs(delta);
        this.UpdatePrimaryWheelTriad(this.Primary.Wheel);
    }

    public void UpdateSecondaryWheelSquare(double wheel)
    {
        double delta = this.Primary.Wheel - wheel;
        this.SecondaryWheelDistance = Math.Abs(delta);
        this.Secondary1.UpdateFromWheel(wheel);
        double secondary2Wheel = (wheel + 180.0).NormalizeAngleDegrees();
        this.Secondary2.UpdateFromWheel(secondary2Wheel);
    }

    public void UpdateAllShades(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(pixelX, pixelY);
        this.Complementary.UpdateAll(pixelX, pixelY);
        this.Secondary1.UpdateAll(pixelX, pixelY);
        this.Secondary2.UpdateAll(pixelX, pixelY);
    }

    public void UpdateAllPrimaryShadeMonochromatic(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(pixelX, pixelY);
    }

    public void UpdateAllPrimaryShadeMonochromaticComplementary(int pixelX, int pixelY)
    {
        this.Primary.UpdateAll(pixelX, pixelY);
        this.Complementary.UpdateAll(pixelX, pixelY);
    }

    public void UpdateOnePrimaryShadeMonochromatic(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        this.Primary.UpdateOne(shadeKind, pixelX, pixelY);
    }

    public void UpdateOnePrimaryShadeMonochromaticComplementary(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        this.Primary.UpdateOne(shadeKind, pixelX, pixelY);
        this.Complementary.UpdateOne(shadeKind, pixelX, pixelY);
    }

    public static int ToAngle(double wheel) => (int)Math.Round(wheel * 10.0);

    public int PrimaryAngle() => Palette.ToAngle(this.Primary.Wheel);

    public double ComplementaryWheel()
        => (this.Primary.Wheel + 180.0).NormalizeAngleDegrees();

    public int ComplementaryAngle()
    {
        double oppposite = (this.Primary.Wheel + 180.0).NormalizeAngleDegrees();
        return (int)Math.Round(oppposite * 10.0);
    }

    public double TriadWheel1()
        => (this.Primary.Wheel + this.SecondaryWheelDistance).NormalizeAngleDegrees();

    public double TriadWheel2()
        => (this.Primary.Wheel - this.SecondaryWheelDistance).NormalizeAngleDegrees();

    [Conditional("DEBUG")]
    public void Dump()
    {
        this.Primary.Dump("Primary");
        this.Complementary.Dump("Complementary");
        this.Secondary1.Dump("Secondary1");
        this.Secondary2.Dump("Secondary2");

        // Create a JSON from the palette, save on disk with time stamp
        var fm = Model.fileManager;
        string name = "Palette_" + FileManagerModel.TimestampString(); 
        fm.Save<Palette>(
            FileManagerModel.Area.User, FileManagerModel.Kind.Json, name, this);
    }
}
