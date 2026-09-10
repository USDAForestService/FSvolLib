namespace FSvolLib.Interop;

public enum VolumeCalculationType
{
    FVS = 0,
    FIA,
    CRUISE,
    VARIABLELOGLENGTH,
};

public enum AuxFlag : byte
{
    NONE = (byte)'\0',
    R10YOUNGGROWTH = (byte)'Y',
    R6DOUGFIR = (byte)'F',
    PLANTATION = (byte)'P',
};


public class VolumeCalculationOptions
{
    public int FiaCode;
    public AuxFlag AuxFlag = AuxFlag.NONE; // optional defaults to null char

    public int Region;
    public int Forest;
    public int District;
    public int PrimaryProduct;
    public int SecondaryProduct;
    public VolumeCalculationType VolumeCalculationType = VolumeCalculationType.FVS;
    public string? VolumeEquationNumberOverride;
    public string? EcoRegion; // optional fia only
    public int BasalArea = 0;
    public int SiteIndex = 0;
}
