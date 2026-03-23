namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shared;

using Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class FileFormatViewModel : ViewModel<FileFormatView>
{
    private readonly PaletteExportFormat paletteExportFormat;

    [ObservableProperty]
    public partial string Name { get; set; }

    public FileFormatViewModel(PaletteExportFormat resourceFormat)
    {
        this.paletteExportFormat = resourceFormat;
        this.Name = this.paletteExportFormat.ToFriendlyName();
    }

    public PaletteExportFormat PaletteExportFormat => this.paletteExportFormat;

}
