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

public sealed class Shades
{
    [JsonRequired]
    public Shade Lighter { get; set; } = new();

    [JsonRequired]
    public Shade Light { get; set; } = new();

    [JsonRequired]
    public Shade Base { get; set; } = new();

    [JsonRequired]
    public Shade Dark { get; set; } = new();

    [JsonRequired]
    public Shade Darker { get; set; } = new();

    public Shades() { /* needed for serialization */ }

    public void Update (ShadeMap shadeMap )
    {
        double baseHue = this.Base.Color.H; 
        this.Lighter.Update(baseHue, shadeMap);
        this.Light.Update(baseHue, shadeMap);
        this.Dark.Update(baseHue, shadeMap);
        this.Darker.Update(baseHue, shadeMap);
    }

    //public Shades(HsvColor baseColor, double saturationFactor, double brightnessFactor)
    //{
    //    this.Base = new();
    //    this.Base.Color = baseColor;

    //    //const double More = 1.0;
    //    //const double Less = 1.0;

    //    this.Light = new();
    //    this.Light.Update(
    //        baseColor.H, 

    //        saturationFactor, brightnessFactor);
    //    this.Lighter = new();
    //    this.Dark = new();
    //    this.Darker = new();
    //}
}
