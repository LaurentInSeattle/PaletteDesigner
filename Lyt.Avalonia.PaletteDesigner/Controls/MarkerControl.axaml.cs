namespace Lyt.Avalonia.PaletteDesigner.Controls;

public partial class MarkerControl : UserControl
{
    private Canvas? parentCanvas;
    private IBrush? brush;
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
        if (canvas is null)
        {
            Debugger.Break();
            throw new ArgumentNullException("No canvas");
        }

        this.parentCanvas = canvas;
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

        this.brush = this.ellipse.Stroke; 
        this.ellipse.Stroke = new SolidColorBrush(Colors.DarkOrchid);
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

        this.ellipse.Stroke = this.brush;
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

    /// <summary> FillBrush Styled Property </summary>
    public static readonly StyledProperty<IBrush> FillBrushProperty =
        AvaloniaProperty.Register<MarkerControl, IBrush>(nameof(FillBrush), defaultValue: Brushes.Aquamarine);

    /// <summary> Gets or sets the FillBrush property.</summary>
    public IBrush FillBrush
    {
        get => this.GetValue(FillBrushProperty);
        set
        {
            this.SetValue(FillBrushProperty, value);
            this.ellipse.Fill = value;
        }
    }

    /// <summary> StrokeBrush Styled Property </summary>
    public static readonly StyledProperty<IBrush> StrokeBrushProperty =
        AvaloniaProperty.Register<MarkerControl, IBrush>(nameof(StrokeBrush), defaultValue: Brushes.Aquamarine);

    /// <summary> Gets or sets the StrokeBrush property.</summary>
    public IBrush StrokeBrush
    {
        get => this.GetValue(FillBrushProperty);
        set
        {
            this.SetValue(StrokeBrushProperty, value);
            this.ellipse.Stroke = value;
        }
    }

    /// <summary> StrokeBrushThickness Styled Property </summary>
    public static readonly StyledProperty<double> StrokeBrushThicknessProperty =
        AvaloniaProperty.Register<MarkerControl, double>(
            nameof(StrokeBrushThickness),
            defaultValue: 1.0,
            inherits: false,
            defaultBindingMode: BindingMode.OneWay,
            validate: null,
            coerce: CoerceStrokeBrushThickness,
            enableDataValidation: false);


    /// <summary> Gets or sets the StrokeBrushThickness property.</summary>
    public double StrokeBrushThickness
    {
        get => this.GetValue(StrokeBrushThicknessProperty);
        set
        {
            this.SetValue(StrokeBrushThicknessProperty, value);
            this.ellipse.StrokeThickness = value;
        }
    }

    /// <summary> Coerces the StrokeBrushThickness value. </summary>
    private static double CoerceStrokeBrushThickness(
        AvaloniaObject sender, double newBackgroundBorderThickness) => newBackgroundBorderThickness;     
}