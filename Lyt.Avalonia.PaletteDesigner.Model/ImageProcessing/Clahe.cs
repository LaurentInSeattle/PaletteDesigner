namespace Lyt.Avalonia.PaletteDesigner.Model.ImageProcessing;

/*
CLAHE, which stands for Contrast Limited Adaptive Histogram Equalization, is an image processing technique that 
enhances image contrast by dividing an image into smaller regions, calculating a histogram for each region, 
and then applying a clipped and redistributed histogram equalization to each region. 
Unlike traditional histogram equalization, CLAHE limits the amount of contrast amplification 
to avoid noise amplification in homogeneous areas. It is widely used in applications like enhancing foggy images, 
improving medical images for diagnosis, and strengthening details in low-light conditions.  

How CLAHE Works

1. Tile Generation:
The input image is divided into a grid of smaller rectangular regions called tiles. 

2. Histogram Equalization per Tile:
A histogram is computed for each individual tile. 

3. Contrast Limiting:
A clip limit is applied to the histogram to prevent extreme contrast amplification. 
Any excess values in the histogram are redistributed to other bins. 

4. Interpolation:
The contrast-enhanced tiles are then combined to form the final output image, often using bilinear 
interpolation to ensure a smooth transition between the processed regions. 

See also: 
    https://en.wikipedia.org/wiki/Adaptive_histogram_equalization 
    https://en.wikipedia.org/wiki/Histogram_equalization
    https://en.wikipedia.org/wiki/Shadow_and_highlight_enhancement

 */

public sealed unsafe class Clahe
{
    private const int grayLevels = 256;
    private const float clipTolerance = 2.5F;

    private readonly float clipLimit; // contrast limiting parameter
    private readonly int numberBinX;
    private readonly int numberBinY;
    private readonly int[,][] histograms;
    private readonly int[,][] LUTs;

    private int binXsize;
    private int binYsize;

    private byte* sourceImageBytes;
    private byte* resultImageBytes;
    private byte[]? resultImage;
    private int imageHeight;
    private int imageWidth;

    public Clahe(int nrBinX = 8, int nrBinY = 8, float cLimit = 0.5f)
    {
        this.numberBinX = nrBinX;
        this.numberBinY = nrBinY;
        this.clipLimit = cLimit;
        this.histograms = new int[this.numberBinX, this.numberBinY][];
        this.LUTs = new int[this.numberBinX, this.numberBinY][];
    }

    // Assumes: ( DANGER ! ) 
    // - BGRA image buffer 
    // - frameBufferAddress is LOCKED 
    // - Stride == width 
    // - Width and Height are multiple of the counts of bins 
    public unsafe byte[] Process(nint frameBufferAddress, int height, int width)
    {
        this.imageHeight = height;
        this.imageWidth = width;

        // Make sure that the X and Y size of the image are multiples of the numbers of bins
        if (0 != width % this.numberBinX)
        {
            throw new ArgumentException("Width");
        }

        if (0 != height % this.numberBinY)
        {
            throw new ArgumentException("Height");
        }

        this.binXsize = width / this.numberBinX;
        this.binYsize = height / this.numberBinY;

        byte* p = (byte*)frameBufferAddress;
        this.sourceImageBytes = p;

        // 1. compute histograms
        this.ComputeHistograms();

        // 2. clip contrast
        this.ClipContrast();

        // 3. compute cumulative histograms
        this.ComputeCumulativeHistogram();

        // 3. for each histogram, compute the equalization LUT
        this.ComputeEqualizationLUT();

        // 4. apply transformation based on LUTs
        this.EqualizeHistogram();

        if (this.resultImage is null)
        {
            throw new Exception("CLAHE failed ;(");
        }

        return this.resultImage;
    }

    private void ComputeHistograms()
    {
        void ComputeHistogram(int i, int j)
        {
            // directly histogram in INT array of 256 values 
            int x0 = i * this.binXsize;
            int y0 = j * this.binYsize;
            int x1 = (i + 1) * this.binXsize;
            int y1 = (j + 1) * this.binYsize;

            int[] histogram = new int[256];
            for (int x = x0; x < x1; x++)
            {
                for (int y = y0; y < y1; y++)
                {
                    // Get pixel color 
                    uint pixel = this.GetPixel(x, y);

                    // convert pixel to grayscale and increment corresponding slot in histogram
                    histogram[ToGrayScale(pixel)] += 1;
                }
            }

            this.histograms[i, j] = histogram;
        }

        // TODO: Parallelize
        for (int i = 0; i < this.numberBinX; i++)
        {
            for (int j = 0; j < this.numberBinY; j++)
            {
                ComputeHistogram(i, j);
            }
        }
    }

