namespace Lyt.Avalonia.PaletteDesigner.Workflow.Language;

public sealed partial class LanguageInfoViewModel : ViewModel<LanguageInfoView>
{
    private const string UriPath = "avares://Lyt.Avalonia.PaletteDesigner/Assets/Images/Flags/";

    [ObservableProperty]
    private string key;

    [ObservableProperty]
    private string name;

    [ObservableProperty]
    private Bitmap? flagOne;

    [ObservableProperty]
    private Bitmap flagTwo;

    public LanguageInfoViewModel(string key, string name, string flagOne, string flagTwo)
    {
        this.Key = key;
        this.Name = name;
        if (string.IsNullOrWhiteSpace(flagTwo))
        {
            this.FlagTwo = new Bitmap(AssetLoader.Open(new Uri(UriPath + flagOne)));
        } 
        else
        {
            this.FlagOne = new Bitmap(AssetLoader.Open(new Uri(UriPath + flagOne)));
            this.FlagTwo = new Bitmap(AssetLoader.Open(new Uri(UriPath + flagTwo)));
        }
    }
}
