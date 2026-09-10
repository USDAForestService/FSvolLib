using System.Runtime.InteropServices;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct VolumeCalculationOptions_C
{
    public int fiaCode;
    public byte auxFlag;
    public int region;
    public int forest;
    public int district;
    public int primaryProduct;
    public int secondaryProduct;
    public int volumeCalculationOptions;
    public IntPtr volumeEquationNumberOverride;
    public IntPtr ecoRegion;
    public int basalArea;
    public int siteIndex;

    public VolumeCalculationOptions_C() { }

    public VolumeCalculationOptions_C(VolumeCalculationOptions vco)
    {
        this.fiaCode = vco.FiaCode;
        this.auxFlag = (byte)vco.AuxFlag;
        this.region = vco.Region;
        this.forest = vco.Forest;
        this.district = vco.District;
        this.primaryProduct = vco.PrimaryProduct;
        this.secondaryProduct = vco.SecondaryProduct;
        this.volumeCalculationOptions = (int)vco.VolumeCalculationType;
        this.volumeEquationNumberOverride = MarshalString(vco.VolumeEquationNumberOverride);
        this.ecoRegion = MarshalString(vco.EcoRegion);
        this.basalArea = vco.BasalArea;
        this.siteIndex = vco.SiteIndex;
    }

    public IntPtr MarshalString(string? str)
    {
        return string.IsNullOrEmpty(str) ? IntPtr.Zero : Marshal.StringToCoTaskMemUTF8(str);
    }
}
