namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using static Lyt.Avalonia.Controls.Utilities;

public partial class WizardThemeComponentView : View
{
    private static readonly SolidColorBrush normalBrush;
    private static readonly SolidColorBrush? hotBrush;
    private static readonly SolidColorBrush? darkBrush;

    static WizardThemeComponentView()
    {
        normalBrush = new SolidColorBrush(Colors.Transparent);

        TryFindResource<SolidColorBrush>("OrangePeel_0_100", out SolidColorBrush? brush);
        if (brush is not null)
        {
            hotBrush = brush;
        }

        TryFindResource<SolidColorBrush>("DeepBlue_0_080", out brush);
        if (brush is not null)
        {
            darkBrush = brush;
        }
    }

    public WizardThemeComponentView() : base()
    {
        this.ClearDropVisuals();
        DragDrop.SetAllowDrop(this.DropBorder, true);
        this.DropBorder.AddHandler(DragDrop.DragEnterEvent, this.OnDragEnter);
        this.DropBorder.AddHandler(DragDrop.DragLeaveEvent, this.OnDragLeave);
        this.DropBorder.AddHandler(DragDrop.DropEvent, this.OnDrop);
        new DragOverAble().Attach(this);
        new DropAble().Attach(this);
    }

    ~WizardThemeComponentView()
    {
        DragDrop.SetAllowDrop(this.DropBorder, false);
        this.DropBorder.RemoveHandler(DragDrop.DragEnterEvent, this.OnDragEnter);
        this.DropBorder.RemoveHandler(DragDrop.DragLeaveEvent, this.OnDragLeave);
        this.DropBorder.RemoveHandler(DragDrop.DropEvent, this.OnDrop);
    }

    private void OnDragEnter(object? _, DragEventArgs e) => this.HighlightDropVisuals();

    private void OnDragLeave(object? _, DragEventArgs e) => this.ClearDropVisuals();

    private void OnDrop(object? _, DragEventArgs dragEventArgs) => this.ClearDropVisuals();

    private void HighlightDropVisuals()
    {
        if (hotBrush is not null)
        {
            this.DropRectangle.Stroke = hotBrush;
        }

        if (darkBrush is not null)
        {
            this.InnerRectangle.Stroke = darkBrush;
        }
    }

    private void ClearDropVisuals()
    {
        if (normalBrush is not null)
        {
            this.DropRectangle.Stroke = normalBrush;
            this.InnerRectangle.Stroke = normalBrush;
        }
    }
}