    private void ComputeCumulativeHistogram()
    {
        for (int x = 0; x < this.numberBinX; x++)
        {
            for (int y = 0; y < this.numberBinY; y++)
            {
                for (int i = 1; i < 256; i++)
                {
                    this.histograms[x, y][i] += this.histograms[x, y][i - 1];
                }
            }
        }
    }

    private void ComputeEqualizationLUT()
    {
        void ComputeEqualizationLUT(int i, int j)
        {
            int[] hist = this.histograms[i, j];
            int[] equalHist = new int[256];
            int count = this.binXsize * this.binYsize - 1;
            for (int k = 0; k < 256; k++)
            {
                equalHist[k] = (int)Math.Round((double)(hist[k] - hist[0]) / count * (grayLevels - 1));
            }

            this.LUTs[i, j] = equalHist;
        }

        // for each bin, compute histogram equalization and return a LUT
        for (int i = 0; i < this.numberBinX; i++)
        {
            for (int j = 0; j < this.numberBinY; j++)
            {
                ComputeEqualizationLUT(i, j);
            }
        }
    }

    private unsafe void EqualizeHistogram()
    {
        // Create result buffer, pin it 
        this.resultImage = new byte[this.imageWidth * this.imageHeight * 4];
        fixed (byte* arrayPtr = this.resultImage)
        {
            // The 'dataArray' is pinned here, and 'arrayPtr' points to its first element.
            this.resultImageBytes = arrayPtr;

            // for each bin, apply LUT
            for (int i = 0; i < this.numberBinX; i++)
            {
                for (int j = 0; j < this.numberBinY; j++)
                {
                    int x0 = i * this.binXsize;
                    int y0 = j * this.binYsize;
                    int x1 = (i + 1) * this.binXsize;
                    int y1 = (j + 1) * this.binYsize;

                    for (int x = x0; x < x1; x++)
                    {
                        for (int y = y0; y < y1; y++)
                        {
                            // Get pixel color 
                            uint pixel = this.GetPixel(x, y);

                            // convert pixel to grayscale and transform intensity 
                            byte gScale = ToGrayScale(pixel);
                            gScale = (byte)this.TransformPixelIntensity(x, y, i, j, gScale);

                            // TODO: Use true colors
                            uint color =
                                (uint)0x0FF_00_00_00 |
                                (uint)(gScale << 16) |
                                (uint)(gScale << 8) |
                                gScale;
                            this.SetPixel(x, y, color);
                        }
                    }
                }
            }
        }
    }

