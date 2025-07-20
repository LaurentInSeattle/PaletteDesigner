namespace Lyt.Avalonia.PaletteDesigner.Model;

using Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

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
            palette.UpdatePrimaryWheelMonochromatic(primaryWheel);


            //var lookup = this.ColorLookupTable;
            //if (lookup is null)
            //{
            //    return false;
            //}

            //var color = palette.Primary.Base.Color;
            //double saturationPrimary = color.S;
            //double brightnessPrimary = color.V;

            //int anglePrimary = (int)Math.Round(primaryWheel * 10.0);
            //double oppposite = (primaryWheel + 180.0).NormalizeAngleDegrees();
            //int angleComplementary = (int)Math.Round(oppposite * 10.0);
            //if (lookup.TryGetValue(anglePrimary, out RgbColor? rgbColorPrimary) &&
            //    lookup.TryGetValue(angleComplementary, out RgbColor? rgbColorComplementary))
            //{
            //    if ((rgbColorPrimary is null) || (rgbColorComplementary is null))
            //    {
            //        return false;
            //    }

            //    var hsvColorPrimary = rgbColorPrimary.ToHsv();
            //    var hsvColorComplementary = rgbColorComplementary.ToHsv();

            //    palette.Primary = p.Primary;
            //    palette.Complementary = p.Complementary;
            //}

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
            palette.UpdateAllPrimaryShadeMonochromatic(pixelX, pixelY);
            return true;
        });

    public void UpdateOnePalettePrimaryShade(ShadeKind shadeKind, int pixelX, int pixelY)
        => this.UpdatePalette((Palette palette) =>
        {
            palette.UpdateOnePrimaryShadeMonochromatic(shadeKind, pixelX, pixelY);
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

    private void UpdateShades(double hue, Shades shades, int X, int Y)
    {
        const double More = 1.25;
        const double Less = 1.0;
        const double brightnessStep = 0.1;
        const double saturationStep = 0.1;
        const int brightnessStepPixel = (int)(brightnessStep * PaletteDesignerModel.ShadesImageDimension);
        const int saturationStepPixel = (int)(saturationStep * PaletteDesignerModel.ShadesImageDimension); ;

        Position Adjust(Position position)
        {
            double half = PaletteDesignerModel.ShadesImageDimension / 2.0;
            double x = (position.X - half) / half;
            double y = (half - position.Y) / half;
            double radius = Math.Min(1.0, Math.Sqrt(x * x + y * y));
            double angle = Math.Atan2(y, x);
            x = radius * Math.Cos(angle);
            y = radius * Math.Sin(angle);
            int newX = (int)(x * half + half);
            int newY = (int)(half - y * half);
            newX = newX.Clip(PaletteDesignerModel.ShadesImageMax);
            newY = newY.Clip(PaletteDesignerModel.ShadesImageMax);
            return new Position { X = newX, Y = newY };
        }

        Shade CreateShade(double saturationFactor, double brightnessFactor)
        {
            var position = 
                new Position(
                    (int)(X + saturationFactor * saturationStepPixel),
                    (int)(Y + brightnessFactor * brightnessStepPixel));
            position = Adjust(position);
            if (this.ShadeColorMap.TryGetValue(position.X, position.Y, out HsvColor? shadeColor) &&
                shadeColor is not null)
            {
                shadeColor.H = hue;
                return new Shade(shadeColor, position);
            }

            throw new Exception("Ouch!");
        }

        shades.Lighter = CreateShade(-More, -More);
        shades.Light = CreateShade(-Less, -Less);
        shades.Dark = CreateShade(Less, Less);
        shades.Darker = CreateShade(More, More);
    }
}
