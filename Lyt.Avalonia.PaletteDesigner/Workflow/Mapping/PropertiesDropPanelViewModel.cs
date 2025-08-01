namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class PropertiesDropPanelViewModel : ViewModel<PropertiesDropPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public PropertiesDropPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;    
    }
}
