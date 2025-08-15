namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class ImagingViewModel : ViewModel<ImagingView>
{
    private const int PixelCountMax = 1920 * 1080 / 4; // HD size divided by 4, about 1/2 Mega pixels 
    private const int DepthAnalysis = 120; // Iterations for KMeans 
    private const double BrightnessMin = 0.05;
    private const double BrightnessMax = 0.98;

    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ObservableCollection<ImageSwatchViewModel> swatchesViewModels;

    //[ObservableProperty]
    //private ColorWheelViewModel colorWheelViewModel;
    
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
        this.SwatchesViewModels = [];
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
        this.OnPaletteFromImage();
    }

    private void OnPaletteFromImage()
    {
        try
        {
            // string path = @"C:\Users\Laurent\Desktop\Jolla.jpg";
            string path = @"C:\Users\Laurent\Desktop\Kauai.jpg";
            // string path = @"C:\Users\Laurent\Desktop\Florida.jpg";
            // string path = @"C:\Users\Laurent\Desktop\Designer.png";

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
                Debug.WriteLine("Colors: " + colors.Length);
                int depthAnalysis =
                    Debugger.IsAttached ? ImagingViewModel.DepthAnalysis / 2 : ImagingViewModel.DepthAnalysis;
                var palette = PaletteDesignerModel.ExtractPalette(colors, 21, depthAnalysis);
                Debug.WriteLine("Palette: " + palette.Count);

                List<ImageSwatchViewModel> list = new(palette.Count);
                foreach (var rgbColor in palette)
                {
                    // Eliminate colors too dark or too bright 
                    Model.ColorObjects.HsvColor hsvColor = rgbColor.ToHsv();
                    double brightness = hsvColor.V;
                    if ((brightness > BrightnessMin) || (brightness < BrightnessMax))
                    {
                        var vm = new ImageSwatchViewModel(rgbColor, hsvColor);
                        list.Add(vm);
                    }
                }

                var sortedList =
                    (from vm in list 
                     orderby vm.HsvColor.H ascending , vm.HsvColor.V descending 
                     select vm).ToList();
                this.SwatchesViewModels = new(sortedList);
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
