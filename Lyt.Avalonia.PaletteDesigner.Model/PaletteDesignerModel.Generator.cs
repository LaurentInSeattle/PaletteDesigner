namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public Palette GenerateFromBgraImageBuffer(
        nint frameBufferAddress, int height, int width, int clusterCount = 16)
    {
        if (this.ActiveProject is null)
        {
            return new(); 
        }

        var hues = this.ExtractHuesFromBgraBuffer(frameBufferAddress, height, width);
        //var sortedHues = 
        //    (from  hue in hues orderby hue.Value descending select hue ).ToList();
        //for ( int i = 0; i < Math.Min(50, sortedHues.Count); ++ i)
        //{
        //    var kvp = sortedHues[i];
        //    Debug.WriteLine("Hue: {0}  - Count: {1}", kvp.Key, kvp.Value); 
        //}

        //clusterCount = Math.Min(clusterCount, sortedHues.Count);

        //var clusters = this.GenerateClusters(hues, clusterCount);
        //var palette = new Palette
        //{
        //    Name = "FromImage",
        //    Kind = PaletteKind.Quadrichromatic,
        //};

        //List<KeyValuePair<int,int>> selectedHues = new (clusterCount);
        //foreach (var cluster in clusters)
        //{
        //    var highestValue = cluster.OrderByDescending(x => x.Value).FirstOrDefault();
        //    selectedHues.Add(highestValue); 
        //}

        //var sortedKvpHues = selectedHues.OrderByDescending(x => x.Value).ToList();

        //double ToAngle (int index )
        //{
        //    KeyValuePair<int, int> kvp = sortedKvpHues[index];
        //    return kvp.Key / 10.0 ;
        //}
        var peaks = this.FindPeaks(hues, 40);
        double ToAngle(int index)
        {
            int peak = peaks[index];
            return peak - 360.0;
        }

        var currentPalette = this.ActiveProject!.Palette;
        currentPalette.Name = "FromImage";
        currentPalette.Kind = PaletteKind.Quadrichromatic;
        this.OnWheelAngleChanged(WheelKind.Primary, ToAngle(0));
        this.OnWheelAngleChanged(WheelKind.Secondary1, ToAngle(1));
        this.OnWheelAngleChanged(WheelKind.Secondary2, ToAngle(2));
        this.OnWheelAngleChanged(WheelKind.Complementary, ToAngle(3));
        currentPalette.ResetAllShades();

        return currentPalette;
    }

    /// <summary> ExtractHuesFromBgraBuffer : frameBufferAddress is supposed to be locked </summary>
    private unsafe Dictionary<int, int> ExtractHuesFromBgraBuffer (nint frameBufferAddress, int height, int width)
    {
        Dictionary<int, int> hues = [];
        byte* p = (byte*)frameBufferAddress;
        for (int row = 0; row < height; ++row)
        {
            for (int col = 0; col < width; ++col)
            {
                byte blue = *p++;
                byte green = *p++;
                byte red = *p++;
                p++; // alpha

                RgbColor rgbColor = new(red, blue, green);
                HsvColor hsvColor = rgbColor.ToHsv();
                int hue = 360 + (int)Math.Round(hsvColor.H / 1.0);
                if (hues.TryGetValue(hue, out int value))
                {
                    hues[hue] = ++value;
                }
                else
                {
                    hues[hue] = 1;
                }
            }
        }

        return hues;
    }

    private List<int> FindPeaks (Dictionary<int, int> hues, int width)
    {
        List<int> peaks = [];

        for ( int pass =  0; pass < 4; ++pass)
        {
            var sortedHues =
                (from hue in hues orderby hue.Value descending select hue).ToList();
            Debug.WriteLine("Sorted Hues Count: {0}", sortedHues.Count);
            for (int i = 0; i < Math.Min(20, sortedHues.Count); ++i)
            {
                var kvp = sortedHues[i];
                Debug.WriteLine("Hue: {0}  - Count: {1}", kvp.Key, kvp.Value);
            }

            int peak = sortedHues[0].Key; 
            peaks.Add(peak);
            Debug.WriteLine("Added: {0}", peak);
            for (int key = peak - width; key <= peak + width; ++key )
            {
                hues.Remove(key);
            }            
        }

        return peaks;
    }
    /// <summary> One dimensional implementation of K-Means to find clusters. </summary>
    /// <param name="hues">Integer hues as tenths of degree on the 360 degrees color wheel.</param>
    /// <param name="clusterCount"></param>
    /// <returns> the clusters </returns>
    private List<Dictionary<int, int>> GenerateClusters(
        Dictionary<int, int> hues, int clusterCount=4)
    {
        static int FindNearestCenter(int color, int[] centers)
        {
            int nearest = centers[0];
            int minDistance = int.MaxValue;
            foreach (int center in centers)
            {
                int distance = Math.Abs(color - center);
                if (distance < minDistance)
                {
                    nearest = center;
                    minDistance = distance;
                }
            }

            return nearest;
        }

        // Initialize the clusters
        var clusters = new List<Dictionary<int, int>>();
        for (int i = 0; i < clusterCount; ++i)
        {
            clusters.Add([]);
        }

        // Select the initial cluster centers randomly
        int[] centers = hues.Keys.OrderBy(c => Guid.NewGuid()).Take(clusterCount).ToArray();
        // int[] centers = new int[clusterCount]; 
        //for (int i = 0; i < clusterCount; ++i)
        //{
        //    centers[i] = hues.Keys.;
        //}

        // Loop until the clusters stabilize
        bool changed = true;
        while (changed)
        {
            changed = false;

            // Assign each color to the nearest cluster center
            foreach (int color in hues.Keys)
            {
                int nearest = FindNearestCenter(color, centers);
                int clusterIndex = Array.IndexOf(centers, nearest);
                clusters[clusterIndex][color] = hues[color];
            }

            // Recompute the cluster centers
            for (int i = 0; i < clusterCount; ++i)
            {
                int count = 0;
                int sum = 0;
                foreach (int color in clusters[i].Keys)
                {
                    sum += color; 
                    ++ count;
                }

                if (count > 0)
                {
                    int newCenter = sum / count;
                    if (!newCenter.Equals(centers[i]))
                    {
                        centers[i] = newCenter;
                        changed = true;
                    }
                } 
            }
        }

        // Return the clusters
        return clusters;
    }
} 