    private void ClipContrast()
    {
        void ClipContrast(int[] histogram)
        {
            // in this function, we clip the histogram to a ceiling value
            int bottom = 0;
            int top = (int)Math.Round(this.clipLimit * 256);
            int diff;
            int middle;
            do
            {
                diff = 0;
                middle = (int)Math.Round((top + bottom) / 2.0f);
                for (int i = 0; i < 256; i++)
                {
                    if (histogram[i] > middle)
                    {
                        diff += histogram[i] - middle;
                    }
                }
                if (diff + middle == top)
                {
                    break;
                }
                else if (diff + middle > top)
                {
                    top = middle;
                }
                else
                {
                    bottom = middle;
                }
            } while (top - bottom > clipTolerance);

            // now, the clip value = middle, compute diff
            diff = 0;
            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] > middle)
                {
                    diff += histogram[i] - middle;
                    histogram[i] = middle; // actually clip it here
                }
            }

            // redistribute excess
            diff = (int)Math.Round(diff / 256.0f); // value to be redistributed to each bin
            for (int i = 0; i < 256; i++)
            {
                histogram[i] += diff;
            }
        }

        for (int i = 0; i < this.numberBinX; i++)
        {
            for (int j = 0; j < this.numberBinY; j++)
            {
                ClipContrast(this.histograms[i, j]);
            }
        }
    }

    private int TransformPixelIntensity(int x, int y, int binX, int binY, byte pixVal)
    {
        // TODO: 
        // When using Kauai.jpg: Crash IOOR with: 
        // x = 121 , y = 1012
        // binX = 0 , binY = 7 
        // pixVal = 92 

        float val = 0.0f;

        // compute position of the corner points ... TODO : should be done only once

        // TODO: There should be no floats here
        float x0 = this.binXsize / 2;
        float x1 = this.imageWidth - x0;
        float y0 = this.binYsize / 2;
        float y1 = this.imageHeight - y0;

        // TODO: There should be no floats here
        float xi = (int)((binX + 0.5) * this.binXsize);
        float yi = (int)((binY + 0.5) * this.binYsize);

        int Lookup(int bdx = 0, int bdy = 0) => this.LUTs[binX + bdx, binY + bdy][pixVal];

        if ((x <= x0 && (y <= y0 || y >= y1)) ||
            (x >= x1 && (y <= y0 || y >= y1)))
        {
            // first, corners => go look in LUT, no interpolation
            val = Lookup();
        }
        else if (x <= x0 || x >= x1)
        {
            // then, bands => linear interpolation
            if (y < yi)
            {
                val = (yi - y) * this.LUTs[binX, binY - 1][pixVal];
                val += (this.binYsize - yi + y) * this.LUTs[binX, binY][pixVal];
                val /= this.binYsize;
            }
            else if (y == yi)
            {
                val = this.LUTs[binX, binY][pixVal];
            }
            else // y > yi
            {
                val = (y - yi) * this.LUTs[binX, binY + 1][pixVal];
                val += (this.binYsize - y + yi) * this.LUTs[binX, binY][pixVal];
                val /= this.binYsize;
            }
        }
        else if (y <= y0 || y >= y1)
        {
            // linear interpolation
            if (x < xi)
            {
                val = (xi - x) * this.LUTs[binX - 1, binY][pixVal];
                val += (this.binXsize - xi + x) * this.LUTs[binX, binY][pixVal];
                val /= this.binXsize;
            }
            else if (x == xi)
            {
                val = this.LUTs[binX, binY][pixVal];
            }
            else // x > xi
            {
                val = (x - xi) * this.LUTs[binX + 1, binY][pixVal];
                val += (this.binXsize - x + xi) * this.LUTs[binX, binY][pixVal];
                val /= this.binXsize;
            }
        }
        else
        {
            // finally, all the rest --> bilinear interp
            int dx = (int)Math.Round(x - xi);
            int dy = (int)Math.Round(y - yi);
            if (x >= xi)
            {
                if (y >= yi)
                {
                    val = BilinearIinterpolation(
                        dx, dy, this.binXsize, this.binYsize,
                            this.LUTs[binX, binY][pixVal], this.LUTs[binX + 1, binY][pixVal],
                            this.LUTs[binX, binY + 1][pixVal], this.LUTs[binX + 1, binY + 1][pixVal]);
                }
                else
                {
                    val = BilinearIinterpolation(
                        dx, this.binYsize + dy, this.binXsize, this.binYsize,
                            this.LUTs[binX, binY - 1][pixVal], this.LUTs[binX + 1, binY - 1][pixVal],
                            this.LUTs[binX, binY][pixVal], this.LUTs[binX + 1, binY][pixVal]);
                }
            }
            else
            {
                if (y >= yi)
                {
                    val = BilinearIinterpolation(
                        this.binXsize + dx, dy, this.binXsize, this.binYsize,
                        this.LUTs[binX - 1, binY][pixVal], this.LUTs[binX, binY][pixVal],
                        this.LUTs[binX - 1, binY + 1][pixVal], this.LUTs[binX, binY + 1][pixVal]);
                }
                else
                {
                    val = BilinearIinterpolation(
                        this.binXsize + dx, this.binYsize + dy, this.binXsize, this.binYsize,
                        this.LUTs[binX - 1, binY - 1][pixVal], this.LUTs[binX, binY - 1][pixVal],
                        this.LUTs[binX - 1, binY][pixVal], this.LUTs[binX, binY][pixVal]);
                }
            }
        }

#if DEBUG
        if (val < 0 || val > 255) // debug - this should never happen
        {
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
#endif

        return (int)Math.Round(val);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float BilinearIinterpolation(int x, int y, int width, int height, int v1, int v2, int v3, int v4)
    {
#if DEBUG
        if (x < 0 || y < 0)
        {
            // debug, this should never happen
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
#endif

        float val1 = (x * v2 + (width - x) * v1) / (float)width;
        float val2 = (x * v4 + (width - x) * v3) / (float)width;
        float res = (val2 * y + ((float)height - y) * val1) / (float)height;

#if DEBUG
        if (res > 255)
        {
            // debug, this should never happen
            if (Debugger.IsAttached) { Debugger.Break(); }
        }
#endif

        return res;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private uint GetPixel(int col, int row) // or x, y 
    {
        uint* intPointer = (uint*)this.sourceImageBytes;
        int offset = row * this.imageWidth + col;
        return intPointer[offset];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetPixel(int col, int row, uint pixel) // or x, y 
    {
        uint* intPointer = (uint*)this.resultImageBytes;
        int offset = row * this.imageWidth + col;
        intPointer[offset] = pixel;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static byte ToGrayScale(uint argb)
    {
        byte blue = (byte)(argb & 0x0FF);
        byte green = (byte)((argb & 0x0FF00) >> 8);
        byte red = (byte)((argb & 0x0FF0000) >> 16);
        float grayscale = (0.299f * red) + (0.587f * green) + (0.114f * blue);
        return (byte)grayscale;
    }
}
