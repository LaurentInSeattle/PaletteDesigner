namespace Lyt.Avalonia.PaletteDesigner.Workflow.Language;

public sealed partial class LanguageInfoViewModel : ViewModel<LanguageInfoView>
{
    private const string UriPath = "avares://Lyt.Avalonia.PaletteDesigner/Assets/Images/Flags/";

    [ObservableProperty]
    public partial string Key { get; set; }

    [ObservableProperty]
    public partial string Name { get; set; }

    [ObservableProperty]
    public partial Bitmap? FlagOne { get; set; }

    [ObservableProperty]
    public partial Bitmap FlagTwo { get; set; }

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
