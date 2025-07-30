namespace Lyt.Avalonia.PaletteDesigner.Utilities;

using Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;
using HsvColor = Model.PaletteObjects.HsvColor;

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

    public static string ToRgbHexString(this HsvColor hsv)
    {
        var rgb = hsv.ToRgb();
        return
            string.Format(
                "{0:X2} {1:X2} {2:X2}",
                (int)Math.Round(rgb.R),
                (int)Math.Round(rgb.G),
                (int)Math.Round(rgb.B));
    }

    public static string ToRgbPercentString(this HsvColor hsv)
    {
        var rgb = hsv.ToRgb();
        return
            string.Format(
                "{0:D2} {1:D2} {2:D2}",
                (int)Math.Round(100.0 * rgb.R / 255.0),
                (int)Math.Round(100.0 * rgb.G / 255.0),
                (int)Math.Round(100.0 * rgb.B / 255.0));
    }

    public static string ToRgbDecimalString(this HsvColor hsv)
    {
        var rgb = hsv.ToRgb();
        return
            string.Format(
                "{0:D3} {1:D3} {2:D3}",
                (int)Math.Round(rgb.R),
                (int)Math.Round(rgb.G),
                (int)Math.Round(rgb.B));
    }

    public static SolidColorBrush ToBrush(this Shade shade) => shade.Color.ToBrush();

    public static string ToRgbHexString(this Shade shade) => shade.Color.ToRgbHexString();
    public static string ToRgbPercentString(this Shade shade) => shade.Color.ToRgbPercentString();
    public static string ToRgbDecimalString(this Shade shade) => shade.Color.ToRgbDecimalString();
}
