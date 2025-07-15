namespace Lyt.Avalonia.PaletteDesigner.Model;

using Lyt.Avalonia.PaletteDesigner.Utilities;
using static Lyt.Persistence.FileManagerModel;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public const string DefaultLanguage = "it-IT";
    private const string AstroPicModelFilename = "AstroPicData";

    private static readonly PaletteDesignerModel DefaultData =
        new()
        {
            Language = DefaultLanguage,
            IsFirstRun = true,
        };

    private readonly FileManagerModel fileManager;
    private readonly ILocalizer localizer; 
    private readonly Lock lockObject = new();
    private readonly FileId modelFileId;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
    public PaletteDesignerModel() : base(null, null)
    {
        this.modelFileId = new FileId(Area.User, Kind.Json, PaletteDesignerModel.AstroPicModelFilename);
        // Do not inject the FileManagerModel instance: a parameter-less ctor is required for Deserialization 
        // Empty CTOR required for deserialization 
        this.ShouldAutoSave = false;
    }
#pragma warning restore CS8625 
#pragma warning restore CS8618

    public PaletteDesignerModel(
        FileManagerModel fileManager,
        ILocalizer localizer,
        IMessenger messenger, 
        ILogger logger) : base(messenger, logger)
    {
        this.fileManager = fileManager;
        this.localizer = localizer;
        this.modelFileId = new FileId(Area.User, Kind.Json, PaletteDesignerModel.AstroPicModelFilename);
        this.ShouldAutoSave = true;
    }

    public override async Task Initialize()
    {
        this.IsInitializing = true; 
        await this.Load();
        this.IsInitializing = false;
        this.IsDirty = false;
    }

    public override async Task Shutdown()
    {
        if (this.IsDirty)
        {
            await this.Save();
        }
    }

    public Task Load()
    {
        try
        {
            if (!this.fileManager.Exists(this.modelFileId))
            {
                this.fileManager.Save(this.modelFileId, PaletteDesignerModel.DefaultData);
            }

            PaletteDesignerModel model = this.fileManager.Load<PaletteDesignerModel>(this.modelFileId);

            // Copy all properties with attribute [JsonRequired]
            base.CopyJSonRequiredProperties<PaletteDesignerModel>(model);

            this.LoadColorWheel();
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            string msg = "Failed to load Model from " + this.modelFileId.Filename;
            this.Logger.Fatal(msg);
            throw new Exception("", ex);
        }
    }

    private void LoadColorWheel()
    {
        SerializationUtilities.SetResourcesPath("Lyt.Avalonia.PaletteDesigner.Model.Resources"); 
        string serialized = SerializationUtilities.LoadEmbeddedTextResource("ColorWheel.json", out string? _);
        var colorWheel = SerializationUtilities.Deserialize<Dictionary<int, RgbColor>>(serialized);
        if (colorWheel is null) 
        {
            throw new Exception("Failed to load color wheel");
        }

        this.ColorLookupTable = colorWheel;
    }

    public override Task Save()
    {
        // Null check is needed !
        // If the File Manager is null we are currently loading the model and activating properties on a second instance 
        // causing dirtyness, and in such case we must avoid the null crash and anyway there is no need to save anything.
        if (this.fileManager is not null)
        {
#if DEBUG 
            //if (this.fileManager.Exists(this.modelFileId))
            //{
            //    this.fileManager.Duplicate(this.modelFileId);
            //}
#endif // DEBUG 

            this.fileManager.Save(this.modelFileId, this);

#if DEBUG 
            try
            {
                string path = this.fileManager.MakePath(this.modelFileId);
                var fileInfo = new FileInfo(path);
                if ( fileInfo.Length < 1024 )
                {
                    if (Debugger.IsAttached) { Debugger.Break(); }
                    this.Logger.Warning("Model file is too small!"); 
                }
            }
            catch (Exception ex)
            {
                if ( Debugger.IsAttached ) {  Debugger.Break(); }
                Debug.WriteLine(ex);
            }
#endif // DEBUG 

            base.Save();
        }

        return Task.CompletedTask;
    }

    public void SelectLanguage(string languageKey)
    {
        this.Language = languageKey;
        this.localizer.SelectLanguage(languageKey);
    }
}
