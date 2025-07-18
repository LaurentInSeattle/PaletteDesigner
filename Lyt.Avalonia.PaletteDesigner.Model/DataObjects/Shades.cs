namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public enum ShadeMode
{
    Locked,
    Unlocked,
}

public enum ShadeKind
{
    None,

    Lighter,
    Light,
    Base,
    Dark,
    Darker,
}

// Mutable 
public sealed class Shade
{
    public Shade( /* required for serialization */)
    {
        this.Color = new () ;
        this.Position = new();
    }

    public Shade(HsvColor color , Position position)
    {
        this.Color = color;
        this.Position = position;
    }

    public HsvColor Color { get; set; } = new();

    public Position Position { get; set; } = new();
}

public sealed class Shades
{
    public Shade Lighter { get; set; } = new();

    public Shade Light { get; set; } = new();

    public Shade Base { get; set; } = new();

    public Shade Dark { get; set; } = new();

    public Shade Darker { get; set; } = new();

    public Shades() { /* needed for serialization */ }

    public Shades(HsvColor baseColor, double saturationFactor, double brightnessFactor)
    {
        this.Base.Color = baseColor;

        HsvColor Shade(double multiply)
        {
            HsvColor color = baseColor.Intensify(1.0 + multiply * brightnessFactor);
            return color.Saturate(1.0 + multiply * saturationFactor);
        }

        this.Light.Color = Shade(1.0);
        this.Lighter.Color = Shade(2.0);
        this.Dark.Color = Shade(-1.0);
        this.Darker.Color = Shade(-2.0);
    }
}
