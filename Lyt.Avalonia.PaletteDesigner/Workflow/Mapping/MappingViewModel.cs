namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class MappingViewModel : ViewModel<MappingView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorsDragPanelViewModel colorsDragPanelViewModel;

    [ObservableProperty]
    private PropertiesDropPanelViewModel propertiesDropPanelViewModel;

    [ObservableProperty]
    private WidgetsPreviewViewModel widgetsPreviewViewModel;

    public MappingViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorsDragPanelViewModel = new (paletteDesignerModel);
        this.PropertiesDropPanelViewModel = new (paletteDesignerModel);
        this.WidgetsPreviewViewModel = new("Preview"); 
    }
}
