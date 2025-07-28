namespace Lyt.Avalonia.PaletteDesigner.Workflow.Settings;

public sealed partial class SettingsViewModel : ViewModel<SettingsView>
{
    private readonly PaletteDesignerModel paletteDesignerModel;

    public SettingsViewModel(PaletteDesignerModel astroPicModel)
    {
        this.paletteDesignerModel = astroPicModel;
    }

    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.Populate();
    }

    public override void Activate(object? activationParameters)
    {
        base.Activate(activationParameters);
        this.Populate();
    }

    private void OnToolbarCommand(ToolbarCommandMessage message)
    {
        switch (message.Command)
        {
            // Ignore all other commands 
            default:
                break;
        }
    }

    private void Populate()
    {
        //With (this.isPopulating) 
        //{
        //}
    }

}
