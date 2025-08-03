namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public partial class MappingView : View
{
#pragma warning disable CS8618
    // Non-nullable field must contain a non-null value when exiting constructor.
    // Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static Canvas DragCanvas;
#pragma warning restore CA2211 
#pragma warning restore CS8618

    public MappingView() : base() => DragCanvas ??= this.MainWindowCanvas;

    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        base.OnDataContextChanged(sender, e);
        new DragOverAble().Attach(this);
    }
}

