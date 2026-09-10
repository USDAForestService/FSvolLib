using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct LogOutput_C
{
    public int logNumber;
    public int product;
    [MarshalAs(UnmanagedType.I1)]
    public bool isSecondary;
    public double smallEndDiameterActual;
    public double largeEndDiameterActual;
    public double smallEndDiameterScaled;
    public double largeEndDiameterScaled;
    public double length;
    public double heightToLargeEndDiameter;
    public double grossBoardFoot;
    public double grossCubicFoot;
    public double internationalBoardFoot;
    public double greenWeight;
    public double dryWeight;


}
