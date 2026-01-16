namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public partial class WizardView : View
{
    internal void AddSwatchView (WizardSwatchView swatchView, SwatchKind swatchKind, int index)
    {
        this.SwatchGrid.Children.Add(swatchView);
        swatchView.SetValue(Grid.RowProperty, (int)swatchKind);
        swatchView.SetValue(Grid.ColumnProperty, index);
    }
}
