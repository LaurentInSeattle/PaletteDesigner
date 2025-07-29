namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class FileFormatViewModel : ViewModel<FileFormatView>
{
    private readonly PaletteExportFormat paletteExportFormat;

    [ObservableProperty]
    private string name;

    public FileFormatViewModel(PaletteExportFormat resourceFormat)
    {
        this.paletteExportFormat = resourceFormat;
        this.Name = this.paletteExportFormat.ToFriendlyName();
    }

    public PaletteExportFormat PaletteExportFormat => this.paletteExportFormat;

}
