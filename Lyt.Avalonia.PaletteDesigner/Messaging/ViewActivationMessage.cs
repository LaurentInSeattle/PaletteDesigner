namespace Lyt.Avalonia.PaletteDesigner.Messaging;

public sealed record class ViewActivationMessage(
    ViewActivationMessage.ActivatedView View, object? ActivationParameter = null)
{
    public enum ActivatedView
    {
        Design,
        Imaging,
        Mapping,
        Language,
        Settings, 

        Intro,
        Interactive,
        CreateNew,
        Projects,
        RunProject,

        GoBack,
        Exit,
        Wizard,
    }
}
