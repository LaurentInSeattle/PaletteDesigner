namespace Lyt.Avalonia.PaletteDesigner.Workflow.Imaging;

using static Lyt.Avalonia.Controls.Utilities;

public partial class DropView : View
{
    private static readonly SolidColorBrush? normalBrush;
    private static readonly SolidColorBrush? hotBrush;

    static DropView()
    {
        TryFindResource<SolidColorBrush>("LightAqua_0_120", out SolidColorBrush? brush);
        if (brush is not null)
        {
            normalBrush = brush;
        }

        TryFindResource<SolidColorBrush>("OrangePeel_0_100", out brush);
        if (brush is not null)
        {
            hotBrush = brush;
        }
    }

    public DropView() : base ()
    {
        if (normalBrush is not null)
        {
            this.DropRectangle.Stroke = normalBrush;
        }

        DragDrop.SetAllowDrop(this.DropBorder, true);
        this.DropBorder.AddHandler(DragDrop.DropEvent, this.OnDrop);
        this.DropBorder.AddHandler(DragDrop.DragEnterEvent, this.OnDragEnter);
        this.DropBorder.AddHandler(DragDrop.DragLeaveEvent, this.OnDragLeave);
    }

    ~DropView()
    {
        DragDrop.SetAllowDrop(this.DropBorder, false);
        this.DropBorder.RemoveHandler(DragDrop.DropEvent, this.OnDrop);
        this.DropBorder.RemoveHandler(DragDrop.DragEnterEvent, this.OnDragEnter);
        this.DropBorder.RemoveHandler(DragDrop.DragLeaveEvent, this.OnDragLeave);
    }

    private void OnDragEnter(object? _, DragEventArgs e)
    {
        if (hotBrush is not null)
        {
            this.DropRectangle.Stroke = hotBrush;
        }
    }

    private void OnDragLeave(object? _, DragEventArgs e)
    {
        if (normalBrush is not null)
        {
            this.DropRectangle.Stroke = normalBrush;
        }
    }

    private void OnDrop(object? _, DragEventArgs dragEventArgs)
    {
        if (normalBrush is not null)
        {
            this.DropRectangle.Stroke = normalBrush;
        }

        IDataObject data = dragEventArgs.Data;
        var files = data.GetFiles();
        if (files is not null)
        {
            foreach (IStorageItem file in files)
            {
                string path = file.Path.LocalPath;
                Debug.WriteLine("Dropped: " + path);
                if (File.Exists(path))
                {
                    if (this.DataContext is DropViewModel dropViewModel)
                    {
                        if (dropViewModel.OnDrop(path))
                        {
                            break; 
                        } 
                    }
                }
            }
        }

        dragEventArgs.Handled = true;
    }
}