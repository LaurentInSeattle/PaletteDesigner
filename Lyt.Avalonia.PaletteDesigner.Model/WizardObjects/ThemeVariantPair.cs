namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

internal class ThemeVariantPair
{
    public string Name { get; set; } = string.Empty;

    public ThemeVariantColors Light { get; set; } = new ();

    public ThemeVariantColors Dark { get; set; } = new ();       
}
