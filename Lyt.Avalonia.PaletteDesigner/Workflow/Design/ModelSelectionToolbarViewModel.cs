namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

using Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class ModelSelectionToolbarViewModel : ViewModel<ModelSelectionToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public ModelSelectionToolbarViewModel()
        => this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnModelSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            PaletteKind paletteKind = Enum.TryParse(tag, out PaletteKind kind) ? kind : PaletteKind.Unknown;
            if (paletteKind != PaletteKind.Unknown)
            {
                // Update model 
                this.paletteDesignerModel.UpdatePaletteKind(paletteKind);
            }
        }
    }
}
