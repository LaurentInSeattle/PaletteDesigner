namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

public sealed partial class MiniPaletteViewModel : ViewModel<MiniPaletteView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;
    private readonly bool isPreset;
    private readonly PaletteColorViewModel PrimaryShades;
    private readonly PaletteColorViewModel ComplementaryShades;
    private readonly PaletteColorViewModel Secondary1Shades;
    private readonly PaletteColorViewModel Secondary2Shades;

    [ObservableProperty]
    private PaletteColorViewModel leftShades;

    [ObservableProperty]
    private PaletteColorViewModel middleLeftShades;

    [ObservableProperty]
    private PaletteColorViewModel middleRightShades;

    [ObservableProperty]
    private PaletteColorViewModel rightShades;

    // Cannot be an observable property because DataContext is set programmatically 
    private int PrimaryShadesColumnSpan
    {
        set
        {
            if (this.IsBound)
            {
                this.View.PrimaryShades.SetValue(Grid.ColumnSpanProperty, value);
            }
        }
    }

    public MiniPaletteViewModel(
        PaletteDesignerModel paletteDesignerModel, bool isPreset = false)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.isPreset = isPreset;
        this.PrimaryShadesColumnSpan = 4;
        this.PrimaryShades = new();
        this.ComplementaryShades = new();
        this.Secondary1Shades = new();
        this.Secondary2Shades = new();
        this.LeftShades = this.PrimaryShades;
        this.MiddleLeftShades = this.PrimaryShades;
        this.MiddleRightShades = this.PrimaryShades;
        this.RightShades = this.PrimaryShades;
        this.isPreset = isPreset;
    }

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        if(this.paletteDesignerModel.ActiveProject is null)
        {
            // Not ready ? Should throw ? 
            // Should never happen since we have a palette 
            return;
        }

        if (!palette.AreShadesLocked && this.isPreset)
        {
            this.PrimaryShadesColumnSpan = 7;
            var selectedWheel = palette.SelectedWheel;
            Shades shades = selectedWheel.ToShadesFrom(palette);
            this.PrimaryShades.Update(shades);
        }
        else
        {
            Shades shades = palette.Primary;
            this.PrimaryShades.Update(shades);

            if ((colorCount == 1) || (colorCount == 2))
            {
                // Secondary shades same as Primary 
                this.LeftShades = this.PrimaryShades;
                this.MiddleLeftShades = this.PrimaryShades;
                this.MiddleRightShades = this.PrimaryShades;
                if (colorCount == 2)
                {
                    // colorCount == 1 => Complementary same as primary 
                    shades = palette.Complementary;
                    this.PrimaryShadesColumnSpan = 5;
                    this.RightShades = this.ComplementaryShades;
                    this.ComplementaryShades.Update(shades);
                }
                else
                {
                    this.PrimaryShadesColumnSpan = 7;
                }
            }
            else // colorCount == 3 or 4 
            {
                this.PrimaryShadesColumnSpan = 1;

                this.LeftShades = this.PrimaryShades;

                shades = palette.Secondary1;
                this.Secondary1Shades.Update(shades);
                this.MiddleLeftShades = this.Secondary1Shades;

                shades = palette.Secondary2;
                this.Secondary2Shades.Update(shades);
                this.MiddleRightShades = this.Secondary2Shades;

                if (colorCount == 4)
                {
                    shades = palette.Complementary;
                    this.ComplementaryShades.Update(shades);
                    this.RightShades = this.ComplementaryShades;
                }
                else
                {
                    // colorCount == 3 => Complementary same as primary 
                    this.RightShades = this.PrimaryShades;
                }
            }
        } 
    }
}