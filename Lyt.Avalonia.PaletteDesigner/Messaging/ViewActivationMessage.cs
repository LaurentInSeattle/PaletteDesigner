namespace Lyt.Avalonia.PaletteDesigner.Messaging;

public sealed record class ViewActivationMessage(
    ViewActivationMessage.ActivatedView View, object? ActivationParameter = null)
{
    public enum ActivatedView
    {
        Language,
        Settings, 

        Intro,
        Interactive,
        CreateNew,
        Projects,
        RunProject,

        GoBack,
        Exit,
    }
}
