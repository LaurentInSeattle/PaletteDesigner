namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

[JsonConverter(typeof(JsonStringEnumConverter<ThemeValuesDisplayMode>))]
public enum ThemeValuesDisplayMode
{
    Hex, 
    Percent, 
    Decimal,
}
