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

    public DesignViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorWheelViewModel = new(paletteDesignerModel);
        this.PalettePreviewViewModel = new(paletteDesignerModel);
        this.modelSelectionToolbarViewModel = new();
        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        // Need to wait so that canvas is ready 
        Schedule.OnUiThread(550, () =>
        {
            // For now 
            if (this.paletteDesignerModel.ActiveProject is null)
            {
                Debugger.Break();
                return;
            }

            var palette = this.Palette;
            palette.Reset();
            this.OnModelUpdated(null);
        }, DispatcherPriority.Background);

    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    private void OnModelUpdated(ModelUpdatedMessage? _)
    {
        var palette = this.Palette;
        this.ColorWheelViewModel.Update(palette);
        this.PalettePreviewViewModel.Update(palette);
    }
}
