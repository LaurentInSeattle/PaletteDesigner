namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

using Lyt.Avalonia.PaletteDesigner.Model.ProjectObjects;

public sealed partial class TextPreviewViewModel : ViewModel<TextPreviewView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private Palette? palette;
    private WheelKind wheelKindForeground;
    private ShadeKind shadeKindForeground;
    private WheelKind wheelKindBackground;
    private ShadeKind shadeKindBackground;

    [ObservableProperty]
    SolidColorBrush foregroundBrush;

    [ObservableProperty]
    SolidColorBrush backgroundBrush;

    [ObservableProperty]
    string setupText;
    
    public TextPreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.BackgroundBrush = new SolidColorBrush(Colors.Black);
        this.ForegroundBrush = new SolidColorBrush(Colors.White);
        this.SetupText = string.Empty;

        this.wheelKindForeground = WheelKind.Primary;
        this.shadeKindForeground = ShadeKind.Lighter;
        this.wheelKindBackground = WheelKind.Complementary;
        this.shadeKindBackground = ShadeKind.Darker;
        this.Colorize();
        this.Localize();

        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    public void Update(TextSampleSetup textSampleSetup)
    {
        this.wheelKindForeground = textSampleSetup.WheelKindForeground;
        this.shadeKindForeground = textSampleSetup.ShadeKindForeground;
        this.wheelKindBackground = textSampleSetup.WheelKindBackground;
        this.shadeKindBackground = textSampleSetup.ShadeKindBackground;
        this.Localize();
        this.Colorize();
    }

    private void OnLanguageChanged(LanguageChangedMessage? _) => this.Localize(); 

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage _)
    {
        if ((this.paletteDesignerModel.ActiveProject is not Project project) ||
            (project.Palette is null))
        {
            // Should never happen, BYNK... 
            return;
        }

        this.palette = project.Palette;
        this.Localize();
        this.Colorize();
    }

    private void Localize()
    {
        if (this.palette is null)
        {
            return;
        }

        this.SetupText =
            string.Format(
                "{0} {1}   ~   {2} {3}",
                this.Localize(this.wheelKindForeground.ToLocalizationKey(this.palette)),
                this.Localize(this.shadeKindForeground.ToLocalizationKey()),
                this.Localize(this.wheelKindBackground.ToLocalizationKey(this.palette)),
                this.Localize(this.shadeKindBackground.ToLocalizationKey())
            );
    }

    private void Colorize()
    {
        if (this.palette is null)
        {
            return;
        }

        var shades = this.wheelKindForeground.ToShadesFrom(this.palette); 
        var shade = this.shadeKindForeground.ToShadeFrom(shades);
        this.ForegroundBrush = shade.ToBrush();
        shades = this.wheelKindBackground.ToShadesFrom(this.palette);
        shade = this.shadeKindBackground.ToShadeFrom(shades);
        this.BackgroundBrush = shade.ToBrush();
    }
}
