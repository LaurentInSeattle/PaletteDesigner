namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public void UpdateThemeProperty(
        ColorThemeVariant themeVariant, string propertyName, Shade shade, double opacity)
        => this.UpdateTheme((ColorTheme colorTheme) =>
        {
            colorTheme.SetShade(themeVariant.Name, propertyName, shade);
            colorTheme.SetOpacity(themeVariant.Name, propertyName, opacity);
            return true;
        });

    private bool UpdateTheme(Func<ColorTheme, bool> action)
    {
        bool result = this.ActionTheme(action);
        if (result)
        {
            this.Messenger.Publish(new ModelUpdatedMessage());
        }

        return result;
    }

    private bool ActionTheme(Func<ColorTheme, bool> action)
    {
        if (this.ActiveProject is null)
        {
            return false;
        }

        var theme = this.ActiveProject.ColorTheme;
        if (theme is null)
        {
            return false;
        }

        // CONSIDER: Similar, needed ? 
        //if ((Palette.ColorWheel is null) || (Palette.ShadeMap is null))
        //{
        //    throw new Exception("Palette class has not been setup");
        //}

        return action(theme);
    }

}
