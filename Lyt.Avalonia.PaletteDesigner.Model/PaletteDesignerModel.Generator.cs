namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    /// <summary> Extracts an array of unique RGB colours from a BGRA image buffer </summary>
    /// <param name="frameBufferAddress">Supposed to be already locked </param>
    public static unsafe RgbColor[] ExtractRgbFromBgraBuffer(nint frameBufferAddress, int height, int width)
    {
        var hashset = new HashSet<uint>(height * width);
        byte* p = (byte*)frameBufferAddress;
        for (int row = 0; row < height; ++row)
        {
            for (int col = 0; col < width; ++col)
            {
                byte blue = *p++;
                byte green = *p++;
                byte red = *p++;
                p++; // alpha: skip

                uint color = (uint)(red << 16) | (uint)(green << 8) | (uint)blue;
                hashset.Add(color);
            }
        }

        Debug.WriteLine("Pixels: " + (height * width).ToString());
        Debug.WriteLine("Unique Colors: " + hashset.Count.ToString());

        var colors = new RgbColor[hashset.Count];
        int index = 0;
        foreach (uint color in hashset)
        {
            RgbColor rgbColor = new(color);
            colors[index++] = rgbColor;
        }

        return colors;
    }

    /// <summary> Run the KMeans algorithm to return a list of colour swatches </summary>
    public static List<Cluster<LabColor>> ExtractSwatches(RgbColor[] rgbPixels, int clusterCount, int depthAnalysis)
    {
        List<LabColor> labPixels = [.. rgbPixels.Select(rgb => new LabColor(rgb))];
        return new Clusterer<LabColor>(clusterCount, depthAnalysis).Discover(labPixels);
    }
}
