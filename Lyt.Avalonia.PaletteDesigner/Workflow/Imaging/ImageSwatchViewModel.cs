namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImageSwatchViewModel : ViewModel <ImageSwatchView>
{
    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;


    public ImageSwatchViewModel(RgbColor rgbColor, Model.ColorObjects.HsvColor hsvColor)
    {
        this.ColorBrush = rgbColor.ToBrush();
        this.RgbHex = hsvColor.ToRgbHexString();
        this.RgbDec = hsvColor.ToRgbDecimalString();
    }
}
