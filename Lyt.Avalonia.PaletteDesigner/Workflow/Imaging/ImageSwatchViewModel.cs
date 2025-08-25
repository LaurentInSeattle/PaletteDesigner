namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImageSwatchViewModel : ViewModel <ImageSwatchView>
{
    public readonly Cluster<LabColor> Cluster; 
        
    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string frequency = string.Empty;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;

    public ImageSwatchViewModel(Cluster<LabColor> cluster)
    {
        this.Cluster = cluster;
        RgbColor rgbColor = cluster.Payload.ToRgb();
        this.ColorBrush = rgbColor.ToBrush();
        this.RgbHex = string.Format("# {0}", rgbColor.ToRgbHexString());
        this.RgbDec = string.Format("\u2022 {0}", rgbColor.ToRgbDecString());
        this.Localize(); 

        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    private void OnLanguageChanged(LanguageChangedMessage message) => this.Localize(); 

    private void Localize() 
    {
        double frequencyPercentage = 100.0 * this.Cluster.Count / (double)this.Cluster.Total;
        string occurences = this.Localize("Imaging.Occurences");
        this.Frequency = string.Format("{0}: {1:F1} %", occurences, frequencyPercentage);
    }
}
