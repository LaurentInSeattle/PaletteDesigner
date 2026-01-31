namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardThemeViewModel : 
    ViewModel <WizardThemeView>, 
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelWizardUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly PaletteThemeVariant themeVariant;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private SolidColorBrush backgroundBrush;

    [ObservableProperty]
    private SolidColorBrush foregroundBrush;

    [ObservableProperty]
    private SolidColorBrush accentBrush;

    [ObservableProperty]
    private SolidColorBrush discordantBrush;

    [ObservableProperty]
    private WizardThemeComponentViewModel backgroundComponent;
    
    [ObservableProperty]    
    private WizardThemeComponentViewModel foregroundComponent;

    [ObservableProperty]
    private WizardThemeComponentViewModel accentComponent;

    [ObservableProperty]
    private WizardThemeComponentViewModel discordantComponent; 

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardThemeViewModel(
        PaletteDesignerModel paletteDesignerModel,
        PaletteThemeVariant themeVariant)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.themeVariant = themeVariant;
        this.Name = this.themeVariant == PaletteThemeVariant.Light ? "Light" : "Dark";
        this.BackgroundBrush = new SolidColorBrush(Colors.Gray);
        this.ForegroundBrush = new SolidColorBrush(Colors.LightPink);
        this.AccentBrush = new SolidColorBrush(Colors.Firebrick);
        this.DiscordantBrush = new SolidColorBrush(Colors.DodgerBlue);
        this.BackgroundComponent = 
            new WizardThemeComponentViewModel(this.paletteDesignerModel, this.themeVariant, ThemeComponent.Background);
        this.ForegroundComponent = 
            new WizardThemeComponentViewModel(this.paletteDesignerModel, this.themeVariant, ThemeComponent.Foreground);
        this.AccentComponent = 
            new WizardThemeComponentViewModel(this.paletteDesignerModel, this.themeVariant, ThemeComponent.Accent);
        this.DiscordantComponent = 
            new WizardThemeComponentViewModel(this.paletteDesignerModel, this.themeVariant, ThemeComponent.Discordant);

        this.Localize();
        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelWizardUpdatedMessage>();
    }

    public void Receive(LanguageChangedMessage message) => this.Localize();

    public void Receive(ModelWizardUpdatedMessage message)
    {
        if (this.paletteDesignerModel.ActiveProject is not Project project)
        {
            return;
        }

        HsvColor[] hsvColors = project.WizardPalette.GetThemeColors(this.themeVariant);
        this.BackgroundBrush = hsvColors[0].ToBrush();
        this.ForegroundBrush = hsvColors[1].ToBrush();
        this.AccentBrush = hsvColors[2].ToBrush();
        this.DiscordantBrush = hsvColors[3].ToBrush();
    }

    private void Localize()
        // TODO: Localize theme names
        => this.Name = this.themeVariant == PaletteThemeVariant.Light ? "Light" : "Dark";
}
