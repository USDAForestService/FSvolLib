using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct VolumeCalculationOptions_C
{
    public int fiaCode = 0;
    public byte auxFlag = byte.MinValue;
    public int region = 0;
    public int forest = 0;
    public int district = 0;
    public int primaryProduct = 0;
    public int secondaryProduct = 0;
    public int volumeCalculationOptions = 0;
    public IntPtr volumeEquationNumberOverride = IntPtr.Zero;
    public IntPtr ecoRegion = IntPtr.Zero;
    public int basalArea = 0;
    public int siteIndex = 0;

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
        this.volumeEquationNumberOverride = vco.VolumeEquationNumberOverride.MarshalString();
        this.ecoRegion = vco.EcoRegion.MarshalString();
        this.basalArea = vco.BasalArea;
        this.siteIndex = vco.SiteIndex;
    }

    
}
