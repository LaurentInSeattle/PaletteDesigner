namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed class WizardPalette
{
    public const int PaletteWidth = 9;

    public const int PaletteCenter = 4;

    public const double DefaultSaturation = 0.75;

    public const double DefaultValue = 0.75;

    [JsonRequired]
    public double BaseWheel { get; set; }

    [JsonRequired]
    public double CurvePower { get; set; }

    [JsonRequired]
    public int CurveAngleStep { get; set; }

    [JsonRequired]
    public double WheelAngleStep { get; set; }

    [JsonRequired]
    public double Highlights { get; set; }

    [JsonRequired]
    public double Shadows { get; set; }

    [JsonIgnore]
    public bool IsReset { get; private set; }

    private Dictionary<int, Vector3> CurveLookup { get; set; } = [];

    [JsonIgnore]
    public HsvColor[] LightColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] BaseColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] DarkColors { get; set; } = new HsvColor[PaletteWidth];

    public WizardPalette()
    {
        this.CurvePower = 2.0;
        this.CurveAngleStep = 8;
        this.WheelAngleStep = 5.0;
    }

    public HsvColor GetColor(SwatchKind swatchKind, int index)
    {
        return swatchKind switch
        {
            SwatchKind.Light => this.LightColors[index],
            SwatchKind.Base => this.BaseColors[index],
            SwatchKind.Dark => this.DarkColors[index],
            _ => throw new InvalidOperationException($"Invalid SwatchKind {swatchKind}"),
        };
    }

    public void Reset()
    {
        this.IsReset = true;

        this.CurvePower = 2.0;
        this.CurveAngleStep = 0;
        this.WheelAngleStep = 0.0;
        this.Highlights = 1.0;
        this.Shadows = 1.0;

        // DEBUG ! 
        this.BaseWheel = 150.0;
        this.CurvePower = 3.5;
        this.CurveAngleStep = 11;
        this.WheelAngleStep = 25.0;
        this.Highlights = 1.8;
        this.Shadows = 1.7;

        this.BuildCurveLookup();
        this.SetHues();

        new ModelWizardUpdatedMessage().Publish();
    }

    public void SetBaseWheel(double baseWheel)
    {
        this.BaseWheel = baseWheel;
        if (Palette.HueWheel.TryGetValue(Palette.ToAngle(this.BaseWheel), out double hue))
        {
            var baseColor = new HsvColor(hue, DefaultSaturation, DefaultValue);
            for (int i = 0; i < PaletteWidth; i++)
            {
                this.LightColors[i] = baseColor;
                this.BaseColors[i] = baseColor;
                this.DarkColors[i] = baseColor;
            }

            this.IsReset = false;
        }
        else
        {
            throw new InvalidOperationException($"HueWheel does not contain angle {Palette.ToAngle(this.BaseWheel)}");
        }
    }

    public void SetWheels()
    {
        int[] angles = this.CalculateAngles();
        if (Palette.HueWheel.TryGetValue(Palette.ToAngle(this.BaseWheel), out double hue))
        {
            for (int i = 0; i < PaletteWidth; i++)
            {
                int angle = angles[i];
                Vector3 vector = this.CurveLookup[angle];
                double saturation = vector.Y;
                double value = vector.X;
                var color = new HsvColor(hue, saturation, value);
                this.LightColors[i] = color;
                this.BaseColors[i] = color;
                this.DarkColors[i] = color;
            }

            this.IsReset = false;
        }
        else
        {
            throw new InvalidOperationException($"HueWheel does not contain angle {Palette.ToAngle(this.BaseWheel)}");
        }
    }

    public void SetHues()
    {
        int[] angles = this.CalculateAngles();
        double[] hues = this.CalculateHues();
        for (int i = 0; i < PaletteWidth; i++)
        {
            int angle = angles[i];
            Vector3 vector = this.CurveLookup[angle];
            double saturation = vector.Y;
            double value = vector.X;
            double wheel = hues[i];
            if (Palette.HueWheel.TryGetValue(Palette.ToAngle(wheel), out double hue))
            {
                var baseColor = new HsvColor(hue, saturation, value);
                this.BaseColors[i] = baseColor;

                // Brighter and less saturated 
                double highlightSaturation = (saturation / this.Highlights).Clip();
                double highlightValue = (value * this.Highlights).Clip();
                var lightColor = new HsvColor(hue, highlightSaturation, highlightValue);
                this.LightColors[i] = lightColor;

                // Darker and more saturated 
                double shadowSaturation = (saturation * this.Shadows).Clip();
                double shadowValue = (value / this.Shadows).Clip();
                var darkColor = new HsvColor(hue, shadowSaturation, shadowValue);
                this.DarkColors[i] = darkColor;
            }
            else
            {
                throw new InvalidOperationException($"HueWheel does not contain angle {Palette.ToAngle(this.BaseWheel)}");
            }

        }

        this.IsReset = false;
    }

    private double[] CalculateHues ()
    {
        double[] hues = new double[PaletteWidth];
        hues[PaletteCenter] = this.BaseWheel;
        for (int i = PaletteCenter + 1; i < PaletteWidth; ++i)
        {
            double hue = hues[i - 1] + this.WheelAngleStep;
            if (hue >= 360.0)
            {
                hues[i] = hue - 360.0;
            }
            else
            {
                hues[i] = hue;
            }
        }

        for (int i = PaletteCenter - 1; i >= 0; --i)
        {
            double hue = hues[i + 1] - this.WheelAngleStep;
            if (hue < 0)
            {
                hues[i] = hue + 360.0;
            }
            else
            {
                hues[i] = hue;
            }
        }

        return hues;
    }

    private int[] CalculateAngles ()
    {
        int[] angles = new int[PaletteWidth];
        angles[PaletteCenter] = 45;
        for (int i = PaletteCenter + 1; i < PaletteWidth; ++i)
        {
            int angle = angles[i - 1] + this.CurveAngleStep;
            if (angle <= 90)
            {
                angles[i] = angle;
            }
            else
            {
                angles[i] = 90;
            }
        }

        for (int i = PaletteCenter - 1; i >= 0; --i)
        {
            int angle = angles[i + 1] - this.CurveAngleStep;
            if (angle >= 0)
            {
                angles[i] = angle;
            }
            else
            {
                angles[i] = 0;
            }
        }

        return angles;
    }

    private void BuildCurveLookup()
    {
        this.CurveLookup.Clear();
        const int lookupSize = 1_001;
        for (int i = 0; i < lookupSize; i++)
        {
            double t = i / (lookupSize - 1.0);
            double curveValue = this.Curve(t);
            double angle = Math.Atan2(curveValue, t);
            double angleDegrees = angle * (180.0 / Math.PI);
            int roundedAngle = (int)Math.Round(angleDegrees);
            double roundingError = Math.Abs(angleDegrees - roundedAngle);
            var vector = new Vector3((float)t, (float)curveValue, (float)roundingError);
            if (this.CurveLookup.TryGetValue(roundedAngle, out Vector3 value))
            {
                if (vector.Z < value.Z)
                {
                    this.CurveLookup[roundedAngle] = vector;
                }
            }
            else
            {
                this.CurveLookup.Add(roundedAngle, vector);
            }
        }
    }

    private double Curve(double t)
    {
        if (t <= 0)
        {
            return 1.0;
        }

        if (t >= 1.0)
        {
            return 0.0;
        }

        if (Math.Abs(this.CurvePower - 1.0) < 0.01)
        {
            return 1.0 - t;
        }
        else if (Math.Abs(this.CurvePower - 2.0) < 0.01)
        {
            return 1.0 - t * t;
        }
        else
        {
            return 1.0 - Math.Pow(t, this.CurvePower);
        }
    }
}
