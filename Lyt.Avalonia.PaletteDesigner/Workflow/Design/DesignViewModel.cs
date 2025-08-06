namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class DesignViewModel : ViewModel<DesignView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorWheelViewModel colorWheelViewModel;

    [ObservableProperty]
    private PalettePreviewViewModel palettePreviewViewModel;

    [ObservableProperty]
    private ModelSelectionToolbarViewModel modelSelectionToolbarViewModel;

    [ObservableProperty]
    private ExportToolbarViewModel exportToolbarViewModel;

    [ObservableProperty]
    private ShadeSelectionToolbarViewModel shadeSelectionToolbarViewModel;

    [ObservableProperty]
    private ShadesPresetsToolbarViewModel shadesPresetsToolbarViewModel;

    public DesignViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorWheelViewModel = new(paletteDesignerModel);
        this.PalettePreviewViewModel = new(paletteDesignerModel);
        this.ModelSelectionToolbarViewModel = new();
        this.ExportToolbarViewModel = new();
        this.ShadeSelectionToolbarViewModel = new();
        this.shadesPresetsToolbarViewModel = new();

        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelUpdated);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        // Need to wait so that canvas is ready 
        Schedule.OnUiThread(250, () =>
        {
            // For now 
            if (this.paletteDesignerModel.ActiveProject is null)
            {
                Debugger.Break();
                return;
            }

            var palette = this.Palette;
            palette.Reset();
            this.ColorWheelViewModel.OnWheelAngleChanged(WheelKind.Primary, palette.Primary.Wheel);
        }, DispatcherPriority.Background);

    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    private void OnModelUpdated(ModelPaletteUpdatedMessage? _)
    {
        var palette = this.Palette;
        this.ColorWheelViewModel.Update(palette);
        this.PalettePreviewViewModel.Update(palette);
    }
}
