namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public static class ShadeMap
{
    private const double SqrtOfTwo = 1.414_213_6;
    private const double SqrtOfTwoByTwo = SqrtOfTwo / 2.0;
    private const double BaseBrightness = 0.0;

    public static NestedDictionary<int, int, HsvColor> Create(int dimension)
    {
        NestedDictionary<int, int, HsvColor> map = new(dimension, dimension);

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
                map.Add(row, col, new HsvColor(1.0, saturation, brightness));
            }
        }

        return map;
    }

    // Keys: 100 times saturation and brightness 
    // Value: Tuples of 'best' row / col 
    public static NestedDictionary<int, int, Tuple<int,int>> CreateReverseShadeMap(
        NestedDictionary<int, int, HsvColor> shadeMap)
    {
        int dimension = shadeMap.Capacity;
        NestedDictionary<int, int, Tuple<int, int>> map = new(dimension, dimension);

        // TODO 
        return map;
    }
}
