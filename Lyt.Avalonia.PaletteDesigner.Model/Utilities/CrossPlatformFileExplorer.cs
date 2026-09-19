namespace Lyt.Avalonia.PaletteDesigner.Model.Utilities;

using System.Runtime.InteropServices;
using System.IO;

public static class CrossPlatformFileExplorer
{
    public static void OpenInExplorer(this string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
        {
            throw new DirectoryNotFoundException($"The directory '{folderPath}' does not exist.");
        }

        // Standardize the path format for cross platform safety
        folderPath = Path.GetFullPath(folderPath);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows: Calls explorer.exe directly
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{folderPath}\"",
                UseShellExecute = false
            });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            // macOS: Uses the 'open' command line utility
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "open",
                Arguments = $"\"{folderPath}\"",
                UseShellExecute = false
            });
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Linux: xdg-open handles the user's default desktop environment file manager
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "xdg-open",
                Arguments = $"\"{folderPath}\"",
                UseShellExecute = false
            });
        }
        else
        {
            throw new PlatformNotSupportedException("Unsupported operating system.");
        }
    }
}