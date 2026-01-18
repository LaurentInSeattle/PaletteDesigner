namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardThemeViewModel : 
    ViewModel <WizardThemeView>, 
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelWizardUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

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
    private string hsv = string.Empty;

    public WizardThemeViewModel(PaletteDesignerModel paletteDesignerModel, string name)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.Name = name;
        this.BackgroundBrush = new SolidColorBrush(Colors.Gray);
        this.ForegroundBrush = new SolidColorBrush(Colors.LightPink);
        this.AccentBrush = new SolidColorBrush(Colors.Firebrick);
        this.DiscordantBrush = new SolidColorBrush(Colors.DodgerBlue);
        this.Localize();
        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelWizardUpdatedMessage>();
    }

    public void Receive(LanguageChangedMessage message) => this.Localize();

    public void Receive(ModelWizardUpdatedMessage message)
    {
        // HsvColor hsvColor = this.paletteDesignerModel.ActiveProject!.WizardPalette.GetColor(this.Kind, this.Index); 
        // this.ColorBrush = hsvColor.ToBrush();
    }

    private void Localize() 
    {
    }

    private void Update()
    {
        //RgbColor rgbColor = cluster.Payload.ToRgb();
        //this.ColorBrush = rgbColor.ToBrush();
        //this.RgbHex = string.Format("# {0}", rgbColor.ToRgbHexString());
        //this.RgbDec = string.Format("\u2022 {0}", rgbColor.ToRgbDecString());
    }
}
