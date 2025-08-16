namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans;

public sealed class Clusterer(int k, int maxIterations = 100, int? seed = null)
{
    private readonly int clusterCount = k;
    private readonly int maxIterations = maxIterations;
    private readonly Random random = seed.HasValue ? new Random(seed.Value) : new Random((int)DateTime.Now.Ticks);

    public List<Cluster> Discover(List<LabColor> data)
    {
        if (data.Count < clusterCount)
        {
            throw new ArgumentException("Not enough data points for the number of clusters.");
        }

        // Randomly initialize centroids
        var initialColors = data.OrderBy(_ => random.Next()).Take(clusterCount).ToList();
        var centroids = new List<Cluster>(initialColors.Count);
        foreach (var color  in initialColors)
        {
            centroids.Add(new Cluster() { Count = 0 , LabColor = color });    
        }

        int[] assignments = new int[data.Count];
        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            bool changed = false;

            // Assign points to closest centroid
            for (int i = 0; i < data.Count; i++)
            {
                int bestIndex = -1;
                double bestDistance = double.MaxValue;
                for (int j = 0; j < clusterCount; j++)
                {
                    double distance = LabColor.Distance(data[i], centroids[j].LabColor);
                    if (distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestIndex = j;
                    }
                }

                if (assignments[i] != bestIndex)
                {
                    assignments[i] = bestIndex;
                    changed = true;
                }
            }

            // Exit early if no changes
            if (!changed)
            {
                break;
            }

            // Recalculate centroids
            var newClusters = new Cluster[clusterCount];
            for (int k = 0; k < clusterCount; k++)
            {
                newClusters[k] = new Cluster();
            }

            int[] counts = new int[clusterCount];
            for (int i = 0; i < data.Count; i++)
            {
                int cluster = assignments[i];
                var colorAtCluster = newClusters[cluster].LabColor;
                var dataAtI = data[i];
                colorAtCluster.L += dataAtI.L;
                colorAtCluster.A += dataAtI.A;
                colorAtCluster.B += dataAtI.B;
                counts[cluster]++;
            }

            for (int j = 0; j < clusterCount; j++)
            {
                if (counts[j] == 0)
                {
                    continue; // Avoid divide-by-zero
                }

                var colorAtJ = newClusters[j].LabColor;
                int countAtJ = counts[j];
                centroids[j].LabColor = 
                    new LabColor( colorAtJ.L / countAtJ, colorAtJ.A / countAtJ, colorAtJ.B / countAtJ);
                centroids[j].Count = countAtJ;
            }
        }

        foreach (var centroid in centroids)
        {
            centroid.Total = data.Count;
        }

        return centroids;
    }
}
