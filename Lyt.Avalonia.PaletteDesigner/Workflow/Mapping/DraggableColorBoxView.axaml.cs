namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public partial class DraggableColorBoxView : View
{
    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (this.DataContext is DraggableColorBoxViewModel viewModel)
        {
            if (!viewModel.IsGhost)
            {
                // Don't do that for the ghost view 
                base.OnDataContextChanged(sender, e);
            }
        }
    } 
}