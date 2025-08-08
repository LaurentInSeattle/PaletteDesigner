namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public enum ShadeMode
{
    Locked,
    Unlocked,
}

public enum ShadeKind
{
    None,

    Lighter,
    Light,
    Base,
    Dark,
    Darker,
}

public static class ShadeKindExtensions
{
    public static Shade ToShadeFrom(this ShadeKind shadeKind, Shades shades)
        => shadeKind switch
        {
            ShadeKind.Lighter => shades.Lighter,
            ShadeKind.Light => shades.Light,
            ShadeKind.Base => shades.Base,
            ShadeKind.Dark => shades.Dark,
            ShadeKind.Darker => shades.Darker,

            _ => throw new ArgumentException("Shade Kind is unkown"),
        };

    public static string ToLocalizationKey(this ShadeKind shadeKind)
        => shadeKind switch
        {
            ShadeKind.Lighter => "Design.Toolbar.Shade.Lighter",
            ShadeKind.Light => "Design.Toolbar.Shade.Light",
            ShadeKind.Base => "Design.Toolbar.Shade.Base",
            ShadeKind.Dark => "Design.Toolbar.Shade.Dark",
            ShadeKind.Darker => "Design.Toolbar.Shade.Darker",

            _ => throw new ArgumentException("Shade Kind is unkown"),
        };
}

