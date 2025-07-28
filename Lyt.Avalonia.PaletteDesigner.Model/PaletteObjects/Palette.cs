namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette
{
    public void Reset()
    {
        this.Kind = PaletteKind.TriadComplementary;
        this.Primary.Wheel = 90.0;
        this.Primary.UpdateFromWheel(this.Primary.Wheel);
        this.ResetAllShades();
    }

    #region Wheel Updates 

    public void UpdatePrimaryWheelMonochromatic(double primaryWheel) 
        => this.Primary.UpdateFromWheel(primaryWheel);

    public void UpdateShadesWheel(Shades shades, double wheel)
    {
        shades.Wheel = wheel;
        if (Palette.HueWheel.TryGetValue(Palette.ToAngle(wheel), out double hue))
        {
            var color = shades.Base.Color;
            var hsvColor = new HsvColor(hue, color.S, color.V);
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
        => this.Complementary.UpdateFromWheel(this.ComplementaryWheel());

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

    #endregion Wheel Updates 

    public void ResetAllShades()
    {
        this.Primary.Reset();
        this.Complementary.Reset();
        this.Secondary1.Reset();
        this.Secondary2.Reset();
    }

    public void ApplyShadesPreset(ShadesPreset shadesPreset)
    {
        this.Primary.ApplyShadesPreset(shadesPreset);
        this.Complementary.ApplyShadesPreset(shadesPreset);
        this.Secondary1.ApplyShadesPreset(shadesPreset);
        this.Secondary2.ApplyShadesPreset(shadesPreset);
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

        var preset = new ShadesPreset("Medium", this.Primary);
        name = "Medium_" + FileManagerModel.TimestampString();
        fm.Save<ShadesPreset>(
            FileManagerModel.Area.User, FileManagerModel.Kind.Json, name, preset);
    }
}
