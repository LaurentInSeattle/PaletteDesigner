namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans.Generic;

public interface IClusterable<T> where T : class, IClusterable<T>, new()
{
    static abstract double Distance ( T x, T y);

    static abstract T Average ( IEnumerable<T> values );
}
