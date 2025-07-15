namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public sealed partial class ShadesValuesViewModel : ViewModel<ShadesValuesView>
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private bool isVisible = true;

    [ObservableProperty]
    private string lighter = string.Empty;

    [ObservableProperty]
    private string light = string.Empty;

    [ObservableProperty]
    private string medium = string.Empty;

    [ObservableProperty]
    private string dark = string.Empty;

    [ObservableProperty]
    private string darker = string.Empty;

    public ShadesValuesViewModel(string name)
    {
        this.Name = name;
        this.Show(); 
    }

    public void Show ( bool show = true ) => this.IsVisible = show;

    public void Update(Shades shades)
    {
        // TODO: Various formats 
        this.Lighter = shades.Lighter.ToRgbHexString();
        this.Light = shades.Light.ToRgbHexString();
        this.Medium = shades.Base.ToRgbHexString();
        this.Dark = shades.Dark.ToRgbHexString();
        this.Darker = shades.Darker.ToRgbHexString();
    }
}
