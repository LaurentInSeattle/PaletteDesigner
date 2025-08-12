namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

public sealed partial class ExportToolbarViewModel : ViewModel<ExportToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly IToaster toaster;

    [ObservableProperty]
    private int selectedFileFormatIndex;

    [ObservableProperty]
    private ObservableCollection<FileFormatViewModel> fileFormats;


    public ExportToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.toaster = App.GetRequiredService<IToaster>();

        this.FileFormats = [];
        foreach (PaletteExportFormat resourceFormat in PaletteExportFormatExtensions.AvailableFormats)
        {
            this.FileFormats.Add(new FileFormatViewModel(resourceFormat));
        }

        this.SelectedFileFormatIndex = 0;
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnExport()
    {
        //if (Debugger.IsAttached)
        //{
        //    this.OnTestPaletteFromImage();
        //}
        //else
        {
            var fileFormatViewModel = this.FileFormats[this.SelectedFileFormatIndex];
            PaletteExportFormat exportFormat = fileFormatViewModel.PaletteExportFormat;
            bool success = this.paletteDesignerModel.ExportPalette(exportFormat, out string message);
            if (success)
            {
                this.toaster.Show(
                    "Exported!", "Palette sucessfully exported to: " + message,
                    dismissDelay: 5_000, InformationLevel.Success);
            }
            else
            {
                this.toaster.Show(
                    "Error", "Failed to export palette: " + message,
                    dismissDelay: 12_000, InformationLevel.Error);
            }
        }
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
            if (bitmap is not null)
            {
                using var frameBuffer = bitmap.Lock();
                {
                    var palette =
                        this.paletteDesignerModel.GenerateFromBgraImageBuffer(
                            frameBuffer.Address, frameBuffer.Size.Height, frameBuffer.Size.Width, 4);
                }
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
