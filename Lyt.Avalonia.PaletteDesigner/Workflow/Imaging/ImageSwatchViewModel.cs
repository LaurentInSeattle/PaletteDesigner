namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImageSwatchViewModel : ViewModel <ImageSwatchView>, IRecipient<LanguageChangedMessage>
{
    public readonly Cluster<LabColor> Cluster;

    [ObservableProperty]
    public partial SolidColorBrush ColorBrush { get; set; }

    [ObservableProperty]
    public partial string Frequency { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string RgbHex { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string RgbDec { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Hsv { get; set; } = string.Empty;

    public ImageSwatchViewModel(Cluster<LabColor> cluster)
    {
        this.Cluster = cluster;
        RgbColor rgbColor = cluster.Payload.ToRgb();
        this.ColorBrush = rgbColor.ToBrush();
        this.RgbHex = string.Format("# {0}", rgbColor.ToRgbHexString());
        this.RgbDec = string.Format("\u2022 {0}", rgbColor.ToRgbDecString());
        this.Localize(); 

        this.Subscribe<LanguageChangedMessage>();
    }

    public void Receive(LanguageChangedMessage message) => this.Localize(); 

    private void Localize() 
    {
        double frequencyPercentage = 100.0 * this.Cluster.Count / (double)this.Cluster.Total;
        string occurences = this.Localize("Imaging.Occurences");
        this.Frequency = string.Format("{0}: {1:F1} %", occurences, frequencyPercentage);
    }
}
