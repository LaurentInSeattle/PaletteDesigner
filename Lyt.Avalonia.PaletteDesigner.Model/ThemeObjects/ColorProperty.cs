namespace Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed class ColorProperty
{
    public string Name { get; set; } = string.Empty;

    // Indexed by variant name (dark, etc)
    public Dictionary<string, ColorPropertyValue> PropertyValues { get; set; } = [];
}
