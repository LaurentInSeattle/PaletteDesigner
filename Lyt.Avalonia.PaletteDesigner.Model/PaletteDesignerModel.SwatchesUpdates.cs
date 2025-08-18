namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    public bool SaveSwatches(string name, List<Cluster> clusters)
    {
        if (this.ActiveProject is null)
        {
            // throw new Exception("No project");
            return false;
        } 

        List<Swatch> swatches = new (clusters.Count);
        foreach (Cluster cluster in clusters)
        {
            swatches.Add(new Swatch(cluster)); 
        } 

        ColorSwatches colorSwatches = new()
        {
            Name = name,
            Swatches = swatches, 
        };

        this.ActiveProject.Swatches = colorSwatches;
        return true;
    }

    public bool SaveSwatches(string name, List<Swatch> swatches)
    {
        if (this.ActiveProject is null)
        {
            // throw new Exception("No project");
            return false;
        }

        ColorSwatches colorSwatches = new()
        {
            Name = name,
            Swatches = swatches,
        };

        this.ActiveProject.Swatches = colorSwatches;
        return true;
    }
}
