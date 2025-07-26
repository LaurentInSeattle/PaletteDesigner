namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed class ShadesPreset
{
    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public Position Lighter { get; set; } = new();

    [JsonRequired]
    public Position Light { get; set; } = new();

    [JsonRequired]
    public Position Base { get; set; } = new();

    [JsonRequired]
    public Position Dark { get; set; } = new();

    [JsonRequired]
    public Position Darker { get; set; } = new();

    public ShadesPreset() { /* needed for serialization */ }

    public ShadesPreset(string name, Shades shades)
    {
        this.Name = name;   
        this.Lighter = shades.Lighter.Position.ToSizeIndependant();
        this.Light = shades.Light.Position.ToSizeIndependant();
        this.Base = shades.Base.Position.ToSizeIndependant();
        this.Dark = shades.Dark.Position.ToSizeIndependant();
        this.Darker = shades.Darker.Position.ToSizeIndependant();
    }

    public void ApplyTo(Shades shades)
    {
        shades.Lighter.Position = this.Lighter.FromSizeIndependant();
        shades.Light.Position = this.Light.FromSizeIndependant();
        shades.Base.Position = this.Base.FromSizeIndependant();
        shades.Dark.Position = this.Dark.FromSizeIndependant();
        shades.Darker.Position = this.Darker.FromSizeIndependant();
    }
}
