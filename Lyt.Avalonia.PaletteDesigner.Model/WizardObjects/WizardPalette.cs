namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed class WizardPalette
{
    [JsonRequired]
    public double BaseWheel { get; set; }

    [JsonRequired]
    public double CurvePower { get; set; }

    [JsonRequired]
    public double WheelAngleDistance { get; set; }

    [JsonRequired]
    public double Highlights { get; set; }
}
