namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

using static GeneralExtensions; 

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    #region The 20 observable brush properties 

    [ObservableProperty]
    private SolidColorBrush primaryBaseBrush = new();

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
    private ShadesValuesViewModel primaryShadesValues;

    [ObservableProperty] 
    private ShadesValuesViewModel complementaryShadesValues;

    [ObservableProperty]
    private PaletteColorViewModel primaryShades;

    [ObservableProperty]
    private PaletteColorViewModel complementaryShades;

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    private double saturationSliderValue;

    [ObservableProperty]
    private double brightnessSliderValue;

    [ObservableProperty]
    private string wheelValue = string.Empty;

    [ObservableProperty]
    private string saturationValue = string.Empty;

    [ObservableProperty]
    private string brightnessValue = string.Empty;

    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate; 

    private double wheel;

    private double saturation;

    private double brightness;

    public PalettePreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PrimaryShadesValues = new ShadesValuesViewModel("Primary");
        this.ComplementaryShadesValues = new ShadesValuesViewModel("Complementary");
        this.PrimaryShades = new ();
        this.ComplementaryShades = new();
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.WheelSliderValue = 0.0;
        this.SaturationSliderValue = 0.67;
        this.BrightnessSliderValue = 0.67;
        this.WheelValue = string.Empty;
        this.SaturationValue = string.Empty;
        this.BrightnessValue = string.Empty;
        this.UpdateLabels();
    }

    partial void OnWheelSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return; 
        } 

        this.wheel = value;
        this.UpdateLabels();
        this.paletteDesignerModel.UpdatePalettePrimaryWheel(value);
    }

    partial void OnSaturationSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.saturation = value;
        this.UpdateLabels();
        this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    partial void OnBrightnessSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.brightness = value;
        this.UpdateLabels();
        this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);
        this.SaturationValue = string.Format("{0:F1} %", this.saturation * 100.0);
        this.BrightnessValue = string.Format("{0:F1} %", this.brightness * 100.0);
    }

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        With(ref this.isProgrammaticUpdate, () =>
        {
            this.wheel = palette.PrimaryWheel; 
            this.WheelSliderValue = palette.PrimaryWheel;
            var primary = palette.Primary.Base;
            this.saturation = primary.S; 
            this.SaturationSliderValue = primary.S;
            this.brightness = primary.V;
            this.BrightnessSliderValue = primary.V;
            this.UpdateLabels();
        });

        Shades shades = palette.Primary;
        this.PrimaryLighterBrush = shades.Lighter.ToBrush();
        this.PrimaryLightBrush = shades.Light.ToBrush();
        this.PrimaryBaseBrush = shades.Base.ToBrush();
        this.PrimaryDarkBrush = shades.Dark.ToBrush();
        this.PrimaryDarkerBrush = shades.Darker.ToBrush();

        this.PrimaryShadesValues.Update(shades);
        this.PrimaryShades.Update(shades);

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
        if (hasComplementary)
        {
            this.ComplementaryShades.Update(shades);
            this.ComplementaryShadesValues.Update(shades);
        }

        this.ComplementaryShades.Show(show: hasComplementary);
        this.ComplementaryShadesValues.Show(show: hasComplementary);
    }
}
