namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette : IExportAble
{
    public void Reset()
    {
        this.Kind = PaletteKind.Triad;
        this.Primary.Wheel = 105.0;
        this.UpdatePrimaryWheelTriad();
        this.ResetAllShades();
    }

    #region Wheel Updates 

    public void UpdatePrimaryWheelMonochromatic(double primaryWheel) 
        => this.Primary.UpdateFromWheel(primaryWheel);

    public void UpdatePrimaryWheelTriad()
    {
        // Assumes that the primary wheel has already been updated and
        // updates the secondaries and eventually the complement
        this.Secondary1.UpdateFromWheel(this.TriadWheel1());
        this.Secondary2.UpdateFromWheel(this.TriadWheel2());
        if (this.Kind.HasComplementary())
        {
            this.Complementary.UpdateFromWheel(this.ComplementaryWheel());
        }
    }

    // Assumes that the primary wheel has already been updated and
    // updates only the complement
    public void UpdatePrimaryWheelComplementary() 
        => this.Complementary.UpdateFromWheel(this.ComplementaryWheel());

    public void UpdatePrimaryWheelSquare(double primaryWheel)
    {
        this.UpdatePrimaryWheelComplementary();
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
        this.UpdatePrimaryWheelTriad();
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
        this.ForAllShades((kind, shades) =>
        {
            shades.Reset();
        });
    }

    public void ApplyShadesPreset(ShadesPreset shadesPreset)
    {
        if (this.AreShadesLocked)
        {
            this.ForAllShades((kind, shades) =>
            {
                shades.ApplyShadesPreset(shadesPreset);
            });
        }
        else
        {
            var selectedWheel = this.SelectedWheel;
            Shades shades = selectedWheel.ToShadesFrom(this);
            shades.ApplyShadesPreset(shadesPreset);
        }
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
        this.ForAllShades((kind,shades) =>
        {
            shades.Dump(kind.ToString());
        });

        var fm = Model.fileManager;
        var preset = new ShadesPreset("Medium", this.Primary);
        string name = "Medium_" + FileManagerModel.TimestampString();
        fm.Save<ShadesPreset>(
            FileManagerModel.Area.User, FileManagerModel.Kind.Json, name, preset);
    }
}
