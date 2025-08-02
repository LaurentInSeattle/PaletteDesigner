namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class PropertyDropViewModel : ViewModel<PropertyDropView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private Palette? palette;
    private WheelKind wheelKind;
    private ShadeKind shadeKind;
    private Shade? shade;

    [ObservableProperty]
    private SolidColorBrush shadeBrush;

    [ObservableProperty]
    private SolidColorBrush borderBrush;

    [ObservableProperty]
    private string propertyName;

    public PropertyDropViewModel(PaletteDesignerModel paletteDesignerModel, string propertyName)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PropertyName = propertyName;
        this.BorderBrush = new SolidColorBrush(Colors.Blue);
        this.ShadeBrush = new SolidColorBrush(Colors.SandyBrown);
    }
}
