namespace Lyt.Quantics.Studio.Behaviors.DragDrop;

using global::Avalonia.Input;

/// <summary> Behaviour for objects that are dragged around. </summary>
public sealed class DragAble : BehaviorBase<View>
{
    /// <summary> Delay triggering the Long Press event on the view model.</summary>
    private const int LongPressDelay = 777; // milliseconds

    /// <summary> Minimal drag distance triggering the drag abd drop operation.</summary>
    private const double MinimalDragDistance = 4.5; // pixels

    private bool isPointerPressed;
    private bool isDragging;
    private PointerPoint pointerPressedPoint;
    private UserControl? ghostView;
    private Canvas? dragCanvas;
    private IDragAbleViewModel? draggableBindable;
    private DispatcherTimer? timer;

    public DragAble(Canvas? canvas) => this.dragCanvas = canvas;

    protected override void OnAttached()
    {
        _ = this.Guard();
        this.HookPointerEvents();
    }

    protected override void OnDetaching() => this.UnhookPointerEvents();

    public View View => this.GuardAssociatedObject();

    public IDragAbleViewModel DraggableBindable
    {
        get => this.draggableBindable is not null ?
            this.draggableBindable :
            throw new InvalidOperationException("Not attached or invalid asociated object.");
        private set => this.draggableBindable = value;
    }

    private View Guard()
    {
        var view = base.GuardAssociatedObject();
        if ((view.DataContext is null) ||
            (view.DataContext is not IDragAbleViewModel iDraggableBindable))
        {
            throw new InvalidOperationException("Not attached or invalid asociated object.");
        }

        this.DraggableBindable = iDraggableBindable;
        return view;
    }

    private void HookPointerEvents()
    {
        View view = this.View;
        view.PointerPressed += this.OnPointerPressed;
        view.PointerReleased += this.OnPointerReleased;
        view.PointerMoved += this.OnPointerMoved;
        view.PointerEntered += this.OnPointerEntered;
        view.PointerExited += this.OnPointerExited;
    }

    private void UnhookPointerEvents()
    {
        View view = this.View;
        view.PointerPressed -= this.OnPointerPressed;
        view.PointerReleased -= this.OnPointerReleased;
        view.PointerMoved -= this.OnPointerMoved;
        view.PointerEntered -= this.OnPointerEntered;
        view.PointerExited -= this.OnPointerExited;
    }

    private void OnPointerEntered(object? sender, PointerEventArgs pointerEventArgs)
        => this.DraggableBindable.OnEntered();

    private void OnPointerExited(object? sender, PointerEventArgs pointerEventArgs)
        => this.DraggableBindable.OnExited();

    private void OnPointerPressed(object? sender, PointerPressedEventArgs pointerPressedEventArgs)
    {
        // Debug.WriteLine("Pressed");
        View view = this.View;
        this.isPointerPressed = true;
        this.pointerPressedPoint = pointerPressedEventArgs.GetCurrentPoint(view);
        this.StartTimer();
    }

    private void OnPointerMoved(object? sender, PointerEventArgs pointerEventArgs)
    {
        if (!this.isPointerPressed)
        {
            this.isPointerPressed = false;
            return;
        }

        if (this.isDragging)
        {
            this.StopTimer();

            // Debug.WriteLine("Dragging...");
            this.AdjustGhostPosition(pointerEventArgs);
            return;
        }
        else
        {
            // Debug.WriteLine("Moving...");
            View view = this.View;
            Point currentPosition = pointerEventArgs.GetPosition(view);
            var distance = Point.Distance(currentPosition, pointerPressedPoint.Position);
            if (distance <= MinimalDragDistance)
            {
                // Debug.WriteLine("Too close.");
                return;
            }

            // Drag and drop begins, it's not going to be a long press, therfore stop the timer.
            this.StopTimer();
            this.BeginDrag(pointerEventArgs);
        }
    }

    private void OnPointerReleased(object? sender, PointerReleasedEventArgs args)
    {
        this.StopTimer();
        // Debug.WriteLine("Released");
        if (this.isDragging || !this.isPointerPressed)
        {
            return;
        }

        this.isPointerPressed = false;

        // It's a Click 
        // The view model will decide whether or not the object is editable
        // For gates: Check if toolbox, parameters, etc...
        bool isRightClick = args.InitialPressMouseButton == MouseButton.Right;
        this.DraggableBindable.OnClicked(isRightClick);
    }

