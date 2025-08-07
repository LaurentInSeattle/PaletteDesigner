namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadeSelectionToolbarViewModel : ViewModel<ShadeSelectionToolbarView>
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

    [ObservableProperty]
    private bool showShadesPresets;

    [ObservableProperty]
    private bool showShadesValues;

    public ShadeSelectionToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);

        this.ShowShadesPresets = false;
        this.ShowShadesValues = true;
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    partial void OnShowShadesPresetsChanged(bool value)
        => this.Messenger.Publish(new PresetsVisibilityMessage(value));

    partial void OnShowShadesValuesChanged(bool value)
        => this.Messenger.Publish(new ShadesValuesVisibilityMessage(value));

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

    [RelayCommand]
    public void OnDisplayMode(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ShadesValuesDisplayMode mode = 
                Enum.TryParse(tag, out ShadesValuesDisplayMode kind) ? kind : ShadesValuesDisplayMode.Hex;
            this.paletteDesignerModel.ShadesValuesDisplayMode = mode;
            this.Messenger.Publish(new ModelShadesDisplayModeUpdated());
        }
    }
    

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage _)
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
