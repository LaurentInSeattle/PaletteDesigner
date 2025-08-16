namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImagingToolbarViewModel : ViewModel<ImagingToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    //[ObservableProperty]
    //private SolidColorBrush primaryBaseBrush = new();

      
    public ImagingToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    //partial void OnShowShadesPresetsChanged(bool value)
    //    => this.Messenger.Publish(new PresetsVisibilityMessage(value));

    //partial void OnShowShadesValuesChanged(bool value)
    //    => this.Messenger.Publish(new ShadesValuesVisibilityMessage(value));

    //partial void OnShowTextSamplesChanged(bool value)
    //    => this.Messenger.Publish(new TextSamplesVisibilityMessage(value));

    [RelayCommand]
    public void OnLockSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ShadeMode shadeMode = Enum.TryParse(tag, out ShadeMode kind) ? kind : ShadeMode.Locked;
            this.paletteDesignerModel.UpdatePaletteShadeMode(shadeMode);
        }
    }

}
