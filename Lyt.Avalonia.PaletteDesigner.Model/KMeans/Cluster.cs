namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans;

public sealed class Cluster
{
    public int Count { get; set; }

    public int Total { get; set; }

    public LabColor LabColor { get; set; } = new();
}
