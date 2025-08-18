namespace Lyt.Avalonia.PaletteDesigner.Model.SwatchObjects;

public sealed class ColorSwatches
{
    public string Name { get; set; } = string.Empty;

    public List<Swatch> Swatches { get; set; } = [];

    public ColorSwatches DeepClone()
    {
        var list = new List<Swatch>(this.Swatches.Count);
        foreach (var swatch in this.Swatches)
        {
            list.Add(swatch.DeepClone());
        }

        ColorSwatches colorSwatches = new()
        {
            Name = new string(this.Name),
            Swatches = list, 
        };

        return colorSwatches;
    }
}
