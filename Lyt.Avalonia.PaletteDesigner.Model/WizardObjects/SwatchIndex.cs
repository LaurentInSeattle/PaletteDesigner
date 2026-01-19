namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

/// <summary> Needs to be serializable record class but C# 14 does not support that yet. </summary>
public sealed class SwatchIndex
{
    public SwatchIndex()  { /* Required for serialization */ }

    public SwatchIndex(SwatchKind kind, int index) 
    { 
        this.Kind = kind;
        this.Index = index;
    }

    /* Public setters: Required for serialization */
    public SwatchKind Kind { get; set; }

    public int Index { get; set; }
}
