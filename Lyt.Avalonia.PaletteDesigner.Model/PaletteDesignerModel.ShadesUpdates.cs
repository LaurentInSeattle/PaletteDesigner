namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public void ResetShades()
        => this.UpdatePalette((Palette palette) =>
        {
            palette.ResetAllShades();
            return true;
        });

    public void OnShadeMarkerPositionChanged(ShadeKind shadeKind, int pixelX, int pixelY)
        => this.UpdatePalette((Palette palette) =>
        {
            if (palette.AreShadesLocked)
            {
                palette.Primary.UpdatePosition(shadeKind, pixelX, pixelY);
                palette.Complementary.UpdatePosition(shadeKind, pixelX, pixelY);
                palette.Secondary1.UpdatePosition(shadeKind, pixelX, pixelY);
                palette.Secondary2.UpdatePosition(shadeKind, pixelX, pixelY);
            }
            else
            {
                WheelKind selectedWheel = palette.SelectedWheel; 
                if ( selectedWheel == WheelKind.Unknown)
                {
                    return false; 
                }

                var shades = palette.SelectedWheel.ToShadesFrom(palette);
                shades.UpdatePosition(shadeKind, pixelX, pixelY);
            }

            return true;
        });
}
