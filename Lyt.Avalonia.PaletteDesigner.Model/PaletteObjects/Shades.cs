namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

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
    public double Wheel { get; set; }

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

/*
Primary
    Lighter
        X: 65  Y: 71
        Hue: 351.0  Sat: 0.3  Bri: 1.0
    Light
        X: 124  Y: 85
        Hue: 351.0  Sat: 0.6  Bri: 1.0
    Base
        X: 167  Y: 124
        Hue: 351.0  Sat: 0.8  Bri: 0.8
    Dark
        X: 194  Y: 169
        Hue: 351.0  Sat: 0.9  Bri: 0.6
    Darker
        X: 203  Y: 225
        Hue: 351.0  Sat: 1.0  Bri: 0.4
*/

    public void Reset(ShadeMap shadeMap)
    {
        int pixelX = PaletteDesignerModel.ShadesImageCenter;
        int pixelY = PaletteDesignerModel.ShadesImageCenter;
        const double brightnessStep = 0.1;
        const double saturationStep = 0.1;
        const int brightnessStepPixel = (int)(brightnessStep * PaletteDesignerModel.ShadesImageDimension);
        const int saturationStepPixel = (int)(saturationStep * PaletteDesignerModel.ShadesImageDimension); ;

        var position = new Position(pixelX, pixelY);
        position.Adjust();
        this.Base.Position = position;
        pixelX = position.X;
        pixelY = position.Y;

        int offsetToCenterX = Math.Abs((pixelX - PaletteDesignerModel.ShadesImageCenter) / 2);
        int offsetToCenterY = Math.Abs((pixelY - PaletteDesignerModel.ShadesImageCenter) / 2);
        int halfOffsetToCenterX = offsetToCenterX / 2;
        int halfOffsetToCenterY = offsetToCenterY / 2;

        double baseHue = this.Base.Color.H;
        int lighterX = pixelX - 2 * saturationStepPixel - offsetToCenterY + offsetToCenterX;
        int lighterY = pixelY - 2 * brightnessStepPixel + offsetToCenterY - offsetToCenterX;
        this.Lighter.MoveTo(baseHue, shadeMap, lighterX, lighterY);

        int lightX = pixelX - saturationStepPixel - halfOffsetToCenterY + halfOffsetToCenterX; 
        int lightY = pixelY - brightnessStepPixel + halfOffsetToCenterY - halfOffsetToCenterX; ;
        this.Light.MoveTo(baseHue, shadeMap, lightX, lightY);

        int darkX = pixelX + saturationStepPixel + halfOffsetToCenterY - halfOffsetToCenterX;
        int darkY = pixelY + brightnessStepPixel + halfOffsetToCenterY - halfOffsetToCenterX;
        this.Dark.MoveTo(baseHue, shadeMap, darkX, darkY);

        int darkerX = pixelX + 2 * saturationStepPixel + offsetToCenterY - offsetToCenterX;
        int darkerY = pixelY + 2 * brightnessStepPixel + offsetToCenterY - offsetToCenterX;
        this.Darker.MoveTo(baseHue, shadeMap, darkerX, darkerY);
    }

    public void CopyShadesFrom(ShadeMap shadeMap, Shades shades)
    {
        //foreach (var shade in
        //    new[] { this.Lighter, this.Light, this.Base, this.Dark, this.Darker })
        //{
        //    var position = shade.Position; 
        //    shade.MoveTo(shade.Color.H, shadeMap, position.X, position.Y);
        //}
    }

    public void UpdateFromWheel(double wheel, Dictionary<int, RgbColor> colorWheel, ShadeMap shadeMap)
    {
        this.Wheel = wheel;
        if (colorWheel.TryGetValue(Palette.ToAngle(this.Wheel), out RgbColor? rgbColor))
        {
            if (rgbColor is null)
            {
                throw new Exception("No such angle");
            }

            var hsvColor = rgbColor.ToHsv();
            var color = this.Base.Color;
            hsvColor.S = color.S;
            hsvColor.V = color.V;
            this.Base.Color = hsvColor;
            this.UpdateAllShadeColors(shadeMap);
        }
    }

    // Moving base marker to absolute position 
    // All four others follow by the same offset 
    public void UpdateAll(ShadeMap shadeMap, int pixelX, int pixelY)
    {
        var oldPosition = this.Base.Position; 
        var newPosition = new Position(pixelX, pixelY);
        newPosition.Adjust();
        Position delta = newPosition.Delta(oldPosition);
        this.Base.Position = newPosition;
        
        double baseHue = this.Base.Color.H;
        this.Base.UpdateColors(baseHue, shadeMap);

        this.Lighter.MoveBy(baseHue, shadeMap, delta);
        this.Light.MoveBy(baseHue, shadeMap, delta);
        this.Dark.MoveBy(baseHue, shadeMap, delta);
        this.Darker.MoveBy(baseHue, shadeMap, delta);
    }

    // Moving ONE marker, but NOT the base, to an absolute position 
    // All other unchanged 
    public void UpdateOne(ShadeMap shadeMap, ShadeKind shadeKind, int pixelX, int pixelY)
    {
        var position = new Position(pixelX, pixelY);
        position.Adjust();
        pixelX = position.X;
        pixelY = position.Y;

        double baseHue = this.Base.Color.H;
        switch (shadeKind)
        {
            case ShadeKind.Base:
                throw new Exception("Should be used by base marker!");

            case ShadeKind.None:
                return ;

            case ShadeKind.Lighter:
                this.Lighter.MoveTo(baseHue, shadeMap, pixelX, pixelY);
                break;

            case ShadeKind.Light:
                this.Light.MoveTo(baseHue, shadeMap, pixelX, pixelY);
                break;

            case ShadeKind.Dark:
                this.Dark.MoveTo(baseHue, shadeMap, pixelX, pixelY);
                break;

            case ShadeKind.Darker:
                this.Darker.MoveTo(baseHue, shadeMap, pixelX, pixelY);
                break;
        }
    }

    // Change colors without changing markers positions 
    public void UpdateAllShadeColors(ShadeMap shadeMap)
    {
        double baseHue = this.Base.Color.H;
        this.Lighter.UpdateColors(baseHue, shadeMap);
        this.Light.UpdateColors(baseHue, shadeMap);
        this.Dark.UpdateColors(baseHue, shadeMap);
        this.Darker.UpdateColors(baseHue, shadeMap);
    }

    [Conditional("DEBUG")]
    public void Dump(string name)
    {
        Debug.WriteLine(name);
        Debug.Indent();
        this.Lighter.Dump("Lighter");
        this.Light.Dump("Light");
        this.Base.Dump("Base");
        this.Dark.Dump("Dark");
        this.Darker.Dump("Darker");
        Debug.Unindent();
    }
}
