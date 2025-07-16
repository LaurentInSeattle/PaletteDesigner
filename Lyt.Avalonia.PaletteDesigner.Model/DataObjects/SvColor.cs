namespace Lyt.Avalonia.PaletteDesigner.Model.DataObjects;

public sealed class SvColor
{
    public double S { get; set; } = 0.5;

    public double V { get; set; } = 0.5;
}

public sealed class SvColorList : List<SvColor>
{
    public SvColorList() : base()
    {
        this.Add(new SvColor());
        this.Add(new SvColor());
        this.Add(new SvColor());
        this.Add(new SvColor());
        this.Add(new SvColor());
    }
}
