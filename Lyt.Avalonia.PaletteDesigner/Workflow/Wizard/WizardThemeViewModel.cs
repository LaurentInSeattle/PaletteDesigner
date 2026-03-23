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
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial SolidColorBrush BackgroundBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush ForegroundBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush AccentBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush DiscordantBrush { get; set; }

    [ObservableProperty]
    public partial WizardThemeComponentViewModel BackgroundComponent { get; set; }

    [ObservableProperty]    
    private WizardThemeComponentViewModel foregroundComponent;

    [ObservableProperty]
    public partial WizardThemeComponentViewModel AccentComponent { get; set; }

    [ObservableProperty]
    public partial WizardThemeComponentViewModel DiscordantComponent { get; set; }

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardThemeViewModel(
        PaletteDesignerModel paletteDesignerModel,
        PaletteThemeVariant themeVariant)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.themeVariant = themeVariant;
        this.Name = string.Empty; 
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

    // Localize theme names
    private void Localize()
        => this.Name =            
            this.Localize(
                this.themeVariant == PaletteThemeVariant.Light ? 
                "Wizard.Theme.Light" : 
                "Wizard.Theme.Dark");
}
