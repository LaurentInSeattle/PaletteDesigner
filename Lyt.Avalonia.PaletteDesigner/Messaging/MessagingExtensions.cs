namespace Lyt.Avalonia.PaletteDesigner.Messaging;

using static ViewActivationMessage;
using static ToolbarCommandMessage;

public static class ApplicationMessagingExtensions
{
    private static readonly IDialogService dialogService;

    static ApplicationMessagingExtensions()
        => ApplicationMessagingExtensions.dialogService = App.GetRequiredService<IDialogService>();

    public static void ActivateView(
        ViewActivationMessage.ActivatedView view, object? activationParameter = null)
        => new ViewActivationMessage(view, activationParameter).Publish();

    public static void Command(ToolbarCommand command, object? parameter = null)
    {
        // All toolbar messaging is disabled, visual state of the toolbar is handled with
        // another type of message, provided by the Modal Dialog management in the framework
        if (ApplicationMessagingExtensions.dialogService.IsModal)
        {
            return;
        }

        new ToolbarCommandMessage(command, parameter).Publish();
    }

    public static void NavigateTo(ActivatedView view)
    {
        bool programmaticNavigation = true;
        new ViewActivationMessage(view, programmaticNavigation).Publish();
    }

    public static void Select(ActivatedView activatedView, object? parameter = null)
        => ViewSelector<ActivatedView>.Select(activatedView, parameter);
}
