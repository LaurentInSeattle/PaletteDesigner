namespace Lyt.Avalonia.PaletteDesigner.Workflow.Language;

using static Messaging.ViewActivationMessage;

public sealed partial class LanguageToolbarViewModel : ViewModel<LanguageToolbarView>
{
#pragma warning disable CA1822 // Mark members as static
    [RelayCommand]
    public void OnNext() => ViewSelector<ActivatedView>.Select(ActivatedView.Intro);
#pragma warning restore CA1822 
}
