namespace Lyt.Quantics.Studio.Behaviors.DragDrop;

using global::Avalonia.Input;

/// <summary> 
/// Behaviour for controls and views that should support visualising the 'ghost' view of 
/// 'DragAble' objects that are dragged around. 
/// </summary>
public class DragOverAble(
    Action? hideDropTarget = null,
    Func<IDropTarget, Point, bool>? showDropTarget = null)
        : BehaviorBase<View>
{
    private readonly Action? hideDropTarget = hideDropTarget;
    private readonly Func<IDropTarget, Point, bool>? showDropTarget = showDropTarget;

    protected override void OnAttached()
    {
        View view = base.GuardAssociatedObject();
        DragDrop.SetAllowDrop(view, true);
        view.AddHandler(DragDrop.DragOverEvent, this.OnDragOver);
    }

    protected override void OnDetaching()
    {
        if (this.AssociatedObject is View view)
        {
            DragDrop.SetAllowDrop(view, false);
            view.RemoveHandler(DragDrop.DragOverEvent, this.OnDragOver);
        }
    }

    private void OnDragOver(object? sender, DragEventArgs dragEventArgs)
    {
        dragEventArgs.DragEffects = DragDropEffects.None;
        if (this.AssociatedObject is not View view)
        {
            return;
        }

        bool showedDropTarget = false;
        var data = dragEventArgs.Data;
        var formats = data.GetDataFormats().ToList();
        if (formats is not null && formats.Count > 0)
        {
            foreach (string? format in formats)
            {
                if (string.IsNullOrWhiteSpace(format))
                {
                    continue;
                }

                object? dragDropObject = data.Get(format);
                if (dragDropObject is IDragAbleViewModel draggableBindable)
                {
                    var draggable = draggableBindable.DragAble;
                    if (draggable is null)
                    {
                        break;
                    }

                    draggable.OnParentDragOver(dragEventArgs);
                    if (view.DataContext is IDropTarget dropTarget)
                    {
                        Point position = dragEventArgs.GetPosition(view);
                        if (dropTarget.CanDrop(position, dragDropObject))
                        {
                            dragEventArgs.DragEffects = DragDropEffects.Move;
                            if (this.showDropTarget is not null)
                            {
                                showedDropTarget = this.showDropTarget.Invoke(dropTarget, position);
                            }
                        }
                    }

                    break;
                }
            }
        }

        if (!showedDropTarget)
        {
            this.hideDropTarget?.Invoke();
        }

        dragEventArgs.Handled = true;
    }
}
