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

    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        ColorGroup colorGroup = new(this.Name.ToString());
        int index = 0;
        var sortedSwatched = 
            from swatch in this.Swatches orderby swatch.Usage descending select swatch; 
        foreach (var swatch in sortedSwatched)
        {
            var rgb = swatch.HsvColor.ToRgb();
            byte r = (byte)Math.Round(rgb.R);
            byte g = (byte)Math.Round(rgb.G);
            byte b = (byte)Math.Round(rgb.B);
            // Debug.WriteLine (index.ToString("D3") + " " + rgb.ToRgbDecString() );
            ColorEntry colorEntry = new(index.ToString("D3"), r, g, b);
            colorGroup.Colors.Add(colorEntry);

            ++ index;
        }

        document.Groups.Add(colorGroup);
        return document;
    }
}
