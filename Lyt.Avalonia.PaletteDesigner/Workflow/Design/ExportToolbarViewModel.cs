namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ExportToolbarViewModel : ViewModel<ExportToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    private int selectedFileFormatIndex;

    [ObservableProperty]
    private ObservableCollection<FileFormatViewModel> fileFormats;


    public ExportToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.toaster = App.GetRequiredService<IToaster>();

        this.FileFormats = [];
        foreach (PaletteExportFormat resourceFormat in PaletteExportFormatExtensions.AvailableFormats)
        {
            this.FileFormats.Add(new FileFormatViewModel(resourceFormat));
        }

        this.SelectedFileFormatIndex = 0;
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnRandomize() => this.paletteDesignerModel.RandomizePalette();

    [RelayCommand]
    public void OnExport()
    {
        var fileFormatViewModel = this.FileFormats[this.SelectedFileFormatIndex];
        PaletteExportFormat exportFormat = fileFormatViewModel.PaletteExportFormat;
        bool success = this.paletteDesignerModel.ExportPalette(exportFormat, out string message);
        if (success)
        {
            this.toaster.Show(
                "Exported!", "Palette sucessfully exported to: " + message,
                dismissDelay: 5_000, InformationLevel.Success);
        }
        else
        {
            this.toaster.Show(
                "Error", "Failed to export palette: " + message,
                dismissDelay: 12_000, InformationLevel.Error);
        }
    }
}
