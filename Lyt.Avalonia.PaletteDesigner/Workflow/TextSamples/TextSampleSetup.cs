namespace Lyt.Avalonia.PaletteDesigner.Workflow.TextSamples;

public sealed class TextSampleSetup
{
    public WheelKind WheelKindForeground { get; private set; }
    public ShadeKind ShadeKindForeground { get; private set; }
    public WheelKind WheelKindBackground { get; private set; }
    public ShadeKind ShadeKindBackground { get; private set; }

    public TextSampleSetup(
        TextSamplesDisplayMode displayMode,
        WheelKind foreground, 
        WheelKind background)
    {
        this.WheelKindForeground = foreground;
        this.WheelKindBackground = background;
        bool light = displayMode == TextSamplesDisplayMode.Light;
        this.ShadeKindBackground = light ? ShadeKind.Lighter : ShadeKind.Darker;
        this.ShadeKindForeground = light ? ShadeKind.Darker : ShadeKind.Lighter;
    }
}

