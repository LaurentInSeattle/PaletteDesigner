namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    internal Random random = new((int)DateTime.Now.Ticks);

    private bool suspendPaletteUpdates;

    private double RandomizeWheel(Palette palette)
    {
        double wheel = this.random.NextDouble() * 360.0;
        int retries = 666;
        while (retries > 0)
        {
            wheel = this.random.NextDouble() * 360.0;
            if (Math.Abs(wheel - palette.Primary.Wheel) > 10.0 &&
                Math.Abs(wheel - palette.Complementary.Wheel) > 10.0 &&
                Math.Abs(wheel - palette.Secondary1.Wheel) > 10.0 &&
                Math.Abs(wheel - palette.Secondary2.Wheel) > 10.0)
            {
                return wheel;
            } 

            --retries;
            if (retries < 0)
            {
                return wheel;
            }
        }

        return wheel;
    }

    private double RandomizeWheelDistance(Palette palette)
    {
        double distance = this.random.NextDouble() * 360.0;
        int retries = 666;
        while (retries > 0)
        {
            distance = this.random.NextDouble() * 360.0;
            if (Math.Abs(distance - palette.SecondaryWheelDistance) > 10.0 && 
                distance > 23.0 && 
                distance < 180.0 - 23.0 )
            {
                return distance;
            }

            --retries;
            if (retries < 0)
            {
                return distance;
            }
        }

        return distance;
    }

    public void RandomizePalette()
        => this.UpdatePalette(palette =>
        {
            GeneralExtensions.With(ref this.suspendPaletteUpdates, () =>
            {
                try
                {
                    // Randomize a shade preset 
                    var shadesPresets = this.ShadesPresets.Values.ToList();
                    int index = this.random.Next(shadesPresets.Count);
                    ShadesPreset shadesPreset = shadesPresets[index];
                    shadesPreset = ShadesPreset.FromSizeIndependant(shadesPreset);
                    palette.ApplyShadesPreset(shadesPreset);

                    // Randomize a wheel distance for triadic 
                    palette.SecondaryWheelDistance = this.RandomizeWheelDistance(palette);

                    // Randomize all wheels, may throw 
                    double primary = this.RandomizeWheel(palette);
                    this.UpdatePalettePrimaryWheel(primary);
                    double complementary = this.RandomizeWheel(palette);
                    this.UpdatePaletteComplementaryWheel(complementary);
                    double secondary1 = this.RandomizeWheel(palette);
                    this.UpdatePaletteSecondary1Wheel(secondary1);
                    double secondary2 = this.RandomizeWheel(palette);
                    this.UpdatePaletteSecondary2Wheel(secondary2);
                }
                catch 
                {
                    // Swallow silently
                }
            });

            return true;
        });


    public bool UpdatePaletteKind(PaletteKind paletteKind)
        => this.UpdatePalette(palette =>
        {
            palette.Kind = paletteKind;
            this.UpdatePalettePrimaryWheel(palette.Primary.Wheel);

            return true;
        });

    public void UpdatePaletteShadeMode(ShadeMode shadeMode)
        => this.UpdatePalette(palette =>
        {
            palette.AreShadesLocked = shadeMode == ShadeMode.Locked;
            if (palette.AreShadesLocked)
            {
                palette.SelectedWheel = WheelKind.Primary;
            }

            return true;
        });

    // Select the set of shades the user wants to edit 
    public void UpdatePaletteWheelShadeMode(WheelKind wheel)
        => this.UpdatePalette(palette =>
        {
            if (palette.AreShadesLocked)
            {
                return false;
            }

            if (wheel == WheelKind.Unknown)
            {
                return false;
            }

            palette.SelectedWheel = wheel;
            return true;
        });

    private bool UpdatePalette(Func<Palette, bool> action)
    {
        bool result = this.ActionPalette(action);
        if (result && !this.suspendPaletteUpdates)
        {
            new ModelPaletteUpdatedMessage().Publish();
        }

        return result;
    }

    private bool ActionPalette(Func<Palette, bool> action)
    {
        if (this.ActiveProject is null)
        {
            return false;
        }

        var palette = this.ActiveProject.Palette;
        if (palette is null)
        {
            return false;
        }

        if ((Palette.ColorWheel is null) || (Palette.ShadeMap is null))
        {
            throw new Exception("Palette class has not been setup");
        }

        return action(palette);
    }

    [Conditional("DEBUG")]
    public void Dump()
        => this.UpdatePalette(palette =>
        {
            palette.Dump();
            return true;
        });
}
