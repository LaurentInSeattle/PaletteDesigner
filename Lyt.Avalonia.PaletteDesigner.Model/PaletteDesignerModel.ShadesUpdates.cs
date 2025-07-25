namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public void ResetShades()
        => this.UpdatePalette((Palette palette) =>
        {
            palette.ResetAllShades();
            return true;
        });

    public void UpdateAllPalettePrimaryShade(int pixelX, int pixelY)
        => this.UpdatePalette((Palette palette) =>
        {
            if (palette.AreShadesLocked)
            {
                palette.UpdateAllShades(pixelX, pixelY);
            }
            else
            {
                switch (palette.Kind)
                {
                    default:
                    case PaletteKind.Unknown:
                        throw new Exception("Missing kind");

                    case PaletteKind.Monochromatic:
                    case PaletteKind.Duochromatic:
                    case PaletteKind.Trichromatic:
                    case PaletteKind.Quadrichromatic:
                        palette.UpdateAllPrimaryShadeMonochromatic(pixelX, pixelY);
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
                case PaletteKind.Duochromatic:
                case PaletteKind.Trichromatic:
                case PaletteKind.Quadrichromatic:
                    palette.UpdateOnePrimaryShadeMonochromatic(shadeKind, pixelX, pixelY);
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
}
