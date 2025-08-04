namespace Lyt.Avalonia.PaletteDesigner.Model.ThemeObjects;

public sealed class ColorTheme
{
    public ColorTheme() { /* req'd for serialisation */ }

    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public string FriendlyName { get; set; } = string.Empty;

    [JsonRequired]
    public Dictionary<string, ColorThemeVariant> Variants { get; set; } = [];

    [JsonRequired]
    public Dictionary<string, ColorProperty> Properties { get; set; } = [];

    public ColorTheme(ColorThemeDefinition colorThemeDefinition)
    {
        this.Name = colorThemeDefinition.Name;
        this.FriendlyName = colorThemeDefinition.FriendlyName;
        bool isFirst = true;
        foreach (string variant in colorThemeDefinition.Variants)
        {
            ColorThemeVariant colorThemeVariant = new();
            if (isFirst)
            {
                colorThemeVariant.IsDefault = true;
                isFirst = false;
            }

            colorThemeVariant.Name = variant;
            this.Variants.Add(variant, colorThemeVariant);
        }

        foreach (string propertyName in colorThemeDefinition.PropertyNames)
        {
            var colorProperty = new ColorProperty()
            {
                Name = propertyName,
                PropertyValues = [],
            };

            var values = colorProperty.PropertyValues;
            foreach (string variantKey in this.Variants.Keys)
            {
                var value = new ColorPropertyValue();
                values.Add(variantKey, value);
            }

            this.Properties.Add(propertyName, colorProperty);
        }
    }

    public static ColorTheme TestCreate()
    {
        var theme = new ColorTheme()
        {
            Name = "TestTheme",
            Variants = new Dictionary<string, ColorThemeVariant>()
            {
                {  "Light", new(){ Name = "Light" , IsDefault = true } },
                {  "Dark", new(){ Name = "Dark"} },
            },
            Properties = new Dictionary<string, ColorProperty>()
            {
                {
                    "Background" , new ColorProperty()
                    {
                        Name ="Background" ,
                        PropertyValues = new Dictionary<string, ColorPropertyValue>()
                        {
                            { "Light", new ColorPropertyValue() {  Rgb = 0xFF_FF_FF, Opacity = 1.0} },
                            { "Dark", new ColorPropertyValue() {  Rgb = 0x20_20_20, Opacity = 1.0} },
                        }
                    }
                },
                {
                    "Foreground" , new ColorProperty()
                    {
                        Name ="Foreground" ,
                        PropertyValues = new Dictionary<string, ColorPropertyValue>()
                        {
                            { "Light", new ColorPropertyValue() {  Rgb = 0x10_10_10, Opacity = 1.0} },
                            { "Dark", new ColorPropertyValue() {  Rgb = 0xF0_F0_F0, Opacity = 1.0} },
                        }
                    }
                }
            }
        };

        return theme;
    }

    public uint GetArgbColor(string variantName, string colorPropertyName)
    {
        var colorPropertyValue = this.GetColorPropertyValue(variantName, colorPropertyName);
        return colorPropertyValue.ToUintArgb();
    }

    public RgbColor GetRgbColor(string variantName, string colorPropertyName)
    {
        var colorPropertyValue = this.GetColorPropertyValue(variantName, colorPropertyName);
        uint rgb = colorPropertyValue.Rgb;
        return new(rgb);
    }

    public double GetOpacity(string variantName, string colorPropertyName)
    {
        var colorPropertyValue = this.GetColorPropertyValue(variantName, colorPropertyName);
        return colorPropertyValue.Opacity;
    }

    public void SetShade(string variantName, string colorPropertyName, Shade shade)
    {
        var colorPropertyValue = this.GetColorPropertyValue(variantName, colorPropertyName);
        RgbColor rgbColor = shade.Color.ToRgb();
        colorPropertyValue.Rgb = rgbColor.ToRgbUint();
    }

    public void SetOpacity(string variantName, string colorPropertyName, double opacity)
    {
        var colorPropertyValue = this.GetColorPropertyValue(variantName, colorPropertyName);
        colorPropertyValue.Opacity = opacity;
    }

    private ColorPropertyValue GetColorPropertyValue(string variantName, string colorPropertyName)
    {
        if (!this.Variants.ContainsKey(variantName))
        {
            throw new Exception("No such theme variant: " + variantName + " for: " + this.Name);
        }

        if (!this.Properties.TryGetValue(colorPropertyName, out ColorProperty? colorProperty)
            || colorProperty is null)
        {
            throw new Exception(
                "No such color property: " + colorPropertyName + " for theme: " + this.Name);
        }

        if (!colorProperty.PropertyValues.TryGetValue(variantName, out ColorPropertyValue? colorPropertyValue)
            || colorPropertyValue is null)
        {
            throw new Exception(
                "No such color property: " + colorPropertyName +
                " for theme variant: " + variantName
                + " for theme: " + this.Name);
        }

        return colorPropertyValue;
    }
}
