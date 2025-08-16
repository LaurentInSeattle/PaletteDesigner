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

    [ObservableProperty]
    private DropViewModel dropViewModel;

    [ObservableProperty]
    private Bitmap? sourceImage ;

    [ObservableProperty]
    private ExportToolbarViewModel exportToolbarViewModel;

    //[ObservableProperty]
    //private ShadeSelectionToolbarViewModel shadeSelectionToolbarViewModel;

    //[ObservableProperty]
    //private ShadesPresetsToolbarViewModel shadesPresetsToolbarViewModel;

    public ImagingViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.DropViewModel = new DropViewModel();
        this.ExportToolbarViewModel = new(PaletteFamily.Image);

        this.SwatchesViewModels = [];

        //this.TextPreviewPanelViewModel = new(paletteDesignerModel);
        //this.ModelSelectionToolbarViewModel = new();
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

    public bool Select(string path)
    {
        try
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            if ((imageBytes is null) || (imageBytes.Length < 256))
            {
                throw new Exception("Failed to read image from disk: " + path);
            }

            // Keep the original to display on the UI at best resolution 
            var sourceBitmap = 
                WriteableBitmap.Decode(new MemoryStream(imageBytes)) ?? 
                throw new Exception("Failed to decode image: " + path);
            int pixelCount = sourceBitmap.PixelSize.Width * sourceBitmap.PixelSize.Height;
            int pixelCountMax =
                Debugger.IsAttached ? ImagingViewModel.PixelCountMax / 2 : ImagingViewModel.PixelCountMax;
            WriteableBitmap? thumbnailBitmap = null;
            if (pixelCount > pixelCountMax)
            {
                // Decode again the bitmap to reduce processing time 
                double ratio = pixelCount / pixelCountMax;
                int newWidth = (int)(sourceBitmap.PixelSize.Width / ratio);
                thumbnailBitmap = WriteableBitmap.DecodeToWidth(new MemoryStream(imageBytes), newWidth);
            }

            if (thumbnailBitmap is not null)
            {
                this.SourceImage = sourceBitmap;

                using var frameBuffer = thumbnailBitmap.Lock();
                var colors =
                    PaletteDesignerModel.ExtractRgbFromBgraBuffer(
                        frameBuffer.Address, frameBuffer.Size.Height, frameBuffer.Size.Width);
                Debug.WriteLine("Colors: " + colors.Length);
                int depthAnalysis =
                    Debugger.IsAttached ? ImagingViewModel.DepthAnalysis / 2 : ImagingViewModel.DepthAnalysis;
                var swatches = PaletteDesignerModel.ExtractSwatches(colors, 21, depthAnalysis);
                Debug.WriteLine("Palette: " + swatches.Count);

                List<ImageSwatchViewModel> list = new(swatches.Count);
                foreach (var swatch in swatches)
                {
                    // Eliminate colors too dark or too bright 
                    var rgbColor = swatch.LabColor.ToRgb();
                    Model.ColorObjects.HsvColor hsvColor = rgbColor.ToHsv();
                    double brightness = hsvColor.V;
                    if ((brightness > BrightnessMin) || (brightness < BrightnessMax))
                    {
                        var vm = new ImageSwatchViewModel(swatch);
                        list.Add(vm);
                    }
                }

                var sortedList =
                    (from vm in list 
                     orderby vm.HsvColor.H ascending , vm.HsvColor.V descending 
                     select vm).ToList();
                this.SwatchesViewModels = new(sortedList);
                return true; 
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
            return false;
        }
    }
}
