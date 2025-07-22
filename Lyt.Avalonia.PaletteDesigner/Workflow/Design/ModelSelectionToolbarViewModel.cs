namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ModelSelectionToolbarViewModel : ViewModel<ModelSelectionToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel; 

    public ModelSelectionToolbarViewModel()
        => this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();

    [RelayCommand]
    public void OnModelSelect(object? parameter)
    {
        // Update model 
        PaletteKind paletteKind = PaletteKind.Unknown;
        if (parameter is string tag)
        {
            paletteKind = Enum.TryParse(tag, out PaletteKind kind) ? kind : PaletteKind.Unknown;
            if (paletteKind != PaletteKind.Unknown)
            {
                this.paletteDesignerModel.UpdatePaletteKind(paletteKind);
            }
        }
    }

    [RelayCommand]
    public void OnLockSelect(object? parameter)
    {
        // Update model 
        ShadeMode shadeMode = ShadeMode.Locked;
        if (parameter is string tag)
        {
            shadeMode = Enum.TryParse(tag, out ShadeMode kind) ? kind : ShadeMode.Locked;
            this.paletteDesignerModel.UpdatePaletteShadeMode(shadeMode);
        }
    }
}
