namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ExportToolbarViewModel : ViewModel<ExportToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public ExportToolbarViewModel()
        => this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnExport()
    {
        bool success = this.paletteDesignerModel.ExportPalette(PaletteExportFormat.AvaloniaAxaml, out string message);
    }
}
