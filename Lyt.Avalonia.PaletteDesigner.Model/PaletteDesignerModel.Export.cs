namespace Lyt.Avalonia.PaletteDesigner.Model;

using static Lyt.Persistence.FileManagerModel;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool ExportPalette(PaletteFamily paletteFamily, PaletteExportFormat exportFormat, out string message)
    {
        if ( this.ActiveProject is null)
        {
            message = "No active project.";
            return false;
        }

        IExportAble? exportable = null;
        if (paletteFamily == PaletteFamily.Designed)
        {
            exportable = this.ActiveProject.Palette;
        }
        else if (paletteFamily == PaletteFamily.Image)
        {
            exportable = this.ActiveProject.Swatches;
        }
        else if (paletteFamily == PaletteFamily.Wizard)
        {
            exportable = this.ActiveProject.WizardPalette;
        }

        if (exportable is null)
        {
            message = "Unsupported palette family.";
            return false;
        } 

        return this.Export(exportable, exportFormat, paletteFamily, out message);
    }

    private bool Export(
        IExportAble exportable, 
        PaletteExportFormat exportFormat,
        PaletteFamily paletteFamily,
        out string message)
    {
        string? newPath = string.Empty;

        try
        {
            string targetName = exportFormat.ExportTargetFileName(paletteFamily);
            string name = string.Concat(targetName, "_", TimestampString());

            if (exportFormat == PaletteExportFormat.AdobeAse)
            {
                // Convert the palette, save on disk with time stamp
                var aseDocument = exportable.ToAseDocument();
                string fullName = string.Concat(name, ".ase");
                string path =
                    this.fileManager.MakePath(Area.User, Kind.BinaryNoExtension, fullName);
                aseDocument.Save(path);
                newPath = path;
                return true;
            }
            else if (exportFormat == PaletteExportFormat.ApplicationJSon)
            {
                // Create a JSON directly from the palette, save on disk with time stamp
                string serializedJson = exportable.ToJsonString(this.fileManager);
                string path = this.fileManager.MakePath(Area.User, Kind.Json, name);
                File.WriteAllText(path, serializedJson);
                newPath = path;
                return true;
            }
            else
            {
                Parameters parameters = exportable.ToTemplateParameters();
                ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
                string template =
                    ResourcesUtilities.LoadEmbeddedTextResource(
                        exportFormat.ResourceFileName(paletteFamily), out string? _);
                var templator = new TextGenerator(template);
                var result = templator.Generate(parameters);
                if (result.Item1)
                {
                    // Create a text file from the palette, save on disk with time stamp
                    this.fileManager.Save<string>(Area.User, Kind.Text, name, result.Item2, AppJsonContext.Default.String);

                    // rename to .axaml or xaml or whatever
                    string extension = exportFormat.ExtensionFileName();
                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
                    newPath = path.ChangeFileExtension(extension);
                    return true;
                }
                else
                {
                    newPath = "Failed to generate output file: " + result.Item2;
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            newPath = ex.Message;
            return false;
        }
        finally
        {
            message = newPath is null ? string.Empty : newPath;
        }
    }
}

