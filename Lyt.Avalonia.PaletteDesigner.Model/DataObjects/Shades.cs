namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public enum ShadeMode
{
    Locked,
    Unlocked,
}

public enum Shade
{
    None,

    Lighter,
    Light,
    Base,
    Dark,
    Darker,
}

public sealed class Shades
{
    public HsvColor Lighter { get; set; } = new();
    public HsvColor Light { get; set; } = new();
    public HsvColor Base { get; set; } = new();
    public HsvColor Dark { get; set; } = new();
    public HsvColor Darker { get; set; } = new();

    public Shades() { /* needed for serialization */ }

    public Shades(HsvColor baseColor, double saturationFactor, double brightnessFactor)
    {
        this.Base = baseColor;
        double saturation = baseColor.S;
        double brightness = baseColor.V;

        HsvColor Shade(double multiply)
        {
            HsvColor color = baseColor.Intensify(1.0 + multiply * brightnessFactor);
            return color.Saturate(1.0 + multiply * saturationFactor);
        }

        this.Light = Shade(1.0);
        this.Lighter = Shade(2.0);
        this.Dark = Shade(-1.0);
        this.Darker = Shade(-2.0);
    }
}
