namespace Lyt.Avalonia.PaletteDesigner.Controls;

public partial class ContainerControl : UserControl
{
    public ContainerControl() =>  this.InitializeComponent();

    public static readonly StyledProperty<object?> ContainerControlContentProperty = 
        AvaloniaProperty.Register<ContainerControl, object?>(nameof(ContainerControlContent), null);

    public object? ContainerControlContent
    {
        get => this.GetValue(ContainerControlContentProperty);
        set
        {
            this.SetValue(ContainerControlContentProperty, value);
            this.presenter.Content = value; 
        } 
    }

    /// <summary> Title Styled Property </summary>
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<ContainerControl, string>(
            nameof(Title),
            defaultValue: string.Empty,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceTitle,
            enableDataValidation: false);

    /// <summary> Gets or sets the Title property.</summary>
    public string Title
    {
        get => this.GetValue(TitleProperty);
        set
        {
            this.SetValue(TitleProperty, value);
            this.titleTextBlock.Text = value;
        }
    }

    /// <summary> Coerces the Title value. </summary>
    private static string CoerceTitle(AvaloniaObject sender, string newText)
    {
        if (sender is ContainerControl containerControl)
        {
            containerControl.titleTextBlock.Text = newText;
        }

        return newText;
    }

    /// <summary> Text Styled Property </summary>
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<ContainerControl, string>(
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
            this.textTextBlock.Text = value;
        }
    }

    /// <summary> Coerces the Text value. </summary>
    private static string CoerceText(AvaloniaObject sender, string newText)
    {
        if (sender is ContainerControl containerControl)
        {
            containerControl.textTextBlock.Text = newText;
        }

        return newText;
    }

    /// <summary> Foreground Styled Property </summary>
    public static new readonly StyledProperty<IBrush> ForegroundProperty =
        AvaloniaProperty.Register<ContainerControl, IBrush>(
            nameof(Foreground),
            defaultValue: Brushes.Aquamarine,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceForeground,
            enableDataValidation: false);

    /// <summary> Gets or sets the Foreground property.</summary>
    public new IBrush Foreground
    {
        get => this.GetValue(ForegroundProperty);

        set
        {
            this.SetValue(ForegroundProperty, value);
            this.rectangle.Fill = value;
        }
    }

    /// <summary> Coerces the Foreground value. </summary>
    private static IBrush CoerceForeground(AvaloniaObject sender, IBrush newForeground)
    {
        if (sender is ContainerControl containerControl)
        {
            containerControl.rectangle.Fill = newForeground;
        }

        return newForeground;
    }

}