namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class WidgetsPreviewViewModel : ViewModel<WidgetsPreviewView>
{
    [ObservableProperty]
    private string title;

    public WidgetsPreviewViewModel(string title)
    {
        this.Title = title;
    }
}
