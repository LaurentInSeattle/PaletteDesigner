namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

using Lyt.Avalonia.Controls.Glyphs;
using Lyt.Avalonia.Mvvm.Utilities;
using static GeneralExtensions;

public sealed partial class ShadeSelectionToolbarViewModel : ViewModel<ShadeSelectionToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate;

    [ObservableProperty]
    private SolidColorBrush primaryBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush complementaryBaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondary1BaseBrush = new();

    [ObservableProperty]
    private SolidColorBrush secondary2BaseBrush = new();

    [ObservableProperty]
    private bool isPrimaryVisible;

    [ObservableProperty]
    private bool isComplementaryVisible;

    [ObservableProperty]
    private bool isSecondary1Visible;

    [ObservableProperty]
    private bool isSecondary2Visible;

    [ObservableProperty]
    private bool isShadingDisabled;

    [ObservableProperty]
    private bool showShadesPresets;

    [ObservableProperty]
    private bool showShadesValues;

    [ObservableProperty]
    private bool showTextSamples;
      
    public ShadeSelectionToolbarViewModel()
    {
        this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();
        this.Messenger.Subscribe<ModelPaletteUpdatedMessage>(this.OnModelPaletteUpdated);

        this.ShowShadesPresets = false;
        this.ShowShadesValues = true;
        this.ShowTextSamples = false;
    }

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    partial void OnShowShadesPresetsChanged(bool value)
        => this.Messenger.Publish(new PresetsVisibilityMessage(value));

    partial void OnShowShadesValuesChanged(bool value)
        => this.Messenger.Publish(new ShadesValuesVisibilityMessage(value));

    partial void OnShowTextSamplesChanged(bool value)
        => this.Messenger.Publish(new TextSamplesVisibilityMessage(value));

    [RelayCommand]
    public void OnLockSelect(object? parameter)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        if (parameter is string tag)
        {
            // Update model 
            ShadeMode shadeMode = Enum.TryParse(tag, out ShadeMode kind) ? kind : ShadeMode.Locked;
            this.paletteDesignerModel.UpdatePaletteShadeMode(shadeMode);
        }
    }

    [RelayCommand]
    public void OnColorSelect(object? parameter)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        if (parameter is string tag)
        {
            // Update model 
            WheelKind wheel = Enum.TryParse(tag, out WheelKind kind) ? kind : WheelKind.Unknown;
            this.paletteDesignerModel.UpdatePaletteWheelShadeMode(wheel);
        }
    }

    [RelayCommand]
    public void OnDisplayMode(object? parameter)
    {
        if (parameter is string tag)
        {
            // Update model 
            ShadesValuesDisplayMode mode = 
                Enum.TryParse(tag, out ShadesValuesDisplayMode kind) ? kind : ShadesValuesDisplayMode.Hex;
            this.paletteDesignerModel.ShadesValuesDisplayMode = mode;
            this.Messenger.Publish(new ModelShadesDisplayModeUpdated());
        }
    }
    
    private void OnModelPaletteUpdated(ModelPaletteUpdatedMessage _)
    {
        var palette = this.Palette;
        this.PrimaryBaseBrush = palette.Primary.Base.ToBrush();
        this.ComplementaryBaseBrush = palette.Complementary.Base.ToBrush();
        this.Secondary1BaseBrush = palette.Secondary1.Base.ToBrush();
        this.Secondary2BaseBrush = palette.Secondary2.Base.ToBrush();

        this.IsShadingDisabled = palette.AreShadesLocked; 

        PaletteKind paletteKind = palette.Kind;
        this.IsPrimaryVisible = true;
        this.IsComplementaryVisible = paletteKind.HasComplementaryMarker();
        this.IsSecondary1Visible = paletteKind.HasSecondary1Marker();
        this.IsSecondary2Visible = paletteKind.HasSecondary2Marker();
    }

    public void ProgrammaticSelect(Palette palette)
    {
        GlyphButton? glyphButtonLocked = this.View.FindChildControlParametrized<GlyphButton>(ShadeMode.Locked.ToString());
        GlyphButton? glyphButtonUnlocked = this.View.FindChildControlParametrized<GlyphButton>(ShadeMode.Unlocked.ToString());
        if ((glyphButtonLocked is not null)&& (glyphButtonUnlocked is not null))
        {
            With(ref this.isProgrammaticUpdate, () =>
            {
                glyphButtonLocked.IsSelected = palette.AreShadesLocked;
                glyphButtonUnlocked.IsSelected = !palette.AreShadesLocked;
            });
        }

        if (!palette.AreShadesLocked)
        {
            string parameterWheel = palette.SelectedWheel.ToString();
            GlyphButton? glyphButtonWheel = this.View.FindChildControlParametrized<GlyphButton>(parameterWheel);
            if (glyphButtonWheel is not null)
            {
                With(ref this.isProgrammaticUpdate, () =>
                {
                    if (glyphButtonWheel.Group is SelectionGroup group)
                    {
                        group.Select(glyphButtonWheel);
                    }
                    else
                    {
                        glyphButtonWheel.IsSelected = true;
                    }
                });
            }
        }
    }
}
