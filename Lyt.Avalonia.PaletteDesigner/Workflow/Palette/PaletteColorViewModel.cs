namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public partial class PaletteColorViewModel : ViewModel<PaletteColorView>
{
    [ObservableProperty]
    private SolidColorBrush baseBrush;

    [ObservableProperty]
    private SolidColorBrush lighterBrush;

    [ObservableProperty]
    private SolidColorBrush lightBrush;

    [ObservableProperty]
    private SolidColorBrush darkBrush;

    [ObservableProperty]
    private SolidColorBrush darkerBrush;

    [ObservableProperty]
    private bool isFlatLayout;

    [ObservableProperty]
    private bool isPrimaryLayout;

    public PaletteColorViewModel()
    {
        this.lighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.lightBrush = new SolidColorBrush(Colors.LightBlue);
        this.baseBrush = new SolidColorBrush(Colors.Blue);
        this.darkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.darkerBrush = new SolidColorBrush(Colors.DarkBlue);

        this.IsPrimaryLayout = true;
    }

    public void Update(Shades shades)
    {
        this.LighterBrush = shades.Lighter.ToBrush();
        this.LightBrush = shades.Light.ToBrush();
        this.BaseBrush = shades.Base.ToBrush();
        this.DarkBrush = shades.Dark.ToBrush();
        this.DarkerBrush = shades.Darker.ToBrush();
    }
}
