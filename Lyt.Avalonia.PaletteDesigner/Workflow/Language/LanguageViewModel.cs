namespace Lyt.Avalonia.PaletteDesigner.Workflow.Language;

public sealed partial class LanguageViewModel : ViewModel<LanguageView>
{
    private static readonly List<LanguageInfoViewModel> SupportedLanguages =
    [
        new LanguageInfoViewModel("uk-UA", "Українська мова" , "Ukraine.png", string.Empty) ,
        new LanguageInfoViewModel("it-IT", "Italiano" , "Italy.png" , "San_Marino.png" ) ,
        new LanguageInfoViewModel("fr-FR", "Français" , "France.png" , "Quebec.png" ) ,
        new LanguageInfoViewModel("en-US", "English" , "United_Kingdom.png" , "Canada.png" ) ,
        
        new LanguageInfoViewModel("es-ES", "Español" , "Spain.png" , "Mexico.png" ) ,
        new LanguageInfoViewModel("de-DE", "Deutsch" , "Germany.png" , "Austria.png" ) ,
        new LanguageInfoViewModel("bg-BG", "Български език" , "Bulgaria.png" , string.Empty ) ,
        new LanguageInfoViewModel("el-GR", "Ελληνικά" , "Greece.png", "Cyprus.png" ) ,
        new LanguageInfoViewModel("jp-JP", "日本語", "Japan.png" , string.Empty ) ,

        new LanguageInfoViewModel("hy-AM", "Հայերէն", "Armenia.png" , string.Empty ) ,

        new LanguageInfoViewModel("ko-KO", "한국인 - 조선어", "South_Korea.png" , "North_Korea.png") ,
        new LanguageInfoViewModel("zh-CN", "簡體 中文", "China.png" , string.Empty ) ,
        new LanguageInfoViewModel("zh-TW", "繁體 中文", "Taiwan.png" , string.Empty ) ,

        // Hindi and Bengali hi-IN /  bn-BD 
        new LanguageInfoViewModel("hi-IN", "हिन्दी", "India.png" , string.Empty ) ,
        new LanguageInfoViewModel("bn-BD", "বাঙ্গালী", "Bangladesh.png" , string.Empty ) ,
        // {  "hu-HU" , new Language( "", "hu", "", "Magyar", "Hungary") },
        new LanguageInfoViewModel("hu-HU", "Magyar" , "Hungary.png" , string.Empty ) ,
    ];

    private readonly PaletteDesignerModel paletteDesignerModel;
    private bool isInitializing; 

    public LanguageViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.isInitializing = true;
        this.Languages = [.. SupportedLanguages];
        this.isInitializing = false;
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);
        string key = this.paletteDesignerModel.Language;
        int index = 0;
        for (int i = 0; i < this.Languages.Count; ++i)
        {
            if (key == this.Languages[i].Key)
            {
                index = i;
                break;
            } 
        }

        this.isInitializing = true;
        this.SelectedLanguageIndex = index;
        this.isInitializing = false;
    }

    [ObservableProperty]
    private int selectedLanguageIndex ; 

    partial void OnSelectedLanguageIndexChanged(int value)
    { 
        // Do not change the language when initializing 
        if (this.isInitializing)
        {
            return; 
        } 

        string languageKey = this.Languages[value].Key; 
        Debug.WriteLine("Selected language: " + languageKey);
        this.paletteDesignerModel.SelectLanguage (languageKey);
    }

    [ObservableProperty]    
    private ObservableCollection<LanguageInfoViewModel> languages; 
}
