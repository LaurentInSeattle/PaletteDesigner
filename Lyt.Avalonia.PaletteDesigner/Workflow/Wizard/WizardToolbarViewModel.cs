namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public sealed partial class WizardToolbarViewModel : ViewModel<WizardToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool showShadesValues;


    public WizardToolbarViewModel() 
        => this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();

    partial void OnShowShadesValuesChanged(bool value)
        => new ThemeValuesVisibilityMessage(value).Publish();

#pragma warning disable CA1822  
    // Mark members as static
    // Relay commands cannot be static

    [RelayCommand]
    public void OnDisplayMode(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ThemeValuesDisplayMode mode =
                Enum.TryParse(tag, out ThemeValuesDisplayMode kind) ? kind : ThemeValuesDisplayMode.Hex;
            this.paletteDesignerModel.ThemeValuesDisplayMode = mode;
            new ModelThemeDisplayModeUpdated().Publish();
        }
    }

    [RelayCommand]
    public void OnRandomize() => this.paletteDesignerModel.WizardPaletteRandomize();

#pragma warning restore CA1822 // Mark members as static
}
