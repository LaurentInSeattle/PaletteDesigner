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

        return this.Export(exportable, exportFormat, out message);
    }

    private bool Export(IExportAble exportable, PaletteExportFormat exportFormat, out string message)
    {
        string? newPath = string.Empty;

        try
        {
            string targetName = exportFormat.TargetFileName();
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
                string json = exportable.ToJsonString();
                string path = this.fileManager.MakePath(Area.User, Kind.Json, name);
                newPath = path.ChangeFileExtension(FileManagerModel.JsonExtension);
                return true;
            }
            else
            {
                Parameters parameters = exportable.ToTemplateParameters();

                ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
                string template =
                    ResourcesUtilities.LoadEmbeddedTextResource(exportFormat.ResourceFileName(), out string? _);
                var templator = new TextGenerator(template);
                var result = templator.Generate(parameters);
                if (result.Item1)
                {
                    // Create a text file from the palette, save on disk with time stamp
                    this.fileManager.Save(Area.User, Kind.Text, name, result.Item2);

                    // rename to .axaml or xaml or whatever
                    string extension = exportFormat.ExtensionFileName();
                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
                    newPath = path.ChangeFileExtension(extension);
                }
                else
                {
                    newPath = "Failed to generate output file: " + result.Item2;
                    return false;
                }
            }

            return true;
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

    #region Remove Unused Methods Later 
    //private bool ExportDesignedPalette(PaletteExportFormat exportFormat, out string message)
    //{
    //    message = string.Empty;
    //    bool result = this.ActionPalette((palette) =>
    //    {
    //        try
    //        {
    //            string targetName = exportFormat.TargetFileName();
    //            string name = string.Concat(targetName, "_", TimestampString());

    //            if (exportFormat == PaletteExportFormat.AdobeAse)
    //            {
    //                var aseDocument = palette.ToAseDocument();
    //                string fullName = string.Concat(name, ".ase");
    //                string path =
    //                    this.fileManager.MakePath(Area.User, Kind.BinaryNoExtension, fullName);
    //                aseDocument.Save(path);
    //                this.newPath = path;
    //                return true;
    //            }
    //            else if (exportFormat == PaletteExportFormat.ApplicationJSon)
    //            {
    //                // Create a JSON directly from the palette, save on disk with time stamp
    //                this.fileManager.Save(Area.User, Kind.Json, name, palette);
    //                this.newPath = this.fileManager.MakePath(Area.User, Kind.Json, name);
    //                return true;
    //            }
    //            else
    //            {
    //                Parameters parameters = palette.ToTemplateParameters();

    //                ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
    //                string template = 
    //                    ResourcesUtilities.LoadEmbeddedTextResource(exportFormat.ResourceFileName(), out string? _);
    //                var templator = new TextGenerator(template);
    //                var result = templator.Generate(parameters);
    //                if (result.Item1)
    //                {
    //                    // Create a text file from the palette, save on disk with time stamp
    //                    this.fileManager.Save(Area.User, Kind.Text, name, result.Item2);

    //                    // rename to .axaml or xaml or whatever
    //                    string extension = exportFormat.ExtensionFileName();
    //                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
    //                    this.newPath = path.ChangeFileExtension(extension);
    //                }
    //                else
    //                {
    //                    this.newPath = "Failed to generate output file: " + result.Item2;
    //                    return false;
    //                }
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            this.newPath = ex.Message;
    //            return false;
    //        }
    //    });

    //    message = this.newPath is null ? string.Empty : this.newPath;
    //    return result;
    //}

    //private bool ExportImagePalette(PaletteExportFormat exportFormat, out string message)
    //{
    //    message = string.Empty;
    //    bool result = this.ActionSwatches((swatches) =>
    //    {
    //        try
    //        {
    //            string targetName = exportFormat.TargetFileName();
    //            string name = string.Concat(targetName, "_", TimestampString());

    //            if (exportFormat == PaletteExportFormat.AdobeAse)
    //            {
    //                var aseDocument = swatches.ToAseDocument();
    //                string fullName = string.Concat(name, ".ase");
    //                string path =
    //                    this.fileManager.MakePath(Area.User, Kind.BinaryNoExtension, fullName);
    //                aseDocument.Save(path);
    //                this.newPath = path;
    //                return true;
    //            }
    //            else if (exportFormat == PaletteExportFormat.ApplicationJSon)
    //            {
    //                // Create a JSON directly from the palette, save on disk with time stamp
    //                this.fileManager.Save(Area.User, Kind.Json, name, swatches);
    //                this.newPath = this.fileManager.MakePath(Area.User, Kind.Json, name);
    //                return true;
    //            }
    //            else
    //            {
    //                Parameters parameters = swatches.ToTemplateParameters();
    //                ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
    //                string template =
    //                    ResourcesUtilities.LoadEmbeddedTextResource(
    //                        exportFormat.ResourceFileName(PaletteFamily.Image), out string? _);
    //                var templator = new TextGenerator(template);
    //                var result = templator.Generate(parameters);
    //                if (result.Item1)
    //                {
    //                    // Create a text file from the palette, save on disk with time stamp
    //                    this.fileManager.Save(Area.User, Kind.Text, name, result);

    //                    // rename to .axaml or xaml or whatever
    //                    string extension = exportFormat.ExtensionFileName();
    //                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
    //                    this.newPath = path.ChangeFileExtension(extension);
    //                }
    //                else
    //                {
    //                    this.newPath = "Failed to generate output file: " + result.Item2;
    //                    return false;
    //                }
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            this.newPath = ex.Message;
    //            return false;
    //        }
    //    });

    //    message = this.newPath is null ? string.Empty : this.newPath;
    //    return result;
    //}

    //private bool ExportWizardPalette(PaletteExportFormat exportFormat, out string message)
    //{
    //    message = string.Empty;
    //    bool result = this.ActionSwatches((swatches) =>
    //    {
    //        try
    //        {
    //            string targetName = exportFormat.TargetFileName();
    //            string name = string.Concat(targetName, "_", TimestampString());

    //            if (exportFormat == PaletteExportFormat.AdobeAse)
    //            {
    //                var aseDocument = swatches.ToAseDocument();
    //                string fullName = string.Concat(name, ".ase");
    //                string path =
    //                    this.fileManager.MakePath(Area.User, Kind.BinaryNoExtension, fullName);
    //                aseDocument.Save(path);
    //                this.newPath = path;
    //                return true;
    //            }
    //            else if (exportFormat == PaletteExportFormat.ApplicationJSon)
    //            {
    //                // Create a JSON directly from the palette, save on disk with time stamp
    //                this.fileManager.Save(Area.User, Kind.Json, name, swatches);
    //                this.newPath = this.fileManager.MakePath(Area.User, Kind.Json, name);
    //                return true;
    //            }
    //            else
    //            {
    //                Parameters parameters = swatches.ToTemplateParameters();
    //                ResourcesUtilities.SetResourcesPath(exportFormat.ResourcePath());
    //                string template =
    //                    ResourcesUtilities.LoadEmbeddedTextResource(
    //                        exportFormat.ResourceFileName(PaletteFamily.Image), out string? _);
    //                var templator = new TextGenerator(template);
    //                var result = templator.Generate(parameters);
    //                if (result.Item1)
    //                {
    //                    // Create a text file from the palette, save on disk with time stamp
    //                    this.fileManager.Save(Area.User, Kind.Text, name, result);

    //                    // rename to .axaml or xaml or whatever
    //                    string extension = exportFormat.ExtensionFileName();
    //                    string path = this.fileManager.MakePath(Area.User, Kind.Text, name);
    //                    this.newPath = path.ChangeFileExtension(extension);
    //                }
    //                else
    //                {
    //                    this.newPath = "Failed to generate output file: " + result.Item2;
    //                    return false;
    //                }
    //            }

    //            return true;
    //        }
    //        catch (Exception ex)
    //        {
    //            this.newPath = ex.Message;
    //            return false;
    //        }
    //    });

    //    message = this.newPath is null ? string.Empty : this.newPath;
    //    return result;
    //}

    #endregion Remove Unused Methods Later 

}

