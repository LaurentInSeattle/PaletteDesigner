namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using global::Avalonia.Themes.Fluent;

public sealed partial class MappingViewModel : ViewModel<MappingView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ColorsDragPanelViewModel colorsDragPanelViewModel;

    [ObservableProperty]
    private PropertiesDropPanelViewModel propertiesDropPanelViewModel;

    [ObservableProperty]
    private WidgetsPreviewViewModel widgetsPreviewViewModel;

    public MappingViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.ColorsDragPanelViewModel = new(paletteDesignerModel);
        this.PropertiesDropPanelViewModel = new(paletteDesignerModel);
        this.WidgetsPreviewViewModel = new("Preview");

        this.Messenger.Subscribe<ModelThemeUpdatedMessage>(this.OnModelThemeUpdated);
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
    }

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage? _)
    {
    }

    private void OnModelThemeUpdated(ModelThemeUpdatedMessage? _)
    {
        // Debugger.Break();

        var lightColorPaletteResources = this.CreateColorPaletteResources(isDark: false);
        var darkColorPaletteResources = this.CreateColorPaletteResources(isDark: true);
        this.WidgetsPreviewViewModel.UpdatePalettes(lightColorPaletteResources, darkColorPaletteResources);
    }


    public ColorPaletteResources CreateColorPaletteResources(bool isDark)
    {
        var colorTheme = this.paletteDesignerModel.ActiveProject!.ColorTheme;
        string variant = isDark ? "Dark" : "Light"; 

        Color ToColor(string name)
            => Color.FromUInt32(colorTheme.GetArgbColor(variant, name));

        return new ColorPaletteResources
        {
            RegionColor = isDark ? Colors.Blue: Colors.Red,

            Accent = ToColor("SystemAccentColor"),

            ErrorText = ToColor("SystemErrorTextColor"),

            AltHigh = ToColor("SystemAltHighColor"),
            AltLow = ToColor("SystemAltLowColor"),
            AltMedium = ToColor("SystemAltMediumColor"),
            AltMediumHigh = ToColor("SystemAltMediumHighColor"),
            AltMediumLow = ToColor("SystemAltMediumLowColor"),

            BaseHigh = ToColor("SystemBaseHighColor"),
            BaseLow = ToColor("SystemBaseLowColor"),
            BaseMedium = ToColor("SystemBaseMediumColor"),
            BaseMediumHigh = ToColor("SystemBaseMediumHighColor"),
            BaseMediumLow = ToColor("SystemBaseMediumLowColor"),

            ChromeAltLow = ToColor("SystemChromeAltLowColor"),

            ChromeBlackHigh = ToColor("SystemChromeBlackHighColor"),
            ChromeBlackLow = ToColor("SystemChromeBlackLowColor"),
            ChromeBlackMedium = ToColor("SystemChromeBlackMediumColor"),
            ChromeBlackMediumLow = ToColor("SystemChromeBlackLowColor"),
            
            ChromeDisabledHigh = ToColor("SystemChromeDisabledHighColor"),
            ChromeDisabledLow = ToColor("SystemChromeDisabledLowColor"),

            ChromeGray = ToColor("SystemChromeGrayColor"),
            ChromeHigh = ToColor("SystemChromeHighColor"),
            ChromeLow = ToColor("SystemChromeLowColor"),
            ChromeMedium = ToColor("SystemChromeMediumColor"),
            ChromeMediumLow = ToColor("SystemChromeMediumLowColor"),
            ChromeWhite = ToColor("SystemChromeWhiteColor"),

            ListLow = ToColor("SystemListLowColor"),
            ListMedium = ToColor("SystemListMediumColor")
        };
    }

    /*
     */

}
