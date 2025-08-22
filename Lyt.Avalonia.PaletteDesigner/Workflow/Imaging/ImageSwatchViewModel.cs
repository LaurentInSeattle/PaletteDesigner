namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImageSwatchViewModel : ViewModel <ImageSwatchView>
{
    public readonly Cluster Cluster; 
        
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

    public ImageSwatchViewModel(Cluster cluster)
    {
        this.Cluster = cluster;
        RgbColor rgbColor = cluster.LabColor.ToRgb();
        this.ColorBrush = rgbColor.ToBrush();
        this.RgbHex = string.Format("# {0}", rgbColor.ToRgbHexString());
        this.RgbDec = string.Format("\u2022 {0}", rgbColor.ToRgbDecString());
        double frequencyPercentage = 100.0 * cluster.Count / (double)cluster.Total;
        this.Frequency = string.Format("\u03BD {0:F1} %", frequencyPercentage);
    }
}
