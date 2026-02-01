namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardThemeValuesViewModel : 
    ViewModel <WizardThemeView>, 
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelWizardUpdatedMessage>,
    IRecipient<ModelThemeDisplayModeUpdated>,
    IRecipient<ThemeValuesVisibilityMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly PaletteThemeVariant themeVariant;

    [ObservableProperty]
    private bool showValues;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string background;

    [ObservableProperty]
    private string foreground;

    [ObservableProperty]
    private string accent;

    [ObservableProperty]
    private string discordant;

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardThemeValuesViewModel(
        PaletteDesignerModel paletteDesignerModel,
        PaletteThemeVariant themeVariant)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.themeVariant = themeVariant;
        this.ShowValues = false;
        this.Name = string.Empty;
        this.Background = string.Empty;
        this.Foreground = string.Empty;
        this.Accent = string.Empty;
        this.Discordant = string.Empty;

        this.Localize();
        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelWizardUpdatedMessage>();
        this.Subscribe<ModelThemeDisplayModeUpdated>();
        this.Subscribe<ThemeValuesVisibilityMessage>();
    }

    public void Receive(LanguageChangedMessage message) => this.Localize();

    public void Receive(ModelWizardUpdatedMessage message) => this.Update();

    public void Receive(ModelThemeDisplayModeUpdated message) => this.Update();
    
    public void Receive(ThemeValuesVisibilityMessage message) 
        => this.ShowValues = message.Show;

    // Localize theme names
    private void Localize()
        => this.Name =
            this.Localize(
                this.themeVariant == PaletteThemeVariant.Light ?
                "Wizard.Theme.Light" :
                "Wizard.Theme.Dark");

    private void Update()
    {
        if (this.paletteDesignerModel.ActiveProject is not Project project)
        {
            return;
        }

        HsvColor[] hsvColors = project.WizardPalette.GetThemeColors(this.themeVariant);

        var displayMode = this.paletteDesignerModel.ThemeValuesDisplayMode;

        var backgroundColor = hsvColors[0].ToRgb();
        var foregroundColor = hsvColors[1].ToRgb();
        var accentColor = hsvColors[2].ToRgb();
        var discordantColor = hsvColors[3].ToRgb();

        switch (displayMode)
        {
            default:
            case ThemeValuesDisplayMode.Hex:
                this.Background = string.Format("  {0}", backgroundColor.ToRgbHexString());
                this.Foreground = string.Format("  {0}", foregroundColor.ToRgbHexString());
                this.Accent = string.Format("  {0}", accentColor.ToRgbHexString());
                this.Discordant = string.Format("  {0}", discordantColor.ToRgbHexString());
                break;

            case ThemeValuesDisplayMode.Percent:
                this.Background = string.Format("  {0}", backgroundColor.ToRgbPercentString());
                this.Foreground = string.Format("  {0}", foregroundColor.ToRgbPercentString());
                this.Accent = string.Format("  {0}", accentColor.ToRgbPercentString());
                this.Discordant = string.Format("  {0}", discordantColor.ToRgbPercentString());
                break;

            case ThemeValuesDisplayMode.Decimal:
                this.Background = string.Format("\u2022 {0}", backgroundColor.ToRgbDecString());
                this.Foreground = string.Format("\u2022 {0}", foregroundColor.ToRgbDecString());
                this.Accent = string.Format("\u2022 {0}", accentColor.ToRgbDecString());
                this.Discordant = string.Format("\u2022 {0}", discordantColor.ToRgbDecString());
                break;
        }
    }
}
