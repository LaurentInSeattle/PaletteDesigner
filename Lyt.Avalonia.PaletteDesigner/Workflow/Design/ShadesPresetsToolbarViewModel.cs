namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ShadesPresetsToolbarViewModel : ViewModel<ShadesPresetsToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    [ObservableProperty]
    private bool visible;

    public ShadesPresetsToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<ModelUpdatedMessage>(this.OnModelUpdated);
        this.Messenger.Subscribe<PresetsVisibilityMessage>(this.OnPresetsVisibility);
        this.MiniPaletteViewModel = new ();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.Show(show: false);
    }

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

    private void OnModelUpdated(ModelUpdatedMessage _)
    {
        var palette = this.Palette;
        this.MiniPaletteViewModel.Update(palette);
        PaletteKind paletteKind = palette.Kind;
    }
}