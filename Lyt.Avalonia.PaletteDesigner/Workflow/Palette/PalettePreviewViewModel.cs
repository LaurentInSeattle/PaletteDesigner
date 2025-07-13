namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    #region The 20 observable brush properties 

    [ObservableProperty]
    private SolidColorBrush baseBaseBrush; 

    [ObservableProperty]
    private SolidColorBrush baseLighterBrush;

    [ObservableProperty]
    private SolidColorBrush baseLightBrush;

    [ObservableProperty]
    private SolidColorBrush baseDarkBrush;

    [ObservableProperty]
    private SolidColorBrush baseDarkerBrush;

    [ObservableProperty]
    private SolidColorBrush complementaryBaseBrush;

    [ObservableProperty]
    private SolidColorBrush complementaryLighterBrush;

    [ObservableProperty]
    private SolidColorBrush complementaryLightBrush;

    [ObservableProperty]
    private SolidColorBrush complementaryDarkBrush;

    [ObservableProperty]
    private SolidColorBrush complementaryDarkerBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryTopBaseBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryTopLighterBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryTopLightBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryTopDarkBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryTopDarkerBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryBotBaseBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryBotLighterBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryBotLightBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryBotDarkBrush;

    [ObservableProperty]
    private SolidColorBrush secondaryBotDarkerBrush;

    #endregion 20 observable brush properties 

    public PalettePreviewViewModel()
    {
        this.BaseLighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.BaseLightBrush = new SolidColorBrush(Colors.LightBlue);
        this.BaseBaseBrush = new SolidColorBrush(Colors.Blue);
        this.BaseDarkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.BaseDarkerBrush = new SolidColorBrush(Colors.DarkBlue);

        this.ComplementaryLighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.ComplementaryLightBrush = new SolidColorBrush(Colors.LightBlue);
        this.ComplementaryBaseBrush = new SolidColorBrush(Colors.Blue);
        this.ComplementaryDarkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.ComplementaryDarkerBrush = new SolidColorBrush(Colors.DarkBlue);

        this.SecondaryTopLighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.SecondaryTopLightBrush = new SolidColorBrush(Colors.LightBlue);
        this.SecondaryTopBaseBrush = new SolidColorBrush(Colors.Blue);
        this.SecondaryTopDarkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.SecondaryTopDarkerBrush = new SolidColorBrush(Colors.DarkBlue);

        this.SecondaryBotLighterBrush = new SolidColorBrush(Colors.AntiqueWhite);
        this.SecondaryBotLightBrush = new SolidColorBrush(Colors.LightBlue);
        this.SecondaryBotBaseBrush = new SolidColorBrush(Colors.Blue);
        this.SecondaryBotDarkBrush = new SolidColorBrush(Colors.DarkSlateBlue);
        this.SecondaryBotDarkerBrush = new SolidColorBrush(Colors.DarkBlue);

        var palette = new Palette (
            "Test", PaletteKind.MonochromaticComplementary,
            0.0, 1.0, 1.0, 
            0.15, 0.15);
        this.Update(palette);
    }

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        Shades shades = palette.Primary;
        this.BaseLighterBrush = shades.Lighter.ToBrush();
        this.BaseLightBrush = shades.Light.ToBrush();
        this.BaseBaseBrush = shades.Base.ToBrush();
        this.BaseDarkBrush = shades.Dark.ToBrush();
        this.BaseDarkerBrush = shades.Darker.ToBrush();

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
