namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImagingViewModel : ViewModel<ImagingView>
{
    private const int PixelCountMax = 1920 * 1080 / 4; // HD size divided by 4, about 1/2 Mega pixels 
    private const int DepthAnalysis = 120; // HD size divided by 4, about 1/2 Mega pixels 

    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorWheelViewModel colorWheelViewModel;

    //[ObservableProperty]
    //private PalettePreviewViewModel palettePreviewViewModel;

    //[ObservableProperty]
    //private TextPreviewPanelViewModel textPreviewPanelViewModel;

    //[ObservableProperty]
    //private ModelSelectionToolbarViewModel modelSelectionToolbarViewModel;

    //[ObservableProperty]
    //private ExportToolbarViewModel exportToolbarViewModel;

    //[ObservableProperty]
    //private ShadeSelectionToolbarViewModel shadeSelectionToolbarViewModel;

    //[ObservableProperty]
    //private ShadesPresetsToolbarViewModel shadesPresetsToolbarViewModel;


    public ImagingViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        //this.ColorWheelViewModel = new(paletteDesignerModel);
        //this.PalettePreviewViewModel = new(paletteDesignerModel);
        //this.TextPreviewPanelViewModel = new(paletteDesignerModel);
        //this.ModelSelectionToolbarViewModel = new();
        //this.ExportToolbarViewModel = new();
        //this.ShadeSelectionToolbarViewModel = new();
        //this.ShadesPresetsToolbarViewModel = new();

        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage? _)
    {
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);
        this.OnTestPaletteFromImage();
    }

    [Conditional("DEBUG")]
    private void OnTestPaletteFromImage()
    {
        // DANGER Zone ~ Does not work as expected 
        try
        {
            string path = @"C:\Users\Laurent\Desktop\Jolla.jpg";
            // string path = @"C:\Users\Laurent\Desktop\Kauai.jpg";
            // string path = @"C:\Users\Laurent\Desktop\Test.png";

            byte[] imageBytes = File.ReadAllBytes(path);
            if ((imageBytes is null) || (imageBytes.Length < 256))
            {
                throw new Exception("Failed to read image from disk: " + path);
            }

            var bitmap = WriteableBitmap.Decode(new MemoryStream(imageBytes));
            int pixelCount = bitmap.PixelSize.Width * bitmap.PixelSize.Height;
            int pixelCountMax =
                Debugger.IsAttached ? ImagingViewModel.PixelCountMax / 2 : ImagingViewModel.PixelCountMax;
            if (pixelCount > pixelCountMax)
            {
                // Decode again the bitmap to reduce processing time 
                double ratio = pixelCount / pixelCountMax;
                int newWidth = (int)(bitmap.PixelSize.Width / ratio);
                bitmap = WriteableBitmap.DecodeToWidth(new MemoryStream(imageBytes), newWidth);
            }

            if (bitmap is not null)
            {
                using var frameBuffer = bitmap.Lock();
                var colors =
                    PaletteDesignerModel.ExtractRgbFromBgraBuffer(
                        frameBuffer.Address, frameBuffer.Size.Height, frameBuffer.Size.Width);
                int depthAnalysis =
                    Debugger.IsAttached ? ImagingViewModel.DepthAnalysis / 2 : ImagingViewModel.DepthAnalysis;
                var palette = PaletteDesignerModel.ExtractPalette(colors, 12, depthAnalysis);
            }
            else
            {
                throw new Exception("Failed to load image: " + path);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            this.Logger.Warning(ex.ToString());
        }
    }

}
