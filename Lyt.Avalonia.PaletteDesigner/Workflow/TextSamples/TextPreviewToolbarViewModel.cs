namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewToolbarViewModel : ViewModel<TextPreviewToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
      
    public TextPreviewToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;
    
    [RelayCommand]
    public void OnVariantSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            TextSamplesDisplayMode mode =
                Enum.TryParse(tag, out TextSamplesDisplayMode kind) ? kind : TextSamplesDisplayMode.Dark;
            this.paletteDesignerModel.TextSamplesDisplayMode = mode;
            this.Messenger.Publish(new ModelTextSamplesDisplayModeUpdated());
        }
    }


    [RelayCommand]
    public void OnWheelSelect(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            WheelKind wheel = Enum.TryParse(tag, out WheelKind kind) ? kind : WheelKind.Unknown;
            this.paletteDesignerModel.TextSamplesSelectedWheel = wheel;
            this.paletteDesignerModel.UpdatePaletteWheelShadeMode(wheel);

            this.Messenger.Publish(new ModelTextSamplesDisplayModeUpdated());
        }
    }
}
