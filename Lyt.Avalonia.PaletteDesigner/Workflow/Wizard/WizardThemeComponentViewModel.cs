namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using System;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardThemeComponentViewModel : 
    ViewModel <WizardThemeView>, 
    IRecipient<ModelWizardUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly PaletteThemeVariant themeVariant;
    private readonly ThemeComponent themeComponent;

    [ObservableProperty]
    private SolidColorBrush colorBrush;

    //[ObservableProperty]
    //private string hsv = string.Empty;

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

    public void Receive(ModelWizardUpdatedMessage message)
    {
        HsvColor hsvColor = this.paletteDesignerModel.ActiveProject!.WizardPalette.GetThemeComponentColor(this.themeVariant, this.themeComponent);
        this.ColorBrush = hsvColor.ToBrush();
    }

    internal bool OnDrop(WizardSwatchViewModel wizardSwatchViewModel)
    {
        this.paletteDesignerModel.ActiveProject!.WizardPalette.SetThemeComponentColor(
            this.themeVariant, this.themeComponent,
            wizardSwatchViewModel.SwatchIndex, wizardSwatchViewModel.Index);
        return true; 
    }
}
