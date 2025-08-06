namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class WidgetsPreviewViewModel : ViewModel<WidgetsPreviewView>
{
    [ObservableProperty]
    private string title;

    public WidgetsPreviewViewModel(string title)
    {
        this.Title = title;
    }

    public void UpdatePalettes(
        ColorPaletteResources darkColorPaletteResources, ColorPaletteResources lightColorPaletteResources )
    {
        this.View.Styles.Clear();
        this.View.Styles.Add(
            new FluentTheme
            {
                Palettes =
                {
                    [ThemeVariant.Light] = lightColorPaletteResources,
                    [ThemeVariant.Dark] = darkColorPaletteResources
                }
            } );
    }
}
