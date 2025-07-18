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
    public static NestedDictionary<int, int, Tuple<int, int>> CreateReverseShadeMap(
        NestedDictionary<int, int, HsvColor> shadeMap)
    {
        int dimension = shadeMap.Capacity;
        NestedDictionary<int, int, Tuple<int, int>> reverseMap = new(dimension, dimension);
        foreach (int row in shadeMap.Keys)
        {
            var cols = shadeMap[row];
            foreach (int col in cols.Keys)
            {
                var hsvColor = cols[col];
                int intSaturation = (int)(hsvColor.S * 100.0);
                int intBrightness = (int)(hsvColor.V * 100.0);
                if (!reverseMap.ContainsKey(intSaturation, intBrightness))
                {
                    reverseMap.Add(intSaturation, intBrightness, new Tuple<int, int>(row, col));
                } 
            }
        }

        return reverseMap;
    }

    public static Tuple<int, int> Lookup(
        this NestedDictionary<int, int, Tuple<int, int>> shadeLocations,
        double saturation, double brightness)
    {
        int intSaturation = (int)(saturation * 100.0);
        int intBrightness = (int)(brightness * 100.0);

        if (shadeLocations.TryGetValue(intSaturation, intBrightness, out Tuple<int, int>? foundLocation) &&
            foundLocation is not null)
        {
            Debug.WriteLine(string.Format(
                "S: {0:F2}  B:{1:F2}  Row: {2}  Col: {3}", 
                saturation, brightness, foundLocation.Item1, foundLocation.Item2));
            return foundLocation;
        }

        double minSquaredDistance = double.MaxValue;
        Tuple<int, int> bestLocation = new(0, 0);
        foreach (int sat in shadeLocations.Keys)
        {
            var brightnesses = shadeLocations[sat];
            foreach (int bri in brightnesses.Keys)
            {
                double deltaSat = sat - saturation * 100.0;
                double deltaBri = bri - brightness * 100.0;
                double squaredDistance = deltaSat * deltaSat + deltaBri * deltaBri;
                if (squaredDistance < minSquaredDistance)
                {
                    bestLocation = brightnesses[bri];
                    minSquaredDistance = squaredDistance;
                    if (minSquaredDistance < 0.01)
                    {
                        Debug.WriteLine(string.Format(
                            "S: {0:F2}  B:{1:F2}  Row: {2}  Col: {3}",
                            saturation, brightness, bestLocation.Item1, bestLocation.Item2));
                        return bestLocation;
                    }
                }
            }
        }

        return bestLocation;
    }
}
