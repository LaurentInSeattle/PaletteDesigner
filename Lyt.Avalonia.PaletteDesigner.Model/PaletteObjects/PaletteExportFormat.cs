namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public enum PaletteExportFormat
{
    None,

    AvaloniaAxaml,
    MicrosoftXaml,
    CssStyleSheet,
}

public static class PaletteExportFormatExtensions
{
    public static string ResourcePath(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AvaloniaAxaml or 
            PaletteExportFormat.MicrosoftXaml or
            PaletteExportFormat.CssStyleSheet
                => "Lyt.Avalonia.PaletteDesigner.Resources.PaletteExportTemplates",
            _ => throw new NotImplementedException("Todo")
        };

    public static string ResourceFileName (this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AvaloniaAxaml => "AvaloniaPaletteTemplate.txt",
            PaletteExportFormat.MicrosoftXaml => "MsftXamlPaletteTemplate.txt",
            PaletteExportFormat.CssStyleSheet => "CssPaletteTemplate.txt",
            _ => throw new NotImplementedException("Todo")
        };

    public static string TargetFileName(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AvaloniaAxaml => "AvaloniaPalette",
            PaletteExportFormat.MicrosoftXaml => "MicrosoftPalette",
            PaletteExportFormat.CssStyleSheet => "CssPalette",
            _ => throw new NotImplementedException("Todo")
        };

    public static string ExtensionFileName(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AvaloniaAxaml => ".axaml",
            PaletteExportFormat.MicrosoftXaml => ".xaml",
            PaletteExportFormat.CssStyleSheet=> ".css",
            _ => throw new NotImplementedException("Todo")
        };

    public static string ToFriendlyName(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize (for now) 
            PaletteExportFormat.AvaloniaAxaml => "Avalonia aXaml",
            PaletteExportFormat.MicrosoftXaml => "Microdoft Xaml",
            PaletteExportFormat.CssStyleSheet => "CSS Style Sheet",
            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

    public static List<PaletteExportFormat> AvailableFormats
        => [
                PaletteExportFormat.AvaloniaAxaml, 
                PaletteExportFormat.MicrosoftXaml, 
                PaletteExportFormat.CssStyleSheet
           ];
}