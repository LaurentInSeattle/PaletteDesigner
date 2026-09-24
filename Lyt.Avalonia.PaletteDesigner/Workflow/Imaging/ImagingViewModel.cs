namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

using global::Avalonia.Media.Imaging;

using Lyt.FileSystem;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class ImagingViewModel : ViewModel<ImagingView>
{
    private const long ImageFileMaxLength = 60L * 1024L * 1024L;

    private const int PixelCountMax = 1920 * 1080 / 4; // HD size divided by 4, about 1/2 Mega pixels 
    private const int DepthAnalysis = 250; // Iterations for KMeans 
    private const double BrightnessMin = 0.05;
    private const double BrightnessMax = 0.98;

    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly IToaster toaster; 

    private WriteableBitmap? bitmapToProcess;

    private string swatchesName;

    [ObservableProperty]
    public partial ObservableCollection<ImageSwatchViewModel> SwatchesViewModels { get; set; }

    [ObservableProperty]
    public partial DropViewModel DropViewModel { get; set; }

    [ObservableProperty]
    public partial SpinViewModel SpinViewModel { get; set; }

    [ObservableProperty]
    public partial ImagingToolbarViewModel ImagingToolbarViewModel { get; set; }

    [ObservableProperty]
    public partial ExportToolbarViewModel ExportToolbarViewModel { get; set; }

    [ObservableProperty]
    public partial Bitmap? SourceImage { get; set; }

    [ObservableProperty]
    public partial double SwatchesOpacity { get; set; }

    [ObservableProperty]
    public partial bool CalculationInProgress { get; set; }

    [ObservableProperty]
    public partial string ImageTitle { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ImagePath { get; set; } = string.Empty;

    public ImagingViewModel(PaletteDesignerModel paletteDesignerModel, IToaster toaster)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.toaster = toaster; 

        this.SpinViewModel = new SpinViewModel()
        {
            IsVisible = false,
            IsActive = false,
        };

        this.DropViewModel = new DropViewModel
        {
            IsVisible = true
        };

        this.ImagingToolbarViewModel = new();
        this.ExportToolbarViewModel = new(PaletteFamily.Image);
        this.SwatchesOpacity = 0.0;
        this.swatchesName = string.Empty;
        this.SwatchesViewModels = [];

        this.CalculationInProgress = false;
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

        this.ImagingToolbarViewModel.ProgrammaticUpdate(swatches);
        this.OnDropOrReload(swatches.ImagePath, isReload: true);
    }


    public bool OnDropOrReload(string path, bool isReload = false)
    {
        void LogAndMessageUser(string logMessage, string userMessage)
        {
            Debug.WriteLine(logMessage);
            this.Logger.Warning(logMessage);
            if (!isReload)
            {
                Dispatch.OnUiThread(() =>
                {
                    // Localize and toast userMessage 
                    string errorMessage = this.Localize("Toast.Error");
                    string displayedMessage = this.Localize(userMessage);
                    this.toaster.Show(errorMessage, displayedMessage, 3_500, InformationLevel.Warning); 
                }, DispatcherPriority.Background);
            }
        }

        try
        {
            FileInfo fileInfo = new(path);
            if (!fileInfo.Exists)
            {
                LogAndMessageUser($"File does not exist: {path}", "File.NotExist");
                return false;
            }

            long length = fileInfo.Length;
            if ((length == 0) || (length < 256L))
            {
                LogAndMessageUser($"File length is too small: {path}", "File.TooSmall");
                return false;
            }

            if (length > ImageFileMaxLength)
            {
                LogAndMessageUser($"File length is too large: {path}", "File.TooBig");
                return false;
            }

            string extension = fileInfo.Extension.ToLowerInvariant();
            bool supported =
                extension == ".jpg" || extension == ".jpeg" || extension == ".bmp" ||
                extension == ".png" || extension == ".webp";
            if (!supported)
            {
                LogAndMessageUser($"Image File format not supported: {path}", "File.ImageNotSupported");
                return false;
            }

            if (!FileSystemExtensions.IsReadable(path))
            {
                LogAndMessageUser($"File cannot be read: {path}", "File.CantRead");
                return false;
            }

            byte[] imageBytes = File.ReadAllBytes(path);
            if ((imageBytes is null) || (imageBytes.Length < 256) || imageBytes.Length != length)
            {
                LogAndMessageUser($"Failed to read image from disk: {path}", "File.FailRead");
                return false;
            }

            this.ImagePath = path;
            this.swatchesName = fileInfo.Name;
            if (string.IsNullOrWhiteSpace(this.swatchesName))
            {
                this.swatchesName = "Default";
            }
            else
            {
                this.swatchesName = this.swatchesName.Replace(extension, string.Empty);
                this.swatchesName = this.swatchesName.Replace("_", " ");
                this.swatchesName = this.swatchesName.Replace("-", " ");
                this.swatchesName = this.swatchesName.BeautifyEnumString();
                this.ImageTitle = this.swatchesName;
            }

            // Keep the original to display on the UI at best resolution 
            var sourceBitmap = WriteableBitmap.Decode(new MemoryStream(imageBytes));
            if ( sourceBitmap is null )
            {
                LogAndMessageUser($"Failed to decode image: {path}", "File.Exception");
                return false;
            }

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

            this.bitmapToProcess = thumbnailBitmap is not null ? thumbnailBitmap : sourceBitmap;
            if (this.bitmapToProcess is not null)
            {
                this.SourceImage = sourceBitmap;
                return this.ReProcessBitmap();
            }
            else
            {
                throw new Exception("Failed to load image: " + path);
            }
        }
        catch (Exception ex)
        {
            LogAndMessageUser($"Exception thrown while processing: {ex}", "Error while processing image file.");
            return false;
        }
    }

    public bool ReProcessBitmap()
    {
        if (this.CalculationInProgress)
        {
            return false;
        }

        if (this.bitmapToProcess is not null)
        {
            this.CalculationInProgress = true;
            this.SpinViewModel.IsVisible = true;
            this.SpinViewModel.IsActive = true;
            this.DropViewModel.IsVisible = false;
            this.SwatchesOpacity = 0.25;
            return this.ProcessBitmap();
        }
        else
        {
            throw new Exception("No bitmap image");
        }
    }

    private unsafe bool ProcessBitmap()
    {
        try
        {
            if (this.bitmapToProcess is not null)
            {
                using var frameBuffer = this.bitmapToProcess.Lock();
                var colors =
                    PaletteDesignerModel.ExtractRgbFromBgraBuffer(
                        frameBuffer.Address, frameBuffer.Size.Height, frameBuffer.Size.Width) ??
                    throw new Exception("Failed to extract colours from bitmap image");
                Debug.WriteLine("Colors: " + colors.Length);

                int depthAnalysis =
                    this.paletteDesignerModel.IsDeepImagingAlgorithmStrength ?
                        ImagingViewModel.DepthAnalysis :
                        ImagingViewModel.DepthAnalysis / 2;

                // Divide even further in DEBUG mode 
                depthAnalysis = Debugger.IsAttached ? (int)(0.75 * depthAnalysis) : depthAnalysis;
                int clusters = this.paletteDesignerModel.ImagingAlgorithmClusters;
                Debug.WriteLine("Iterations: " + depthAnalysis + " - Clusters: " + clusters);

                // Calculate that in a thread and let the UI spin until its done
                Task.Run(() =>
                {
                    var swatches = PaletteDesignerModel.ExtractSwatches(colors, clusters, depthAnalysis);
                    Debug.WriteLine("Palette: " + swatches.Count);
                    if (swatches.Count > 0)
                    {
                        Dispatch.OnUiThread(() => { this.ProcessSwatches(swatches); });
                    }
                });

                return true;
            }
            else
            {
                throw new Exception("No bitmap image");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            this.Logger.Warning(ex.ToString());
            return false;
        }
    }

    private bool ProcessSwatches(List<Cluster<LabColor>> swatches)
    {
        this.SpinViewModel.IsActive = false;
        this.SpinViewModel.IsVisible = false;
        this.DropViewModel.IsVisible = true;

        List<ImageSwatchViewModel> list = new(swatches.Count);
        foreach (var swatch in swatches)
        {
            // Eliminate colors too dark or too bright 
            var rgbColor = swatch.Payload.ToRgb();
            HsvColor hsvColor = rgbColor.ToHsv();
            double brightness = hsvColor.V;
            if ((brightness > BrightnessMin) || (brightness < BrightnessMax))
            {
                var vm = new ImageSwatchViewModel(swatch);
                list.Add(vm);
            }
        }

        // Color Sort
        //var sortedList =
        //    (from vm in list
        //     orderby 
        //         vm.HsvColor.H ascending,
        //         vm.HsvColor.S descending,
        //         vm.HsvColor.V descending
        //     select vm).ToList();

        // Frequency Sort
        var sortedList =
            (from vm in list
             orderby vm.Cluster.Count descending
             select vm).ToList();
        this.SwatchesViewModels = new(sortedList);
        this.SwatchesOpacity = 1.0;
        this.CalculationInProgress = false;

        // Save swatches
        if (this.paletteDesignerModel.ActiveProject is not null)
        {
            return this.paletteDesignerModel.SaveSwatches(this.ImagePath, this.swatchesName, swatches);
        }

        return false;
    }
}
