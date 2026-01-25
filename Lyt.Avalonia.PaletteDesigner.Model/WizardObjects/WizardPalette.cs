namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed partial class WizardPalette : IExportAble
{
    public const int PaletteWidth = 9;

    public const int PaletteCenter = 4;

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

    [JsonRequired]
    public int ThemeVariantStyleIndex { get; set; }

    [JsonRequired]
    public ThemeVariantColors LightVariant { get; set; }
        = new ThemeVariantColors() { ThemeVariant = PaletteThemeVariant.Light };

    [JsonRequired]
    public ThemeVariantColors DarkVariant { get; set; }
        = new ThemeVariantColors() { ThemeVariant = PaletteThemeVariant.Dark };

    [JsonIgnore]
    public bool IsReset { get; private set; }

    private Dictionary<int, Vector3> CurveLookup { get; set; } = [];

    [JsonIgnore]
    public HsvColor[] LighterColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] LightColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] BaseColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] DarkColors { get; set; } = new HsvColor[PaletteWidth];

    [JsonIgnore]
    public HsvColor[] DarkerColors { get; set; } = new HsvColor[PaletteWidth];

    public WizardPalette() { }

    public HsvColor GetColor(SwatchIndex swatchIndex)
    {
        SwatchKind swatchKind = swatchIndex.Kind;
        int index = swatchIndex.Index;
        return swatchKind switch
        {
            SwatchKind.Lighter => this.LighterColors[index],
            SwatchKind.Light => this.LightColors[index],
            SwatchKind.Base => this.BaseColors[index],
            SwatchKind.Dark => this.DarkColors[index],
            SwatchKind.Darker => this.DarkerColors[index],
            _ => throw new InvalidOperationException($"Invalid SwatchKind {swatchKind}"),
        };
    }

    public HsvColor[] GetThemeColors(PaletteThemeVariant themeVariant)
    {
        var hsvColors = new HsvColor[4];
        ThemeVariantColors variant =
            themeVariant == PaletteThemeVariant.Light ? this.LightVariant : this.DarkVariant;
        hsvColors[0] = this.GetColor(variant.Background);
        hsvColors[1] = this.GetColor(variant.Foreground);
        hsvColors[2] = this.GetColor(variant.Accent);
        hsvColors[3] = this.GetColor(variant.Discordant);
        return hsvColors;
    }

    public HsvColor GetThemeComponentColor(PaletteThemeVariant themeVariant, ThemeComponent themeComponent)
    {
        ThemeVariantColors variant =
            themeVariant == PaletteThemeVariant.Light ? this.LightVariant : this.DarkVariant;
        return themeComponent switch
        {
            ThemeComponent.Background => this.GetColor(variant.Background),
            ThemeComponent.Foreground => this.GetColor(variant.Foreground),
            ThemeComponent.Accent => this.GetColor(variant.Accent),
            ThemeComponent.Discordant => this.GetColor(variant.Discordant),
            _ => throw new InvalidOperationException($"Invalid ThemeComponent {themeComponent}"),
        };
    }

    public void SetThemeComponentColor(
        PaletteThemeVariant themeVariant, ThemeComponent themeComponent, SwatchIndex swatchIndex)
    {
        ThemeVariantColors variant =
            themeVariant == PaletteThemeVariant.Light ? this.LightVariant : this.DarkVariant;
        switch (themeComponent)
        {
            case ThemeComponent.Background:
                variant.Background = swatchIndex;
                break;
            case ThemeComponent.Foreground:
                variant.Foreground = swatchIndex;
                break;
            case ThemeComponent.Accent:
                variant.Accent = swatchIndex;
                break;
            case ThemeComponent.Discordant:
                variant.Discordant = swatchIndex;
                break;
            default:
                throw new InvalidOperationException($"Invalid ThemeComponent {themeComponent}");
        }

        new ModelWizardUpdatedMessage().Publish();
    }

    internal void Reset()
    {
        this.IsReset = true;

        this.BaseWheel = 150.0;
        this.CurvePower = 3.0;
        this.CurveAngleStep = 6;
        this.WheelAngleStep = 12.0;
        this.Highlights = 2.15;
        this.Shadows = 1.4;
        this.ThemeVariantStyleIndex = 2;
        this.UpdateThemeVariants();
        this.BuildCurveLookup();
        this.Update();
    }

    private void UpdateThemeVariants()
    {
        var preset = ThemePresets.All[this.ThemeVariantStyleIndex];
        this.LightVariant = preset.Light;
        this.DarkVariant = preset.Dark;
    }

    internal void SetWheel(double baseWheel)
    {
        this.BaseWheel = baseWheel;
        this.Update();
    }

    internal void SetCurvePower(double value)
    {
        this.CurvePower = value;
        this.BuildCurveLookup();
        this.Update();
    }

    internal void SetCurveAngleStep(int value)
    {
        this.CurveAngleStep = value;
        this.BuildCurveLookup();
        this.Update();
    }

    internal void SetWheelAngleStep(double value)
    {
        this.WheelAngleStep = value;
        this.Update();
    }

    internal void SetHighlights(double value)
    {
        this.Highlights = value;
        this.Update();
    }

    internal void SetShadows(double value)
    {
        this.Shadows = value;
        this.Update();
    }

    internal void SetStyle(int value)
    {
        this.ThemeVariantStyleIndex = value;
        this.UpdateThemeVariants();
        new ModelWizardUpdatedMessage().Publish();
    }

    private void Update()
    {
        int[] angles = this.CalculateAngles();
        int[] wheels = this.CalculateWheels();
        for (int i = 0; i < PaletteWidth; i++)
        {
            int angle = angles[i];
            Vector3 vector = this.CurveLookup[angle];
            double saturation = vector.Y;
            double value = vector.X;
            int wheelAngle = wheels[i];
            if (Palette.HueWheel.TryGetValue(wheelAngle, out double hue))
            {
                var baseColor = new HsvColor(hue, saturation, value);
                this.BaseColors[i] = baseColor;

                HsvColor Enlighten (double light)
                {
                    double highlightSaturation = (saturation / light).Clip();
                    double highlightValue = (value * light).Clip();
                    return new HsvColor(hue, highlightSaturation, highlightValue);
                }

                HsvColor Darken(double dark)
                {
                    double shadowSaturation = (saturation * dark).Clip();
                    double shadowValue = (value / dark).Clip();
                    return new HsvColor(hue, shadowSaturation, shadowValue);
                }

                // Bright and less saturated 
                this.LightColors[i] = Enlighten (this.Highlights * 0.80) ;

                // Brighter and even less saturated 
                this.LighterColors[i] = Enlighten(this.Highlights * 1.20);

                // Dark and more saturated 
                this.DarkColors[i] = Darken(this.Shadows * 0.80);

                // Darker and even more saturated 
                this.DarkerColors[i] = Darken(this.Shadows * 1.20);
            }
            else
            {
                throw new InvalidOperationException($"HueWheel does not contain angle {wheelAngle}");
            }

        }

        this.IsReset = false;
        new ModelWizardUpdatedMessage().Publish();
    }

    private int[] CalculateWheels()
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

        int[] wheels = new int[9];
        for (int i = 0; i < PaletteWidth; ++i)
        {
            int wheelAngle = Palette.ToAngle(hues[i]);
            if (wheelAngle < 0 || wheelAngle >= 3600)
            {
                wheelAngle = 0;
            }

            wheels[i] = wheelAngle;
        }

        return wheels;
    }

    private int[] CalculateAngles()
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
