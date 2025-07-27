namespace Lyt.Avalonia.PaletteDesigner.Workflow.Shading;

using System;
using static GeneralExtensions; 

public partial class PalettePreviewViewModel : ViewModel<PalettePreviewView>
{
    private readonly ShadesValuesViewModel PrimaryShadesValues;

    private readonly ShadesValuesViewModel Secondary1ShadesValues;

    private readonly ShadesValuesViewModel Secondary2ShadesValues;

    private readonly ShadesValuesViewModel ComplementaryShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel topLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomLeftShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel topRightShadesValues;

    [ObservableProperty]
    private ShadesValuesViewModel bottomRightShadesValues;

    [ObservableProperty]
    private MiniPaletteViewModel miniPaletteViewModel;

    [ObservableProperty]
    private MaxiPaletteViewModel maxiPaletteViewModel;

    [ObservableProperty]
    private double wheelSliderValue;

    [ObservableProperty]
    private string wheelValue = string.Empty;

    private readonly PaletteDesignerModel paletteDesignerModel;

    private bool isProgrammaticUpdate; 

    private double wheel;

    public PalettePreviewViewModel(PaletteDesignerModel paletteDesignerModel)
    {
        this.paletteDesignerModel = paletteDesignerModel;
        this.MiniPaletteViewModel = new();
        this.MaxiPaletteViewModel = new();
        this.PrimaryShadesValues = new ShadesValuesViewModel("Dominant");
        this.ComplementaryShadesValues = new ShadesValuesViewModel("Accent");
        this.Secondary1ShadesValues = new ShadesValuesViewModel("Discord #1");
        this.Secondary2ShadesValues = new ShadesValuesViewModel("Discord #2");
        this.TopLeftShadesValues     = this.PrimaryShadesValues;
        this.BottomLeftShadesValues  = this.PrimaryShadesValues;
        this.TopRightShadesValues    = this.PrimaryShadesValues;
        this.BottomRightShadesValues = this.PrimaryShadesValues;

        this.Messenger.Subscribe<ShadesValuesVisibilityMessage>(this.OnShadesValuesVisibility);
    }

    private void OnShadesValuesVisibility(ShadesValuesVisibilityMessage message)
        => this.Show(message.Show);

    public void Show(bool show = true)
    {
        if (this.IsBound)
        {
            var grid = this.View.MainGrid;
            var columns = grid.ColumnDefinitions;
            var newGridLength = new GridLength(show ? 120.0 : 0.0);
            columns[0].Width = newGridLength;
            columns[2].Width = newGridLength;
            grid.Width = show ? 840.0 : 600.0; 
        }
    }
    public override void OnViewLoaded()
    {
        base.OnViewLoaded();
        this.Show(); 
        this.WheelSliderValue = 0.0;
        this.WheelValue = string.Empty;
        this.UpdateLabels();
    }

    partial void OnWheelSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return; 
        } 

        this.wheel = value;
        this.UpdateLabels();
        this.paletteDesignerModel.UpdatePalettePrimaryWheel(value);
    }

    public void UpdateLabels() 
        => this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);

    public void Update(Palette palette)
    {
        int colorCount = palette.Kind.ColorCount();
        if ((colorCount < 1) || (colorCount > 4))
        {
            // Not ready ? Should throw ? 
            return;
        }

        With(ref this.isProgrammaticUpdate, () =>
        {
            this.wheel = palette.Primary.Wheel; 
            this.WheelSliderValue = palette.Primary.Wheel;
            var primaryColor = palette.Primary.Base.Color;
            this.UpdateLabels();
        });

        this.MiniPaletteViewModel.Update(palette);
        this.MaxiPaletteViewModel.Update(palette);

        Shades shades = palette.Primary;
        this.PrimaryShadesValues.Update(shades);
        if ((colorCount == 1) || (colorCount == 2))
        {
            this.TopLeftShadesValues = this.PrimaryShadesValues;
            this.BottomLeftShadesValues = this.PrimaryShadesValues;
            this.TopRightShadesValues = this.PrimaryShadesValues;
            if (colorCount == 2)
            {
                // colorCount == 1 => Complementary same as primary 
                shades = palette.Complementary;
                this.ComplementaryShadesValues.Update(shades);
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
            }
            else
            {
                this.BottomRightShadesValues = this.PrimaryShadesValues;
            }
        }
        else // colorCount == 3 or 4 
        {
            this.TopLeftShadesValues = this.PrimaryShadesValues;
            shades = palette.Secondary1;
            this.Secondary1ShadesValues.Update(shades);
            this.TopRightShadesValues = this.Secondary1ShadesValues;

            shades = palette.Secondary2;
            this.Secondary2ShadesValues.Update(shades);
            this.BottomLeftShadesValues = this.Secondary2ShadesValues;

            if (colorCount == 4)
            {
                shades = palette.Complementary;
                this.ComplementaryShadesValues.Update(shades);
                this.BottomRightShadesValues = this.ComplementaryShadesValues;
            }
            else
            {
                // colorCount == 3 => Complementary same as primary 
                shades = palette.Primary;
                this.BottomRightShadesValues = this.PrimaryShadesValues;
            }
        }
    }
}

