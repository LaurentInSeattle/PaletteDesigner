namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public sealed partial class ShadesValuesViewModel : ViewModel<ShadesValuesView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsVisible { get; set; } = true;

    [ObservableProperty]
    public partial string Lighter { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Light { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Medium { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Dark { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Darker { get; set; } = string.Empty;

    public ShadesValuesViewModel(string name)
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Name = name;
        this.Show();
    }

    public void Show(bool show = true) => this.IsVisible = show;

    public void Update(Shades shades)
    {
        var mode = this.paletteDesignerModel.ShadesValuesDisplayMode;
        switch (mode)
        {
            default:
            case ShadesValuesDisplayMode.Hex:
                this.Lighter = shades.Lighter.ToRgbHexString();
                this.Light = shades.Light.ToRgbHexString();
                this.Medium = shades.Base.ToRgbHexString();
                this.Dark = shades.Dark.ToRgbHexString();
                this.Darker = shades.Darker.ToRgbHexString();
                break;

            case ShadesValuesDisplayMode.Percent:
                this.Lighter = shades.Lighter.ToRgbPercentString();
                this.Light = shades.Light.ToRgbPercentString();
                this.Medium = shades.Base.ToRgbPercentString();
                this.Dark = shades.Dark.ToRgbPercentString();
                this.Darker = shades.Darker.ToRgbPercentString();
                break;

            case ShadesValuesDisplayMode.Decimal:
                this.Lighter = shades.Lighter.ToRgbDecimalString();
                this.Light = shades.Light.ToRgbDecimalString();
                this.Medium = shades.Base.ToRgbDecimalString();
                this.Dark = shades.Dark.ToRgbDecimalString();
                this.Darker = shades.Darker.ToRgbDecimalString();
                break;
        }
    }

    public void Update(string name) => this.Name = name;
}