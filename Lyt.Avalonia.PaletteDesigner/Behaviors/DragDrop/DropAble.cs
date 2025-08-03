namespace Lyt.Quantics.Studio.Behaviors.DragDrop;

using global::Avalonia.Input;

/// <summary> 
/// Behaviour for controls and views that should support visualising a potential drop location 
/// and actual dropping of the 'DragAble' objects that are dragged around. 
/// </summary>
public class DropAble(Action<IDropTarget?> hideDropTarget) : BehaviorBase<View>
{
    private readonly Action<IDropTarget?> hideDropTarget = hideDropTarget;

    protected override void OnAttached()
    {
        View view = this.GuardAssociatedObject();
        Debug.WriteLine("DropAble | Attached to: " + this.AssociatedObject!.GetType().Name);
        DragDrop.SetAllowDrop(view, true);
        view.AddHandler(DragDrop.DropEvent, this.OnDrop);
    }

    protected override void OnDetaching()
    {
        if (this.AssociatedObject is View view)
        {
            DragDrop.SetAllowDrop(view, false);
            view.RemoveHandler(DragDrop.DropEvent, this.OnDrop);
        }
    }

    private void OnDrop(object? sender, DragEventArgs dragEventArgs)
    {
        if (this.AssociatedObject is not View view)
        {
            return;
        }

        IDropTarget? target = null; 
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
                if (dragDropObject is IDragAbleViewModel draggableViewModel)
                {
                    var draggable = draggableViewModel.DragAble;
                    if (draggable is null)
                    {
                        break;
                    }

                    if (view.DataContext is IDropTarget dropTarget)
                    {
                        target = dropTarget;
                        dropTarget.OnDrop(dragEventArgs.GetPosition(view), dragDropObject);
                    }

                    break;
                }
            }
        }

        this.hideDropTarget?.Invoke(target);
        dragEventArgs.Handled = true;
    }
}
