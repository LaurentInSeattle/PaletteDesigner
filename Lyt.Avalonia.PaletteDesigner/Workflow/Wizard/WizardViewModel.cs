namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using global::Avalonia.Media.Imaging;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardViewModel : ViewModel<WizardView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    //[ObservableProperty]
    //private ObservableCollection<WizardSwatchViewModel> swatchesViewModels;


    //[ObservableProperty]
    //private ImagingToolbarViewModel imagingToolbarViewModel;

    //[ObservableProperty]
    //private ExportToolbarViewModel exportToolbarViewModel;

    public WizardViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;

        //this.ImagingToolbarViewModel = new();
        //this.ExportToolbarViewModel = new(PaletteFamily.Image);
        // this.SwatchesViewModels = [];
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        if (this.paletteDesignerModel.ActiveProject is not Project project)
        {
            return;
        }

        if (project.Swatches is not ColorSwatches swatches)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(swatches.ImagePath))
        {
            return;
        }

        //this.ImagingToolbarViewModel.ProgrammaticUpdate(swatches);
    }
}
