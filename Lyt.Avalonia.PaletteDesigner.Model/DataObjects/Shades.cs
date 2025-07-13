namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public enum ShadeMode
{
    Locked,
    Unlocked,
}

public enum Shade
{
    None,

    Lighter,
    Light,
    Base,
    Dark,
    Darker,
}

public sealed class Shades
{
    public HsvColor Lighter { get; set; } = new();
    public HsvColor Light { get; set; } = new();
    public HsvColor Base { get; set; } = new();
    public HsvColor Dark { get; set; } = new();
    public HsvColor Darker { get; set; } = new();
}
