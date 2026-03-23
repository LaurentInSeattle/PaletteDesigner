namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public sealed partial class MaxiPaletteViewModel : ViewModel<MaxiPaletteView>
{
    #region The 20 observable brush properties 

    [ObservableProperty]
    public partial SolidColorBrush PrimaryBaseBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush PrimaryLighterBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush PrimaryLightBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush PrimaryDarkBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush PrimaryDarkerBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush ComplementaryBaseBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush ComplementaryLighterBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush ComplementaryLightBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush ComplementaryDarkBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush ComplementaryDarkerBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryTopBaseBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryTopLighterBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryTopLightBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryTopDarkBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryTopDarkerBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryBotBaseBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryBotLighterBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryBotLightBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryBotDarkBrush { get; set; } = new();

    [ObservableProperty]
    public partial SolidColorBrush SecondaryBotDarkerBrush { get; set; } = new();

    #endregion 20 observable brush properties 

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        Shades shades = palette.Primary;
        this.PrimaryLighterBrush = shades.Lighter.ToBrush();
        this.PrimaryLightBrush = shades.Light.ToBrush();
        this.PrimaryBaseBrush = shades.Base.ToBrush();
        this.PrimaryDarkBrush = shades.Dark.ToBrush();
        this.PrimaryDarkerBrush = shades.Darker.ToBrush();

        if ((colorCount == 1) || (colorCount == 2))
        {
            // Secondary shades same as Primary 
            this.SecondaryTopLighterBrush = shades.Lighter.ToBrush();
            this.SecondaryTopLightBrush = shades.Light.ToBrush();
            this.SecondaryTopBaseBrush = shades.Base.ToBrush();
            this.SecondaryTopDarkBrush = shades.Dark.ToBrush();
            this.SecondaryTopDarkerBrush = shades.Darker.ToBrush();

            this.SecondaryBotLighterBrush = shades.Lighter.ToBrush();
            this.SecondaryBotLightBrush = shades.Light.ToBrush();
            this.SecondaryBotBaseBrush = shades.Base.ToBrush();
            this.SecondaryBotDarkBrush = shades.Dark.ToBrush();
            this.SecondaryBotDarkerBrush = shades.Darker.ToBrush();

            if (colorCount == 2)
            {
                // colorCount == 1 => Complementary same as primary 
                shades = palette.Complementary;
            }

            this.ComplementaryLighterBrush = shades.Lighter.ToBrush();
            this.ComplementaryLightBrush = shades.Light.ToBrush();
            this.ComplementaryBaseBrush = shades.Base.ToBrush();
            this.ComplementaryDarkBrush = shades.Dark.ToBrush();
            this.ComplementaryDarkerBrush = shades.Darker.ToBrush();
        }
        else // colorCount == 3 or 4 
        {
            if (colorCount == 4)
            {
                // colorCount == 3 => Complementary same as primary 
                shades = palette.Complementary;
            }

            // Use brushes for all shades set when checking color count 
            this.ComplementaryLighterBrush = shades.Lighter.ToBrush();
            this.ComplementaryLightBrush = shades.Light.ToBrush();
            this.ComplementaryBaseBrush = shades.Base.ToBrush();
            this.ComplementaryDarkBrush = shades.Dark.ToBrush();
            this.ComplementaryDarkerBrush = shades.Darker.ToBrush();

            shades = palette.Secondary1;
            this.SecondaryTopLighterBrush = shades.Lighter.ToBrush();
            this.SecondaryTopLightBrush = shades.Light.ToBrush();
            this.SecondaryTopBaseBrush = shades.Base.ToBrush();
            this.SecondaryTopDarkBrush = shades.Dark.ToBrush();
            this.SecondaryTopDarkerBrush = shades.Darker.ToBrush();

            shades = palette.Secondary2;
            this.SecondaryBotLighterBrush = shades.Lighter.ToBrush();
            this.SecondaryBotLightBrush = shades.Light.ToBrush();
            this.SecondaryBotBaseBrush = shades.Base.ToBrush();
            this.SecondaryBotDarkBrush = shades.Dark.ToBrush();
            this.SecondaryBotDarkerBrush = shades.Darker.ToBrush();
        }
    }
}
