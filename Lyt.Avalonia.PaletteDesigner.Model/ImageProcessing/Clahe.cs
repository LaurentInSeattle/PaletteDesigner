namespace Lyt.Avalonia.PaletteDesigner.Model.ImageProcessing;

public sealed unsafe class Clahe
{
    private const int grayLevels = 256;
    private const float clipTolerance = 2.5F;

    private readonly float clipLimit; // contrast limiting parameter
    private readonly int numberBinX;
    private readonly int numberBinY;
    private readonly int[,][] iHists;
    private readonly int[,][] LUTs;

    // THese two should be integers !!!
    private float binXsize;
    private float binYsize;

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
        this.iHists = new int[numberBinX, numberBinY][];
        this.LUTs = new int[numberBinX, numberBinY][];
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

        // TODO: make sure that the X and Y size of the image are multiples of the numbers of bins
        this.binXsize = width / numberBinX;
        this.binYsize = height / numberBinY;

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
        int[] ComputeHistogram(int i, int j)
        {
            // directly histogram in INT array of 256 values 
            int x0 = (int)Math.Round(i * binXsize);
            int y0 = (int)Math.Round(j * binYsize);
            int x1 = (int)Math.Round((i + 1) * binXsize);
            int y1 = (int)Math.Round((j + 1) * binYsize);

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

            return histogram;
        }

        // TODO: Parallelize
        for (int i = 0; i < numberBinX; i++)
        {
            for (int j = 0; j < numberBinY; j++)
            {
                iHists[i, j] = ComputeHistogram(i, j);
            }
        }
    }

    private void ComputeCumulativeHistogram()
    {
        for (int x = 0; x < numberBinX; x++)
        {
            for (int y = 0; y < numberBinY; y++)
            {
                for (int i = 1; i < 256; i++)
                {
                    iHists[x, y][i] += iHists[x, y][i - 1];
                }
            }
        }
    }

    private void ComputeEqualizationLUT()
    {
        void ComputeEqualizationLUT(int i , int j)
        {
            int[] hist = iHists[i, j];
            int[] equalHist = new int[256];
            int count = (int)Math.Round(this.binXsize * binYsize) - 1;
            for (int k = 0; k < 256; k++)
            {
                equalHist[k] = (int)Math.Round((double)(hist[k] - hist[0]) / count * (grayLevels - 1));
            }

            LUTs[i, j] = equalHist;
        }

        // for each bin, compute histogram equalization and return a LUT
        for (int i = 0; i < numberBinX; i++)
        {
            for (int j = 0; j < numberBinY; j++)
            {
                ComputeEqualizationLUT(i,j);
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
            for (int i = 0; i < numberBinX; i++)
            {
                for (int j = 0; j < numberBinY; j++)
                {
                    int x0 = (int)Math.Round(i * binXsize);
                    int y0 = (int)Math.Round(j * binYsize);
                    int x1 = (int)Math.Round((i + 1) * binXsize);
                    int y1 = (int)Math.Round((j + 1) * binYsize);
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
        int[] ClipContrast(int[] histo)
        {
            // in this function, we clip the histogram to a ceiling value
            int middle, top, bottom;
            bottom = 0;
            top = (int)Math.Round(clipLimit * 256);
            int diff;
            do
            {
                diff = 0;
                middle = (int)Math.Round((top + bottom) / 2F);
                for (int i = 0; i < 256; i++)
                {
                    if (histo[i] > middle)
                    {
                        diff += histo[i] - middle;
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

            // now, the clip value = middle
            // compute diff
            diff = 0;
            for (int i = 0; i < 256; i++)
            {
                if (histo[i] > middle)
                {
                    diff += histo[i] - middle;
                    histo[i] = middle; // actually clip it here
                }
            }

            // redistribute excess
            diff = (int)Math.Round(diff / 256.0f); // value to be redistributed to each bin
            for (int i = 0; i < 256; i++)
            {
                histo[i] += diff;
            }

            return histo;
        }

        for (int i = 0; i < numberBinX; i++)
        {
            for (int j = 0; j < numberBinY; j++)
            {
                iHists[i, j] = ClipContrast(iHists[i, j]);
            }
        }
    }

    private int TransformPixelIntensity(int x, int y, int binX, int binY, byte pixVal)
    {
        // TODO: 
        // Crash IOOR with: 
        // x = 121 , y = 1012
        // binX = 0 , binY = 7 
        // pixVal = 92 


        float val = 0.0f;

        // compute position of the corner points ... TODO : should be done only once
        float x0 = binXsize / 2;
        float x1 = this.imageWidth - x0;
        float y0 = binYsize / 2;
        float y1 = this.imageHeight - y0;
        float xi = (int)((binX + 0.5) * binXsize);
        float yi = (int)((binY + 0.5) * binYsize);
        if ((x <= x0 && (y <= y0 || y >= y1)) ||
            (x >= x1 && (y <= y0 || y >= y1)))
        {
            // first, corners
            // go look in LUT, no interpolation
            val = LUTs[binX, binY][pixVal];
        }
        else if (x <= x0 || x >= x1)
        {
            // then, bands
            // linear interpolation
            if (y < yi)
            {
                val = (yi - y) * LUTs[binX, binY - 1][pixVal];
                val += (binYsize - yi + y) * LUTs[binX, binY][pixVal];
                val /= binYsize;
            }
            else if (y == yi)
            {
                val = LUTs[binX, binY][pixVal];
            }
            else // y > yi
            {
                val = (y - yi) * LUTs[binX, binY + 1][pixVal];
                val += (binYsize - y + yi) * LUTs[binX, binY][pixVal];
                val /= binYsize;
            }
        }
        else if (y <= y0 || y >= y1)
        {
            // linear interpolation
            if (x < xi)
            {
                val = (xi - x) * LUTs[binX - 1, binY][pixVal];
                val += (binXsize - xi + x) * LUTs[binX, binY][pixVal];
                val /= binXsize;
            }
            else if (x == xi)
            {
                val = LUTs[binX, binY][pixVal];
            }
            else // x > xi
            {
                val = (x - xi) * LUTs[binX + 1, binY][pixVal];
                val += (binXsize - x + xi) * LUTs[binX, binY][pixVal];
                val /= binXsize;
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
                    val = BilinearIinterpolation(dx, dy, (int)binXsize, (int)binYsize,
                            LUTs[binX, binY][pixVal], LUTs[binX + 1, binY][pixVal], LUTs[binX, binY + 1][pixVal], LUTs[binX + 1, binY + 1][pixVal]);
                }
                else
                {
                    val = BilinearIinterpolation(dx, (int)binYsize + dy, (int)binXsize, (int)binYsize,
                            LUTs[binX, binY - 1][pixVal], LUTs[binX + 1, binY - 1][pixVal], LUTs[binX, binY][pixVal], LUTs[binX + 1, binY][pixVal]);
                }
            }
            else
            {
                if (y >= yi)
                {
                    val = BilinearIinterpolation((int)binXsize + dx, dy, (int)binXsize, (int)binYsize,
                        LUTs[binX - 1, binY][pixVal], LUTs[binX, binY][pixVal], LUTs[binX - 1, binY + 1][pixVal], LUTs[binX, binY + 1][pixVal]);
                }
                else
                {
                    val = BilinearIinterpolation((int)binXsize + dx, (int)binYsize + dy, (int)binXsize, (int)binYsize,
                        LUTs[binX - 1, binY - 1][pixVal], LUTs[binX, binY - 1][pixVal], LUTs[binX - 1, binY][pixVal], LUTs[binX, binY][pixVal]);
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
