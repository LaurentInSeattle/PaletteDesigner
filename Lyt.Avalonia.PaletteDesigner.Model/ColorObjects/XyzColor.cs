namespace Lyt.Avalonia.PaletteDesigner.Model.ColorObjects;

public sealed class XyzColor
{
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    // RGB (0-255) to Linear RGB (0-1)
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double SrgbToLinear(double c)
    {
        c /= 255.0;
        return c <= 0.04045 ? c / 12.92 : Math.Pow((c + 0.055) / 1.055, 2.4);
    }

    public XyzColor() { }

    public XyzColor(RgbColor rgbColor) 
    {
        double r = SrgbToLinear(rgbColor.R);
        double g = SrgbToLinear(rgbColor.G);
        double b = SrgbToLinear(rgbColor.B);

        this.X = r * 0.4124 + g * 0.3576 + b * 0.1805;
        this.Y = r * 0.2126 + g * 0.7152 + b * 0.0722;
        this.Z = r * 0.0193 + g * 0.1192 + b * 0.9505;
    }
}
