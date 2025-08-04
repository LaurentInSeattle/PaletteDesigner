namespace Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed class ColorThemeDefinition
{
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public string FriendlyName { get; set; } = string.Empty;

    // First in list is the default 
    [JsonRequired]
    public List<string> Variants { get; set; } = [];

    [JsonRequired]
    public List<string> PropertyNames { get; set; } = [];

    public static ColorThemeDefinition CreateFluent()
        => new ()
        {
            Name = "Fluent",
            FriendlyName = "Avalonia Fluent Theme",
            Variants = [ "Default" , "Dark"],
            PropertyNames =
            [
                "SystemAltHighColor",
                "SystemAltLowColor",
                "SystemAltMediumColor",
                "SystemAltMediumHighColor",
                "SystemAltMediumLowColor",

                "SystemBaseHighColor",
                "SystemBaseLowColor",
                "SystemBaseMediumColor",
                "SystemBaseMediumHighColor",
                "SystemBaseMediumLowColor",
                
                "SystemChromeAltLowColor",
                "SystemChromeBlackHighColor",
                "SystemChromeBlackLowColor",
                "SystemChromeBlackMediumLowColor",
                "SystemChromeBlackMediumColor",
                "SystemChromeDisabledHighColor",
                "SystemChromeDisabledLowColor",
                "SystemChromeHighColor",
                "SystemChromeLowColor",
                "SystemChromeMediumColor",
                "SystemChromeMediumLowColor",
                "SystemChromeWhiteColor",
                "SystemChromeGrayColor",
                
                "SystemListLowColor",
                "SystemListMediumColor",
                
                "SystemErrorTextColor",
                
                "SystemRegionColor",
                
                "SystemRevealListLowColor",
                "SystemRevealListMediumColor",
            ],
        };
}
