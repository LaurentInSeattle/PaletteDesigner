namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

public sealed partial class DropViewModel : ViewModel<DropView>
{
    [ObservableProperty]
    private bool isVisible; 

    /// <summary> Returns true if the path is a valid image file. </summary>
    internal bool OnDrop(string path)
    {
        try             
        {
            byte[] imageBytes = File.ReadAllBytes(path);
            if ((imageBytes is null) || (imageBytes.Length < 256))
            {
                throw new Exception("Failed to read image from disk: " + path);
            }

            var viewModel = App.GetRequiredService<ImagingViewModel>(); 
            return viewModel.Select(path);

            throw new Exception("Failed to load image: " + path); 
        }
        catch (Exception ex) 
        { 
            this.Logger.Warning(ex.ToString());
            return false;
        }
    }
}
