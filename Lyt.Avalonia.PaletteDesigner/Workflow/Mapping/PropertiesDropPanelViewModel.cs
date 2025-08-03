namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

public sealed partial class PropertiesDropPanelViewModel : ViewModel<PropertiesDropPanelView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    private ObservableCollection<PropertyDropViewModel> propertyDropViewModels;

    [ObservableProperty]
    private string themeName;

    public PropertiesDropPanelViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.PropertyDropViewModels = [];
        this.ThemeName = string.Empty;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        // TODO !!! 
        List<PropertyDropViewModel> list = [];
        for (int i = 0; i <12 ; i++) 
        {
            // TODO !!! 
            string propertyName = 
                string.Format("Fluent {0}" , (45 +i*i).ToString("X4"));

            var propertyDropViewModel = new PropertyDropViewModel(this.paletteDesignerModel, propertyName);
            list.Add(propertyDropViewModel);
        }

        this.PropertyDropViewModels = new(list);

        // TODO !!! 
        this.ThemeName = "Avalonia/UWP Fluent Theme";
    }

    // MAy not need the palette 

    //public Palette Palette =>
    //    this.paletteDesignerModel.ActiveProject == null ?
    //        throw new Exception("No active project") :
    //        this.paletteDesignerModel.ActiveProject.Palette;
}