    private void BeginDrag(PointerEventArgs pointerEventArgs)
    {
        // Debug.WriteLine("Try Begin Drag");
        if (this.isDragging)
        {
            return;
        }

        _ = this.GuardAssociatedObject();
        bool allowDrag = this.DraggableBindable.OnBeginDrag();
        if (!allowDrag)
        {
            // Debug.WriteLine("Dragging rejected");
            return;
        }

        // Debug.WriteLine("Drag == true ");
        this.isDragging = true;

        // Create the ghost view  
        this.ghostView = this.DraggableBindable.CreateGhostView();
        if (this.ghostView is null)
        {
            // Debug.WriteLine("Failed to create ghost view");
            return;
        }

        if (!this.ValidateGhost(out Canvas? canvas))
        {
            // Debug.WriteLine("No canvas");
            return;
        }

        canvas!.Children.Add(this.ghostView);
        this.AdjustGhostPosition(pointerEventArgs.GetPosition(canvas));
        Debug.WriteLine("ghost view created");

        // Launch the DragDrop task, fire and forget 
        this.DoDragDrop(pointerEventArgs, canvas!);
    }

    private async void DoDragDrop(PointerEventArgs pointerEventArgs, Canvas canvas)
    {
        if (this.ghostView is null)
        {
            // Debug.WriteLine("Failed to create ghost view");
            return;
        }

        this.UnhookPointerEvents();
        var dragData = new DataObject();
        string dragAndDropFormat = this.DraggableBindable.DragDropFormat;
        dragData.Set(dragAndDropFormat, this.DraggableBindable);

        // Debug.WriteLine("Sarting DnD thread");
        var result = await DragDrop.DoDragDrop(pointerEventArgs, dragData, DragDropEffects.Move);
        // Debug.WriteLine($"DragAndDrop result: {result}");

        canvas.Children.Remove(this.ghostView);
        this.ghostView.DataContext = null;

        // Debug.WriteLine("Nullifying ghost view");
        this.ghostView = null;

        // Debug.WriteLine("Drag == false");
        this.isPointerPressed = false;
        this.isDragging = false;
        this.HookPointerEvents();
    }

    private void AdjustGhostPosition(Point position)
    {
        // Debug.WriteLine("AdjustGhostPosition from point");
        if (!this.ValidateGhost(out Canvas? _))
        {
            return;
        }

        Point newPosition = new(position.X + 4, position.Y + 4);
        this.ghostView!.SetValue(Canvas.LeftProperty, newPosition.X);
        this.ghostView.SetValue(Canvas.TopProperty, newPosition.Y);

        // Debug.WriteLine("GhostView Position: " + position.ToString());
        // Debug.WriteLine("GhostView Bounds: " + this.ghostView.Bounds.ToString());
        //var bounds = this.ghostView.Bounds;
        //if ((bounds.Width < 0.001) || (bounds.Height < 0.001))
        //{
        //    // Debug.WriteLine("Failed to create ghost view");
        //    // Debugger.Break();
        //    //return;
        //}
    }

    private void AdjustGhostPosition(PointerEventArgs pointerEventArgs)
    {
        // Debug.WriteLine("AdjustGhostPosition from pointer");
        if (!this.ValidateGhost(out Canvas? canvas))
        {
            return;
        }

        Point position = pointerEventArgs.GetPosition(canvas!);
        this.AdjustGhostPosition(position); 
    }

    private bool ValidateGhost(out Canvas? canvas)
    {
        canvas = null;
        if (this.ghostView is null)
        {
            // Debug.WriteLine("No ghost");
            return false;
        }

        if (this.dragCanvas is null)
        {
            // No canvas provided: try to find one 
            if (App.MainWindow is not MainWindow mainWindow)
            {
                // Debug.WriteLine("No main window");
                return false;
            }

            canvas = mainWindow.FindChildControl<Canvas>();
            if (canvas is null)
            {
                // Debug.WriteLine("No canvas found in main window");
                return false;
            }
        } 
        else
        {
            canvas = this.dragCanvas; 
        }

        return true;
    }

    public void OnParentDragOver(DragEventArgs dragEventArgs)
    {
        // Debug.WriteLine("On Parent Drag Over ");
        if (!this.ValidateGhost(out Canvas? canvas))
        {
            return;
        }

        Point position = dragEventArgs.GetPosition(canvas!);
        this.AdjustGhostPosition(position);
    }

    #region Timer

    private void StartTimer()
    {
        this.StopTimer();
        this.timer = new DispatcherTimer()
        {
            Interval = TimeSpan.FromMilliseconds(LongPressDelay),
            IsEnabled = true,
        };
        this.timer.Tick += this.OnTimerTick;
    }

    private void StopTimer()
    {
        if (this.timer is not null)
        {
            this.timer.IsEnabled = false;
            this.timer.Stop();
            this.timer.Tick -= this.OnTimerTick;
            this.timer = null;
        }
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        this.StopTimer();
        if ((this.draggableBindable is not null) &&
            (this.isPointerPressed) &&
            (!this.isDragging))
        {
            this.draggableBindable.OnLongPress();
        }
    }

    #endregion Timer
}