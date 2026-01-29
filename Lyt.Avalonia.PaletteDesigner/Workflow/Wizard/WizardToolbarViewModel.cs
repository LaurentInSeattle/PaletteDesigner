namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

public sealed partial class WizardToolbarViewModel : ViewModel<WizardToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    //[ObservableProperty]
    //private bool isDeepAlgorithmStrength;

    //[ObservableProperty]
    //private double clustersSliderValue;

    //[ObservableProperty]
    //private string clustersValue = string.Empty;

    public WizardToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        //this.IsDeepAlgorithmStrength = false;
        //this.ClustersSliderValue = 10.0;
        //this.UpdateSliderLabel();
    }

    //partial void OnIsDeepAlgorithmStrengthChanged(bool value)
    //    => this.paletteDesignerModel.IsDeepImagingAlgorithmStrength = value;

    //public void UpdateSliderLabel()
    //    => this.ClustersValue = string.Format("{0:D}", (int)this.ClustersSliderValue);

    //partial void OnClustersSliderValueChanged(double value)
    //{
    //    this.UpdateSliderLabel();
    //    this.paletteDesignerModel.ImagingAlgorithmClusters = (int)value;
    //}

    //public void ProgrammaticUpdate(ColorSwatches swatches)
    //{
    //    this.IsDeepAlgorithmStrength = swatches.IsDeepAlgorithmStrength;
    //    this.ClustersSliderValue = swatches.Swatches.Count;
    //}

    [RelayCommand]
#pragma warning disable CA1822  // Mark members as static
    // Relay commands cannot be static
    public void OnRecalculate()
    {
        var viewModel = App.GetRequiredService<ImagingViewModel>();
        viewModel.ReProcessBitmap();
    }
#pragma warning restore CA1822 // Mark members as static
}
