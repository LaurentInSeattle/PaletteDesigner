namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using global::Avalonia.Media.Imaging;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardViewModel : ViewModel<WizardView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isLoaded;
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

        this.CreateSwatches(); 
        //if (this.paletteDesignerModel.ActiveProject is not Project project)
        //{
        //    return;
        //}

        //if (project.Swatches is not ColorSwatches swatches)
        //{
        //    return;
        //}

        //if (string.IsNullOrWhiteSpace(swatches.ImagePath))
        //{
        //    return;
        //}

        //this.ImagingToolbarViewModel.ProgrammaticUpdate(swatches);
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);

        // DEBUG !
        Schedule.OnUiThread(1_000,
            () =>
            {
                this.paletteDesignerModel.ActiveProject!.WizardPalette.Reset();
            }, DispatcherPriority.Background);
    }

    private void CreateSwatches()
    {
        if ( this.isLoaded)
        {
            return;
        }

        for (int row = 0; row < 3; row++)
        {
            SwatchKind swatchKind = (SwatchKind)row;
            for (int index = 0; index < WizardPalette.PaletteWidth; index++)
            {
                var swatchViewModel = new WizardSwatchViewModel(this.paletteDesignerModel, swatchKind, index);
                var swatchView = swatchViewModel.CreateViewAndBind();
                this.View.AddSwatchView(swatchView, swatchKind, index);
            }
        }

        this.isLoaded = true;
    }
}
