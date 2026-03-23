namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public sealed partial class WizardViewModel :
    ViewModel<WizardView>,
    IRecipient<ModelWizardUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isLoaded;
    private bool isProgrammaticUpdate;
    private bool isFirstActivate;

    #region Observable Properties 

    [ObservableProperty]
    public partial WizardToolbarViewModel WizardToolbarViewModel { get; set; }

    [ObservableProperty]
    public partial ExportToolbarViewModel ExportToolbarViewModel { get; set; }

    [ObservableProperty]
    public partial WizardThemeViewModel LightThemeViewModel { get; set; }

    [ObservableProperty]
    public partial WizardThemeViewModel DarkThemeViewModel { get; set; }

    [ObservableProperty]
    public partial WizardThemeValuesViewModel LightThemeValuesViewModel { get; set; }

    [ObservableProperty]
    public partial WizardThemeValuesViewModel DarkThemeValuesViewModel { get; set; }

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    public partial string WheelValue { get; set; } = string.Empty;

    private double wheel;

    [ObservableProperty]
    public partial double CurvePowerSliderValue { get; set; }

    [ObservableProperty]
    public partial string CurvePowerValue { get; set; } = string.Empty;

    private double curvePower;

    [ObservableProperty]
    public partial double CurveAngleStepSliderValue { get; set; }

    [ObservableProperty]
    public partial string CurveAngleStepValue { get; set; } = string.Empty;

    private int curveAngleStep;

    [ObservableProperty]
    public partial double WheelAngleStepSliderValue { get; set; }

    [ObservableProperty]
    public partial string WheelAngleStepValue { get; set; } = string.Empty;

    private double wheelAngleStep;

    [ObservableProperty]
    public partial double LightnessSliderValue { get; set; }

    [ObservableProperty]
    public partial string LightnessValue { get; set; } = string.Empty;

    private double lightness;

    [ObservableProperty]
    public partial double HighlightsSliderValue { get; set; }

    [ObservableProperty]
    public partial string HighlightsValue { get; set; } = string.Empty;

    private double highlights;

    [ObservableProperty]
    public partial double ShadowsSliderValue { get; set; }

    [ObservableProperty]
    public partial string ShadowsValue { get; set; } = string.Empty;

    private double shadows;

    [ObservableProperty]
    public partial double StyleSliderValue { get; set; }

    [ObservableProperty]
    public partial string StyleValue { get; set; } = string.Empty;

    private int style;

    #endregion Observable Properties 

    public WizardViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.LightThemeViewModel = 
            new WizardThemeViewModel(this.paletteDesignerModel, PaletteThemeVariant.Light);
        this.DarkThemeViewModel = 
            new WizardThemeViewModel(this.paletteDesignerModel, PaletteThemeVariant.Dark);
        this.LightThemeValuesViewModel =
            new WizardThemeValuesViewModel(this.paletteDesignerModel, PaletteThemeVariant.Light);
        this.DarkThemeValuesViewModel =
            new WizardThemeValuesViewModel(this.paletteDesignerModel, PaletteThemeVariant.Dark);


        this.isFirstActivate = true;
        this.Subscribe<ModelWizardUpdatedMessage>(); 

        this.WizardToolbarViewModel = new();
        this.ExportToolbarViewModel = new(PaletteFamily.Wizard);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.CreateSwatches();
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);

        // DEBUG !
        if (this.isFirstActivate)
        {
            this.isFirstActivate = false;
            Schedule.OnUiThread(
                120, this.paletteDesignerModel.WizardPaletteReset, DispatcherPriority.Background);
            return;
        }
    }

    public void Receive(ModelWizardUpdatedMessage message)
    {
        if (this.paletteDesignerModel.ActiveProject is null)
        {
            return;
        }

        var palette = this.paletteDesignerModel.ActiveProject.WizardPalette;
        if (palette.IsReset)
        {
            GeneralExtensions.With(ref this.isProgrammaticUpdate, () =>
            {
                this.WheelSliderValue = palette.BaseWheel;
                this.CurvePowerSliderValue = palette.CurvePower;
                this.CurveAngleStepSliderValue = palette.CurveAngleStep;
                this.WheelAngleStepSliderValue = palette.WheelAngleStep;
                this.LightnessSliderValue = palette.Lightness;
                this.HighlightsSliderValue = palette.Highlights;
                this.ShadowsSliderValue = palette.Shadows;
                this.StyleSliderValue = palette.ThemeVariantStyleIndex;
            });
        } 
    }

    private void CreateSwatches()
    {
        if (this.isLoaded)
        {
            return;
        }

        for (int row = 0; row < 5; row++)
        {
            var swatchKind = (SwatchKind)row;
            for (int index = 0; index < WizardPalette.PaletteWidth; index++)
            {
                var swatchViewModel =
                    new WizardSwatchViewModel(
                        this.paletteDesignerModel,
                        isGhost: false,
                        this.View.DragCanvas,
                        swatchKind,
                        index);
                var swatchView = swatchViewModel.CreateViewAndBind();
                this.View.AddSwatchView(swatchView, swatchKind, index);
            }
        }

        this.isLoaded = true;
    }

    partial void OnWheelSliderValueChanged(double value)
    {
        this.wheel = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetWheel(value);
    }

    partial void OnCurvePowerSliderValueChanged(double value)
    {
        this.curvePower = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetCurvePower(value);
    }

    partial void OnCurveAngleStepSliderValueChanged(double value)
    {
        this.curveAngleStep = (int)value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetCurveAngleStep(this.curveAngleStep);
    }

    partial void OnWheelAngleStepSliderValueChanged(double value)
    {
        this.wheelAngleStep = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetWheelAngleStep(value);
    }

    partial void OnHighlightsSliderValueChanged(double value)
    {
        this.highlights = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetHighlights(value);
    }

    partial void OnLightnessSliderValueChanged(double value)
    {
        this.lightness = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetLightness(value);
    }

    partial void OnShadowsSliderValueChanged(double value)
    {
        this.shadows = value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetShadows(value);
    }

    partial void OnStyleSliderValueChanged(double value)
    {
        this.style = (int)value;
        this.UpdateLabels();

        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.paletteDesignerModel.WizardPaletteSetStyle(this.style);
    }

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:D} \u00B0", (int)(0.5 + this.wheel));
        this.CurvePowerValue = string.Format("{0:F1}", this.curvePower);
        this.CurveAngleStepValue = string.Format("{0:D}", this.curveAngleStep);
        this.WheelAngleStepValue = string.Format("{0:F1} \u00B0", this.wheelAngleStep);
        this.LightnessValue = string.Format("{0:D} %", (int)(100.0 * this.lightness));
        this.HighlightsValue = string.Format("{0:D} %", (int)(100.0 * this.highlights));
        this.ShadowsValue = string.Format("{0:D} %", (int)(100.0 * this.shadows));

        // No Need to show a value for the Style slider 
        // this.StyleValue = ...
    }
}
