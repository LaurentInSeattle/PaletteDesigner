namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadesPresetViewModel : 
    ViewModel<ShadesPresetView>,
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelPaletteUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly ShadesPreset shadesPreset;

    [ObservableProperty]
    private string presetName = string.Empty;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    public ShadesPresetViewModel(ShadesPreset shadesPreset)
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.shadesPreset = ShadesPreset.FromSizeIndependant(shadesPreset);

        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelPaletteUpdatedMessage>();
        this.MiniPaletteViewModel = new(this.paletteDesignerModel, isPreset:true);

        // Localize preset name 
        this.Localize();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnShadeSelect()
        => this.paletteDesignerModel.ApplyShadesPreset(this.shadesPreset);

    public void Receive(ModelPaletteUpdatedMessage _)
    {
        var palette = this.Palette.DeepClone();
        palette.ApplyShadesPreset(this.shadesPreset);
        this.MiniPaletteViewModel.Update(palette);
    }

    public void Receive(LanguageChangedMessage? _) => this.Localize(); 

    private void Localize() 
        // Localize preset name 
        => this.PresetName = this.Localize(string.Concat("Design.Preset.", shadesPreset.Name));
}
