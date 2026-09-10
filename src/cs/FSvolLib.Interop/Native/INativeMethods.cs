
namespace FSvolLib.Interop.Native
{
    public interface INativeMethods
    {
        NativeDelegateTypes.CalculateVolume CalculateVolume { get; }

        NativeDelegateTypes.CalculateVolumeWithMerchRules CalculateVolumeWithMerchRules { get; }

        NativeDelegateTypes.GetVolumeEquationNumber GetVolumeEquationNumber { get; }

        NativeDelegateTypes.GetHeightAtDiameter GetHeightAtDiameter { get; }

        NativeDelegateTypes.GetDiameterAtHeight GetDiameterAtHeight { get; }

        NativeDelegateTypes.GetNumberOfLogs GetNumberOfLogs { get; }

        NativeDelegateTypes.GetVersion GetVersion { get; }

        NativeDelegateTypes.free_tree_output_c free_tree_output_c { get; }

        NativeDelegateTypes.free_error_info_c free_error_info_c { get; }
    }
}
