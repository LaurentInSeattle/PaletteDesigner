namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    #region The 20 observable brush properties 

    [ObservableProperty]
    private  SolidColorBrush primaryBaseBrush = new (); 

    [ObservableProperty]
    private SolidColorBrush primaryLighterBrush = new();

    [ObservableProperty]
    private SolidColorBrush primaryLightBrush = new();

    [ObservableProperty]
    private SolidColorBrush primaryDarkBrush = new();

    [ObservableProperty]
    private SolidColorBrush primaryDarkerBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryLighterBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryLightBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryDarkBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryDarkerBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryTopBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryTopLighterBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryTopLightBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryTopDarkBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryTopDarkerBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryBotBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryBotLighterBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryBotLightBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryBotDarkBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondaryBotDarkerBrush = new();

    #endregion 20 observable brush properties 

    [ObservableProperty]
    private string primaryLighter = string.Empty;

    [ObservableProperty]
    private  string primaryLight = string.Empty;

    [ObservableProperty]
    private string primaryBase = string.Empty;

    [ObservableProperty]
    private string primaryDark = string.Empty;

    [ObservableProperty]
    private string primaryDarker = string.Empty;

    [ObservableProperty]
    private string complementaryLighter = string.Empty;

    [ObservableProperty]
    private string complementaryLight = string.Empty;

    [ObservableProperty]
    private string complementaryBase = string.Empty;

    [ObservableProperty]
    private string complementaryDark = string.Empty;

    [ObservableProperty]
    private string complementaryDarker = string.Empty;

    [ObservableProperty]
    private double hueSliderValue;

    [ObservableProperty]
    private double saturationSliderValue;

    [ObservableProperty]
    private double brightnessSliderValue;

    private readonly PaletteDesignerModel paletteDesignerModel;

    private double hue;

    private double saturation;

    private double brightness;

    public PalettePreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.SaturationSliderValue = 0.67;
        this.BrightnessSliderValue = 0.67;
    }

    partial void OnHueSliderValueChanged(double value)
    {
        this.hue = value;
        this.Update();
    }

    partial void OnSaturationSliderValueChanged(double value)
    {
        this.saturation = value;
        this.Update(); 
    }

    partial void OnBrightnessSliderValueChanged(double value)
    {
        this.brightness = value;
        this.Update();
    }

    public void Update()
    {
        var lookup = this.paletteDesignerModel.ColorLookupTable;
        if (lookup is null) 
        {
            return;
        } 

        int anglePrimary = (int)Math.Round(this.hue* 10.0);
        double oppposite = (this.hue + 180.0).NormalizeAngleDegrees();
        int angleComplementary = (int)Math.Round(oppposite *10.0);
        if (lookup.TryGetValue(anglePrimary, out RgbColor? rgbColorPrimary) &&
            lookup.TryGetValue(angleComplementary, out RgbColor? rgbColorComplementary))
        {
            if ((rgbColorPrimary is null)|| (rgbColorComplementary is null))
            {
                return;
            }

            var hsvColorPrimary = rgbColorPrimary.ToHsv();
            var hsvColorComplementary = rgbColorComplementary.ToHsv();

            Debug.WriteLine(string.Format("Saturation: {0:F2}   Brightness: {1:F2}", this.saturation, this.brightness));
            var palette = new Palette(
                "Test", PaletteKind.MonochromaticComplementary,
                hsvColorPrimary.H,
                hsvColorComplementary.H,
                this.saturation, this.brightness,
                0.05, 0.30);
            this.Update(palette);
        } 
    }

    public void Update(Palette palette)
    {
        // For pure red, shades in Paletton (hue==0)
        // FF AA AA
        // D4 6A 6A
        // AA 39 39
        // 80 15 15
        // 55 00 00

        // For pure green, shades in Paletton (hue==180)
        // 88 CC 88
        // 55 AA 55
        // 2D 88 2D
        // 11 66 11 
        // 00 44 00

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

        this.PrimaryLighter = shades.Lighter.ToRgbHexString();
        this.PrimaryLight= shades.Light.ToRgbHexString();
        this.PrimaryBase= shades.Base.ToRgbHexString();
        this.PrimaryDark= shades.Dark.ToRgbHexString();
        this.PrimaryDarker= shades.Darker.ToRgbHexString();
        bool hasComplementary = false; 

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
                hasComplementary = true;
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
                hasComplementary = true;
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

        shades = palette.Complementary;
        this.ComplementaryLighter = ! hasComplementary ? string.Empty : shades.Lighter.ToRgbHexString();
        this.ComplementaryLight   = ! hasComplementary ? string.Empty : shades.Light.ToRgbHexString();
        this.ComplementaryBase    = ! hasComplementary ? string.Empty : shades.Base.ToRgbHexString();
        this.ComplementaryDark    = ! hasComplementary ? string.Empty : shades.Dark.ToRgbHexString();
        this.ComplementaryDarker  = ! hasComplementary ? string.Empty : shades.Darker.ToRgbHexString();
    }
}
