namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ColorDragViewModel : ViewModel<ColorDragView>
{
    private readonly Palette palette;
    private readonly WheelKind wheelKind;
    private readonly Shades shades;

    [ObservableProperty]
    private string colorName;

    [ObservableProperty]
    private ObservableCollection<ShadeDragViewModel> shadeDragViewModels; 

    public ColorDragViewModel(Palette palette, WheelKind wheelKind, Shades shades)
    {
        this.palette = palette;
        this.wheelKind = wheelKind;
        this.shades = shades;
        this.ColorName = wheelKind.ToString();
        this.ShadeDragViewModels = [];
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        List<ShadeDragViewModel> list = [];
        this.shades.ForAllShades((shadeKind, shade) =>
        {
            var shadeDragViewModel = new ShadeDragViewModel(this.palette, this.wheelKind, shadeKind, shade);
            list.Add(shadeDragViewModel);
        });

        this.ShadeDragViewModels = new(list);
        this.ColorName = this.wheelKind.ToString();
    }
}
