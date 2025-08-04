namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class PropertyDropViewModel : ViewModel<PropertyDropView>, IDropTarget
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private Palette? palette;
    private WheelKind wheelKind;
    private ShadeKind shadeKind;

    [ObservableProperty]
    private SolidColorBrush shadeBrush;

    [ObservableProperty]
    private SolidColorBrush borderBrush;

    [ObservableProperty]
    private string propertyName;

    [ObservableProperty]
    private double opacitySliderValue;

    [ObservableProperty]
    private double shadeOpacity;

    public PropertyDropViewModel(PaletteDesignerModel paletteDesignerModel, string propertyName)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PropertyName = propertyName.ToFancyString();
        this.BorderBrush = new SolidColorBrush(Colors.Transparent);
        this.ShadeBrush = new SolidColorBrush(0x80808080);
        this.ShadeOpacity = 1.0;
        this.OpacitySliderValue = 1.0;

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    public bool CanDrop(Point point, object droppedObject)
    {
        if (droppedObject is not DraggableColorBoxViewModel)
        {
            Debug.WriteLine("Invalid drop object");
            return false;
        }

        return true;
    }

    public void OnDrop(Point point, object droppedObject)
    {
        if (droppedObject is not DraggableColorBoxViewModel draggableColorBoxViewModel)
        {
            Debug.WriteLine("Invalid drop object");
            return ;
        }

        if (!this.CanDrop(point, draggableColorBoxViewModel))
        {
            return;
        }

        this.ProcessColorBox(draggableColorBoxViewModel); 
    }

    private void OnModelUpdated(ModelUpdatedMessage? _) => this.Colorize();

    private void ProcessColorBox(DraggableColorBoxViewModel draggableColorBoxViewModel)
    {
        this.palette = draggableColorBoxViewModel.palette;
        this.wheelKind = draggableColorBoxViewModel.wheelKind;
        this.shadeKind = draggableColorBoxViewModel.shadeKind;

        this.Colorize();
    }

    private void Colorize()
    {
        if (this.palette is null)
        {
            // No data yet: just do nothing
            return ;
        }

        var shades = this.wheelKind.ToShadesFrom(this.palette!);
        var shade = this.shadeKind.ToShadeFrom(shades);
        this.ShadeBrush = shade.Color.ToBrush();
    }

    partial void OnOpacitySliderValueChanged(double value)
    {
        this.ShadeOpacity = value;
        // this.UpdateLabels();
        // this.paletteDesignerModel.UpdatePalettePrimaryWheel(value);
    }
}
