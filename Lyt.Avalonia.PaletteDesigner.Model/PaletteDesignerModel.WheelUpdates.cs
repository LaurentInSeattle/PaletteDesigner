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
            const double baseAngle = +1.0;
            double angle = clockwise ? -baseAngle : +baseAngle;
            double primaryWheel = (palette.Primary.Wheel + angle).NormalizeAngleDegrees();
            double complementaryWheel = (palette.Complementary.Wheel + angle).NormalizeAngleDegrees();
            double secondary1Wheel = (palette.Secondary1.Wheel + angle).NormalizeAngleDegrees();
            double secondary2Wheel = (palette.Secondary2.Wheel + angle).NormalizeAngleDegrees();

            this.AdjustAllWheels(palette, primaryWheel, complementaryWheel, secondary1Wheel, secondary2Wheel);

            return true;
        });

    public void FlipOppositeWheels()
        => this.UpdatePalette((palette) =>
        {
            double angle = 180.0;
            double primaryWheel = (palette.Primary.Wheel + angle).NormalizeAngleDegrees();
            double complementaryWheel = (palette.Complementary.Wheel + angle).NormalizeAngleDegrees();
            double secondary1Wheel = (palette.Secondary1.Wheel + angle).NormalizeAngleDegrees();
            double secondary2Wheel = (palette.Secondary2.Wheel + angle).NormalizeAngleDegrees();

            this.AdjustAllWheels(palette, primaryWheel, complementaryWheel, secondary1Wheel, secondary2Wheel);

            return true;
        });

    private void AdjustAllWheels(Palette palette, double primaryWheel, double complementaryWheel, double secondary1Wheel, double secondary2Wheel)
    {
        // Need to switch by palette kind to prevent exceptions being thrown
        switch (palette.Kind)
        {
            default:
            case PaletteKind.Unknown:
                throw new Exception("Unknown kind");

            case PaletteKind.MonochromaticComplementary:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                break;

            case PaletteKind.Triad:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                break;

            case PaletteKind.TriadComplementary:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                break;

            case PaletteKind.Square:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                this.UpdatePaletteSecondary1Wheel(secondary1Wheel);
                break;

            // Changing freely all wheels 
            case PaletteKind.Monochromatic:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                break;

            case PaletteKind.Duochromatic:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                this.UpdatePaletteComplementaryWheel(complementaryWheel);
                break;

            case PaletteKind.Trichromatic:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                this.UpdatePaletteSecondary1Wheel(secondary1Wheel);
                this.UpdatePaletteSecondary2Wheel(secondary2Wheel);
                break;

            case PaletteKind.Quadrichromatic:
                this.UpdatePalettePrimaryWheel(primaryWheel);
                this.UpdatePaletteComplementaryWheel(complementaryWheel);
                this.UpdatePaletteSecondary1Wheel(secondary1Wheel);
                this.UpdatePaletteSecondary2Wheel(secondary2Wheel);
                break;
        }
    }
}
