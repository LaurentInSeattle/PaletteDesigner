namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadesPresetViewModel : ViewModel<ShadesPresetView>
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

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
        this.MiniPaletteViewModel = new();

        // Localize preset name 
        this.OnLanguageChanged();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnShadeSelect()
        => this.paletteDesignerModel.ApplyShadesPreset(this.shadesPreset);

    private void OnModelUpdated(ModelUpdatedMessage _)
    {
        var palette = this.Palette.DeepClone();
        palette.ApplyShadesPreset(this.shadesPreset);
        this.MiniPaletteViewModel.Update(palette);
    }

    private void OnLanguageChanged(LanguageChangedMessage? _ = null)
        // Localize preset name 
        => this.PresetName = this.Localize(
            string.Concat("Design.Preset.", shadesPreset.Name));
}
