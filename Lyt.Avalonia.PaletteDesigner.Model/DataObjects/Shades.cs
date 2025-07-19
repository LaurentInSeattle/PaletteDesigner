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

    public void Update (ShadeMap shadeMap, int pixelX, int pixelY)
    {
        const double brightnessStep = 0.1;
        const double saturationStep = 0.1;
        const int brightnessStepPixel = (int)(brightnessStep * PaletteDesignerModel.ShadesImageDimension);
        const int saturationStepPixel = (int)(saturationStep * PaletteDesignerModel.ShadesImageDimension); ;

        double baseHue = this.Base.Color.H;
        var position = new Position(pixelX, pixelY);
        position.Adjust();
        this.Base.Position = position;
        pixelX = position.X;
        pixelY = position.Y;

        int lighterX = pixelX - 2 * saturationStepPixel;
        int lighterY = pixelY - 2 * brightnessStepPixel;
        this.Lighter.Update(baseHue, shadeMap, lighterX, lighterY);

        int lightX = pixelX - saturationStepPixel;
        int lightY = pixelY - brightnessStepPixel;
        this.Light.Update(baseHue, shadeMap, lightX, lightY);

        int darkX = pixelX + saturationStepPixel;
        int darkY = pixelY + brightnessStepPixel;
        this.Dark.Update(baseHue, shadeMap, darkX, darkY);

        int darkerX = pixelX + 2 * saturationStepPixel;
        int darkerY = pixelY + 2 * brightnessStepPixel;
        this.Darker.Update(baseHue, shadeMap, darkerX, darkerY);
    }
}
