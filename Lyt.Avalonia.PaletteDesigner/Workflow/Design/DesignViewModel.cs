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

    public Palette Palette => this.paletteDesignerModel.ActiveProject.Palette;

    private void OnModelUpdated(ModelUpdatedMessage message)
    {
        this.ColorWheelViewModel.UpdateShadesBitmap(this.Palette.Primary.Base.H);
        this.PalettePreviewViewModel.Update(this.Palette);
    }
}
