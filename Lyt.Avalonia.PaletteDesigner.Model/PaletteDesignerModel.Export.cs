﻿namespace Lyt.Avalonia.PaletteDesigner.Model;

using static Lyt.Persistence.FileManagerModel;

public sealed partial class PaletteDesignerModel : ModelBase
{
    private string? newPath = string.Empty;

    public bool ExportPalette(PaletteExportFormat exportFormat, out string message)
    {
        message = string.Empty;
        bool result = this.ActionPalette((palette) =>
        {
            try
            {
                string targetName = exportFormat.TargetFileName();
                string name = string.Concat(targetName, "_", TimestampString());

                if (exportFormat == PaletteExportFormat.AdobeAse)
                {
                    var aseDocument = palette.ToAseDocument();
                    string fullName = string.Concat(name, ".ase");
                    string path = 
                        this.fileManager.MakePath(Area.User, Kind.BinaryNoExtension, fullName);
                    aseDocument.Save(path);
                    this.newPath = path;
                    return true;
                }
                else if (exportFormat == PaletteExportFormat.ApplicationJSon)
                {
                    // Create a JSON directly from the palette, save on disk with time stamp
                    this.fileManager.Save(Area.User, Kind.Json, name, palette);
                    this.newPath = this.fileManager.MakePath(Area.User, Kind.Json, name);
                    return true;
                }
                else
                {
                    ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
                    string template = ResourcesUtilities.LoadEmbeddedTextResource(exportFormat.ResourceFileName(), out string? _);

                    Parameters parameters = palette.ToTemplateParameters();
                    var templator = new TextGenerator(template);
                    string result = templator.Generate(parameters);

                    // Create a text file from the palette, save on disk with time stamp
                    this.fileManager.Save(Area.User, Kind.Text, name, result);

                    // rename to .axaml or xaml or whatever
                    string extension = exportFormat.ExtensionFileName();
                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
                    this.newPath = path.ChangeFileExtension(extension);
                }

                return true;
            }
            catch (Exception ex)
            {
                this.newPath = ex.Message;
                return false;
            }
        });

        message = this.newPath is null ? string.Empty : this.newPath;
        return result;
    }
}
