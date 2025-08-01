namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class MappingViewModel : ViewModel<MappingView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorsDragPanelViewModel colorsDragPanelViewModel;

    [ObservableProperty]
    private PropertiesDropPanelViewModel propertiesDropPanelViewModel;

    public MappingViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorsDragPanelViewModel = new ColorsDragPanelViewModel(paletteDesignerModel);
        this.PropertiesDropPanelViewModel = new PropertiesDropPanelViewModel(paletteDesignerModel);
    }
}
