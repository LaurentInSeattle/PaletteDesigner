namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

using System;

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
        TextSamplesDisplayMode displayMode = paletteDesignerModel.TextSamplesDisplayMode;
        WheelKind wheelKindForeground = WheelKind.Primary;
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
