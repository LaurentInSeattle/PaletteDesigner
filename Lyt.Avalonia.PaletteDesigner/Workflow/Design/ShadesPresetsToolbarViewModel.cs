namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadesPresetsToolbarViewModel : ViewModel<ShadesPresetsToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    public ShadesPresetsToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);

        this.MiniPaletteViewModel = new ();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnShadeSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            //ShadeMode shadeMode = Enum.TryParse(tag, out ShadeMode kind) ? kind : ShadeMode.Locked;
            //this.paletteDesignerModel.UpdatePaletteShadeMode(shadeMode);
        }
    }

    private void OnModelUpdated(ModelUpdatedMessage _)
    {
        var palette = this.Palette;
        this.MiniPaletteViewModel.Update(palette);
        PaletteKind paletteKind = palette.Kind;
    }
}