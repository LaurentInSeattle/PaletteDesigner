namespace Lyt.Avalonia.PaletteDesigner.Model.KMeans.Generic;

/* 
 * 
    The K-means algorithm is NOT deterministic. 

    Random Initialization: 
        K-means often starts by randomly selecting initial cluster centroids. 
        This randomness means that running the algorithm multiple times on the exact same dataset 
        can lead to different starting points and subsequently, different final cluster assignments 
        and configurations.

    Local Optima: 
        Since K-means is an iterative algorithm that aims to minimize the within-cluster sum of squares (WCSS), 
        it can converge to a local optimum rather than the global optimum. The initial random placement of centroids 
        can influence which local optimum is reached.
    
    Different Cluster Configurations: 
        Due to the random initialization and the possibility of converging to different local optima, running 
        K-means multiple times can lead to variations in how data points are grouped into clusters, and even 
        the assignment of cluster labels. 

    Convergence: 
        the K-means algorithm is guaranteed to converge. This convergence is due to the fact that each iteration of 
        the algorithm either decreases the within-cluster sum of squares (WCSS) or leaves it unchanged. Since the WCSS 
        is a non-negative value and is monotonically decreasing, it must eventually reach a stable state where no further 
        improvements can be made. This stable state signifies convergence.

*
*/

/// <summary> Generic Implementation of the KMeans Algorithm </summary>
public sealed class Clusterer<T>(int clusterCount, int maxIterations = 200) 
    where T : class, IClusterable<T>, new()
{
    private readonly int clusterCount = clusterCount;
    private readonly int maxIterations = maxIterations;

    public List<Cluster<T>> Discover(List<T> data)
    {
        int dataCount = data.Count; 
        if (dataCount < clusterCount)
        {
            throw new ArgumentException("Not enough data points for the number of clusters.");
        }

        // Do not randomly initialize centroids to mitigate the lack of determinism.
        // Instead: pick colors in the source list equally spaced. 
        List<T> initialColors = new(clusterCount);
        int step = dataCount / clusterCount;
        int dataIndex = 0;
        for (int init = 0; init < clusterCount; ++ init )
        {
            initialColors.Add(data[dataIndex]);
            dataIndex += step; 
        }

        var centroids = new List<Cluster<T>>(initialColors.Count);
        foreach (var color  in initialColors)
        {
            centroids.Add(new Cluster<T>() { Count = 0 , Payload = color });    
        }

        // Early declarations to prevent creating and releasing these arrays on each iteration
        int[] assignments = new int[dataCount];
        var values = new List<T>[clusterCount];
        for (int k = 0; k < clusterCount; ++k)
        {
            values[k] = [];
        }

        int lastIteration = 0;
        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            bool changed = false;

            // Run multi-threaded 
            // Speed up the processing of assigning points to closest centroid 
            void AssignPointToClosestCentroid(int from, int to)
            {
                for (int i = from; i < to; ++i)
                {
                    // Find smallest distance
                    int bestIndex = -1;
                    double bestDistance = double.MaxValue;
                    for (int j = 0; j < clusterCount; j++)
                    {
                        double distance = T.Distance(data[i], centroids[j].Payload);
                        if (distance < bestDistance)
                        {
                            bestDistance = distance;
                            bestIndex = j;
                        }
                    }

                    if (assignments[i] != bestIndex)
                    {
                        // Assign point to closest centroid
                        assignments[i] = bestIndex;
                        changed = true;
                    }
                }
            }

            Parallelize.ActionOnIndices(dataCount, AssignPointToClosestCentroid); 

            // Exit early if no changes
            if (!changed)
            {
                Debug.WriteLine("Clusters: Early break at iteration: " + iteration); 
                break;
            }

            // Recalculate centroids
            for (int k = 0; k < clusterCount; ++k)
            {
                values[k].Clear();
            }

            void AggregateAssignments(int from, int to)
            {
                for (int iData = from; iData < to; ++iData)
                {
                    int clusterIndex = assignments[iData];
                    // Too slow: 
                    //lock (values[clusterIndex])
                    //{
                        values[clusterIndex].Add(data[iData]);
                    // }
                }
            }

            AggregateAssignments(0, dataCount);

            // This below fails (values need a lock - and that's slow)
            // Adding the lock and using multiple threads is slower that the single threaded version. 
            // So, NOPE, no gain: Parallelize.ActionOnIndices(dataCount, AggregateAssignments);

            void CalculateCentroids(int from, int to)
            {
                for (int centroidIndex = from; centroidIndex < to; ++centroidIndex)
                {
                    List<T> list = values[centroidIndex];
                    int clusterCountAtIndex = list.Count;
                    if (clusterCountAtIndex == 0)
                    {
                        Debug.WriteLine("Clusters: Cluster count is zero: " + iteration);
                        continue; // Avoid divide-by-zero
                    }

                    T average = T.Average(list);
                    centroids[centroidIndex] = new Cluster<T>() { Count = clusterCountAtIndex, Payload = average };
                }
            }

            Parallelize.ActionOnIndices(clusterCount, CalculateCentroids);

            lastIteration = iteration;
        }

        Debug.WriteLine("Clusters: Completed iterations: " + ( 1 + lastIteration).ToString() );

        foreach (var centroid in centroids)
        {
            centroid.Total = dataCount;
        }

        return centroids;
    }
}
