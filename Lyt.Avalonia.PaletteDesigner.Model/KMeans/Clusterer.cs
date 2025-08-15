namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans;

public sealed class Clusterer(int k, int maxIterations = 100, int? seed = null)
{
    private readonly int clusterCount = k;
    private readonly int maxIterations = maxIterations;
    private readonly Random random = seed.HasValue ? new Random(seed.Value) : new Random((int)DateTime.Now.Ticks);

    public List<LabColor> Fit(List<LabColor> data)
    {
        if (data.Count < clusterCount)
        {
            throw new ArgumentException("Not enough data points for the number of clusters.");
        }

        // Randomly initialize centroids
        var centroids = data.OrderBy(_ => random.Next()).Take(clusterCount).ToList();
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
                    double distance = LabColor.Distance(data[i], centroids[j]);
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
            var newCentroids = new (double L, double A, double B)[clusterCount];
            int[] counts = new int[clusterCount];

            for (int i = 0; i < data.Count; i++)
            {
                int cluster = assignments[i];
                newCentroids[cluster].L += data[i].L;
                newCentroids[cluster].A += data[i].A;
                newCentroids[cluster].B += data[i].B;
                counts[cluster]++;
            }

            for (int j = 0; j < clusterCount; j++)
            {
                if (counts[j] == 0)
                {
                    continue; // Avoid divide-by-zero
                }

                centroids[j] = new LabColor(
                    newCentroids[j].L / counts[j],
                    newCentroids[j].A / counts[j],
                    newCentroids[j].B / counts[j]);
            }
        }

        return centroids;
    }
}
