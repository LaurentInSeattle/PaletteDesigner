namespace Lyt.Avalonia.PaletteDesigner.Model.SwatchObjects;

public sealed class Swatch
{
    public Swatch() { }

    public Swatch(Cluster cluster)
    {
        var rgbColor = cluster.LabColor.ToRgb();
        this.HsvColor = rgbColor.ToHsv();
        this.Usage = cluster.Count / (double)cluster.Total;

        // Debug.WriteLine("Cluster Lab Color to Swatch RGB: " + rgbColor.ToRgbDecString());
        // Debug.WriteLine("Cluster Lab to RGB to HSV: " + this.HsvColor.ToString());
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
