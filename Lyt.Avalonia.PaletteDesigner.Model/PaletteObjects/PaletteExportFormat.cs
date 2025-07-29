namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public enum PaletteExportFormat
{
    None,

    AvaloniaAxaml,
    WpfXaml,
    UwpXaml,
}

public static class PaletteExportFormatExtensions
{
    public static string ResourcePath(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            PaletteExportFormat.AvaloniaAxaml 
                => "Lyt.Avalonia.PaletteDesigner.Resources.PaletteExportTemplates",
            _ => throw new NotImplementedException("Todo")
        };

    public static string ResourceFileName (this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            PaletteExportFormat.AvaloniaAxaml => "AvaloniaPaletteTemplate.txt",
            _ => throw new NotImplementedException("Todo")
        };
} 