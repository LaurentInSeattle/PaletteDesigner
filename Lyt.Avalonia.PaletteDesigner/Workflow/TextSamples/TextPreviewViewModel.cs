namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

using Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

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

        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);
    }

    public void Update(
        WheelKind wheelKindForeground, ShadeKind shadeKindForeground,
        WheelKind wheelKindBackground, ShadeKind shadeKindBackground)
    {
        this.wheelKindForeground = wheelKindForeground;
        this.shadeKindForeground = shadeKindForeground;
        this.wheelKindBackground = wheelKindBackground;
        this.shadeKindBackground = shadeKindBackground;
        this.Colorize();
    }

    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage _)
    {
        this.palette = this.paletteDesignerModel.ActiveProject!.Palette;
        this.Colorize(); 
    }

    private void Colorize()
    {
        if (this.palette is null)
        {
            return;
        }

        this.SetupText =
            string.Format(
                "{0} {1}   ~   {2} {3} ",
                this.Localize(this.wheelKindForeground.ToLocalizationKey(this.palette)), 
                this.Localize(this.shadeKindForeground.ToLocalizationKey()), 
                this.Localize(this.wheelKindBackground.ToLocalizationKey(this.palette)), 
                this.Localize(this.shadeKindBackground.ToLocalizationKey()) 
            );
        var shades = this.wheelKindForeground.ToShadesFrom(this.palette); 
        var shade = this.shadeKindForeground.ToShadeFrom(shades);
        this.ForegroundBrush = shade.ToBrush();
        shades = this.wheelKindBackground.ToShadesFrom(this.palette);
        shade = this.shadeKindBackground.ToShadeFrom(shades);
        this.BackgroundBrush = shade.ToBrush();
    }
}
