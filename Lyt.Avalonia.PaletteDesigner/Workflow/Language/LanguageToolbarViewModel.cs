namespace Lyt.Avalonia.PaletteDesigner.Workflow.Language;

using static Lyt.Avalonia.PaletteDesigner.Messaging.ViewActivationMessage;

public sealed partial class LanguageToolbarViewModel : ViewModel<LanguageToolbarView>
{
    [RelayCommand]
    public void OnNext()
        => ViewSelector<ActivatedView>.Select(this.Messenger, ActivatedView.Intro);
}
