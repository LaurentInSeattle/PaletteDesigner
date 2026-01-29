namespace Lyt.Avalonia.PaletteDesigner.Model.ProjectObjects;

using Lyt.Avalonia.PaletteDesigner.Model;

public sealed class Project
{
    public Project() { /* Required for serialization */ }

    [JsonRequired]
    public required string Name { get; set; } = string.Empty;

    public required DateTime Created { get; set; } = DateTime.Now;

    public required DateTime LastUpdated { get; set; } = DateTime.Now;

    [JsonRequired]
    public required PaletteExportFormat Format { get; set; } = PaletteExportFormat.AvaloniaAxaml;

    [JsonRequired]
    public required string FolderPath { get; set; } = string.Empty;

    public Palette Palette { get; set; } = new();

    public ColorSwatches Swatches { get; set; } = new();

    public ColorTheme ColorTheme { get; set; } = new();

    public WizardPalette WizardPalette { get; set; } = new();

    public bool IsInvalid
        =>
            this.Format == PaletteExportFormat.None ||
            string.IsNullOrWhiteSpace(this.Name) ||
            string.IsNullOrWhiteSpace(this.FolderPath);
    
    public bool Validate(out string errorMessageKey)
    {
        errorMessageKey = string.Empty;
        if (this.Format == PaletteExportFormat.None)
        {
            errorMessageKey = "Model.Project.Format";
            return false;
        }

        if (string.IsNullOrWhiteSpace(this.Name))
        {
            errorMessageKey = "Model.Project.Name";
            return false;
        }

        if (string.IsNullOrWhiteSpace(this.FolderPath))
        {
            errorMessageKey = "Model.Project.FolderPath";
            return false;
        }

        DirectoryInfo directoryInfo = new(this.FolderPath);
        if (!directoryInfo.Exists)
        {
            errorMessageKey = "Model.Project.FolderPath";
        }

        // CONSIDER ~ LATER
        // Try to write a dummy file in the target directory to ensure write access is granted
        return true;
    }
}
