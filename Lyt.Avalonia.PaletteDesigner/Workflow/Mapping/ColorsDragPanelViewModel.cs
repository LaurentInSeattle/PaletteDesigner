namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ColorsDragPanelViewModel : ViewModel<ColorsDragPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public ColorsDragPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;        
    }
}
