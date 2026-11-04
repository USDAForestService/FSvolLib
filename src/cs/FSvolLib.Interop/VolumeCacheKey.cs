using System;

namespace FSvolLib.Interop;

public readonly record struct VolumeCacheKey
{
    // Grouping by standard primitives to match C++ inputs perfectly
    public readonly int FiaCode;
    public readonly byte AuxFlag;
    public readonly int Region;
    public readonly int Forest;
    public readonly int District;
    public readonly int PrimaryProduct;
    public readonly int SecondaryProduct;
    public readonly int VolumeCalculationType;
    public readonly string VolumeEquationNumberOverride;
    public readonly string EcoRegion;
    public readonly int BasalArea;
    public readonly int SiteIndex;

    public readonly double TotalHeight;
    public readonly double ReferenceHeight;
    public readonly double MerchHeightSaw;
    public readonly double MerchHeightNonsaw;
    public readonly int MerchHeightUnit;
    public readonly double HeightToFirstLiveLimb;
    public readonly double HeightToTopBroken;
    public readonly bool IsLive;
    public readonly double Dbh;
    public readonly double Drc;
    public readonly double ReferenceDiameter;
    public readonly double TopBrokenDiameter;
    public readonly int FormClass;
    public readonly double StumpHeightOverride;
    public readonly double MinTopDibSawOverride;
    public readonly double MinTopDibNonSawOverride;
    public readonly double Cull;
    public readonly int Decaycd;
    public readonly double CrownRatio;
    public readonly int Stems;

    public static VolumeCacheKey Create(VolumeCalculationOptions opts, TreeMeasurment meas)
    {
        return new VolumeCacheKey(opts, meas);
    }

    private VolumeCacheKey(VolumeCalculationOptions opts, TreeMeasurment meas)
    {
        FiaCode = opts.FiaCode;
        AuxFlag = (byte)opts.AuxFlag;
        Region = opts.Region;
        Forest = opts.Forest;
        District = opts.District;
        PrimaryProduct = opts.PrimaryProduct;
        SecondaryProduct = opts.SecondaryProduct;
        VolumeCalculationType = (int)opts.VolumeCalculationType;
        VolumeEquationNumberOverride = opts.VolumeEquationNumberOverride ?? string.Empty;
        EcoRegion = opts.EcoRegion ?? string.Empty;
        BasalArea = opts.BasalArea;
        SiteIndex = opts.SiteIndex;

        // Apply 1-decimal rounding safety to all double properties
        TotalHeight = Math.Round(meas.TotalHeight, 1);
        ReferenceHeight = Math.Round(meas.ReferenceHeight, 1);
        MerchHeightSaw = Math.Round(meas.MerchHeightSaw, 1);
        MerchHeightNonsaw = Math.Round(meas.MerchHeightNonsaw, 1);
        MerchHeightUnit = (int)meas.MerchHeightUnit;
        HeightToFirstLiveLimb = Math.Round(meas.HeightToFirstLiveLimb, 1);
        HeightToTopBroken = Math.Round(meas.HeightToTopBroken, 1);
        IsLive = meas.IsLive;
        Dbh = Math.Round(meas.Dbh, 1);
        Drc = Math.Round(meas.Drc, 1);
        ReferenceDiameter = Math.Round(meas.ReferenceDiameter, 1);
        TopBrokenDiameter = Math.Round(meas.TopBrokenDiameter, 1);
        FormClass = meas.FormClass;
        StumpHeightOverride = Math.Round(meas.StumpHeightOverride, 1);
        MinTopDibSawOverride = Math.Round(meas.MinTopDibSawOverride, 1);
        MinTopDibNonSawOverride = Math.Round(meas.MinTopDibNonSawOverride, 1);
        Cull = Math.Round(meas.Cull, 1);
        Decaycd = meas.Decaycd;
        CrownRatio = Math.Round(meas.CrownRatio, 1);
        Stems = meas.Stems;
    }
}
