namespace Lyt.Avalonia.PaletteDesigner.Model.SwatchObjects;

public sealed class ColorSwatches : IExportAble
{
    public string ImagePath { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public bool IsDeepAlgorithmStrength { get; set; }

    public List<Swatch> Swatches { get; set; } = [];

    public ColorSwatches DeepClone()
    {
        var list = new List<Swatch>(this.Swatches.Count);
        foreach (var swatch in this.Swatches)
        {
            list.Add(swatch.DeepClone());
        }

        return new()
        {
            ImagePath = new string(this.ImagePath),
            Name = new string(this.Name),
            Swatches = list, 
        };
    }

    public AseDocument ToAseDocument()
    {
        AseDocument document = new();
        ColorGroup colorGroup = new(this.Name.ToString());
        int index = 0;
        var sortedSwatches = 
            from swatch in this.Swatches orderby swatch.Usage descending select swatch; 
        foreach (var swatch in sortedSwatches)
        {
            var rgb = swatch.HsvColor.ToRgb();
            byte r = (byte)Math.Round(rgb.R);
            byte g = (byte)Math.Round(rgb.G);
            byte b = (byte)Math.Round(rgb.B);
            // Debug.WriteLine (index.ToString("D3") + " " + rgb.ToRgbDecString() );
            ColorEntry colorEntry = new(index.ToString("D3"), r, g, b);
            colorGroup.Colors.Add(colorEntry);

            ++ index;
        }

        document.Groups.Add(colorGroup);
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
        string serializedJson = JsonSerializer.Serialize(this.Swatches);
        if (!string.IsNullOrWhiteSpace(serializedJson))
        {
            return serializedJson;
        }

        throw new Exception("Failed to serialize swatches");
    }

    public Parameters ToTemplateParameters()
    {
        List<string> colors = new (this.Swatches.Count);
        var sortedSwatches =
            from swatch in this.Swatches orderby swatch.Usage descending select swatch;
        foreach (var swatch in sortedSwatches)
        {
            var rgb = swatch.HsvColor.ToRgb();
            colors.Add(rgb.ToPoundArgbHexString());
        }

        return
        [
            new Parameter("ImagePaletteSource", this.Name),
            new Parameter("Colors", colors, ParameterKind.Collection) 
        ];
    }
}
