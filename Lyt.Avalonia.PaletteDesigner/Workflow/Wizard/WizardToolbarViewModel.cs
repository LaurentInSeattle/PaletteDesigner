namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public sealed partial class WizardToolbarViewModel : ViewModel<WizardToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool showShadesValues;


    public WizardToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
    }

    partial void OnShowShadesValuesChanged(bool value)
        => new ShadesValuesVisibilityMessage(value).Publish();


#pragma warning disable CA1822  
    // Mark members as static
    // Relay commands cannot be static

    [RelayCommand]
    public void OnDisplayMode(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ShadesValuesDisplayMode mode =
                Enum.TryParse(tag, out ShadesValuesDisplayMode kind) ? kind : ShadesValuesDisplayMode.Hex;
            this.paletteDesignerModel.ShadesValuesDisplayMode = mode;
            new ModelShadesDisplayModeUpdated().Publish();
        }
    }

    [RelayCommand]
    public void OnRecalculate()
    {
        var viewModel = App.GetRequiredService<ImagingViewModel>();
        viewModel.ReProcessBitmap();
    }

#pragma warning restore CA1822 // Mark members as static
}
