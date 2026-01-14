namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public sealed partial class WizardSwatchViewModel : ViewModel <WizardSwatchView>, IRecipient<LanguageChangedMessage>
{        
    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardSwatchViewModel()
    {
        this.ColorBrush = new SolidColorBrush(Colors.Transparent); 
        this.Localize(); 
        this.Subscribe<LanguageChangedMessage>();
    }

    public void Receive(LanguageChangedMessage message) => this.Localize(); 

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
