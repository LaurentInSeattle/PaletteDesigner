namespace Lyt.Avalonia.PaletteDesigner.Workflow.Mapping;

using Lyt.Quantics.Studio.Behaviors.DragDrop;

public partial class PropertyDropView : View
{
    private static PropertyDropView? lastShownDropTarget; 

    protected override void OnDataContextChanged(object? sender, EventArgs e)
    {
        base.OnDataContextChanged(sender, e);

        if (this.DataContext is PropertyDropViewModel)
        {
            new DragOverAble(PropertyDropView.HideDropTarget, PropertyDropView.ShowDropTarget).Attach(this);
            new DropAble(PropertyDropView.HideDropTarget).Attach(this);
        }
    }

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
            if ( viewModel.View is PropertyDropView propertyDropView)
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
}
