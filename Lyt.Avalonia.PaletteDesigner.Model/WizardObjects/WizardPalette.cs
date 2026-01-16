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
    public double WheelAngleDistance { get; set; }

    [JsonRequired]
    public double Highlights { get; set; }

    [JsonRequired]
    public double Shadows { get; set; }

    public HsvColor[] LightColors { get; set; } = new HsvColor[PaletteWidth];

    public HsvColor[] BaseColors { get; set; } = new HsvColor[PaletteWidth];

    public HsvColor[] DarkColors { get; set; } = new HsvColor[PaletteWidth];

    public WizardPalette()
    {
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

    public void Reset ()
    {
        this.SetBaseWheel(90.0);
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
        }
        else
        {
            throw new InvalidOperationException($"HueWheel does not contain angle {Palette.ToAngle(this.BaseWheel)}");
        }
    }
}
