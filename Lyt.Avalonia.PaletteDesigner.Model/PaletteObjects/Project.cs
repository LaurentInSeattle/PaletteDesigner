namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class Project
{
    public Project() { /* Required for serialization */ }

    [JsonRequired]
    public required string Name { get; set; } = string.Empty;

    public required DateTime Created { get; set; } = DateTime.Now;

    public required DateTime LastUpdated { get; set; } = DateTime.Now;

    [JsonRequired]
    public required ResourceFormat Format { get; set; } = ResourceFormat.Unknown;

    [JsonRequired]
    public required string FolderPath { get; set; } = string.Empty;

    public Palette Palette { get; set; } = new(); 

    public bool IsInvalid
        =>
            this.Format == ResourceFormat.Unknown ||
            string.IsNullOrWhiteSpace(this.Name) ||
            string.IsNullOrWhiteSpace(this.FolderPath);
    
    public bool Validate(out string errorMessageKey)
    {
        errorMessageKey = string.Empty;
        if (this.Format == ResourceFormat.Unknown)
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
