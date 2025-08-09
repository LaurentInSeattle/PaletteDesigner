namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

using static GeneralExtensions; 

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    private readonly ShadesValuesViewModel PrimaryShadesValues;

    private readonly ShadesValuesViewModel Secondary1ShadesValues;

    private readonly ShadesValuesViewModel Secondary2ShadesValues;

    private readonly ShadesValuesViewModel ComplementaryShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel topLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel topRightShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomRightShadesValues;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    [ObservableProperty]
    private MaxiPaletteViewModel maxiPaletteViewModel;

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    private string wheelValue = string.Empty;

    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate; 

    private double wheel;

    public PalettePreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.MiniPaletteViewModel = new(this.paletteDesignerModel);
        this.MaxiPaletteViewModel = new();
        
        string dominant = this.Localize("Design.Toolbar.Shade.Primary");
        this.PrimaryShadesValues = new ShadesValuesViewModel(dominant);
        string accent = this.Localize("Design.Toolbar.Shade.Complementary");
        this.ComplementaryShadesValues = new ShadesValuesViewModel(accent);
        string discord1 = this.Localize("Design.Toolbar.Shade.Secondary1");
        this.Secondary1ShadesValues = new ShadesValuesViewModel(discord1);
        string discord2 = this.Localize("Design.Toolbar.Shade.Secondary2");
        this.Secondary2ShadesValues = new ShadesValuesViewModel(discord2);

        this.TopLeftShadesValues     = this.PrimaryShadesValues;
        this.BottomLeftShadesValues  = this.PrimaryShadesValues;
        this.TopRightShadesValues    = this.PrimaryShadesValues;
        this.BottomRightShadesValues = this.PrimaryShadesValues;

        this.Messenger.Subscribe<ShadesValuesVisibilityMessage>(this.OnShadesValuesVisibility);
        this.Messenger.Subscribe<ModelShadesDisplayModeUpdated>(this.OnShadesDisplayModeUpdated);
        this.Messenger.Subscribe<LanguageChangedMessage>(this.OnLanguageChanged);
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    public void ShowValues(bool show = true)
    {
        if (this.IsBound)
        {
            var grid = this.View.MainGrid;
            var columns = grid.ColumnDefinitions;
            var newGridLength = new GridLength(show ? 120.0 : 0.0);
            columns[0].Width = newGridLength;
            columns[2].Width = newGridLength;
            grid.Width = show ? 840.0 : 600.0; 
        }
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.ShowValues(); 
        this.WheelSliderValue = 0.0;
        this.WheelValue = string.Empty;
        this.UpdateLabels();
    }

    partial void OnWheelSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return; 
        } 

        this.wheel = value;
        this.UpdateLabels();
        this.paletteDesignerModel.UpdatePalettePrimaryWheel(value);
    }

    public void UpdateLabels() 
        => this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        With(ref this.isProgrammaticUpdate, () =>
        {
            this.wheel = palette.Primary.Wheel; 
            this.WheelSliderValue = palette.Primary.Wheel;
            var primaryColor = palette.Primary.Base.Color;
            this.UpdateLabels();
        });

        this.MiniPaletteViewModel.Update(palette);
        this.MaxiPaletteViewModel.Update(palette);

        Shades shades = palette.Primary;
        this.PrimaryShadesValues.Update(shades);
        if ((colorCount == 1) || (colorCount == 2))
        {
            this.TopLeftShadesValues = this.PrimaryShadesValues;
            this.BottomLeftShadesValues = this.PrimaryShadesValues;
            this.TopRightShadesValues = this.PrimaryShadesValues;
            if (colorCount == 2)
            {
                // colorCount == 1 => Complementary same as primary 
                shades = palette.Complementary;
                this.ComplementaryShadesValues.Update(shades);
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
            }
            else
            {
                this.BottomRightShadesValues = this.PrimaryShadesValues;
            }
        }
        else // colorCount == 3 or 4 
        {
            this.TopLeftShadesValues = this.PrimaryShadesValues;
            shades = palette.Secondary1;
            this.Secondary1ShadesValues.Update(shades);
            this.TopRightShadesValues = this.Secondary1ShadesValues;

            shades = palette.Secondary2;
            this.Secondary2ShadesValues.Update(shades);
            this.BottomLeftShadesValues = this.Secondary2ShadesValues;

            if (colorCount == 4)
            {
                shades = palette.Complementary;
                this.ComplementaryShadesValues.Update(shades);
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
            }
            else
            {
                // colorCount == 3 => Complementary same as primary 
                shades = palette.Primary;
                this.BottomRightShadesValues = this.PrimaryShadesValues;
            }
        }
    }

    private void OnShadesDisplayModeUpdated(ModelShadesDisplayModeUpdated updated)
    {
        var palette = this.Palette;
        Shades shades = palette.Primary;
        this.PrimaryShadesValues.Update(shades);
        shades = palette.Complementary;
        this.ComplementaryShadesValues.Update(shades);
        shades = palette.Secondary1;
        this.Secondary1ShadesValues.Update(shades);
        shades = palette.Secondary2;
        this.Secondary2ShadesValues.Update(shades);
    }

    private void OnShadesValuesVisibility(ShadesValuesVisibilityMessage message)
        => this.ShowValues(message.Show);

    private void OnLanguageChanged(LanguageChangedMessage message)
    {
        string dominant = this.Localize("Design.Toolbar.Shade.Primary");
        this.PrimaryShadesValues.Update(dominant);
        string accent = this.Localize("Design.Toolbar.Shade.Complementary");
        this.ComplementaryShadesValues.Update(accent);
        string discord1 = this.Localize("Design.Toolbar.Shade.Secondary1");
        this.Secondary1ShadesValues.Update(discord1);
        string discord2 = this.Localize("Design.Toolbar.Shade.Secondary2");
        this.Secondary2ShadesValues.Update(discord2);
    }
}
