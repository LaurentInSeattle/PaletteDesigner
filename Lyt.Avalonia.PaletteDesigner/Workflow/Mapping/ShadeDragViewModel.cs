namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ShadeDragViewModel : ViewModel<ShadeDragView>
{
    // private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly Palette palette;
    private readonly WheelKind wheelKind;
    private readonly ShadeKind shadeKind;
    private readonly Shade shade;

    [ObservableProperty]
    private string shadeName;

    [ObservableProperty]
    private DraggableColorBoxViewModel draggableColorBoxViewModel; 


    public ShadeDragViewModel(
        // PaletteDesignerModel paletteDesignerModel, 
        Palette palette, WheelKind wheelKind, ShadeKind shadeKind, Shade shade)
    {
        // this.paletteDesignerModel = paletteDesignerModel;
        this.palette = palette;
        this.wheelKind = wheelKind;
        this.shadeKind = shadeKind;
        this.shade = shade;

        this.shadeName = shadeKind.ToString();
        this.DraggableColorBoxViewModel = new DraggableColorBoxViewModel(palette, wheelKind, shadeKind, shade);
    }
}
