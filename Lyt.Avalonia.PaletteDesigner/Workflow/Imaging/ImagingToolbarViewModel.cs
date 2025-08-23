namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImagingToolbarViewModel : ViewModel<ImagingToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool isDeepAlgorithmStrength;

    [ObservableProperty]
    private double clustersSliderValue;

    [ObservableProperty]
    private string clustersValue = string.Empty;

    public ImagingToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.IsDeepAlgorithmStrength = false;
        this.ClustersSliderValue = 10.0;
        this.UpdateSliderLabel();
    }

    partial void OnIsDeepAlgorithmStrengthChanged(bool value)
        => this.paletteDesignerModel.IsDeepImagingAlgorithmStrength = value;

    public void UpdateSliderLabel()
        => this.ClustersValue = string.Format("{0:D}", (int)this.ClustersSliderValue);

    partial void OnClustersSliderValueChanged(double value)
    {
        this.UpdateSliderLabel();
        this.paletteDesignerModel.ImagingAlgorithmClusters = (int)value;
    }

    [RelayCommand]
    public void OnRecalculate()
    {
        var viewModel = App.GetRequiredService<ImagingViewModel>();
        viewModel.ReProcessBitmap();
    }

    public void ProgrammaticUpdate(ColorSwatches swatches)
    {
        this.IsDeepAlgorithmStrength = swatches.IsDeepAlgorithmStrength;
        this.ClustersSliderValue = swatches.Swatches.Count;
    }
}
