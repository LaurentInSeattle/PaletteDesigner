namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ColorsDragPanelViewModel : ViewModel<ColorsDragPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ObservableCollection<ColorDragViewModel> colorDragViewModels;

    [ObservableProperty]
    private string paletteName;

    public ColorsDragPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorDragViewModels = [];
        this.PaletteName = string.Empty;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        List<ColorDragViewModel> list = [];
        this.Palette.ForAllShades((kind, shades) =>
        {
            var colorDragViewModel = new ColorDragViewModel(this.Palette, kind, shades);
            list.Add(colorDragViewModel);
        });

        this.ColorDragViewModels = new(list);
        this.PaletteName = this.Palette.Name;
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

}
