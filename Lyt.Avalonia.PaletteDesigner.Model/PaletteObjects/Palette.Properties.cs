namespace Lyt.Avalonia.PaletteDesigner.Model.PaletteObjects;

public sealed partial class Palette
{
#pragma warning disable CS8618 
    // Non-nullable field must contain a non-null value when exiting constructor.
#pragma warning disable CA2211 
//    // Non-constant fields should not be visible

    public static PaletteDesignerModel Model;

    public static Dictionary<int, RgbColor> ColorWheel;

    public static Dictionary<int, double> HueWheel;

    public static ShadeMap ShadeMap;

    public static void Setup(
        PaletteDesignerModel model, 
        Dictionary<int, RgbColor> colorWheel, 
        Dictionary<int, double> hueWheel, 
        ShadeMap shadeMap)
    {
        Palette.Model = model;
        Palette.ColorWheel = colorWheel;
        Palette.HueWheel = hueWheel;
        Palette.ShadeMap = shadeMap;
    }

#pragma warning restore CA2211 
#pragma warning restore CS8618 

    [JsonRequired]
    public string Name { get; set; } = string.Empty;

    [JsonRequired]
    public PaletteKind Kind { get; set; } = PaletteKind.MonochromaticComplementary;

    // for both Triad and Square, otherwise ignored 
    // Degrees on the wheel 
    [JsonRequired]
    public double SecondaryWheelDistance { get; set; } = 27.0;

    [JsonRequired]
    public bool AreShadesLocked { get; set; } = true;

    // If shades are unlocked, the shades user wants to edit  
    [JsonRequired]
    public WheelKind SelectedWheel { get; set; } = WheelKind.Primary;

    [JsonRequired]
    public Shades Primary { get; set; } = new();

    [JsonRequired]
    public Shades Secondary1 { get; set; } = new();

    [JsonRequired]
    public Shades Secondary2 { get; set; } = new();

    [JsonRequired]
    public Shades Complementary { get; set; } = new();

    // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Palette() { /* needed for serialization */ }

    public Palette DeepClone()
    =>  new()
        {
            Name = new string (this.Name),
            Kind = this.Kind,
            SecondaryWheelDistance = this.SecondaryWheelDistance,
            AreShadesLocked = this.AreShadesLocked,
            SelectedWheel = this.SelectedWheel,
            Primary = this.Primary.DeepClone(),
            Secondary1 = this.Secondary1.DeepClone(),
            Secondary2 = this.Secondary2.DeepClone(),
            Complementary = this.Complementary.DeepClone(),
        };

    public void ForAllShades(Action<WheelKind, Shades> doThat)
    {
        foreach (WheelKind wheelKind in Enum.GetValues<WheelKind>())
        {
            if (wheelKind == WheelKind.Unknown)
            {
                continue;
            }

            Shades shades = wheelKind.ToShadesFrom(this);
            doThat(wheelKind, shades);
        }
    }

    public Parameters ToTemplateParameters()
    {
        var parameters = new Parameters();
        var paletteKind = new Parameter("PaletteKind", this.Kind.ToString().BeautifyEnumString());
        parameters.Add(paletteKind);
        foreach (WheelKind wheelKindEnum in Enum.GetValues<WheelKind>())
        {
            if( wheelKindEnum == WheelKind.Unknown )
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
