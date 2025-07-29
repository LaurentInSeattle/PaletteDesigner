namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadesPresetsToolbarViewModel : ViewModel<ShadesPresetsToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool visible;

    [ObservableProperty]
    private ObservableCollection<ShadesPresetViewModel> presets;

    public ShadesPresetsToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<PresetsVisibilityMessage>(this.OnPresetsVisibility);
        this.Presets = []; 
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.Show(show: false);

        List<ShadesPresetViewModel> presets = new(16);
        foreach (var shadesPreset in this.paletteDesignerModel.ShadesPresets.Values)
        {
            ShadesPresetViewModel preset = new(shadesPreset);
            presets.Add(preset);
        }

        this.Presets = new(presets); 
    }

    private void OnPresetsVisibility(PresetsVisibilityMessage message)
        => this.Show(message.Show);

    public void Show(bool show = true)
    {
        this.Visible = show;
        if (this.IsBound)
        {
            this.View.MainGrid.Width = show ? 320.0 : 0.0;
        }
    }
}