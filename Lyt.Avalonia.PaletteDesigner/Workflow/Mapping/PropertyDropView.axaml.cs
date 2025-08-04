namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using Lyt.Avalonia.Controls;

public partial class PropertyDropView : View
{
    private static PropertyDropView? lastShownDropTarget;

    public static void HideDropTarget(IDropTarget? dropTarget = null)
    {
        if (dropTarget is PropertyDropViewModel viewModel)
        {
            if (viewModel.View is PropertyDropView propertyDropView)
            {
                propertyDropView.border.BorderBrush = new SolidColorBrush(Colors.Transparent);
            }
        }
    }

    public static bool ShowDropTarget(IDropTarget? dropTarget, Point position)
    {
        if (dropTarget is PropertyDropViewModel viewModel)
        {
            if (viewModel.View is PropertyDropView propertyDropView)
            {
                if (lastShownDropTarget is not null)
                {
                    lastShownDropTarget.border.BorderBrush = new SolidColorBrush(Colors.Transparent);
                }

                propertyDropView.border.BorderBrush = new SolidColorBrush(Colors.Aquamarine);
                lastShownDropTarget = propertyDropView;

                return true;
            }
        }

        return false;
    }

    public PropertyDropView() : base()
    {
        this.PointerEntered += this.OnPointerEntered;
        this.PointerExited += this.OnPointerExited;
        this.PointerMoved += this.OnPointerMoved;
        this.OpacitySlider.Opacity = 0.01;
        this.OpacitySlider.IsHitTestVisible = true;
    }

    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        base.OnDataContextChanged(sender, e);

        if (this.DataContext is PropertyDropViewModel)
        {
            new DragOverAble(PropertyDropView.HideDropTarget, PropertyDropView.ShowDropTarget).Attach(this);
            new DropAble(PropertyDropView.HideDropTarget).Attach(this);
        }
    }

    private void OnPointerEntered(object? sender, PointerEventArgs pointerEventArgs)
    {
        if (sender is not PropertyDropView)
        {
            return;
        }

        // Debug.WriteLine("Entered");
        this.OpacitySlider.Opacity = 1.0;
    }

    private void OnPointerMoved(object? sender, PointerEventArgs pointerEventArgs)
    {
        if (sender is not PropertyDropView)
        {
            return;
        }

        // Debug.WriteLine("Moved");
        if (this.OpacitySlider.IsPointerInside(pointerEventArgs, inflate: false))
        {
            // Debug.WriteLine("Inside slider");
            return;
        }

        if (!this.IsPointerInside(pointerEventArgs, inflate: false))
        {
            this.OpacitySlider.Opacity = 0.01;
            this.border.BorderBrush = new SolidColorBrush(Colors.Transparent);
        }
    }

    private void OnPointerExited(object? sender, PointerEventArgs pointerEventArgs)
    {
        if (sender is not PropertyDropView)
        {
            return;
        }

        // Debug.WriteLine("Exited");
        if (this.OpacitySlider.IsPointerInside(pointerEventArgs, inflate: false))
        {
            // Debug.WriteLine("Inside slider");
            return;
        }

        this.OpacitySlider.Opacity = 0.01;
        this.border.BorderBrush = new SolidColorBrush(Colors.Transparent);
    }
}
