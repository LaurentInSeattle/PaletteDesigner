namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ColorDragViewModel : ViewModel<ColorDragView>
{
    private readonly WheelKind kind;
    private readonly Shades shades;

    [ObservableProperty]
    private string colorName;

    public ColorDragViewModel(WheelKind kind, Shades shades)
    {
        this.kind = kind;
        this.shades = shades;
        this.ColorName = kind.ToString();
    }
}
