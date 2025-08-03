namespace Lyt.Quantics.Studio.Behaviors.DragDrop;

using global::Avalonia.Input;

/// <summary> 
/// Behaviour for controls and views that should support visualising a potential drop location 
/// and actual dropping of the 'DragAble' objects that are dragged around. 
/// </summary>
public class DropAble(Action hideDropTarget) : BehaviorBase<BehaviorEnabledUserControl>
{
    private readonly Action hideDropTarget = hideDropTarget;

    protected override void OnAttached()
    {
        BehaviorEnabledUserControl userControl = GuardAssociatedObject();
        DragDrop.SetAllowDrop(userControl, true);
        userControl.AddHandler(DragDrop.DropEvent, this.OnDrop);
    }

    protected override void OnDetaching()
    {
        if (this.AssociatedObject is UserControl userControl)
        {
            DragDrop.SetAllowDrop(userControl, false);
            userControl.RemoveHandler(DragDrop.DropEvent, this.OnDrop);
        }
    }

    private void OnDrop(object? sender, DragEventArgs dragEventArgs)
    {
        if (this.AssociatedObject is not UserControl userControl)
        {
            return;
        }

        var data = dragEventArgs.Data;
        var formats = data.GetDataFormats().ToList();
        if (formats is not null && formats.Count > 0)
        {
            foreach (var format in formats)
            {
                object? dragDropObject = data.Get(format);
                if (dragDropObject is IDragAbleViewModel draggableBindable)
                {
                    var draggable = draggableBindable.DragAble;
                    if (draggable is null)
                    {
                        break;
                    }

                    if (userControl.DataContext is IDropTarget dropTarget)
                    {
                        dropTarget.OnDrop(dragEventArgs.GetPosition(userControl), dragDropObject);
                    }

                    break;
                }
            }
        }

        this.hideDropTarget?.Invoke();
        dragEventArgs.Handled = true;
    }
}
