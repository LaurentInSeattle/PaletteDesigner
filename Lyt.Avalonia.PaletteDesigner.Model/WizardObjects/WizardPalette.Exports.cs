namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

using Lyt.ImageProcessing.ColorObjects;

using System.Drawing;

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
            }
            ;

            document.Groups.Add(colorGroup);
        }
        ;

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
        var parameters = new Parameters
        {
            new Parameter("WizardPaletteSource", "'Wizard' Palette")
        };

        void CreateStringColorsCollection(string tag, HsvColor[] hsvColors)
        {
            List<string> colors = new(hsvColors.Length);
            foreach (var hsvColor in hsvColors)
            {
                var rgb = hsvColor.ToRgb();
                colors.Add(rgb.ToPoundArgbHexString());
            }

            var parameter = new Parameter(tag, colors, ParameterKind.Collection);
            parameters.Add(parameter);
        }

        CreateStringColorsCollection("LighterColors", this.LighterColors);
        CreateStringColorsCollection("LightColors", this.LightColors);
        CreateStringColorsCollection("BaseColors", this.BaseColors);
        CreateStringColorsCollection("DarkColors", this.DarkColors);
        CreateStringColorsCollection("DarkerColors", this.DarkerColors);

        void CreateThemeStringColors(string theme, HsvColor[] hsvColors)
        {
            string[] names = 
                [
                    "Background",
                    "Foreground",
                    "Accent",
                    "Discordant",
                ];
            for ( int k = 0; k < hsvColors.Length; ++ k )
            {
                var hsvColor = hsvColors[k];
                var rgb = hsvColor.ToRgb();
                string colorValue = rgb.ToPoundArgbHexString();
                string name = names[k];
                string tag = string.Format("{0}Theme_{1}_ColorValue", theme, name);
                parameters.Add(new Parameter(tag, colorValue));
            }
        }


        HsvColor[] lightColors = this.GetThemeColors(PaletteThemeVariant.Light);
        CreateThemeStringColors("Light", lightColors);
        HsvColor[] darkColors = this.GetThemeColors(PaletteThemeVariant.Dark);
        CreateThemeStringColors("Dark", darkColors);

        return parameters;
    }

    public string ToJsonString(FileManagerModel fileManager)
    {
        // Create temporary object for serialization
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

        return fileManager.Serialize(serializablePalette);
    }
}