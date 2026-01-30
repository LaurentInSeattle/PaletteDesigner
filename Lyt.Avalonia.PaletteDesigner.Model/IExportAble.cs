namespace Lyt.Avalonia.PaletteDesigner.Model;

public interface IExportAble
{
    // Can export to ADOBE AseDocument
    AseDocument ToAseDocument();

    // Can export to JSon string
    string ToJsonString(FileManagerModel fileManager);

    // Can export to text formats using CSX and produce CSX TemplateParameters
    // for use with Lyt.Templator TemplateEngine
    Parameters ToTemplateParameters();
}
