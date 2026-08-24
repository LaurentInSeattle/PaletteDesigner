namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

// Indexed for positioning in the wizard view
[JsonConverter(typeof(JsonStringEnumConverter<SwatchKind>))]
public enum SwatchKind : int 
{
    Lighter, 
    Light,
    Base,
    Dark,
    Darker,
}