#region Saturation and Brightness sliders 
/*
 * 
            <TextBlock
                Grid.Row="1"
                Text="Saturation"
                Theme="{StaticResource Medium}"
                HorizontalAlignment="Right" 
                />
            <Slider
                Grid.Row="1" Grid.Column="1"
                Margin="8 0 8 0"
                Minimum="0.0" Maximum="1.0" 
                SmallChange="0.01" LargeChange="0.05"
                TickFrequency="0.1" TickPlacement="BottomRight"
                Value="{Binding SaturationSliderValue}"
                HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                />
            <TextBlock
                Grid.Row="1" Grid.Column="2"
                Text="{Binding SaturationValue}"
                Theme="{StaticResource Medium}"
                />
            <TextBlock
                Grid.Row="2"
                Text="Brightness"
                Theme="{StaticResource Medium}"
                HorizontalAlignment="Right" 
                />
            <Slider
                Grid.Row="2" Grid.Column="1"
                Margin="8 0 8 0"
                Minimum="0.0" Maximum="1.0"
                SmallChange="0.01" LargeChange="0.05"
                TickFrequency="0.1" TickPlacement="BottomRight"
                Value="{Binding BrightnessSliderValue}"
                HorizontalAlignment="Stretch" VerticalAlignment="Stretch"
                />
            <TextBlock
                Grid.Row="2" Grid.Column="2"
                Text="{Binding BrightnessValue}"
                Theme="{StaticResource Medium}"
                />
 *
    [ObservableProperty]
    private double saturationSliderValue;

    [ObservableProperty]
    private double brightnessSliderValue;

    [ObservableProperty]
    private string saturationValue = string.Empty;

    [ObservableProperty]
    private string brightnessValue = string.Empty;

    private double saturation;

    private double brightness;

CTOR:
        this.SaturationSliderValue = 0.67;
        this.BrightnessSliderValue = 0.67;
        this.SaturationValue = string.Empty;
        this.BrightnessValue = string.Empty;

    partial void OnSaturationSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.saturation = value;
        this.UpdateLabels();
        //this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    partial void OnBrightnessSliderValueChanged(double value)
    {
        if (this.isProgrammaticUpdate)
        {
            return;
        }

        this.brightness = value;
        this.UpdateLabels();
        //this.paletteDesignerModel.UpdatePalettePrimaryShade(this.saturation, this.brightness);
    }

    public void UpdateLabels()
    {
        this.WheelValue = string.Format("{0:F1} \u00B0", this.wheel);
        this.SaturationValue = string.Format("{0:F1} %", this.saturation * 100.0);
        this.BrightnessValue = string.Format("{0:F1} %", this.brightness * 100.0);
    }

Update: 

        With(ref this.isProgrammaticUpdate, () =>
        {
            this.wheel = palette.Primary.Wheel; 
            this.WheelSliderValue = palette.Primary.Wheel;
            var primaryColor = palette.Primary.Base.Color;

            this.saturation = primaryColor.S; 
            this.SaturationSliderValue = primaryColor.S;
            this.brightness = primaryColor.V;
            this.BrightnessSliderValue = primaryColor.V;
            this.UpdateLabels();
        });

*/
#endregion Saturation and Brightness sliders 
