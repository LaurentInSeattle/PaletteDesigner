namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// Screen position or a marker on the shades image:
//      0,0 at top left, 300, 300 at bottom right
//      defaults to center 
public sealed record class Position(
    int X = PaletteDesignerModel.ShadesImageDimension / 2,
    int Y = PaletteDesignerModel.ShadesImageDimension / 2);