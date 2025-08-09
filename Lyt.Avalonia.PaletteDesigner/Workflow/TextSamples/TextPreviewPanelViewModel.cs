namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewPanelViewModel : ViewModel<TextPreviewPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private bool visible;

    [ObservableProperty]
    ObservableCollection<TextPreviewViewModel> textPreviewViewModels;

    public TextPreviewPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.TextPreviewViewModels = [];
        this.Visible = false;

        this.Messenger.Subscribe<TextSamplesVisibilityMessage>(this.OnTextSamplesVisibility);
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
    }

    private void OnTextSamplesVisibility(TextSamplesVisibilityMessage message)
        => this.Show(message.Show);

    public void Show(bool show = true)
    {
        this.Visible = show;
        if (this.IsBound)
        {
            this.View.MainGrid.Width = show ? 460.0 : 0.0;
        }
    }

    public override void OnViewLoaded() 
    {
        base.OnViewLoaded();
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
        this.TextPreviewViewModels.Add(new(this.paletteDesignerModel));
    }

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage message)
    {

    }
}
