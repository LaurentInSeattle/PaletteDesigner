namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public enum ResourceFormat
{
    Unknown = 0, 
    Axaml , // Avalonia Xaml resource file format 
    Resx,   // Good old WinForms 
}

public static class ResourceFormats
{
    public static string ToFriendlyName(this ResourceFormat resourceFormat)
        => resourceFormat switch
        {
            // No need to localize 
            ResourceFormat.Axaml => "Avalonia  .axaml",
            ResourceFormat.Resx => "Microsoft  .resx",
            _ => throw new ArgumentException(null, nameof(resourceFormat)),
        };

    public static string ToFileExtension(this ResourceFormat resourceFormat)
        => resourceFormat switch
        {
            // No need to localize 
            ResourceFormat.Axaml => ".axaml",
            ResourceFormat.Resx => ".resx",
            _ => throw new ArgumentException(null, nameof(resourceFormat)),
        };

    public static List<ResourceFormat> AvailableFormats
        => [ResourceFormat.Axaml, ResourceFormat.Resx]; 
}