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

    public void Update(double baseHue, ShadeMap shadeMap, int x, int y)
    {
        var position =new Position(x, y);
        position.Adjust();

        // Shade map organized in row / col 
        if (shadeMap.TryGetValue(position, out SvShade? svShade) &&
            svShade is not null)
        {
            this.Color = new HsvColor( baseHue, svShade.S, svShade.V);
            this.Position = position;
        }
        else
        {
            // Should never happen ! 
            throw new Exception("Ouch!");
        }
    } 
}


