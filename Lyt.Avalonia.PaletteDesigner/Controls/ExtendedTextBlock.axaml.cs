namespace Lyt.Avalonia.PaletteDesigner.Controls;

using global::Avalonia.Styling;

public partial class ExtendedTextBlock : UserControl
{
    public ExtendedTextBlock() => this.InitializeComponent();

    /// <summary> Text Styled Property </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ExtendedTextBlock, string>(
            nameof(Text),
            defaultValue: string.Empty,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceText,
            enableDataValidation: false);

    /// <summary> Gets or sets the Text property.</summary>
    public string Text
    {
        get => this.GetValue(TextProperty);
        set
        {
            this.SetValue(TextProperty, value);
            this.textBlock.Text = value;
        }
    }

    /// <summary> Coerces the Text value. </summary>
    private static string CoerceText(AvaloniaObject sender, string newText)
    {
        if (sender is ExtendedTextBlock etb)
        {
            etb.textBlock.Text = newText;
        }

        return newText;
    }

    /// <summary> Typography Styled Property </summary>
    public static readonly StyledProperty<ControlTheme> TypographyProperty =
        AvaloniaProperty.Register<ExtendedTextBlock, ControlTheme>(
            nameof(Typography),
            defaultValue: new ControlTheme(),
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: null,
            enableDataValidation: false);

    /// <summary> Gets or sets the Typography property.</summary>
    public ControlTheme Typography
    {
        get => this.GetValue(TypographyProperty);
        set
        {
            this.SetValue(TypographyProperty, value);
            this.ChangeTypography(value);
        }
    }

    private void ChangeTypography(ControlTheme typography)
    {
        this.textBlock.ApplyControlTheme(typography);
        this.textBlock.Text = this.Text;
    }

}