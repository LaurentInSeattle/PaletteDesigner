namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

public static class MathExtensions
{
    public static double Clip(this double x, double max = 1.0)
        => x > max ? max : x < 0.0 ? 0.0 : x;

    public static int Clip(this int x, int max = 300)
        => x > max ? max : x < 0 ? 0 : x;

    public static double NormalizeAngleDegrees(this double angle)
    {
        if (angle < 0)
        {
            return angle + 360.0;
        }
        
        if (angle >= 360.0)
        {
            return angle - 360.0;
        }

        return angle;
    }
}
