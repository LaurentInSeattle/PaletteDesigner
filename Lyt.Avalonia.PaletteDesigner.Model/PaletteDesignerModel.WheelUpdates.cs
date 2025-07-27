namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public void OnWheelAngleChanged(WheelKind wheelKind, double wheel)
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
        => this.UpdatePalette((palette) =>
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
                    palette.UpdateShadesWheel(palette.Secondary1, wheel);
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
        => this.UpdatePalette((palette) =>
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
                    palette.UpdateShadesWheel(palette.Secondary2, wheel);
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
        => this.UpdatePalette((palette) =>
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
        => this.UpdatePalette((palette) =>
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

    public void RotateAllWheels(bool clockwise)
        => this.UpdatePalette((palette) =>
        {
            // TODO : Need to switch by palette kind

            const double baseAngle = +1.0;
            double angle = clockwise ? -baseAngle : +baseAngle;
            double wheel = (palette.Primary.Wheel + angle).NormalizeAngleDegrees();
            this.UpdatePalettePrimaryWheel(wheel);
            try
            {
                wheel = (palette.Complementary.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteComplementaryWheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            try
            {
                wheel = (palette.Secondary1.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteSecondary1Wheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            try
            {
                wheel = (palette.Secondary2.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteSecondary2Wheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            return true;
        });

    public void FlipOppositeWheels()
        => this.UpdatePalette((palette) =>
        {
            double angle = 180.0;
            double wheel = (palette.Primary.Wheel + angle).NormalizeAngleDegrees();
            this.UpdatePalettePrimaryWheel(wheel);
            try
            {
                wheel = (palette.Complementary.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteComplementaryWheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            try
            {
                wheel = (palette.Secondary1.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteSecondary1Wheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            try
            {
                wheel = (palette.Secondary2.Wheel + angle).NormalizeAngleDegrees();
                this.UpdatePaletteSecondary2Wheel(wheel);
            }
            catch { /* swallow as we may not be able to rotate */ }

            return true;
        });
}
