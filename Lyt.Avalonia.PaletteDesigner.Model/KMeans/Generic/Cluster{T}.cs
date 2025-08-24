namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans.Generic;

public sealed class Cluster<T> where T : class, IClusterable<T>, new()
{
    public int Count { get; set; }

    public int Total { get; set; }

    public T Payload { get; set; } = new();
}
