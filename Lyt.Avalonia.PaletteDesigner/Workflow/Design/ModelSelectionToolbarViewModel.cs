namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ModelSelectionToolbarViewModel : ViewModel<ModelSelectionToolbarView>
{
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
                var model = App.GetRequiredService<PaletteDesignerModel>();
                model.UpdatePaletteKind(paletteKind);
            }
        }
    }
}
