namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class RgbColor
{
    public double R { get; set; } = 128.0;

    public double G { get; set; } = 0.0;

    public double B { get; set; } = 0.0;

    public RgbColor() { }

    public RgbColor(double r, double g, double b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

    public RgbColor(RgbColor rgb)
    {
        this.R = rgb.R;
        this.G = rgb.G;
        this.B = rgb.B;
    }

    public RgbColor( HsvColor hsv )
    {
        RgbColor rgb = hsv.ToRgb();
        this.R = rgb.R;
        this.G = rgb.G;
        this.B = rgb.B;
    }

    public RgbColor WithR(double r) => new(r, this.G, this.B);

    public RgbColor WithG(double g) => new(this.R, g, this.B);

    public RgbColor WithB(double b) => new(this.R, this.G, b);

    public HsvColor ToHsv() => RgbColor.ToHsv(this.R, this.G, this.B);

    public static HsvColor ToHsv(double r, double g, double b)
    {
        double m = r;
        if (g < m)
        {
            m = g;
        }

        if (b < m)
        {
            m = b;
        }

        double v = r;
        if (g > v)
        {
            v = g;
        }

        if (b > v)
        {
            v = b;
        }

        double value = 100.0 * v / 255.0;
        double delta = v - m;
        double s; 
        if (v == 0.0)
        {
            s = 0.0;
        }
        else
        {
            s = 100 * delta / v;
        }

        double h = 0.0;
        if (s == 0.0)
        {
            h = 0;
        }
        else
        {
            if (r == v)
            {
                h = 60.0 * (g - b) / delta;
            }
            else if (g == v)
            {
                h = 120.0 + 60.0 * (b - r) / delta;
            }
            else if (b == v)
            {
                h = 240.0 + 60.0 * (r - g) / delta;
            }

            if (h < 0.0)
            {
                h += 360.0;
            }
        }

        return new HsvColor(Math.Round(h), Math.Round(s), Math.Round(value));
    }

    // DOES NOT Include pound sign: FF030014
    public string ToArgbHexString()
        => string.Format(
            "FF{0:X2}{1:X2}{2:X2}",
            (byte)Math.Round(this.R), (byte)Math.Round(this.G), (byte)Math.Round(this.B));

    // Include pound sign: #FF030014
    public string ToPoundArgbHexString()
        => string.Format(
            "#FF{0:X2}{1:X2}{2:X2}",
            (byte)Math.Round(this.R), (byte) Math.Round(this.G), (byte) Math.Round(this.B));
}
