namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// Mutable 
public sealed class Shade
{
    public Shade( /* required for serialization */)
    {
        this.Color = new(0, 0.7, 0.7);
        this.Position = new(PaletteDesignerModel.ShadesImageCenter, PaletteDesignerModel.ShadesImageCenter);
    }

    public Shade(HsvColor color, Position position)
    {
        this.Color = color;
        this.Position = position;
    }

    [JsonIgnore]
    public HsvColor Color { get; set; }

    [JsonRequired]
    public Position Position { get; set; }

    public void Update(
        double baseHue,
        ShadeMap shadeColorMap)
    { 
    } 

    public void Update(
        double baseHue, 
        ShadeMap shadeColorMap, 
        int X, int Y,
        double saturationFactor, double brightnessFactor)
    {
        //const double More = 1.25;
        //const double Less = 1.0;
        const double brightnessStep = 0.1;
        const double saturationStep = 0.1;
        const int brightnessStepPixel = (int)(brightnessStep * PaletteDesignerModel.ShadesImageDimension);
        const int saturationStepPixel = (int)(saturationStep * PaletteDesignerModel.ShadesImageDimension); ;

        var position =
            new Position(
                (int)(X + saturationFactor * saturationStepPixel),
                (int)(Y + brightnessFactor * brightnessStepPixel));
        position.Adjust();
        if (shadeColorMap.TryGetValue(position.X, position.Y, out HsvColor? shadeColor) &&
            shadeColor is not null)
        {
            shadeColor.H = baseHue;
            this.Color = shadeColor;
            this.Position = position;
        }
        else
        {
            throw new Exception("Ouch!");
        }
    } 
}


