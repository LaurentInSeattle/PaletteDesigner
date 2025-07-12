namespace Lyt.Avalonia.PaletteDesigner.Messaging;

using static ViewActivationMessage;
using static ToolbarCommandMessage;

public static class MessagingExtensions
{
    private static readonly IMessenger messenger;
    private static readonly IDialogService dialogService;

    static MessagingExtensions()
    {
        MessagingExtensions.messenger = App.GetRequiredService<IMessenger>();
        MessagingExtensions.dialogService = App.GetRequiredService<IDialogService>();
    }

    public static void Publish<T> ( T message ) where T : class
        => MessagingExtensions.messenger.Publish(message);

    public static void ActivateView(
        ViewActivationMessage.ActivatedView view, object? activationParameter = null)
        => MessagingExtensions.messenger.Publish(
            new ViewActivationMessage(view, activationParameter));

    public static void Command(ToolbarCommand command, object? parameter = null)
    {
        // All toolbar messaging is disabled, visual state of the toolbar is handled with
        // another type of message, provided by the Modal Dialog management in the framework
        if (MessagingExtensions.dialogService.IsModal)
        {
            return;
        }

        MessagingExtensions.messenger.Publish(new ToolbarCommandMessage(command, parameter));
    }

    public static void NavigateTo(ActivatedView view)
    {
        bool programmaticNavigation = true;
        MessagingExtensions.messenger.Publish(new ViewActivationMessage(view, programmaticNavigation));
    }

    public static void Select(ActivatedView activatedView, object? parameter = null)
        => ViewSelector<ActivatedView>.Select(
            MessagingExtensions.messenger, activatedView, parameter);
}
