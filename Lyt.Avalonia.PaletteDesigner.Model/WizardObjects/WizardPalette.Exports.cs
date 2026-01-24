namespace Lyt.Avalonia.PaletteDesigner.Model.WizardObjects;

public sealed partial class WizardPalette : IExportAble
{
    // TODO: Implement AseDocument conversion
    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        //this.ForAllShades((wheelKind, shades) =>
        //{
        //    ColorGroup colorGroup = new(wheelKind.ToString());
        //    shades.ForAllShades((shadeKind, shade) =>
        //    {
        //        var rgb = shade.Color.ToRgb();
        //        byte r = (byte)Math.Round(rgb.R);
        //        byte g = (byte)Math.Round(rgb.G);
        //        byte b = (byte)Math.Round(rgb.B);
        //        ColorEntry colorEntry = new(shadeKind.ToString(), r, g, b);
        //        colorGroup.Colors.Add(colorEntry);
        //    });
        //    document.Groups.Add(colorGroup);
        //});

        return document;
    }

    // TODO: Implement CSX Parameters generation
    public Parameters ToTemplateParameters()
    {
        //List<string> colors = new(this.Swatches.Count);
        //var sortedSwatches =
        //    from swatch in this.Swatches orderby swatch.Usage descending select swatch;
        //foreach (var swatch in sortedSwatches)
        //{
        //    var rgb = swatch.HsvColor.ToRgb();
        //    colors.Add(rgb.ToPoundArgbHexString());
        //}

        //return
        //[
        //    new Parameter("ImagePaletteSource", this.Name),
        //    new Parameter("Colors", colors, ParameterKind.Collection)
        //];

        return new Parameters();
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

        // TODO: Create temprary object for serialization
        // 
        string serializedJson = JsonSerializer.Serialize(this);
        if (!string.IsNullOrWhiteSpace(serializedJson))
        {
            return serializedJson;
        }

        throw new Exception("Failed to serialize wizard palette");
    }
}
