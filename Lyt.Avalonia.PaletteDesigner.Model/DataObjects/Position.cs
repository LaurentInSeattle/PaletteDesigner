namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

// Screen position or a marker on the shades image:
//      0,0 at top left, 300, 300 at bottom right
//      defaults to center of the Shades Image circle 
public sealed class Position(
    int x = PaletteDesignerModel.ShadesImageDimension / 2,
    int y = PaletteDesignerModel.ShadesImageDimension / 2)
{
    public int X { get; set; } = x;

    public int Y { get; set; } = y;

    // Constrain point to be located inside the Shades Image circle 
    public void Adjust()
    {
        // normalize to center 
        double half = PaletteDesignerModel.ShadesImageDimension / 2.0;
        double x = (this.X - half) / half;
        double y = (half - this.Y) / half;
        double radius = Math.Sqrt(x * x + y * y);
        if (radius > 1.0)
        {
            // adjustment is needed
            double minRadius = Math.Min(1.0, radius);
            double angle = Math.Atan2(y, x);
            x = minRadius * Math.Cos(angle);
            y = minRadius * Math.Sin(angle);
            int newX = (int)(0.5 + x * half + half);
            int newY = (int)(0.5 + half - y * half);
            newX = newX.Clip(PaletteDesignerModel.ShadesImageMax);
            newY = newY.Clip(PaletteDesignerModel.ShadesImageMax);
            this.X = newX;
            this.Y = newY;
        }
        // else: No change needed
    }

    public Position MoveBy(Position position)
        => new(this.X + position.X, this.Y + position.Y);

    public Position Delta(Position position)
        => new(this.X - position.X, this.Y - position.Y);

    public double Distance(Position position)
        => Math.Sqrt(
            (this.X - position.X) * (this.X - position.X) +
            (this.Y - position.Y) * (this.Y - position.Y));
}