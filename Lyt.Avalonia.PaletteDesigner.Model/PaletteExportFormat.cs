namespace Lyt.Avalonia.PaletteDesigner.Model;

public enum PaletteExportFormat
{
    None,

    AvaloniaAxaml,
    MicrosoftXaml,
    CssStyleSheet,
    AdobeAse,
    ApplicationJSon,
}

/*
 * 

TODO 

Paint.NET Palette File

Palette files are simple text (*.txt) files, that can be created and edited in most word processors.  
Each file contains color information listed one shade per row in eight digit hexadecimal notation. 
The format used is aarrggbb where aa is the Alpha value, rr is the Red value, gg is the Green value and bb 
the Blue value. 

; Paint.NET Palette File
; Lines that start with a semicolon are comments
; Colors are written as 8-digit hexadecimal numbers: aarrggbb
; For example, this would specify green: FF00FF00
; The alpha ('aa') value specifies how transparent a color is. FF is fully opaque, 00 is fully transparent.
; A palette usually consists of ninety six (96) colors. If there are less than this, the remaining color
; slots will be set to white (FFFFFFFF). If there are more, then the remaining colors will be ignored.
FF000000
FF404040
FFFF0000
FFFF6A00
FF23FFBD
FFB6FF00
FF4CFF00
FF00FF21
FF00FF90
FF00FFFF
FFFFFA7C
FF0026FF
FF4800FF
 
 */

public static class PaletteExportFormatExtensions
{
    public static string ResourcePath(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // Binary, Not template based
            PaletteExportFormat.AdobeAse => string.Empty,

            // Not template based
            PaletteExportFormat.ApplicationJSon => string.Empty,

            // No need to localize 
            PaletteExportFormat.AvaloniaAxaml or 
            PaletteExportFormat.MicrosoftXaml or
            PaletteExportFormat.CssStyleSheet
                => "Lyt.Avalonia.PaletteDesigner.Resources.PaletteExportTemplates",
            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

    // No need to localize, 
    public static string ResourceFileName(
        this PaletteExportFormat paletteExportFormat, PaletteFamily paletteFamily)
    {
        string formatString = paletteExportFormat switch
        {
            // Dont need resources: Binary, Not template based
            PaletteExportFormat.AdobeAse => "Adobe",
            // Text, Not template based
            PaletteExportFormat.ApplicationJSon => "Json",

            PaletteExportFormat.AvaloniaAxaml => "Avalonia",
            PaletteExportFormat.MicrosoftXaml => "MsftXaml",
            PaletteExportFormat.CssStyleSheet => "Css",

            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

        string familyString = paletteFamily switch
        {
            PaletteFamily.Designed => "Designer",
            PaletteFamily.Image => "Image",
            PaletteFamily.Wizard => "Wizard",

            _ => throw new ArgumentException(null, nameof(paletteFamily)),
        };


        return string.Concat(formatString, "_", familyString, "_", "Template.csx");
    }

    public static string ExportTargetFileName(
        this PaletteExportFormat paletteExportFormat, PaletteFamily paletteFamily)
    {  
        string formatString = paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AdobeAse => "Adobe",
            PaletteExportFormat.AvaloniaAxaml => "Avalonia",
            PaletteExportFormat.MicrosoftXaml => "Microsoft",
            PaletteExportFormat.CssStyleSheet => "Css",
            PaletteExportFormat.ApplicationJSon => "Json",

            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

        string familyString = paletteFamily switch
        {
            PaletteFamily.Designed => "Designer",
            PaletteFamily.Image => "Image",
            PaletteFamily.Wizard => "Wizard",

            _ => throw new ArgumentException(null, nameof(paletteFamily)),
        };

        return string.Concat(formatString, "_" , familyString); 
    } 

    public static string ExtensionFileName(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize 
            PaletteExportFormat.AdobeAse => ".ase",
            PaletteExportFormat.AvaloniaAxaml => ".axaml",
            PaletteExportFormat.MicrosoftXaml => ".xaml",
            PaletteExportFormat.CssStyleSheet=> ".css",
            PaletteExportFormat.ApplicationJSon => ".json",
            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

    // For use by UI drop down 
    public static string ToFriendlyName(this PaletteExportFormat paletteExportFormat)
        => paletteExportFormat switch
        {
            // No need to localize (for now) 
            PaletteExportFormat.AdobeAse => "Adobe ASE",
            PaletteExportFormat.AvaloniaAxaml => "Avalonia aXaml",
            PaletteExportFormat.MicrosoftXaml => "Microsoft Xaml",
            PaletteExportFormat.CssStyleSheet => "CSS Style Sheet",
            PaletteExportFormat.ApplicationJSon => "JSon",
            _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        };

    public static List<PaletteExportFormat> AvailableFormats
        => [
                PaletteExportFormat.AvaloniaAxaml, 
                PaletteExportFormat.MicrosoftXaml, 
                PaletteExportFormat.CssStyleSheet,
                PaletteExportFormat.AdobeAse,
                PaletteExportFormat.ApplicationJSon,
           ];
}