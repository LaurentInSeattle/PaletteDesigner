namespace Lyt.Avalonia.PaletteDesigner.Model.SwatchObjects;

public sealed class Swatch
{
    public Swatch() { }

    public Swatch(Cluster cluster)
    {
        this.HsvColor = cluster.LabColor.ToRgb().ToHsv();
        this.Usage = cluster.Count / (double)cluster.Total;
    }

    public double Usage { get; set; } 

    public HsvColor HsvColor { get; set; } = new();

    public Swatch DeepClone() 
        => new()
        {
            Usage = this.Usage,
            HsvColor = new HsvColor(this.HsvColor)
        };
}
