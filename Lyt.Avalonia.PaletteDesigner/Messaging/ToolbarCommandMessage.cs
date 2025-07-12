namespace Lyt.Avalonia.PaletteDesigner.Messaging;

public sealed record class ToolbarCommandMessage(
    ToolbarCommandMessage.ToolbarCommand Command, object? CommandParameter = null)
{
    public enum ToolbarCommand
    {
        // Left - Main toolbar in Shell view 
        RunProject,

        // Right - Main toolbar in Shell view  
        Close,

        // CreateNew toolbar
        CreateNewAddAllLanguages,
        CreateNewClearAllLanguages,
        CreateNewSaveProject,

        // Projects (Tile) 
        DeleteProject,

        // RunProject toolbar
        StopProject,
        StartProject,

        // Etc... Settings toolbars 
    }
}
