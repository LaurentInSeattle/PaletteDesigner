namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed partial class WizardPalette : IExportAble
{
    // AseDocument conversion
    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        void CreateColorGroup(string groupName, HsvColor[] group)
        {
            ColorGroup colorGroup = new(groupName);
            for (int i = 0; i < group.Length; ++i) 
            {
                var rgb = group[i].ToRgb();
                byte r = (byte)Math.Round(rgb.R);
                byte g = (byte)Math.Round(rgb.G);
                byte b = (byte)Math.Round(rgb.B);
                ColorEntry colorEntry = new(i.ToString("D"), r, g, b);
                colorGroup.Colors.Add(colorEntry);
            };

            document.Groups.Add(colorGroup);
        };

        CreateColorGroup("Lighter", this.LighterColors);
        CreateColorGroup("Light", this.LightColors);
        CreateColorGroup("Base", this.BaseColors);
        CreateColorGroup("Dark", this.DarkColors);
        CreateColorGroup("Darker", this.DarkerColors);

        HsvColor[] lightColors = this.GetThemeColors(PaletteThemeVariant.Light);
        CreateColorGroup("Light Theme", lightColors);

        HsvColor[] darkColors = this.GetThemeColors(PaletteThemeVariant.Dark);
        CreateColorGroup("Dark Theme", darkColors);

        return document;
    }

    // CSX Parameters generation
    public Parameters ToTemplateParameters()
    {
        HsvColor[] lightColors = this.GetThemeColors(PaletteThemeVariant.Light);
        HsvColor[] darkColors = this.GetThemeColors(PaletteThemeVariant.Dark);

        return
        [
            new Parameter("ImagePaletteSource", "'Wizard' Palette"),

            new Parameter("Lighter Colors", this.LighterColors, ParameterKind.Collection),
            new Parameter("Light Colors", this.LightColors, ParameterKind.Collection),
            new Parameter("Base Colors", this.BaseColors, ParameterKind.Collection),
            new Parameter("Dark Colors", this.DarkColors, ParameterKind.Collection),
            new Parameter("Darker Colors", this.DarkerColors, ParameterKind.Collection),

            new Parameter("Light Theme", lightColors, ParameterKind.Collection),
            new Parameter("Dark Theme", darkColors, ParameterKind.Collection),
        ];
    }

    public string ToJsonString()
    {
        var jsonSerializerOptions =
            new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                WriteIndented = true,
                IndentSize = 4,
                ReadCommentHandling = JsonCommentHandling.Skip,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,

                // TODO: Check if needed
                // .Net 9 properties 
                //
                // AllowOutOfOrderMetadataProperties = true,
                // RespectRequiredConstructorParameters = true,
                // RespectNullableAnnotations= true,
            };
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        // Create temprary object for serialization
        var serializablePalette = new JSonExportableWizardPalette()
        {
            LighterColors = this.LighterColors,
            LightColors = this.LightColors,
            BaseColors = this.BaseColors,
            DarkColors = this.DarkColors,
            DarkerColors = this.DarkerColors,
            LightThemeColors = this.GetThemeColors(PaletteThemeVariant.Light),
            DarkThemeColors = this.GetThemeColors(PaletteThemeVariant.Dark),
        }; 

        string serializedJson = JsonSerializer.Serialize(serializablePalette);
        if (!string.IsNullOrWhiteSpace(serializedJson))
        {
            return serializedJson;
        }

        throw new Exception("Failed to serialize wizard palette");
    }
}