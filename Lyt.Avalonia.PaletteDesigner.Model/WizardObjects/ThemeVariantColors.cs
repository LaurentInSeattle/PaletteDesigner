namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed class ThemeVariantColors
{
    public ThemeVariantColors() { }

    public PaletteThemeVariant ThemeVariant { get; set; } = PaletteThemeVariant.Light;

    public SwatchIndex Background { get; set; } = new();

    public SwatchIndex Foreground { get; set; } = new();

    public SwatchIndex Accent { get; set; } = new();
    
    public SwatchIndex Discordant { get; set; } = new();
}
