using FSvolLib.Interop.Native.CModels;
using System.Runtime.InteropServices;

namespace FSvolLib.Interop.Native;

public class NativeMethods : INativeMethods
{
    public NativeMethods(NativeDelegateTypes.CalculateVolume calculateVolume, NativeDelegateTypes.CalculateVolumeWithMerchRules calculateVolumeWithMerchRules, NativeDelegateTypes.GetVolumeEquationNumber getVolumeEquationNumber, NativeDelegateTypes.GetHeightAtDiameter getHeightAtDiameter, NativeDelegateTypes.GetDiameterAtHeight getDiameterAtHeight, NativeDelegateTypes.GetNumberOfLogs getNumberOfLogs, NativeDelegateTypes.GetVersion getVersion, NativeDelegateTypes.free_tree_output_c free_tree_output_c, NativeDelegateTypes.free_error_info_c free_error_info_c)
    {
        CalculateVolume = calculateVolume;
        CalculateVolumeWithMerchRules = calculateVolumeWithMerchRules;
        GetVolumeEquationNumber = getVolumeEquationNumber;
        GetHeightAtDiameter = getHeightAtDiameter;
        GetDiameterAtHeight = getDiameterAtHeight;
        GetNumberOfLogs = getNumberOfLogs;
        GetVersion = getVersion;
        this.free_tree_output_c = free_tree_output_c;
        this.free_error_info_c = free_error_info_c;
    }

    public NativeDelegateTypes.CalculateVolume CalculateVolume { get; }

    public NativeDelegateTypes.CalculateVolumeWithMerchRules CalculateVolumeWithMerchRules { get; }

    public NativeDelegateTypes.GetVolumeEquationNumber GetVolumeEquationNumber { get; }

    public NativeDelegateTypes.GetHeightAtDiameter GetHeightAtDiameter { get; }

    public NativeDelegateTypes.GetDiameterAtHeight GetDiameterAtHeight { get; }

    public NativeDelegateTypes.GetNumberOfLogs GetNumberOfLogs { get; }

    public NativeDelegateTypes.GetVersion GetVersion { get; }

    public NativeDelegateTypes.free_tree_output_c free_tree_output_c { get; }

    public NativeDelegateTypes.free_error_info_c free_error_info_c { get; }
}
