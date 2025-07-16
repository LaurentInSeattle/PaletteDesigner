namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class DesignViewModel : ViewModel<DesignView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorWheelViewModel colorWheelViewModel;

    [ObservableProperty]
    private PalettePreviewViewModel palettePreviewViewModel;

    public DesignViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorWheelViewModel = new ColorWheelViewModel(paletteDesignerModel);
        this.PalettePreviewViewModel = new PalettePreviewViewModel(paletteDesignerModel);

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.OnModelUpdated(null);
    }

    public Palette Palette => this.paletteDesignerModel.ActiveProject.Palette;

    private void OnModelUpdated(ModelUpdatedMessage? _)
    {
        var palette = this.Palette;
        this.ColorWheelViewModel.Update(palette);
        this.PalettePreviewViewModel.Update(palette);
    }
}
