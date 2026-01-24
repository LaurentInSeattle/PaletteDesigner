namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public void WizardPaletteReset()
        => this.ActionWizard(palette =>
        {
            palette.Reset();
            return true;
        });

    public void WizardPaletteSetWheel(double baseWheel)
        => this.ActionWizard(palette =>
        {
            palette.SetWheel(baseWheel);
            return true;
        });

    public void WizardPaletteSetCurvePower(double value)
        => this.ActionWizard(palette =>
        {
            palette.SetCurvePower(value);
            return true;
        });

    public void WizardPaletteSetCurveAngleStep(int value)
        => this.ActionWizard(palette =>
        {
            palette.SetCurveAngleStep(value);
            return true;
        });

    public void WizardPaletteSetWheelAngleStep(double value)
        => this.ActionWizard(palette =>
        {
            palette.SetWheelAngleStep(value);
            return true;
        });

    public void WizardPaletteSetHighlights(double value)
        => this.ActionWizard(palette =>
        {
            palette.SetHighlights(value);
            return true;
        });

    public void WizardPaletteSetShadows(double value)
        => this.ActionWizard(palette =>
        {
            palette.SetShadows(value);
            return true;
        });

    public void WizardPaletteSetStyle(int value)
        => this.ActionWizard(palette =>
        {
            palette.SetStyle(value);
            return true;
        });

    private bool ActionWizard(Func<WizardPalette, bool> action)
    {
        if (this.ActiveProject is null)
        {
            return false;
        }

        var palette = this.ActiveProject.WizardPalette;
        if (palette is null)
        {
            return false;
        }

        return action(palette);
    }
}
