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
    private bool isVisible;

    public PaletteColorViewModel()
    {
        this.lighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.lightBrush = new SolidColorBrush(Colors.LightBlue);
        this.baseBrush = new SolidColorBrush(Colors.Blue);
        this.darkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.darkerBrush = new SolidColorBrush(Colors.DarkBlue);

        this.Show();
    }

    public void Show(bool show = true) => this.IsVisible = show;

    public void Update(Shades shades)
    {
        this.Show();

        this.LighterBrush = shades.Lighter.Color.ToBrush();
        this.LightBrush = shades.Light.Color.ToBrush();
        this.BaseBrush = shades.Base.Color.ToBrush();
        this.DarkBrush = shades.Dark.Color.ToBrush();
        this.DarkerBrush = shades.Darker.Color.ToBrush();
    }
}
