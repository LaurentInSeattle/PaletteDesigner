namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette : IExportAble
{
    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        this.ForAllShades((wheelKind, shades) =>
        {
            ColorGroup colorGroup = new(wheelKind.ToString());
            shades.ForAllShades((shadeKind, shade) =>
            {
                var rgb = shade.Color.ToRgb();
                byte r = (byte)Math.Round(rgb.R);
                byte g = (byte)Math.Round(rgb.G);
                byte b = (byte)Math.Round(rgb.B);
                ColorEntry colorEntry = new(shadeKind.ToString(), r, g, b);
                colorGroup.Colors.Add(colorEntry);
            });
            document.Groups.Add(colorGroup);
        });

        return document;
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
        string serializedJson = JsonSerializer.Serialize(this);
        if (!string.IsNullOrWhiteSpace(serializedJson))
        {
            return serializedJson;
        }

        throw new Exception("Failed to serialize designed palette");
    }

    public Parameters ToTemplateParameters()
    {
        var parameters = new Parameters();
        var paletteKind = new Parameter("PaletteKind", this.Kind.ToString().BeautifyEnumString());
        parameters.Add(paletteKind);
        foreach (WheelKind wheelKindEnum in Enum.GetValues<WheelKind>())
        {
            if (wheelKindEnum == WheelKind.Unknown)
            {
                continue;
            }

            string wheelName = wheelKindEnum.ToString();
            //var wheelKind = new Parameter("WheelKind", wheelName);
            //parameters.Add(wheelKind);

            Shades shades = wheelKindEnum.ToShadesFrom(this);
            foreach (ShadeKind shadeKindEnum in Enum.GetValues<ShadeKind>())
            {
                if (shadeKindEnum == ShadeKind.None)
                {
                    continue;
                }

                Shade shade = shadeKindEnum.ToShadeFrom(shades);
                string shadeName = shadeKindEnum.ToString();
                string colorName = string.Concat(wheelName, "_", shadeName);
                string colorNameValue = string.Concat(wheelName, "_", shadeName, "_ColorValue");
                RgbColor rgbColor = shade.Color.ToRgb();
                string colorValue = rgbColor.ToPoundArgbHexString();
                var color = new Parameter(colorNameValue, colorValue);
                parameters.Add(color);

#if DEBUG 
                //string colorStringFormat =
                //    "<Color x:Key=\"{0}_Color\"><# {0}_ColorValue #></Color>";
                //Debug.WriteLine(string.Format(colorStringFormat, colorName));
                //string brushStringFormat =
                //    "<SolidColorBrush x:Key=\"{0}\" Color =\"{{StaticResource {0}_Color}}\" />";
                //Debug.WriteLine(string.Format(brushStringFormat, colorName));
#endif
            }
        }

#if DEBUG 
        //foreach (Parameter parameter in parameters )
        //{
        //    Debug.WriteLine(parameter.Tag + " :   " + parameter.Value); 
        //}
#endif // DEBUG 

        return parameters;
    }
}