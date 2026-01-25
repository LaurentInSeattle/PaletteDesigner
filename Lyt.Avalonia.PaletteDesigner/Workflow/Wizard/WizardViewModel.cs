namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using global::Avalonia.Media.Imaging;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardViewModel : ViewModel<WizardView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isLoaded;
    private bool isProgrammaticUpdate;
    private bool isFirstActivate;


    //[ObservableProperty]
    //private ImagingToolbarViewModel imagingToolbarViewModel;

    //[ObservableProperty]
    //private ExportToolbarViewModel exportToolbarViewModel;

    #region Observable Properties 

    [ObservableProperty]
    private WizardThemeViewModel lightThemeViewModel;

    [ObservableProperty]
    private WizardThemeViewModel darkThemeViewModel;

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    private string wheelValue = string.Empty;

    private double wheel;

    [ObservableProperty]
    private double curvePowerSliderValue;

    [ObservableProperty]
    private string curvePowerValue = string.Empty;

    private double curvePower;

    [ObservableProperty]
    private double curveAngleStepSliderValue;

    [ObservableProperty]
    private string curveAngleStepValue = string.Empty;

    private int curveAngleStep;

    [ObservableProperty]
    private double wheelAngleStepSliderValue;

    [ObservableProperty]
    private string wheelAngleStepValue = string.Empty;

    private double wheelAngleStep;

    [ObservableProperty]
    private double lightnessSliderValue;

    [ObservableProperty]
    private string lightnessValue = string.Empty;

    private double lightness;

    [ObservableProperty]
    private double highlightsSliderValue;

    [ObservableProperty]
    private string highlightsValue = string.Empty;

    private double highlights;

    [ObservableProperty]
    private double shadowsSliderValue;

    [ObservableProperty]
    private string shadowsValue = string.Empty;

    private double shadows;

    [ObservableProperty]
    private double styleSliderValue;

    [ObservableProperty]
    private string styleValue = string.Empty;

    private int style;

    #endregion Observable Properties 

    public WizardViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.LightThemeViewModel = new WizardThemeViewModel(this.paletteDesignerModel, PaletteThemeVariant.Light);
        this.DarkThemeViewModel = new WizardThemeViewModel(this.paletteDesignerModel, PaletteThemeVariant.Dark);

        this.isFirstActivate = true;

        //this.ImagingToolbarViewModel = new();
        //this.ExportToolbarViewModel = new(PaletteFamily.Image);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        this.CreateSwatches();
        //if (this.paletteDesignerModel.ActiveProject is not Project project)
        //{
        //    return;
        //}

        //if (project.Swatches is not ColorSwatches swatches)
        //{
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(swatches.ImagePath))
        //{
        //    return;
        //}

        //this.ImagingToolbarViewModel.ProgrammaticUpdate(swatches);
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);

        // DEBUG !
        if ( this.isFirstActivate)
        {
            this.isFirstActivate = false;
            Schedule.OnUiThread(
                120, this.paletteDesignerModel.WizardPaletteReset, DispatcherPriority.Background);
            return;
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
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.wheel = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetWheel(value);
    }

    partial void OnCurvePowerSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.curvePower = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetCurvePower(value);
    }

    partial void OnCurveAngleStepSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.curveAngleStep = (int)value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetCurveAngleStep(this.curveAngleStep);
    }

    partial void OnWheelAngleStepSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.wheelAngleStep = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetWheelAngleStep(value);
    }

    partial void OnHighlightsSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.highlights = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetHighlights(value);
    }

    partial void OnLightnessSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.lightness = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetLightness(value);
    }

    partial void OnShadowsSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.shadows = value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetShadows(value);
    }

    partial void OnStyleSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.style = (int)value;
        this.UpdateLabels();
        this.paletteDesignerModel.WizardPaletteSetStyle(this.style);
    }

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:D} \u00B0", (int)(0.5 + this.wheel));
        this.CurvePowerValue = string.Format("{0:F1}", this.curvePower);
        this.CurveAngleStepValue = string.Format("{0:D}", this.curveAngleStep);
        this.WheelAngleStepValue = string.Format("{0:F1} \u00B0", this.wheelAngleStep);
        this.LightnessValue = string.Format("{0:D} %", (int)(100.0 * this.highlights));
        this.HighlightsValue = string.Format("{0:D} %", (int)(100.0 * this.highlights));
        this.ShadowsValue = string.Format("{0:D} %", (int)(100.0 * this.shadows));

        // TODO ~ Needed ? 
        // this.StyleValue = ...
    }
}
