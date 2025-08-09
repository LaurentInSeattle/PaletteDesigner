namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewPanelViewModel : ViewModel<TextPreviewPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool visible;

    [ObservableProperty]
    private ObservableCollection<TextPreviewViewModel> textPreviewViewModels;

    [ObservableProperty]
    private TextPreviewToolbarViewModel textPreviewToolbarViewModel; 

    public TextPreviewPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.TextPreviewToolbarViewModel = new();
        this.TextPreviewViewModels = [];
        this.Visible = false;

        this.Messenger.Subscribe<TextSamplesVisibilityMessage>(this.OnTextSamplesVisibility);
        this.Messenger.Subscribe<ModelTextSamplesDisplayModeUpdated>(this.OnModelTextSamplesDisplayMode);
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
    }

    public void Show(bool show = true)
    {
        this.Visible = show;
        if (this.IsBound)
        {
            this.View.MainGrid.Width = show ? 430.0 : 0.0;
        }
    }

    public override void OnViewLoaded() 
    {
        base.OnViewLoaded();
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.UpdateAllSamples();
    }

    private void OnModelTextSamplesDisplayMode(ModelTextSamplesDisplayModeUpdated _) => this.UpdateAllSamples();

    private void OnTextSamplesVisibility(TextSamplesVisibilityMessage message) => this.Show(message.Show);

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage _) => this.UpdateAllSamples(); 

    private void UpdateAllSamples() 
    { 
        var palette = this.paletteDesignerModel.ActiveProject!.Palette;
        TextSamplesDisplayMode displayMode = this.paletteDesignerModel.TextSamplesDisplayMode;
        WheelKind wheelKindForeground = this.paletteDesignerModel.TextSamplesSelectedWheel;
        int index = 0;
        palette.ForAllShades((wheelKindBackground, shades) =>
        {
            TextSampleSetup textSampleSetup = new (displayMode, wheelKindForeground, wheelKindBackground);
            var viewModel = this.TextPreviewViewModels[index];
            viewModel.Update(textSampleSetup); 
            index++;
        }); 
    }
}
