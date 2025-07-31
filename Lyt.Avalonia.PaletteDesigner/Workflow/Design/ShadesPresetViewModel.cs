namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

using Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class ShadesPresetViewModel : ViewModel<ShadesPresetView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly ShadesPreset shadesPreset;

    [ObservableProperty]
    private string presetName;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    public ShadesPresetViewModel(ShadesPreset shadesPreset)
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.shadesPreset = ShadesPreset.FromSizeIndependant(shadesPreset);

        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
        this.MiniPaletteViewModel = new();

        // TODO: Localize preset name 
        this.PresetName = shadesPreset.Name;
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
}
