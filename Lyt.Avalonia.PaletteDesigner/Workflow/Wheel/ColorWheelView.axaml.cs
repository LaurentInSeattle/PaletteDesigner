namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wheel;

public partial class ColorWheelView : View
{
    public ColorWheelView() :base () 
    {
        this.DoubleTapped += this.OnDoubleTapped;
    }

    private void OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (this.DataContext is ColorWheelViewModel colorWheelViewModel)
        {
            colorWheelViewModel.OnResetShadesClick();
        }
    }
}
