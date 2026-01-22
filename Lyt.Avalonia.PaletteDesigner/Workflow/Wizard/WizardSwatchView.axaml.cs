namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public partial class WizardSwatchView : View
{
    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (this.DataContext is WizardSwatchViewModel wizardSwatchViewModel)
        {
            if (!wizardSwatchViewModel.IsGhost)
            {
                // Don't do that for the ghost view 
                base.OnDataContextChanged(sender, e);
            }
        }
    }
}