namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// Screen position or a marker on the shades image:
//      0,0 at top left, 300, 300 at bottom right
//      defaults to center 
public sealed class Position(
    int x = PaletteDesignerModel.ShadesImageDimension / 2,
    int y = PaletteDesignerModel.ShadesImageDimension / 2)
{
    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    public void Adjust()
    {
        double half = PaletteDesignerModel.ShadesImageDimension / 2.0;
        double x = (this.X - half) / half;
        double y = (half - this.Y) / half;
        double radius = Math.Min(1.0, Math.Sqrt(x * x + y * y));
        double angle = Math.Atan2(y, x);
        x = radius * Math.Cos(angle);
        y = radius * Math.Sin(angle);
        int newX = (int)(x * half + half);
        int newY = (int)(half - y * half);
        newX = newX.Clip(PaletteDesignerModel.ShadesImageMax);
        newY = newY.Clip(PaletteDesignerModel.ShadesImageMax);
        this.X = newX; 
        this.Y = newY ;
    }
}