using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct TreeMeasurment_C
{
    public double totalHeight;
    public double referenceHeight;
    public double merchHeightSaw;
    public double merchHeightNonsaw;
    public int merchHeightUnit;
    public double heightToFirstLiveLimb;
    public double heightToTopBroken;
    [MarshalAs(UnmanagedType.I1)]
    public bool isLive;
    public double dbh;
    public double drc;
    public double referenceDiameter;
    public double topBrokenDiameter;
    public int formClass;
    public double stumpHeightOverride;
    public double minTopDibSawOverride;
    public double minTopDibNonSawOverride;
    public double cull;
    public int decaycd;
    public double crownRatio;
    public int stems;

    public TreeMeasurment_C(TreeMeasurment tm)
    {
        this.totalHeight = tm.TotalHeight;
        this.referenceHeight = tm.ReferenceHeight;
        this.merchHeightSaw = tm.MerchHeightSaw;
        this.merchHeightNonsaw = tm.MerchHeightNonsaw;
        this.merchHeightUnit = (int)tm.MerchHeightUnit;
        this.heightToFirstLiveLimb = tm.HeightToFirstLiveLimb;
        this.heightToTopBroken = tm.HeightToTopBroken;
        this.isLive = tm.IsLive;
        this.dbh = tm.Dbh;
        this.drc = tm.Drc;
        this.referenceDiameter = tm.ReferenceDiameter;
        this.topBrokenDiameter = tm.TopBrokenDiameter;
        this.formClass = tm.FormClass;
        this.stumpHeightOverride = tm.StumpHeightOverride;
        this.minTopDibSawOverride = tm.MinTopDibSawOverride;
        this.minTopDibNonSawOverride = tm.MinTopDibNonSawOverride;
        this.cull = tm.Cull;
        this.decaycd = tm.Decaycd;
        this.crownRatio = tm.CrownRatio;
        this.stems = tm.Stems;
    }
}
