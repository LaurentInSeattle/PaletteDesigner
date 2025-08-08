namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewViewModel : ViewModel<TextPreviewView>
{
    [ObservableProperty]
    SolidColorBrush foregroundBrush;

    [ObservableProperty]
    SolidColorBrush backgroundBrush;

    public TextPreviewViewModel()
    {
        this.backgroundBrush = new SolidColorBrush(Colors.Black);
        this.foregroundBrush = new SolidColorBrush(Colors.White);
    }
}
