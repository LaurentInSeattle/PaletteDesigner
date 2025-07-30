namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette
{
    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        this.ForAllShades((wheelKind, shades) =>
        {
            ColorGroup colorGroup = new(wheelKind.ToString());
            shades.ForAllShades((shadeKind, shade) =>
            {
                var rgb = shade.Color.ToRgb();
                byte r = (byte)Math.Round(rgb.R);
                byte g = (byte)Math.Round(rgb.G);
                byte b = (byte)Math.Round(rgb.B);
                ColorEntry colorEntry = new(shadeKind.ToString(), r, g, b);
                colorGroup.Colors.Add(colorEntry);
            });
            document.Groups.Add(colorGroup);
        });

        return document;
    }
}