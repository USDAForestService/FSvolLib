using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct BiomassOutput_C
{
    public double aboveGroundTotal;
    public double branches;
    public double foliage;
    public double stumpWood;
    public double stumpBark;
    public double stemWoodTotal;
    public double stemBarkTotal;
    public double stemPrimaryWood;
    public double stemPrimaryBark;
    public double stemSecondaryWood;
    public double stemSecondaryBark;
    public double stemTipWood;
    public double stemTipBark;
    public double stemTopAndLimb;
}
