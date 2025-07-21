namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool UpdatePaletteKind(PaletteKind paletteKind)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Kind = paletteKind;
            return true;
        });

    public void UpdatePalettePrimaryWheel(double primaryWheel)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Missing kind");

                case PaletteKind.Monochromatic:
                    palette.UpdatePrimaryWheelMonochromatic(primaryWheel);
                    break; 

                case PaletteKind.Duochromatic:
                    break;

                case PaletteKind.Trichromatic:
                    break;

                case PaletteKind.Quadrichromatic:
                    break;

                case PaletteKind.MonochromaticComplementary:
                    palette.UpdatePrimaryWheelMonochromaticComplementary(primaryWheel);
                    break;

                case PaletteKind.Triad:
                    break;

                case PaletteKind.TriadComplementary:
                    break;

                case PaletteKind.Square:
                    break;
            }

            return true;
        });

    public void ResetShades()
        => this.UpdatePalette((Palette palette) =>
        {
            palette.ResetShades();
            return true;
        });

    public void UpdateAllPalettePrimaryShade(int pixelX, int pixelY)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Missing kind");

                case PaletteKind.Monochromatic:
                    palette.UpdateAllPrimaryShadeMonochromatic(pixelX, pixelY);
                    break;

                case PaletteKind.Duochromatic:
                    break;

                case PaletteKind.Trichromatic:
                    break;

                case PaletteKind.Quadrichromatic:
                    break;

                case PaletteKind.MonochromaticComplementary:
                    palette.UpdateAllPrimaryShadeMonochromaticComplementary(pixelX, pixelY);
                    break;

                case PaletteKind.Triad:
                    break;

                case PaletteKind.TriadComplementary:
                    break;

                case PaletteKind.Square:
                    break;
            }

            return true;
        });

    public void UpdateOnePalettePrimaryShade(ShadeKind shadeKind, int pixelX, int pixelY)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Missing kind");

                case PaletteKind.Monochromatic:
                    palette.UpdateOnePrimaryShadeMonochromatic(shadeKind, pixelX, pixelY);
                    break;

                case PaletteKind.Duochromatic:
                    break;

                case PaletteKind.Trichromatic:
                    break;

                case PaletteKind.Quadrichromatic:
                    break;

                case PaletteKind.MonochromaticComplementary:
                    palette.UpdateOnePrimaryShadeMonochromaticComplementary(shadeKind, pixelX, pixelY);
                    break;

                case PaletteKind.Triad:
                    break;

                case PaletteKind.TriadComplementary:
                    break;

                case PaletteKind.Square:
                    break;
            }

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

        bool result = action(palette);
        if (result)
        {
            this.Messenger.Publish(new ModelUpdatedMessage());
        }

        return result;
    }
}
