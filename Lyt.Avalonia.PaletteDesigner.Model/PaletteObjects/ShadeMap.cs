namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class ShadeMap : NestedDictionary<int, int, HsvColor>
{
    private const double SqrtOfTwo = 1.414_213_6;
    private const double SqrtOfTwoByTwo = SqrtOfTwo / 2.0;
    private const double BaseBrightness = 0.0;

    public ShadeMap(int dimension) : base(dimension, dimension)
    {
        double step = 1.0 / dimension;
        double half = dimension / 2.0;
        for (int row = 0; row < dimension; ++row)
        {
            for (int col = 0; col < dimension; ++col)
            {
                // Recenter and normalize to [-1, +1] 
                double x = (col - half) / half;
                double y = (half - row) / half;
                double radius = Math.Sqrt(x * x + y * y);

                // No need to do that because the image is clipped (RoundedImage control) 
                //if (radius >= 1.0)
                //{
                //    // Negative hue to mean transparent, and done for this spot
                //    map.Add(row, col, new HsvColor(-1.0, 0.0, 0.0));
                //    continue;
                //}

                double saturation = col * step * SqrtOfTwo;
                saturation = MathExtensions.Clip(saturation);
                double brightness = (1.0 - row * step) * SqrtOfTwo;
                brightness = BaseBrightness + brightness;
                brightness = MathExtensions.Clip(brightness);
                this.Add(row, col, new HsvColor(1.0, saturation, brightness));
            }
        }
    }

    public bool TryGetValue(Position position, out SvShade? svShade)
    {
        svShade = null;
        if (this.TryGetValue(position.Y, position.X, out HsvColor? hsvColor))
        {
            if (hsvColor is not null)
            {
                svShade = new SvShade(hsvColor.S, hsvColor.V);
                return true;
            }
        }

        return false;
    }
}
