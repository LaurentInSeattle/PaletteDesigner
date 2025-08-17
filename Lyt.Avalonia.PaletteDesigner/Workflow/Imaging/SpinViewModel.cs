namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class SpinViewModel : ViewModel<SpinView>
{
    [ObservableProperty]
    private bool isVisible;

    [ObservableProperty]
    private bool isActive;

    // Important ! 
    // 
    // Do NOT forget ti add the following line in App.axaml:
    // 
    //   <StyleInclude Source="avares://Lyt.Avalonia.Controls/Progress/ProgressRing.axaml"/>
    // 
    // Or else the prgress ring will not show up.... 
}
