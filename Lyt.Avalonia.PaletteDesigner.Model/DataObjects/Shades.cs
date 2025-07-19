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

    public void Update(ShadeMap shadeMap, int pixelX, int pixelY)
    {
        const double brightnessStep = 0.1;
        const double saturationStep = 0.1;
        const int brightnessStepPixel = (int)(brightnessStep * PaletteDesignerModel.ShadesImageDimension);
        const int saturationStepPixel = (int)(saturationStep * PaletteDesignerModel.ShadesImageDimension); ;

        var position = new Position(pixelX, pixelY);
        position.Adjust();
        this.Base.Position = position;
        pixelX = position.X;
        pixelY = position.Y;

        //int offsetToCenterX = Math.Abs((pixelX - PaletteDesignerModel.ShadesImageCenter) / 2);
        //int offsetToCenterY = Math.Abs((pixelY - PaletteDesignerModel.ShadesImageCenter) / 2);
        //int halfOffsetToCenterX = offsetToCenterX / 2;
        //int halfOffsetToCenterY = offsetToCenterY / 2;

        int offsetToCenterX = 0; //
        int offsetToCenterY = 0; // 
        int halfOffsetToCenterX = 0; //
        int halfOffsetToCenterY = 0; //

        double baseHue = this.Base.Color.H;
        int lighterX = pixelX - 2 * saturationStepPixel - offsetToCenterY + offsetToCenterX;
        int lighterY = pixelY - 2 * brightnessStepPixel + offsetToCenterY - offsetToCenterX;
        this.Lighter.Update(baseHue, shadeMap, lighterX, lighterY);

        int lightX = pixelX - saturationStepPixel - halfOffsetToCenterY + halfOffsetToCenterX; 
        int lightY = pixelY - brightnessStepPixel + halfOffsetToCenterY - halfOffsetToCenterX; ;
        this.Light.Update(baseHue, shadeMap, lightX, lightY);

        int darkX = pixelX + saturationStepPixel + halfOffsetToCenterY - halfOffsetToCenterX;
        int darkY = pixelY + brightnessStepPixel + halfOffsetToCenterY - halfOffsetToCenterX;
        this.Dark.Update(baseHue, shadeMap, darkX, darkY);

        int darkerX = pixelX + 2 * saturationStepPixel + 2 * offsetToCenterY - offsetToCenterX;
        int darkerY = pixelY + 2 * brightnessStepPixel + 2 * offsetToCenterY - offsetToCenterX;
        this.Darker.Update(baseHue, shadeMap, darkerX, darkerY);
    }
}
