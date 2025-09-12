namespace Lyt.Avalonia.PaletteDesigner.Model.ColorObjects;

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

    public RgbColor(byte r, byte g, byte b)
    {
        this.R = r;
        this.G = g;
        this.B = b;
    }

    public RgbColor(uint rgb)
    {
        this.R = (rgb & 0xFF0000) >> 16;
        this.G = (rgb & 0x00FF00) >> 8;
        this.B = (rgb & 0x0000FF);
    }

    public RgbColor(RgbColor rgb)
    {
        this.R = rgb.R;
        this.G = rgb.G;
        this.B = rgb.B;
    }

    public RgbColor(HsvColor hsv)
    {
        RgbColor rgb = hsv.ToRgb();
        this.R = rgb.R;
        this.G = rgb.G;
        this.B = rgb.B;
    }

    public RgbColor(LabColor lab)
    {
        RgbColor rgb = lab.ToRgb();
        this.R = rgb.R;
        this.G = rgb.G;
        this.B = rgb.B;
    }

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

        return new HsvColor(h, s / 100.0, value / 100.0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte ToHsvV(byte r, byte g, byte b)
    {
        byte v = r;
        if (g > v)
        {
            v = g;
        }

        if (b > v)
        {
            v = b;
        }

        return v ;        
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte ToGrayScale(uint argb)
    {
        byte blue = (byte)(argb & 0x0FF);
        byte green = (byte)((argb & 0x0FF00) >> 8);
        byte red = (byte)((argb & 0x0FF0000) >> 16);
        float grayscale = (0.299f * red) + (0.587f * green) + (0.114f * blue);
        return (byte)grayscale;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ToBgraUint()
        =>  0x00_00_00_FF |
            (uint)Math.Round(this.B) << 24 |
            (uint)Math.Round(this.G) << 16 |
            (uint)Math.Round(this.R) << 8 ;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ToArgbUint()
        =>  0xFF_00_00_00 | 
            (uint)Math.Round(this.R) << 16 | 
            (uint)Math.Round(this.G) << 8 | 
            (uint)Math.Round(this.B);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public uint ToRgbUint()
        => (uint)Math.Round(this.R) << 16 | (uint)Math.Round(this.G) << 8 | (uint)Math.Round(this.B);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToRgbPercentString()
        => string.Format(
            "{0,3:D} {1,3:D} {2,3:D}",
            (int)Math.Round(100.0 * this.R / 255.0),
            (int)Math.Round(100.0 * this.G / 255.0),
            (int)Math.Round(100.0 * this.B / 255.0));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToRgbDecString()
        => string.Format(
            "{0,3:D} {1,3:D} {2,3:D}",
            (byte)Math.Round(this.R), (byte)Math.Round(this.G), (byte)Math.Round(this.B));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToRgbHexString()
        => string.Format(
            "{0:X2} {1:X2} {2:X2}",
            (int)Math.Round(this.R), (int)Math.Round(this.G), (int)Math.Round(this.B));

    // DOES NOT Include pound sign: FF030014
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToArgbHexString()
        => string.Format(
            "FF{0:X2}{1:X2}{2:X2}",
            (byte)Math.Round(this.R), (byte)Math.Round(this.G), (byte)Math.Round(this.B));

    // Include pound sign: #FF030014
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToPoundArgbHexString()
        => string.Format(
            "#FF{0:X2}{1:X2}{2:X2}",
            (byte)Math.Round(this.R), (byte)Math.Round(this.G), (byte)Math.Round(this.B));
}
