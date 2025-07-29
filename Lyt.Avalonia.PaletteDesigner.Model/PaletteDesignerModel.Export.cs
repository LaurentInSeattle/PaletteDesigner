namespace Lyt.Avalonia.PaletteDesigner.Model;

using static Lyt.Persistence.FileManagerModel;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool ExportPalette(PaletteExportFormat avaloniaAxaml, out string message)
    {
        message = string.Empty;
        ResourcesUtilities.SetResourcesPath(avaloniaAxaml.ResourcePath());
        string template = ResourcesUtilities.LoadEmbeddedTextResource(avaloniaAxaml.ResourceFileName(), out string? _);

        return this.ActionPalette((palette) =>
            {
                Parameters parameters = palette.ToTemplateParameters();
                var templator = new TextGenerator(template);
                string result = templator.Generate(parameters);

                // Create a text file from the palette, save on disk with time stamp
                string name = "AvaloniaPalette_" + FileManagerModel.TimestampString();
                this.fileManager.Save(
                    FileManagerModel.Area.User, FileManagerModel.Kind.Text, name, result);
                // rename to .axaml 
                // todo 
                return true;
            });
    }
}
