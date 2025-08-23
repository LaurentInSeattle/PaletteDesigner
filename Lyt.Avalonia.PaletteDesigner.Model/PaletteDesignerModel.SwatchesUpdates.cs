namespace Lyt.Avalonia.PaletteDesigner.Model;

public sealed partial class PaletteDesignerModel : ModelBase
{
    private bool suspendSwatchesUpdates;

    private bool UpdateSwatches(Func<ColorSwatches, bool> action)
    {
        bool result = this.ActionSwatches(action);
        if (result && !this.suspendSwatchesUpdates)
        {
            this.Messenger.Publish(new ModelSwatchesUpdatedMessage());
        }

        return result;
    }

    private bool ActionSwatches(Func<ColorSwatches, bool> action)
    {
        if (this.ActiveProject is null)
        {
            return false;
        }

        var swatches = this.ActiveProject.Swatches;
        if (swatches is null)
        {
            return false;
        }

        // Nothing like that for swatches, at least for now.
        // 
        //if ((Palette.ColorWheel is null) || (Palette.ShadeMap is null))
        //{
        //    throw new Exception("Palette class has not been setup");
        //}

        return action(swatches);
    }

    public bool SaveSwatches(string path, string name, List<Cluster> clusters)
        => this.ActionSwatches((swatches) =>
        {
            List<Swatch> swatchList = new(clusters.Count);
            foreach (Cluster cluster in clusters)
            {
                swatchList.Add(new Swatch(cluster));
            }

            return this.SaveSwatches(path, name, swatchList); 
        });

    public bool SaveSwatches(string path, string name, List<Swatch> swatchList)
        => this.ActionSwatches((swatches) =>
        {
            ColorSwatches colorSwatches = new()
            {
                ImagePath = path,
                Name = name,
                IsDeepAlgorithmStrength = this.IsDeepImagingAlgorithmStrength,
                Swatches = swatchList,
            };

            this.ActiveProject!.Swatches = colorSwatches;
            return true;
        });
}
