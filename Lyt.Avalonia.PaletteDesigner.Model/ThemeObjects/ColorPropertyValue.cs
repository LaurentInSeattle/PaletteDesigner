namespace Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed class ColorPropertyValue
{
    // defaults to dark gray 
    public uint Rgb { get; set; } = 0x00_40_40_40;

    // defaults to fully opaque 
    public double Opacity { get; set; } = 1.0;

    public uint ToUintArgb () 
    {
        uint opacity = (uint) Math.Round(this.Opacity * 255.0);
        return this.Rgb | (opacity << 24); 
    }
}
