namespace Lyt.Avalonia.PaletteDesigner.Workflow.Wizard;

using HsvColor = Lyt.ImageProcessing.ColorObjects.HsvColor;

public sealed partial class WizardSwatchViewModel : 
    ViewModel <WizardSwatchView>,
    IDragAbleViewModel,
    IRecipient<LanguageChangedMessage>,
    IRecipient<ModelWizardUpdatedMessage>
{
    public const string CustomDragAndDropFormat = "WizardSwatchViewModel";

    private readonly PaletteDesignerModel paletteDesignerModel;

    [ObservableProperty]
    public SolidColorBrush colorBrush;

    [ObservableProperty]
    private string rgbHex = string.Empty;

    [ObservableProperty]
    private string rgbDec = string.Empty;

    [ObservableProperty]
    private string hsv = string.Empty;

    public WizardSwatchViewModel(
        PaletteDesignerModel paletteDesignerModel, 
        bool isGhost, 
        Canvas dragCanvas,
        SwatchKind swatchKind , int index)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.IsGhost = isGhost;
        this.SwatchIndex = new (swatchKind, index);
        this.ColorBrush = new SolidColorBrush(Colors.Transparent);

        if (!this.IsGhost)
        {
            this.DragAble = new DragAble(dragCanvas);
        }

        this.Localize();
        this.Subscribe<LanguageChangedMessage>();
        this.Subscribe<ModelWizardUpdatedMessage>();
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();

        if (!this.IsGhost && this.DragAble is not null && !this.DragAble.IsAttached)
        {
            this.DragAble.Attach(this.View);
        }
    }

    /// <summary> True when this is a ghost view model. </summary>
    public bool IsGhost { get; private set; }

    public SwatchIndex SwatchIndex { get; private set; }

    public int Index { get; private set; }

    public void Receive(LanguageChangedMessage message) => this.Localize();

    public void Receive(ModelWizardUpdatedMessage message)
    {
        HsvColor hsvColor = this.paletteDesignerModel.ActiveProject!.WizardPalette.GetColor(this.SwatchIndex); 
        this.ColorBrush = hsvColor.ToBrush();
        //this.RgbHex = string.Format("# {0}", rgbColor.ToRgbHexString());
        //this.RgbDec = string.Format("\u2022 {0}", rgbColor.ToRgbDecString());
    }

    private void Localize() 
    {
    }

    #region IDraggableBindable Implementation 

    public DragAble? DragAble { get; private set; }

    public string DragDropFormat => WizardSwatchViewModel.CustomDragAndDropFormat;

    public void OnEntered() { }

    public void OnExited() { }

    public void OnLongPress() { }

    public void OnClicked(bool isRightClick) { } 

    public bool OnBeginDrag() => true;

    public View CreateGhostView()
    {
        if ( this.DragAble is null )
        {
            throw new InvalidOperationException("DragAble is null.");
        }

        var ghostView = new WizardSwatchView()
        {
            Width = 100.0,
            Height = 100.0,
        };
        var ghostViewModel = new WizardSwatchViewModel(
            this.paletteDesignerModel,
            isGhost: true,
            this.DragAble.DragCanvas,
            this.SwatchIndex.Kind, this.SwatchIndex.Index)
        {
            ColorBrush = this.ColorBrush,
        };

        ghostView.DataContext = ghostViewModel;
        return ghostView;
    }

    #endregion IDraggableBindable Implementation 

}
