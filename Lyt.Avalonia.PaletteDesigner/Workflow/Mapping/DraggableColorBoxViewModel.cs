namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class DraggableColorBoxViewModel : ViewModel<DraggableColorBoxView>
{    
    private readonly Palette palette;
    private readonly WheelKind wheelKind;
    private readonly ShadeKind shadeKind;
    private readonly Shade shade;

    [ObservableProperty]
    private SolidColorBrush shadeBrush;

    [ObservableProperty]
    private SolidColorBrush borderBrush;

    public DraggableColorBoxViewModel(Palette palette, WheelKind wheelKind, ShadeKind shadeKind, Shade shade)
    {
        this.palette = palette;
        this.wheelKind = wheelKind;
        this.shadeKind = shadeKind;
        this.shade = shade;
        this.BorderBrush = shade.Color.ToBrush();
        this.ShadeBrush = shade.Color.ToBrush();

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    private void OnModelUpdated(ModelUpdatedMessage? _) => this.ShadeBrush = shade.Color.ToBrush();
}
