namespace Lyt.Avalonia.PaletteDesigner.Model.ColorObjects;

public sealed class LabColor
{
    public double L { get; set; }

    public double A { get; set; }

    public double B { get; set; }

    public LabColor() { }

    public LabColor(double l, double a, double b)
    {
        this.L = l;
        this.A = a;
        this.B = b;
    }

    public LabColor(XyzColor xyzColor)
    {
        // Normalize by D65 reference white
        double x = xyzColor.X / 0.95047;
        double y = xyzColor.Y / 1.00000;
        double z = xyzColor.Z / 1.08883;

        x = Pivot(x);
        y = Pivot(y);
        z = Pivot(z);

        this.L = 116.0 * y - 16.0;
        this.A = 500.0 * (x - y);
        this.B = 200.0 * (y - z);
    }

    public LabColor(RgbColor rgbColor)
    {
        double r = XyzColor.SrgbToLinear(rgbColor.R);
        double g = XyzColor.SrgbToLinear(rgbColor.G);
        double b = XyzColor.SrgbToLinear(rgbColor.B);

        double xyzX = r * 0.4124 + g * 0.3576 + b * 0.1805;
        double xyzY = r * 0.2126 + g * 0.7152 + b * 0.0722;
        double xyzZ = r * 0.0193 + g * 0.1192 + b * 0.9505;

        // Normalize by D65 reference white
        double x = xyzX / 0.95047;
        double y = xyzY / 1.00000;
        double z = xyzZ / 1.08883;

        x = Pivot(x);
        y = Pivot(y);
        z = Pivot(z);

        this.L = 116.0 * y - 16.0;
        this.A = 500.0 * (x - y);
        this.B = 200.0 * (y - z);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double Pivot(double n) =>
        n > 0.008856 ? Math.Pow(n, 1.0 / 3.0) : (7.787 * n) + (16.0 / 116.0);

    public static double Distance(LabColor lab1, LabColor lab2)
    {
        double deltaL = lab1.L - lab2.L;
        double deltaA = lab1.A - lab2.A;
        double deltaB = lab1.B - lab2.B;
        return Math.Sqrt(deltaL * deltaL + deltaA * deltaA + deltaB * deltaB);
    }

    public RgbColor ToRgb()
    {
        // Convert LAB to XYZ
        double y = (this.L + 16.0) / 116.0;
        double x = this.A / 500.0 + y;
        double z = y - this.B / 200.0;

        double x3 = Math.Pow(x, 3);
        double y3 = Math.Pow(y, 3);
        double z3 = Math.Pow(z, 3);

        x = 0.95047 * (x3 > 0.008856 ? x3 : (x - 16.0 / 116.0) / 7.787);
        y = 1.00000 * (y3 > 0.008856 ? y3 : (y - 16.0 / 116.0) / 7.787);
        z = 1.08883 * (z3 > 0.008856 ? z3 : (z - 16.0 / 116.0) / 7.787);

        // Convert XYZ to linear RGB
        double r = x * 3.2406 + y * -1.5372 + z * -0.4986;
        double g = x * -0.9689 + y * 1.8758 + z * 0.0415;
        double bC = x * 0.0557 + y * -0.2040 + z * 1.0570;

        // Apply gamma correction
        r = r > 0.0031308 ? 1.055 * Math.Pow(r, 1.0 / 2.4) - 0.055 : 12.92 * r;
        g = g > 0.0031308 ? 1.055 * Math.Pow(g, 1.0 / 2.4) - 0.055 : 12.92 * g;
        bC = bC > 0.0031308 ? 1.055 * Math.Pow(bC, 1.0 / 2.4) - 0.055 : 12.92 * bC;

        // Clamp to [0, 255]
        byte r8 = (byte)Math.Clamp(r * 255, 0, 255);
        byte g8 = (byte)Math.Clamp(g * 255, 0, 255);
        byte b8 = (byte)Math.Clamp(bC * 255, 0, 255);
        return new RgbColor(r8, g8, b8);
    }
}
