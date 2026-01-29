namespace Lyt.Avalonia.PaletteDesigner.Model;

using System.Xml;

public enum PaletteExportFormat
{
    None,

    AvaloniaAxaml,
    MicrosoftXaml,
    CssStyleSheet,
    AdobeAse,
    ApplicationJSon,
}

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


        // TODO: Rename resource files 
        return string.Concat(formatString, "_", familyString, "_", "Template.csx");

        //=> paletteExportFormat switch
        //   {
        //       // Binary, Not template based
        //       PaletteExportFormat.AdobeAse => string.Empty,

        //       // Not template based
        //       PaletteExportFormat.ApplicationJSon => string.Empty,

        //       // No need to localize 
        //       PaletteExportFormat.AvaloniaAxaml =>
        //           paletteFamily == PaletteFamily.Designed ?
        //               "AvaloniaPaletteTemplate.csx" :
        //               paletteFamily == PaletteFamily.Image ?
        //                   "AvaloniaImagePaletteTemplate.csx" : "AvaloniaWizardPaletteTemplate.csx",
        //       PaletteExportFormat.MicrosoftXaml =>
        //           paletteFamily == PaletteFamily.Designed ?
        //               "MsftXamlPaletteTemplate.csx" :
        //               paletteFamily == PaletteFamily.Image ?
        //                   "MsftXamlImagePaletteTemplate.csx" : "MsftXamlWizardPaletteTemplate.csx",
        //       PaletteExportFormat.CssStyleSheet =>
        //           paletteFamily == PaletteFamily.Designed ?
        //               "CssPaletteTemplate.csx" :
        //               paletteFamily == PaletteFamily.Image ?
        //                   "CssImagePaletteTemplate.csx" : "CssWizardPaletteTemplate.csx",
        //       _ => throw new ArgumentException(null, nameof(paletteExportFormat)),
        //   };
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