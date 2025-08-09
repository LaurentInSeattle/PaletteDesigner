namespace Lyt.Avalonia.PaletteDesigner.Model;

using Lyt.Avalonia.PaletteDesigner.Model.ProjectObjects;

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
    public Project? ActiveProject { get; set; }

    [JsonIgnore]
    public Dictionary<string, ShadesPreset> ShadesPresets = new(16);

    [JsonIgnore]
    public ShadesValuesDisplayMode ShadesValuesDisplayMode { get; set; } = ShadesValuesDisplayMode.Hex;

    [JsonIgnore]
    public TextSamplesDisplayMode TextSamplesDisplayMode { get; set; } = TextSamplesDisplayMode.Dark;

    [JsonIgnore]
    public WheelKind TextSamplesSelectedWheel { get; set; } = WheelKind.Primary;

    #endregion Not serialized - No model changed event

    #region NOT serialized - WITH model changed event

    #endregion NOT serialized - WITH model changed event    
}
