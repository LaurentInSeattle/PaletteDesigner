namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed partial class PropertiesDropPanelViewModel : ViewModel<PropertiesDropPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ObservableCollection<PropertyDropViewModel> propertyDropViewModels;

    [ObservableProperty]
    private string themeName;

    [ObservableProperty]
    private string themeVariantName;

    public PropertiesDropPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PropertyDropViewModels = [];
        this.ThemeName = string.Empty;
        this.ThemeVariantName = string.Empty;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        this.ThemeName = this.ColorTheme.FriendlyName;
        this.ThemeVariantName = this.ColorThemeVariant.Name;

        List<PropertyDropViewModel> list = [];
        foreach( var property in this.ColorTheme.Properties)
        {
            string propertyName = property.Key;
            var propertyDropViewModel = new PropertyDropViewModel(this.paletteDesignerModel, propertyName);
            list.Add(propertyDropViewModel);
        }

        this.PropertyDropViewModels = new(list);
    }

    public ColorTheme ColorTheme =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.ColorTheme;

    public ColorThemeVariant ColorThemeVariant =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.ColorTheme.Variants.Values.First();

    // MAy not need the palette 
    //
    //public Palette Palette =>
    //    this.paletteDesignerModel.ActiveProject == null ?
    //        throw new Exception("No active project") :
    //        this.paletteDesignerModel.ActiveProject.Palette;
}

