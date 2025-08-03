namespace Lyt.Quantics.Studio.Behaviors.DragDrop;

/// <summary> Interface contract for view models that have a view that can be dragged </summary>
public interface IDragAbleViewModel
{
    void OnEntered();

    void OnExited();

    void OnLongPress();

    void OnClicked(bool isRightClick);

    bool OnBeginDrag();

    View CreateGhostView();

    string DragDropFormat { get; }

    DragAble? DragAble { get; }
}
