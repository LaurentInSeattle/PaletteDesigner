namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed partial class TextPreviewPanelViewModel : ViewModel<TextPreviewPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    ObservableCollection<TextPreviewViewModel> textPreviewViewModels;

    public TextPreviewPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.TextPreviewViewModels = [];
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
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
