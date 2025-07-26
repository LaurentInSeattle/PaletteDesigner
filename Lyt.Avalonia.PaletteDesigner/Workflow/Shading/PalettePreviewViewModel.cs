namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

using static GeneralExtensions; 

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    private readonly ShadesValuesViewModel PrimaryShadesValues;

    private readonly ShadesValuesViewModel Secondary1ShadesValues;

    private readonly ShadesValuesViewModel Secondary2ShadesValues;

    private readonly ShadesValuesViewModel ComplementaryShadesValues;

    private readonly PaletteColorViewModel PrimaryShades;

    private readonly PaletteColorViewModel ComplementaryShades;

    private readonly PaletteColorViewModel Secondary1Shades;

    private readonly PaletteColorViewModel Secondary2Shades;


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
    private PaletteColorViewModel leftShades;

    [ObservableProperty]
    private PaletteColorViewModel middleLeftShades;

    [ObservableProperty]
    private PaletteColorViewModel middleRightShades;

    [ObservableProperty]
    private PaletteColorViewModel rightShades;

    [ObservableProperty]
    private ShadesValuesViewModel topLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel topRightShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomRightShadesValues;

    // Cannot be an observable property because DataContext is set programmatically 
    private int PrimaryShadesColumnSpan
    {
        set
        {
            if (this.IsBound)
            {
                this.View.PrimaryShades.SetValue(Grid.ColumnSpanProperty, value);
            } 
        } 
    }

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    private string wheelValue = string.Empty;

    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate; 

    private double wheel;

    public PalettePreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PrimaryShadesValues = new ShadesValuesViewModel("Dominant");
        this.ComplementaryShadesValues = new ShadesValuesViewModel("Accent");
        this.Secondary1ShadesValues = new ShadesValuesViewModel("Discord #1");
        this.Secondary2ShadesValues = new ShadesValuesViewModel("Discord #2");
        this.TopLeftShadesValues     = this.PrimaryShadesValues;
        this.BottomLeftShadesValues  = this.PrimaryShadesValues;
        this.TopRightShadesValues    = this.PrimaryShadesValues;
        this.BottomRightShadesValues = this.PrimaryShadesValues;

        this.PrimaryShadesColumnSpan = 4;
        this.PrimaryShades = new ();
        this.ComplementaryShades = new();
        this.Secondary1Shades = new();
        this.Secondary2Shades = new();
        this.LeftShades = this.PrimaryShades;
        this.MiddleLeftShades = this.PrimaryShades;
        this.MiddleRightShades = this.PrimaryShades;
        this.RightShades = this.PrimaryShades;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.WheelSliderValue = 0.0;
        this.WheelValue = string.Empty;
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

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);
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
            this.wheel = palette.Primary.Wheel; 
            this.WheelSliderValue = palette.Primary.Wheel;
            var primaryColor = palette.Primary.Base.Color;
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

            this.TopLeftShadesValues = this.PrimaryShadesValues;
            this.BottomLeftShadesValues = this.PrimaryShadesValues;
            this.TopRightShadesValues = this.PrimaryShadesValues;
            this.LeftShades = this.PrimaryShades;
            this.MiddleLeftShades = this.PrimaryShades;
            this.MiddleRightShades = this.PrimaryShades;
            if (colorCount == 2)
            {
                // colorCount == 1 => Complementary same as primary 
                shades = palette.Complementary;
                this.PrimaryShadesColumnSpan = 5;
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
                this.RightShades = this.ComplementaryShades;
                this.ComplementaryShades.Update(shades);
                string name = "Accent"; 
                this.ComplementaryShadesValues.Update(name);
                this.ComplementaryShadesValues.Update(shades);
            }
            else
            {
                this.PrimaryShadesColumnSpan = 7;
                this.BottomRightShadesValues = this.PrimaryShadesValues;
            }

            this.ComplementaryLighterBrush = shades.Lighter.ToBrush();
            this.ComplementaryLightBrush = shades.Light.ToBrush();
            this.ComplementaryBaseBrush = shades.Base.ToBrush();
            this.ComplementaryDarkBrush = shades.Dark.ToBrush();
            this.ComplementaryDarkerBrush = shades.Darker.ToBrush();
        }
        else // colorCount == 3 or 4 
        {
            this.PrimaryShadesColumnSpan = 1;

            this.TopLeftShadesValues = this.PrimaryShadesValues;
            this.LeftShades = this.PrimaryShades;

            shades = palette.Secondary1;
            this.Secondary1Shades.Update(shades);
            this.Secondary1ShadesValues.Update(shades);
            this.TopRightShadesValues = this.Secondary1ShadesValues;
            this.MiddleLeftShades = this.Secondary1Shades;

            shades = palette.Secondary2;
            this.Secondary2Shades.Update(shades);
            this.Secondary2ShadesValues.Update(shades);
            this.BottomLeftShadesValues = this.Secondary2ShadesValues;
            this.MiddleRightShades = this.Secondary2Shades;

            if (colorCount == 4)
            {
                shades = palette.Complementary;
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
                this.ComplementaryShades.Update(shades);
                this.ComplementaryShadesValues.Update(shades);
                this.RightShades = this.ComplementaryShades;
            }
            else
            {
                // colorCount == 3 => Complementary same as primary 
                shades = palette.Primary;
                this.BottomRightShadesValues = this.PrimaryShadesValues;
                this.RightShades = this.PrimaryShades; 
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

#region Saturation and Brightness sliders 
/*
 * 
            <TextBlock
                Grid.Row="1"
                Text="Saturation"
                Theme="{StaticResource Medium}"
                HorizontalAlignment="Right" 
                />
            <Slider
                Grid.Row="1" Grid.Column="1"
                Margin="8 0 8 0"
                Minimum="0.0" Maximum="1.0" 
                SmallChange="0.01" LargeChange="0.05"
                TickFrequency="0.1" TickPlacement="BottomRight"
                Value="{Binding SaturationSliderValue}"
                HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                />
            <TextBlock
                Grid.Row="1" Grid.Column="2"
                Text="{Binding SaturationValue}"
                Theme="{StaticResource Medium}"
                />
            <TextBlock
                Grid.Row="2"
                Text="Brightness"
                Theme="{StaticResource Medium}"
                HorizontalAlignment="Right" 
                />
            <Slider
                Grid.Row="2" Grid.Column="1"
                Margin="8 0 8 0"
                Minimum="0.0" Maximum="1.0"
                SmallChange="0.01" LargeChange="0.05"
                TickFrequency="0.1" TickPlacement="BottomRight"
                Value="{Binding BrightnessSliderValue}"
                HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                />
            <TextBlock
                Grid.Row="2" Grid.Column="2"
                Text="{Binding BrightnessValue}"
                Theme="{StaticResource Medium}"
                />
 *
    [ObservableProperty]
    private double saturationSliderValue;

    [ObservableProperty]
    private double brightnessSliderValue;

    [ObservableProperty]
    private string saturationValue = string.Empty;

    [ObservableProperty]
    private string brightnessValue = string.Empty;

    private double saturation;

    private double brightness;

CTOR:
        this.SaturationSliderValue = 0.67;
        this.BrightnessSliderValue = 0.67;
        this.SaturationValue = string.Empty;
        this.BrightnessValue = string.Empty;

    partial void OnSaturationSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.saturation = value;
        this.UpdateLabels();
        //this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    partial void OnBrightnessSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.brightness = value;
        this.UpdateLabels();
        //this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);
        this.SaturationValue = string.Format("{0:F1} %", this.saturation * 100.0);
        this.BrightnessValue = string.Format("{0:F1} %", this.brightness * 100.0);
    }

Update: 

        With(ref this.isProgrammaticUpdate, () =>
        {
            this.wheel = palette.Primary.Wheel; 
            this.WheelSliderValue = palette.Primary.Wheel;
            var primaryColor = palette.Primary.Base.Color;

            this.saturation = primaryColor.S; 
            this.SaturationSliderValue = primaryColor.S;
            this.brightness = primaryColor.V;
            this.BrightnessSliderValue = primaryColor.V;
            this.UpdateLabels();
        });

*/
#endregion Saturation and Brightness sliders 
