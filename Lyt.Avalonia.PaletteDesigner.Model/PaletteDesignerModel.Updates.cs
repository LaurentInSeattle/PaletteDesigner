namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool UpdatePaletteKind(PaletteKind paletteKind)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Kind = paletteKind;
            this.UpdatePalettePrimaryWheel(palette.Primary.Wheel);
            //if (paletteKind == PaletteKind.Triad ||
            //    paletteKind == PaletteKind.TriadComplementary)
            //{
            //    palette.UpdatePrimaryWheelTriad(palette.Primary.Wheel);
            //}

            //if (paletteKind.HasComplementary())
            //{
            //    palette.UpdatePrimaryWheelComplementary(palette.Primary.Wheel);
            //}

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

    public void UpdatePaletteWheel(WheelKind wheelKind, double wheel)
    {
        switch (wheelKind)
        {
            default:
            case WheelKind.Unknown:
                throw new Exception("Invalid kind");

            case WheelKind.Primary:
                this.UpdatePalettePrimaryWheel(wheel);
                break;

            case WheelKind.Complementary:
                this.UpdatePaletteComplementaryWheel(wheel);
                break;

            case WheelKind.Secondary1:
                this.UpdatePaletteSecondary1Wheel(wheel);
                break;

            case WheelKind.Secondary2:
                this.UpdatePaletteSecondary2Wheel(wheel);
                break;
        }
    }

    public void UpdatePaletteSecondary1Wheel(double wheel)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Missing kind");

                case PaletteKind.Monochromatic:
                case PaletteKind.MonochromaticComplementary:
                case PaletteKind.Duochromatic:
                    throw new Exception("Invalid palette kind");

                case PaletteKind.Trichromatic:
                case PaletteKind.Quadrichromatic:
                    // TODO 
                    break;

                case PaletteKind.Triad:
                case PaletteKind.TriadComplementary:
                    palette.UpdateSecondaryWheelTriad(wheel);
                    break;

                case PaletteKind.Square:
                    palette.UpdateSecondaryWheelSquare(wheel);
                    break;
            }

            return true;
        });

    public void UpdatePaletteSecondary2Wheel(double wheel)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Missing kind");

                case PaletteKind.Monochromatic:
                case PaletteKind.MonochromaticComplementary:
                case PaletteKind.Duochromatic:
                    throw new Exception("Invalid palette kind");

                case PaletteKind.Trichromatic:
                case PaletteKind.Quadrichromatic:
                    // TODO 
                    break;

                case PaletteKind.Triad:
                case PaletteKind.TriadComplementary:
                    palette.UpdateSecondaryWheelTriad(wheel);
                    break;

                case PaletteKind.Square:
                    palette.UpdateSecondaryWheelSquare(wheel);
                    break;
            }

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
                case PaletteKind.Duochromatic:
                case PaletteKind.Trichromatic:
                case PaletteKind.Quadrichromatic:
                    palette.UpdatePrimaryWheelMonochromatic(primaryWheel);
                    break;

                case PaletteKind.MonochromaticComplementary:
                    palette.UpdatePrimaryWheelMonochromatic(primaryWheel);
                    palette.UpdatePrimaryWheelComplementary(primaryWheel);
                    break;

                case PaletteKind.Triad:
                case PaletteKind.TriadComplementary:
                    palette.UpdatePrimaryWheelMonochromatic(primaryWheel);
                    palette.UpdatePrimaryWheelTriad(primaryWheel);
                    break;

                case PaletteKind.Square:
                    palette.UpdatePrimaryWheelMonochromatic(primaryWheel);
                    palette.UpdatePrimaryWheelSquare(primaryWheel);
                    break;
            }

            return true;
        });

    public void UpdatePaletteComplementaryWheel(double complementaryWheel)
        => this.UpdatePalette((Palette palette) =>
        {
            switch (palette.Kind)
            {
                default:
                case PaletteKind.Unknown:
                    throw new Exception("Unknown kind");

                // No complementary 
                case PaletteKind.Monochromatic:
                case PaletteKind.Triad:

                // Complementary is locked 
                case PaletteKind.MonochromaticComplementary:
                case PaletteKind.TriadComplementary:
                case PaletteKind.Square:
                    throw new Exception("Invalid kind");

                // Changing Freely the complementary 
                case PaletteKind.Duochromatic:
                case PaletteKind.Trichromatic:
                case PaletteKind.Quadrichromatic:
                    palette.UpdateShadesWheel(palette.Complementary, complementaryWheel);
                    break;
            }

            return true;
        });

    public void Dump()
        => this.UpdatePalette((Palette palette) =>
        {
            palette.Dump();
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

        if (( Palette.ColorWheel is null ) || (Palette.ShadeMap is null))
        {
            throw new Exception("Palette has not been setup");
        }

        bool result = action(palette);
        if (result)
        {
            this.Messenger.Publish(new ModelUpdatedMessage());
        }

        return result;
    }
}
