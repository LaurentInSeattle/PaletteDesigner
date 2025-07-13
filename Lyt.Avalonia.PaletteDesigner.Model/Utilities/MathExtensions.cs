namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

public static class MathExtensions
{
    public static double HueToWheel(this double hue)
        => hue <= 120.0 ? Math.Round(hue * 1.5) : Math.Round(180 + (hue - 120) * 0.75);

    public static double WheelToHue(this double wheel)
        => wheel <= 180 ? Math.Round(wheel / 1.5) : Math.Round(120 + (wheel - 180) / 0.75);

    public static double Clip(this double x, double max = 1.0)
        => x > max ? max : x < 0.0 ? 0.0 : x;
}
