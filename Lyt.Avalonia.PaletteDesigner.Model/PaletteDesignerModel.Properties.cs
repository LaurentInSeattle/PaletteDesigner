namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    #region Serialized -  No model changed event

    [JsonRequired]
    public string Language { get => this.Get<string>()!; set => this.Set(value); } 

    /// <summary> This should stay true, ==> But... Just FOR NOW !  </summary>
    [JsonRequired]
    public bool IsFirstRun { get; set; } = false;

    [JsonRequired]
    public List<Project> Projects { get; set; } = [];

    #endregion Serialized -  No model changed event


    #region Not serialized - No model changed event

    [JsonIgnore]
    public bool ModelLoadedNotified { get; set; } = false;

    [JsonIgnore]
    public Project ActiveProject { get; set; } = new()
    {
        Name = "Empty",
        Format = ResourceFormat.Unknown,
        FolderPath = string.Empty,
        Created = DateTime.Now,
        LastUpdated = DateTime.Now,
        Palette = new(),
    };

    public Dictionary<int, RgbColor> ColorLookupTable { get; set; } = [] ;

    #endregion Not serialized - No model changed event


    #region NOT serialized - WITH model changed event

    #endregion NOT serialized - WITH model changed event    
}
