namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImagingToolbarViewModel : ViewModel<ImagingToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    //[ObservableProperty]
    //private SolidColorBrush primaryBaseBrush = new();

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

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    partial void OnIsDeepAlgorithmStrengthChanged(bool value)
        => this.paletteDesignerModel.IsDeepImagingAlgorithmStrength = value;

    public void UpdateSliderLabel()
        => this.ClustersValue = string.Format("{0:D}", (int)this.ClustersSliderValue);

    partial void OnClustersSliderValueChanged(double value)
    {
        this.UpdateSliderLabel();
        this.paletteDesignerModel.ImagingAlgorithmClusters = (int)value;
    }
    //partial void OnShowShadesValuesChanged(bool value)
    //    => this.Messenger.Publish(new ShadesValuesVisibilityMessage(value));

    //partial void OnShowTextSamplesChanged(bool value)
    //    => this.Messenger.Publish(new TextSamplesVisibilityMessage(value));

    [RelayCommand]
    public void OnLockSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ShadeMode shadeMode = Enum.TryParse(tag, out ShadeMode kind) ? kind : ShadeMode.Locked;
            this.paletteDesignerModel.UpdatePaletteShadeMode(shadeMode);
        }
    }
}
