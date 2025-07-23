namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ModelSelectionToolbarViewModel : ViewModel<ModelSelectionToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private SolidColorBrush primaryBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondary1BaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondary2BaseBrush = new();

    [ObservableProperty]
    private bool isPrimaryVisible;

    [ObservableProperty]
    private bool isComplementaryVisible;

    [ObservableProperty]
    private bool isSecondary1Visible;

    [ObservableProperty]
    private bool isSecondary2Visible;

    [ObservableProperty]
    private bool isShadingDisabled;

    public ModelSelectionToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnModelSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            PaletteKind paletteKind = Enum.TryParse(tag, out PaletteKind kind) ? kind : PaletteKind.Unknown;
            if (paletteKind != PaletteKind.Unknown)
            {
                // Update model 
                this.paletteDesignerModel.UpdatePaletteKind(paletteKind);
            }
        }
    }

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

    [RelayCommand]
    public void OnColorSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            WheelKind wheel = Enum.TryParse(tag, out WheelKind kind) ? kind : WheelKind.Unknown;
            this.paletteDesignerModel.UpdatePaletteWheelShadeMode(wheel);
        }
    }

    private void OnModelUpdated(ModelUpdatedMessage _)
    {
        var palette = this.Palette;
        this.PrimaryBaseBrush = palette.Primary.Base.ToBrush();
        this.ComplementaryBaseBrush = palette.Complementary.Base.ToBrush();
        this.Secondary1BaseBrush = palette.Secondary1.Base.ToBrush();
        this.Secondary2BaseBrush = palette.Secondary2.Base.ToBrush();

        this.IsShadingDisabled = palette.AreShadesLocked; 

        PaletteKind paletteKind = palette.Kind;
        this.IsPrimaryVisible = true;
        this.IsComplementaryVisible = paletteKind.HasComplementaryMarker();
        this.IsSecondary1Visible = paletteKind.HasSecondary1Marker();
        this.IsSecondary2Visible = paletteKind.HasSecondary2Marker();
    }
}
