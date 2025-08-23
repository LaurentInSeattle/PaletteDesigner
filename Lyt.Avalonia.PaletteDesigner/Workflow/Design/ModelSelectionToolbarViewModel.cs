namespace Lyt.Avalonia.PaletteDesigner.Workflow.Design;

using static GeneralExtensions;

public sealed partial class ModelSelectionToolbarViewModel : ViewModel<ModelSelectionToolbarView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate;

    public ModelSelectionToolbarViewModel()
        => this.paletteDesignerModel = App.GetRequiredService<PaletteDesignerModel>();

    public Palette Palette =>
        this.paletteDesignerModel.ActiveProject == null ?
            throw new Exception("No active project") :
            this.paletteDesignerModel.ActiveProject.Palette;

    [RelayCommand]
    public void OnModelSelect(object? parameter)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        if (parameter is string tag)
        {
            PaletteKind paletteKind = Enum.TryParse(tag, out PaletteKind kind) ? kind : PaletteKind.Unknown;
            if (paletteKind != PaletteKind.Unknown)
            {
                // Update model 
                this.paletteDesignerModel.UpdatePaletteKind(paletteKind);
            }
        }
    }

    [RelayCommand]
    public void OnRandomize() => this.paletteDesignerModel.RandomizePalette();

    public void ProgrammaticSelect(Palette palette)
    {
        // find the control with a parameter string corresponding to palette kind
        string parameter = palette.Kind.ToString();
        GlyphButton? glyphButton = this.View.FindChildControlParametrized<GlyphButton>(parameter);
        if (glyphButton is not null)
        {
            With(ref this.isProgrammaticUpdate, () =>
            {
                if (glyphButton.Group is SelectionGroup group)
                {
                    group.Select(glyphButton);
                }
                else
                {
                    glyphButton.IsSelected = true;
                }
            });
        }
    }
}
