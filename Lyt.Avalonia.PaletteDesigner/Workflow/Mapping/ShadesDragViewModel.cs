namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ShadesDragViewModel : ViewModel<ShadesDragView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public ShadesDragViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
    }
}
