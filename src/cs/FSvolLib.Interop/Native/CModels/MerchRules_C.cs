using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct MerchRules_C
{
    public int evenOdd;
    public int segmentationOption;
    public double maxLogLength;
    public double minLogLength;
    public double minLengthTop;
    public double minTopDibSaw;
    public double minTopDibNonSaw;
    public double minMerchLength;
    public double stumpHeight;
    public double trim;
    public double barkThicknessRatio;
    public double doubleBarkThicknessAtBrestHeight;
    public double minimumBoardFootDiameter;
    [MarshalAs(UnmanagedType.I1)]
    public bool useCorrectedFactor;
}
