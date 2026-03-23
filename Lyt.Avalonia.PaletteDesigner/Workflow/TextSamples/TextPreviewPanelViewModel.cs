namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewPanelViewModel : 
    ViewModel<TextPreviewPanelView>,
    IRecipient<TextSamplesVisibilityMessage>,
    IRecipient<ModelTextSamplesDisplayModeUpdated>,
    IRecipient<ModelPaletteUpdatedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    public partial bool Visible { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<TextPreviewViewModel> TextPreviewViewModels { get; set; }

    [ObservableProperty]
    public partial TextPreviewToolbarViewModel TextPreviewToolbarViewModel { get; set; }

    public TextPreviewPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.TextPreviewToolbarViewModel = new();
        this.TextPreviewViewModels = 
        [
            new(this.paletteDesignerModel),
            new(this.paletteDesignerModel),
            new(this.paletteDesignerModel),
            new(this.paletteDesignerModel),
        ];
        this.Visible = false;

        this.Subscribe<TextSamplesVisibilityMessage>();
        this.Subscribe<ModelTextSamplesDisplayModeUpdated>();
        this.Subscribe<ModelPaletteUpdatedMessage>();
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
        this.UpdateAllSamples();
    }

    public void Receive(ModelTextSamplesDisplayModeUpdated _) => this.UpdateAllSamples();

    public void Receive(TextSamplesVisibilityMessage message) => this.Show(message.Show);

    public void Receive(ModelPaletteUpdatedMessage _)  => this.UpdateAllSamples(); 

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
