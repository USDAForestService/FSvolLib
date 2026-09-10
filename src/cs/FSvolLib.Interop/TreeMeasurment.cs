namespace FSvolLib.Interop;

public enum MerchHeightUnit { FEET = 0, LOGS8 = 8, LOGS16 = 16, LOGS32 = 32, };

public class TreeMeasurment
{
    public double TotalHeight { get; set; }
    public double ReferenceHeight { get; set; }
    public double MerchHeightSaw { get; set; }
    public double MerchHeightNonsaw { get; set; }
    public MerchHeightUnit MerchHeightUnit { get; set; } = MerchHeightUnit.FEET;
    public double HeightToFirstLiveLimb { get; set; } = 25.0;
    public double HeightToTopBroken { get; set; }
    public bool IsLive { get; set; }
    public double Dbh { get; set; }
    public double Drc { get; set; }
    public double ReferenceDiameter { get; set; }
    public double TopBrokenDiameter { get; set; }
    public int FormClass { get; set; }
    public double StumpHeightOverride { get; set; } = -1.0;
    public double MinTopDibSawOverride { get; set; } = -1.0;
    public double MinTopDibNonSawOverride { get; set; } = -1.0;
    public double Cull { get; set; }
    public int Decaycd { get; set; }
    public double CrownRatio { get; set; }
    public int Stems { get; set; } = 1;
}
