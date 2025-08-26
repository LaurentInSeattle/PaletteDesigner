namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class ColorDragViewModel : ViewModel<ColorDragView> , IRecipient<LanguageChangedMessage>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly Palette palette;
    private readonly WheelKind wheelKind;
    private readonly Shades shades;

    [ObservableProperty]
    private string colorName;

    [ObservableProperty]
    private ObservableCollection<ShadeDragViewModel> shadeDragViewModels; 

    public ColorDragViewModel(
        PaletteDesignerModel paletteDesignerModel,
        Palette palette, WheelKind wheelKind, Shades shades)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.palette = palette;
        this.wheelKind = wheelKind;
        this.shades = shades;
        this.ShadeDragViewModels = [];
        this.ColorName = string.Empty;

        this.Subscribe<LanguageChangedMessage>();
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        List<ShadeDragViewModel> list = [];
        this.shades.ForAllShades((shadeKind, shade) =>
        {
            var shadeDragViewModel = new ShadeDragViewModel(this.palette, this.wheelKind, shadeKind, shade);
            list.Add(shadeDragViewModel);
        });

        this.ShadeDragViewModels = new(list);
        this.Localize();
    }

    public void Receive(LanguageChangedMessage _) => this.Localize();

    private void Localize()
    { 
        string wheelName = wheelKind.ToString();
        string locString = string.Concat("Design.Toolbar.Shade.", wheelName);
        this.ColorName = this.Localize(locString);
    }
}
