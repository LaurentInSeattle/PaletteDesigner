namespace Lyt.Avalonia.PaletteDesigner.Utilities;

using HsvColor = Model.DataObjects.HsvColor;

public static class MediaExtensions
{
    public static RgbColor ToRGB(this Color c) => new(c.R, c.G, c.B);

    public static HsvColor ToHSV(this Color c) => MediaExtensions.ToRGB(c).ToHsv();

    public static Color ToColor(this HsvColor hsv) => MediaExtensions.ToColor(hsv.ToRgb());

    public static Color ToColor(this RgbColor rgb)
        => Color.FromRgb(
                (byte)Math.Round(rgb.R),
                (byte)Math.Round(rgb.G),
                (byte)Math.Round(rgb.B));

    public static SolidColorBrush ToBrush(this RgbColor rgb) => new(ToColor(rgb));

    public static SolidColorBrush ToBrush(this HsvColor hsv) => new(ToColor(hsv));
}
