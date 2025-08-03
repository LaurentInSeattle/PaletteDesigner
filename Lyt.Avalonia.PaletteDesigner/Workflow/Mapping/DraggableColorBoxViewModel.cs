namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using Lyt.Quantics.Studio.Behaviors.DragDrop;

public sealed partial class DraggableColorBoxViewModel : ViewModel<DraggableColorBoxView>, IDragAbleViewModel
{
    public const string CustomDragAndDropFormat = "PaletteViewModel";

    // All four made public so that the drop target can use it 
    public readonly Palette palette;
    public readonly WheelKind wheelKind;
    public readonly ShadeKind shadeKind;
    public readonly Shade shade;

    [ObservableProperty]
    private SolidColorBrush shadeBrush;

    [ObservableProperty]
    private SolidColorBrush borderBrush;

    public DraggableColorBoxViewModel(
        Palette palette, 
        WheelKind wheelKind, ShadeKind shadeKind, Shade shade, 
        bool isGhost =false)
    {
        this.palette = palette;
        this.wheelKind = wheelKind;
        this.shadeKind = shadeKind;
        this.shade = shade;
        this.BorderBrush = shade.Color.ToBrush();
        this.ShadeBrush = shade.Color.ToBrush();

        this.IsGhost = isGhost;
        if (!isGhost)
        {
            this.DragAble = new DragAble(MappingView.DragCanvas);
        } 

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    public override void OnViewLoaded()
    {
        if (!this.IsGhost && this.DragAble is not null && !this.DragAble.IsAttached)
        {
            base.OnViewLoaded();
            this.DragAble.Attach(this.View);
        }
    }

    public bool IsGhost { get; private set; }

    public string DragDropFormat => DraggableColorBoxViewModel.CustomDragAndDropFormat;

    public DragAble? DragAble { get; private set; }

    public View CreateGhostView()
    {
        DraggableColorBoxViewModel ghostViewModel =
            new(this.palette, this.wheelKind, this.shadeKind, this.shade, isGhost: true);
        ghostViewModel.CreateViewAndBind();
        var view = ghostViewModel.View;
        view.ZIndex = 999_999;
        view.Opacity = 0.9;
        return view;
    }

    // Always OK to drag 
    public bool OnBeginDrag() => true;

    public void OnClicked(bool isRightClick) { }

    public void OnEntered() { }

    public void OnExited() { }

    public void OnLongPress() { }

    private void OnModelUpdated(ModelUpdatedMessage? _) => this.ShadeBrush = shade.Color.ToBrush();
}
