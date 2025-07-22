namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// Mutable 
public sealed class Shade
{
    [JsonIgnore]
    public HsvColor Color { get; set; }

    [JsonRequired]
    public Position Position { get; set; }

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

    public Shade (Shade shade)
    {
        this.Color = new HsvColor(shade.Color);
        this.Position = new Position(shade.Position);
    }

    // new absolute position 
    public void MoveTo(double baseHue, ShadeMap shadeMap, int x, int y)
    {
        var position = new Position(x, y);
        position.Adjust();
        this.Position = position;
        this.UpdateColors(baseHue, shadeMap);
    }

    // new relative position 
    public void MoveBy(double baseHue, ShadeMap shadeMap, Position delta)
    {
        var position = this.Position.MoveBy(delta);
        position.Adjust();
        this.Position = position;
        this.UpdateColors(baseHue, shadeMap); 
    }

    // new base hue , no position change 
    public void UpdateColors(double baseHue, ShadeMap shadeMap)
    {
        // Shade map organized in row / col 
        var position = this.Position;
        if (shadeMap.TryGetValue(position, out SvShade? svShade) && svShade is not null)
        {
            this.Color = new HsvColor(baseHue, svShade.S, svShade.V);
        }
        else
        {
            // Should never happen ! 
            throw new Exception("Ouch!");
        }
    }

    public Position Delta(Position position) => this.Position.Delta(position);

    [Conditional("DEBUG")]
    public void Dump(string name)
    {
        Debug.WriteLine(name);
        Debug.Indent();
        Debug.WriteLine(this.Position.ToString());
        Debug.WriteLine(this.Color.ToString());
        Debug.Unindent();
    }
}


