namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public partial class WizardView : View
{
    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        base.OnDataContextChanged(sender, e);
        new DragOverAble().Attach(this);
    }

    internal void AddSwatchView (WizardSwatchView swatchView, SwatchKind swatchKind, int index)
    {
        this.SwatchGrid.Children.Add(swatchView);
        swatchView.SetValue(Grid.RowProperty, (int)swatchKind);
        swatchView.SetValue(Grid.ColumnProperty, index);
    }
}
