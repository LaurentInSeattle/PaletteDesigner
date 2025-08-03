namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using Lyt.Quantics.Studio.Behaviors.DragDrop;

public partial class MappingView : View
{
#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public static Canvas DragCanvas;
#pragma warning restore CS8618 

    public MappingView() : base() => DragCanvas ??= this.MainWindowCanvas;

    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        base.OnDataContextChanged(sender, e);
        new DragOverAble().Attach(this);
    }
}

