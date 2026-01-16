namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardSwatchViewModel : 
    ViewModel <WizardSwatchView>, 
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelWizardUpdatedMessage>
{

    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardSwatchViewModel(PaletteDesignerModel paletteDesignerModel, SwatchKind swatchKind , int index)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.Kind = swatchKind;
        this.Index = index;
        this.ColorBrush = new SolidColorBrush(Colors.Transparent); 
        this.Localize();
        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelWizardUpdatedMessage>();
    }

    public SwatchKind Kind { get; private set; }

    public int Index { get; private set; }

    public void Receive(LanguageChangedMessage message) => this.Localize();

    public void Receive(ModelWizardUpdatedMessage message)
    {
        HsvColor hsvColor = this.paletteDesignerModel.ActiveProject!.WizardPalette.GetColor(this.Kind, this.Index); 
        this.ColorBrush = hsvColor.ToBrush();
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
