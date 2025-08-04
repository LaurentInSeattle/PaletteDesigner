namespace Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed class ColorProperty
{
    public string Name { get; set; } = string.Empty;

    public Dictionary<string, ColorPropertyValue> PropertyValues { get; set; } = [];
}
