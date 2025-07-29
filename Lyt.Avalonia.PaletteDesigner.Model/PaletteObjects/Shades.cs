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

public static class ShadeKindExtensions
{
    public static Shade ToShadeFrom(this ShadeKind shadeKind, Shades shades)
        => shadeKind switch
        {
            ShadeKind.Lighter => shades.Lighter,
            ShadeKind.Light => shades.Light,
            ShadeKind.Base => shades.Base,
            ShadeKind.Dark => shades.Dark,
            ShadeKind.Darker => shades.Darker,

            _ => throw new ArgumentException("Shade Kind is unkown"),
        };
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

    public Shades DeepClone ()
        =>  new () 
            {
                Wheel = this.Wheel,
                Lighter = this.Lighter.DeepClone(),
                Light = this.Light.DeepClone(), 
                Base = this.Base.DeepClone(),   
                Dark = this.Dark.DeepClone(),   
                Darker = this.Darker.DeepClone(),
            };

    public void Reset()
    {
        ShadeMap shadeMap = Palette.ShadeMap; 
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

    public void UpdateFromWheel(double wheel)
    {
        this.Wheel = wheel;
        if (Palette.HueWheel.TryGetValue(Palette.ToAngle(this.Wheel), out double hue))
        {
            var color = this.Base.Color;
            var hsvColor = new HsvColor(hue, color.S, color.V);
            this.Base.Color = hsvColor;
            this.UpdateAllShadeColors(Palette.ShadeMap);
        }
    }

    public void UpdatePosition(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        if (shadeKind == ShadeKind.Base)
        {
            this.UpdateAll(pixelX, pixelY);
        }
        else
        {
            this.UpdateOne(shadeKind, pixelX, pixelY);
        }
    }

    // Moving base marker to absolute position 
    // All four others follow by the same offset 
    private void UpdateAll(int pixelX, int pixelY)
    {
        ShadeMap shadeMap = Palette.ShadeMap;
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
    private void UpdateOne(ShadeKind shadeKind, int pixelX, int pixelY)
    {
        ShadeMap shadeMap = Palette.ShadeMap;
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

    public void ApplyShadesPreset(ShadesPreset shadesPreset)
    {
        double baseHue = this.Base.Color.H;
        ShadeMap shadeMap = Palette.ShadeMap;
        Position position = shadesPreset.Base;
        this.Base.MoveTo(baseHue, shadeMap, position.X, position.Y);
        position = shadesPreset.Light;
        this.Light.MoveTo(baseHue, shadeMap, position.X, position.Y);
        position = shadesPreset.Lighter;
        this.Lighter.MoveTo(baseHue, shadeMap, position.X, position.Y);
        position = shadesPreset.Dark;
        this.Dark.MoveTo(baseHue, shadeMap, position.X, position.Y);
        position = shadesPreset.Darker;
        this.Darker.MoveTo(baseHue, shadeMap, position.X, position.Y);
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
