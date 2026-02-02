namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardThemeComponentViewModel :
    ViewModel<WizardThemeView>,
    IDropTarget,
    IRecipient<ModelWizardUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly PaletteThemeVariant themeVariant;
    private readonly ThemeComponent themeComponent;

    [ObservableProperty]
    private SolidColorBrush colorBrush;

    public WizardThemeComponentViewModel(
        PaletteDesignerModel paletteDesignerModel,
        PaletteThemeVariant themeVariant,
        ThemeComponent themeComponent)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.themeVariant = themeVariant;
        this.themeComponent = themeComponent;
        this.ColorBrush = new SolidColorBrush(Colors.Gray);
        this.Subscribe<ModelWizardUpdatedMessage>();
    }

    public bool CanDrop(Point point, object droppedObject) => droppedObject is WizardSwatchViewModel ;

    public void OnDrop(Point point, object droppedObject)
    {
        if (droppedObject is WizardSwatchViewModel wizardSwatchViewModel)
        {
            if ( this.paletteDesignerModel.ActiveProject is not Project project)
            {
                return;
            }

            project.WizardPalette.SetThemeComponentColor(
                this.themeVariant, this.themeComponent,wizardSwatchViewModel.SwatchIndex);
        }
    }

    public void Receive(ModelWizardUpdatedMessage message)
    {
        if (this.paletteDesignerModel.ActiveProject is not Project project)
        {
            return;
        }

        HsvColor hsvColor = 
            project.WizardPalette.GetThemeComponentColor(this.themeVariant, this.themeComponent);
        this.ColorBrush = hsvColor.ToBrush();
    }
}
