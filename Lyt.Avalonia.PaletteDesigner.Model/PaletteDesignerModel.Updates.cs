namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool UpdatePaletteKind(PaletteKind paletteKind)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Kind = paletteKind;
            this.UpdatePalettePrimaryWheel(palette.Primary.Wheel);

            return true;
        });

    public void UpdatePaletteShadeMode(ShadeMode shadeMode)
        => this.UpdatePalette((Palette palette) =>
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
        => this.UpdatePalette((Palette palette) =>
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

        bool result = action(palette);
        if (result)
        {
            this.Messenger.Publish(new ModelUpdatedMessage());
        }

        return result;
    }

    [Conditional("DEBUG")]
    public void Dump()
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Dump();
            return true;
        });
}
