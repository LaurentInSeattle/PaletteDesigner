namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public sealed class Palette
{
    public string Name { get; set; } = string.Empty;

    public PaletteKind Kind { get; set; }

    public Shades Primary { get; set; } = new();

    public Shades Secondary1 { get; set; } = new();
    
    public Shades Secondary2 { get; set; } = new();
    
    public Shades Complementary { get; set; } = new();
}
