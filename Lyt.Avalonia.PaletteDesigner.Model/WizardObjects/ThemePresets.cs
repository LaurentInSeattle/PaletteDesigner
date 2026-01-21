namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

internal static class ThemePresets
{
    internal static ThemeVariantColors StandardLightTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Light,
            Background = new SwatchIndex(SwatchKind.Light, 1),
            Foreground = new SwatchIndex(SwatchKind.Dark, 7),
            Accent = new SwatchIndex(SwatchKind.Base, 3),
            Discordant= new SwatchIndex(SwatchKind.Base, 5),
        };

    internal static ThemeVariantColors StandardDarkTheme =
        new ()
        {
            ThemeVariant = PaletteThemeVariant.Dark,
            Background = new SwatchIndex(SwatchKind.Dark, 7),
            Foreground = new SwatchIndex(SwatchKind.Light, 1),
            Accent = new SwatchIndex(SwatchKind.Base, 5),
            Discordant = new SwatchIndex(SwatchKind.Base, 3),
        };

    internal static ThemeVariantColors AltStandardLightTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Light,
            Background = new SwatchIndex(SwatchKind.Light, 0),
            Foreground = new SwatchIndex(SwatchKind.Dark, 8),
            Accent = new SwatchIndex(SwatchKind.Base, 2),
            Discordant = new SwatchIndex(SwatchKind.Base, 6),
        };

    internal static ThemeVariantColors AltStandardDarkTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Dark,
            Background = new SwatchIndex(SwatchKind.Dark, 8),
            Foreground = new SwatchIndex(SwatchKind.Light, 0),
            Accent = new SwatchIndex(SwatchKind.Base, 6),
            Discordant = new SwatchIndex(SwatchKind.Base, 2),
        };

    internal static ThemeVariantColors PastelLightTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Light,
            Background = new SwatchIndex(SwatchKind.Light, 0),
            Foreground = new SwatchIndex(SwatchKind.Dark, 5),
            Accent = new SwatchIndex(SwatchKind.Light, 2),
            Discordant = new SwatchIndex(SwatchKind.Light, 6),
        };

    internal static ThemeVariantColors PastelDarkTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Dark,
            Background = new SwatchIndex(SwatchKind.Dark, 5),
            Foreground = new SwatchIndex(SwatchKind.Light, 0),
            Accent = new SwatchIndex(SwatchKind.Light, 6),
            Discordant = new SwatchIndex(SwatchKind.Light, 2),
        };

    internal static ThemeVariantColors AltPastelLightTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Light,
            Background = new SwatchIndex(SwatchKind.Light, 1),
            Foreground = new SwatchIndex(SwatchKind.Dark, 6),
            Accent = new SwatchIndex(SwatchKind.Light, 3),
            Discordant = new SwatchIndex(SwatchKind.Light, 5),
        };

    internal static ThemeVariantColors AltPastelDarkTheme =
        new()
        {
            ThemeVariant = PaletteThemeVariant.Dark,
            Background = new SwatchIndex(SwatchKind.Dark, 6),
            Foreground = new SwatchIndex(SwatchKind.Light, 1),
            Accent = new SwatchIndex(SwatchKind.Light, 3),
            Discordant = new SwatchIndex(SwatchKind.Light, 5),
        };

    internal static ThemeVariantPair Standard =
        new()
        {
            Name = "Standard",
            Light = StandardLightTheme,
            Dark = StandardDarkTheme,
        };

    internal static ThemeVariantPair AltStandard =
        new()
        {
            Name = "AltStandard",
            Light = AltStandardLightTheme,
            Dark = AltStandardDarkTheme,
        };

    internal static ThemeVariantPair Pastel =
        new()
        {
            Name = "Pastel",
            Light = PastelLightTheme,
            Dark = PastelDarkTheme,
        };

    internal static ThemeVariantPair AltPastel =
        new()
        {
            Name = "AltPastel",
            Light = AltPastelLightTheme,
            Dark = AltPastelDarkTheme,
        };

    internal static List<ThemeVariantPair> All =
    [
        Standard,
        AltStandard,
        Pastel,
        AltPastel,
    ];
}
