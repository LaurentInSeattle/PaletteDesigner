namespace Lyt.Avalonia.PaletteDesigner.Controls;

public partial class MarkerControl : UserControl
{
    private Canvas? parentCanvas;
    private bool isDragging;
    private bool canMove;

    public MarkerControl()
    {
        this.InitializeComponent();
        this.PointerPressed += this.OnPointerPressed;
        this.PointerMoved += this.OnPointerMoved;
        this.PointerReleased += this.OnPointerReleased;
        this.Loaded += this.OnLoaded;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        var canvas = MiscUtilities.FindParentControl<Canvas>(this);
        if (canvas == null)
        {
            throw new ArgumentNullException("No canvas");
        }

        this.parentCanvas = canvas;

        //this.ellipse.Fill = new SolidColorBrush(Colors.White);
        //this.ellipse.Stroke = new SolidColorBrush(Colors.White);
        //this.ellipse.StrokeThickness = 0;
    }

    ~MarkerControl()
    {
        this.PointerPressed -= this.OnPointerPressed;
        this.PointerMoved -= this.OnPointerMoved;
        this.PointerReleased -= this.OnPointerReleased;
    }

    private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not MarkerControl)
        {
            Debug.WriteLine("Not sent by a MarkerControl");
            return;
        }

        if (!this.canMove)
        {
            return; 
        }

        this.isDragging = true; 
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (sender is not MarkerControl)
        {
            Debug.WriteLine("Not sent by a MarkerControl");
            return;
        }

        if (!this.canMove)
        {
            return;
        }

        this.isDragging = false;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!this.canMove)
        {
            return;
        }

        if (!this.isDragging)
        {
            return; 
        }

        if ( this.parentCanvas is null )
        {
            Debug.WriteLine("No canvas");
            return;
        }

        if (sender is not MarkerControl markerControl)
        {
            Debug.WriteLine("Not sent by a MarkerControl");
            return;
        }

        var mousePosition = e.GetPosition(this.parentCanvas);
        int pixelX = (int)mousePosition.X;
        int pixelY = (int)mousePosition.Y;

        // Recenter and move the maker 
        Rect bounds = this.parentCanvas.Bounds;
        double x = pixelX - bounds.Width / 2;
        double y = bounds.Height / 2 - pixelY;
        double angleRadians = Math.Atan2(y, x);
        double angleDegrees = (360.0 * angleRadians / Math.Tau).NormalizeAngleDegrees();
        this.Move(angleDegrees);

        if (this.parentCanvas.DataContext is ColorWheelViewModel colorWheelViewModel)
        {
            colorWheelViewModel.OnAngleChanged(angleDegrees);
        }
    }

    public void Move(double angleDegrees)
    {
        if (this.parentCanvas is null)
        {
            Debug.WriteLine("No canvas");
            return;
        }

        // Keep the marker at a fixed distance from the center 
        double angleRadians = angleDegrees * Math.Tau / 360.0; 
        double radius = 260;
        double x = radius * Math.Cos(angleRadians);
        double y = radius * Math.Sin(angleRadians);

        // Translate to top / left 
        Rect bounds = this.parentCanvas.Bounds;
        int pixelX = (int)(x + bounds.Width / 2);
        int pixelY = (int)(bounds.Height / 2 - y);

        // Move the marker 
        this.SetValue(Canvas.LeftProperty, pixelX);
        this.SetValue(Canvas.TopProperty, pixelY);
    }

    /// <summary> CanMove Styled Property </summary>
    public static readonly StyledProperty<bool> CanMoveProperty =
        AvaloniaProperty.Register<MarkerControl, bool>(
            nameof(CanMove),
            defaultValue: false,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceCanMove,
            enableDataValidation: false);

    /// <summary> Gets or sets the CanMove property.</summary>
    public bool CanMove
    {
        get => this.GetValue(CanMoveProperty);
        set => this.SetValue(CanMoveProperty, value);
    }

    private static bool CoerceCanMove(AvaloniaObject sender, bool newCanMove)
    {
        if (sender is MarkerControl markerControl)
        {
            markerControl.canMove = newCanMove;
        }

        return newCanMove;
    }

    /*
    /// <summary> BackgroundBorderThickness Styled Property </summary>
    public static readonly StyledProperty<double> BackgroundBorderThicknessProperty =
        AvaloniaProperty.Register<GlyphButton, double>(
            nameof(BackgroundBorderThickness),
            defaultValue: 1.0,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceBackgroundBorderThickness,
            enableDataValidation: false);


    /// <summary> Gets or sets the BackgroundBorderThickness property.</summary>
    public double BackgroundBorderThickness
    {
        get => this.GetValue(BackgroundBorderThicknessProperty);
        set
        {
            this.SetValue(BackgroundBorderThicknessProperty, value);
            this.border.BorderThickness = new Thickness(value);
            this.rectangleBackground.StrokeThickness = value;
        }
    }

    /// <summary> Coerces the BackgroundBorderThickness value. </summary>
    private static double CoerceBackgroundBorderThickness(AvaloniaObject sender, double newBackgroundBorderThickness) => newBackgroundBorderThickness;
     */
}