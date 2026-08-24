namespace Lyt.Avalonia.PaletteDesigner.Model;

[JsonSourceGenerationOptions(WriteIndented = true)]

[JsonSerializable(typeof(PaletteDesignerModel))]

[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(Palette))]
[JsonSerializable(typeof(Shades))]
[JsonSerializable(typeof(Shade))]
[JsonSerializable(typeof(ShadesPreset))]

[JsonSerializable(typeof(WizardPalette))]
[JsonSerializable(typeof(JSonExportableWizardPalette))]

[JsonSerializable(typeof(AseDocument))]

[JsonSerializable(typeof(Swatch))]
[JsonSerializable(typeof(ColorSwatches))]

[JsonSerializable(typeof(Dictionary<int, RgbColor>))]
[JsonSerializable(typeof(RgbColor))]

public partial class AppJsonContext : JsonSerializerContext
{
}