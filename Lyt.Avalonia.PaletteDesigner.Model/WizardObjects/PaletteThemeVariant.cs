namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

[JsonConverter(typeof(JsonStringEnumConverter<PaletteThemeVariant>))]
public enum PaletteThemeVariant
{
    Light,
    Dark,
}
