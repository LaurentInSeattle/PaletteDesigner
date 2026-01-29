namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

internal sealed class JSonExportableWizardPalette
{
    public HsvColor[] LighterColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] LightColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] BaseColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] DarkColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] DarkerColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] LightThemeColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];

    public HsvColor[] DarkThemeColors { get; set; } = new HsvColor[WizardPalette.PaletteWidth];
}
