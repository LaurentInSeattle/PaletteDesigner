namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

using HsvColor = Model.ColorObjects.HsvColor;

public sealed partial class ImageSwatchViewModel : ViewModel <ImageSwatchView>
{
    public readonly HsvColor HsvColor; 

    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;

    public ImageSwatchViewModel(RgbColor rgbColor, HsvColor hsvColor)
    {
        this.HsvColor = hsvColor;
        this.ColorBrush = rgbColor.ToBrush();
        this.RgbHex = hsvColor.ToRgbHexString();
        this.RgbDec = hsvColor.ToRgbDecimalString();
    }
}
