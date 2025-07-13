namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// See: https://en.wikipedia.org/wiki/HSL_and_HSV 

public sealed class HsvColor
{
    public double H { get; set; } = 0.0;

    public double S { get; set; } = 0.5;

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

    public HsvColor WithH(double h) => new(h, this.S, this.V);

    public HsvColor WithS(double s) => new(this.H, s, this.V);

    public HsvColor WithV(double v) => new(this.H, this.S, v);

    /// <summary> 
    /// Returns a new color based of this color appying the provided brightness factor on the V component. 
    /// </summary>
    public HsvColor Intensify(double factor)
    {
        double value = this.V * factor;
        return this.WithV(value.Clip());
    }

    /// <summary> 
    /// Returns a new color based of this color appying the provided saturation factor on the S component. 
    /// </summary>
    public HsvColor Saturate(double factor)
    {
        double saturation = this.S * factor;
        return this.WithS(saturation.Clip());
    }

    /// <summary> Returns the complementary color of this color  </summary>
    public HsvColor Complementary()
    {
        double complementaryHue = (this.HueToWheel() + 180.0) % 360.0;
        return this.WithH(complementaryHue);
    }

    /// <summary> Returns the colors adjacent to this color at the provided angular distance </summary>
    public Tuple<HsvColor, HsvColor> Triad(double angularDistance)
    {
        double secondaryHuePlus = (this.HueToWheel() + angularDistance) % 360.0;
        double secondaryHueMinus = (this.HueToWheel() - angularDistance) % 360.0;
        return new(this.WithH(secondaryHuePlus), this.WithH(secondaryHueMinus));
    }

    /// <summary> 
    /// Returns the color adjacent to this color at the provided angular distance along 
    /// with its complementary color.
    /// </summary>
    public Tuple<HsvColor, HsvColor> Squared(double angularDistance)
    {
        double secondaryHue = (this.HueToWheel() + angularDistance) % 360.0;
        double secondaryHueComplementary = (this.HueToWheel() + angularDistance + 180.0) % 360.0;
        return new(this.WithH(secondaryHue), this.WithH(secondaryHueComplementary));
    }

    public RgbColor ToRgb() => HsvColor.ToRgb(this.H, this.S, this.V);

    public double HueToWheel() => this.H.HueToWheel();

    public static RgbColor ToRgb(double h, double s, double v)
    {
        if (s == 0)
        {
            double x = Math.Round(v * 2.55);
            return new RgbColor(x, x, x);
        }

        s /= 100;
        v /= 100;
        h /= 60;

        double i = Math.Floor(h);
        double f = h - i;
        double p = v * (1 - s);
        double q = v * (1 - s * f);
        double t = v * (1 - s * (1 - f));
        double r;
        double g;
        double b;
        switch ((int)i)
        {
            case 0:
                r = v;
                g = t;
                b = p;
                break;

            case 1:
                r = q;
                g = v;
                b = p;
                break;

            case 2:
                r = p;
                g = v;
                b = t;
                break;

            case 3:
                r = p;
                g = q;
                b = v;
                break;

            case 4:
                r = t;
                g = p;
                b = v;
                break;

            default:
                r = v;
                g = p;
                b = q;
                break;
        }

        return new RgbColor(Math.Round(r * 255.0), Math.Round(g * 255.0), Math.Round(b * 255.0));
    }
}
