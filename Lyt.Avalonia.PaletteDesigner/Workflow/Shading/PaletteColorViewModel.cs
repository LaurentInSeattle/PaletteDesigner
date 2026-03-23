namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public partial class PaletteColorViewModel : ViewModel<PaletteColorView>
{
    [ObservableProperty]
    public partial SolidColorBrush BaseBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush LighterBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush LightBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush DarkBrush { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush DarkerBrush { get; set; }

    [ObservableProperty]
    public partial bool IsVisible { get; set; }

    public PaletteColorViewModel()
    {
        LighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        LightBrush = new SolidColorBrush(Colors.LightBlue);
        BaseBrush = new SolidColorBrush(Colors.Blue);
        DarkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        DarkerBrush = new SolidColorBrush(Colors.DarkBlue);

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
