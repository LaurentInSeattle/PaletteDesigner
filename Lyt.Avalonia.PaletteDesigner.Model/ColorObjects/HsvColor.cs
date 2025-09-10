namespace Lyt.Avalonia.PaletteDesigner.Model.ColorObjects;

public sealed class HsvColor
{
    /// <summary> Hue Angle in degrees </summary>
    [JsonRequired]
    public double H { get; set; } = 0.0;

    [JsonRequired]
    public double S { get; set; } = 0.5;

    [JsonRequired]
    public double V { get; set; } = 0.5;

    public HsvColor() { }

    public HsvColor(double h, double s, double v)
    {
        this.H = h;
        this.S = s;
        this.V = v;
    }

    public HsvColor(HsvColor hsv)
    {
        this.H = hsv.H;
        this.S = hsv.S;
        this.V = hsv.V;
    }

    public HsvColor(RgbColor rgb)
    {
        HsvColor hsv = rgb.ToHsv();
        this.H = hsv.H;
        this.S = hsv.S;
        this.V = hsv.V;
    }

    public HsvColor(uint bgra)
    {
        byte b = (byte)((bgra & 0xFF000000) >> 24);
        byte g = (byte)((bgra & 0x00FF0000) >> 16);
        byte r = (byte) ((bgra & 0x0000FF00) >> 8);
        var rgb = new RgbColor(r, g, b);
        HsvColor hsv = rgb.ToHsv();
        this.H = hsv.H;
        this.S = hsv.S;
        this.V = hsv.V;
    }

    public void Set (double h, double s, double v)
    {
        this.H = h;
        this.S = s;
        this.V = v;
    }

    public RgbColor ToRgb() => HsvColor.ToRgb(this.H, this.S, this.V);

    public static RgbColor ToRgb(double hue, double saturation, double brightness)
    {
        ToRgb(hue, saturation, brightness, out byte red, out byte green, out byte blue);
        return new RgbColor(red, green,blue);
    }

    public static void ToRgb(
        double hue, double saturation, double brightness,
        out byte red, out byte green, out byte blue)
    {
        if (saturation == 0)
        {
            // No saturation: gray 
            double x = Math.Round(brightness * 255.0);
            red = (byte)x; 
            green = (byte)x; 
            blue = (byte)x;
            return ;
        }

        // the color wheel consists of 6 sectors. Figure out which sector you're in.
        double sectorPosition = hue / 60.0;
        int sectorIndex = (int)(Math.Floor(sectorPosition));

        // get the fractional part of the sector
        double fractionalSector = sectorPosition - sectorIndex;

        // calculate values for the three axes of the color. 
        double p = brightness * (1.0 - saturation);
        double q = brightness * (1.0 - (saturation * fractionalSector));
        double t = brightness * (1.0 - (saturation * (1 - fractionalSector)));

        double r;
        double g;
        double b;
        switch (sectorIndex )
        {
            case 0:
                r = brightness;
                g = t;
                b = p;
                break;

            case 1:
                r = q;
                g = brightness;
                b = p;
                break;

            case 2:
                r = p;
                g = brightness;
                b = t;
                break;

            case 3:
                r = p;
                g = q;
                b = brightness;
                break;

            case 4:
                r = t;
                g = p;
                b = brightness;
                break;

            default:
                r = brightness;
                g = p;
                b = q;
                break;
        }

        red = (byte)Math.Round(r * 255.0);
        green = (byte)Math.Round(g * 255.0);
        blue = (byte)Math.Round(b * 255.0);
    }

    public override string ToString() 
        => string.Format ("Hue: {0:F1}  Sat: {1:F1}  Bri: {2:F1}", this.H, this.S, this.V);
}
