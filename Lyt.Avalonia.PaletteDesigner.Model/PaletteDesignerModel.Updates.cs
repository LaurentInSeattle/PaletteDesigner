namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool UpdatePaletteKind (PaletteKind paletteKind)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Kind = paletteKind;
            return true;
        }); 

    public void UpdatePalettePrimaryWheel(double wheel)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.PrimaryWheel = wheel;

            var lookup = this.ColorLookupTable;
            if (lookup is null)
            {
                return false;
            }

            double saturationPrimary = palette.Primary.Base.S;
            double brightnessPrimary = palette.Primary.Base.V;

            int anglePrimary = (int)Math.Round(wheel * 10.0);
            double oppposite = (wheel + 180.0).NormalizeAngleDegrees();
            int angleComplementary = (int)Math.Round(oppposite * 10.0);
            if (lookup.TryGetValue(anglePrimary, out RgbColor? rgbColorPrimary) &&
                lookup.TryGetValue(angleComplementary, out RgbColor? rgbColorComplementary))
            {
                if ((rgbColorPrimary is null) || (rgbColorComplementary is null))
                {
                    return false;
                }

                var hsvColorPrimary = rgbColorPrimary.ToHsv();
                var hsvColorComplementary = rgbColorComplementary.ToHsv();

                var p = new Palette(
                    "Temp", PaletteKind.MonochromaticComplementary,
                    hsvColorPrimary.H,
                    hsvColorComplementary.H,
                    saturationPrimary, brightnessPrimary,
                    0.05, 0.30);
                palette.Primary = p.Primary;
                palette.Complementary = p.Complementary;
            }

            return true;
        });

    public void UpdatePalettePrimaryShade(double saturation, double brightness)
        => this.UpdatePalette((Palette palette) =>
        {
            double huePrimary = palette.Primary.Base.H;
            double hueComplementary = palette.Complementary.Base.H;
            var p = new Palette(
                "Temp", PaletteKind.MonochromaticComplementary,
                huePrimary, hueComplementary, 
                saturation, brightness,
                0.05, 0.30);
            palette.Primary = p.Primary;
            palette.Complementary = p.Complementary;
            return true;
        });

    private bool UpdatePalette(Func<Palette,bool> action)
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

        bool result = action(palette); 
        if (result)
        {
            this.Messenger.Publish(new ModelUpdatedMessage());
        } 

        return result;
    }
}
