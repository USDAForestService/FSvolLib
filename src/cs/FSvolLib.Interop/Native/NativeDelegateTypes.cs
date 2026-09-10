using FSvolLib.Interop.Native.CModels;
using System.Runtime.InteropServices;

namespace FSvolLib.Interop.Native;

public class NativeDelegateTypes
{
    public delegate bool CalculateVolume(ref VolumeCalculationOptions_C options,
                                            ref TreeMeasurment_C tree,
                                            out IntPtr treeOutput,
                                            out IntPtr error);

    public delegate bool CalculateVolumeWithMerchRules(ref VolumeCalculationOptions_C options,
                                            ref TreeMeasurment_C tree,
                                            ref MerchRules_C merchRules,
                                            out IntPtr treeOutput,
                                            out IntPtr error);

    public delegate void GetVolumeEquationNumber(ref VolumeCalculationOptions_C options,
                                            IntPtr outBuffer,
                                            int bufferLen);

    public delegate double GetHeightAtDiameter([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
                                            ref TreeMeasurment_C tree,
                                            double diameter);

    public delegate double GetDiameterAtHeight([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
                                            ref TreeMeasurment_C tree,
                                            double height);

    public delegate int GetNumberOfLogs(ref VolumeCalculationOptions_C options,
                                            ref TreeMeasurment_C tree);

    public delegate void GetVersion(IntPtr outBuffer, int bufferLen);

    public delegate void free_tree_output_c(IntPtr value);

    public delegate void free_error_info_c(IntPtr value);
}
