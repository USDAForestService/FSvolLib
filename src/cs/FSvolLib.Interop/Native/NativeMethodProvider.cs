using FSvolLib.Interop.Native.CModels;
using System.Runtime.InteropServices;

namespace FSvolLib.Interop.Native
{
    public class NativeMethodProvider : INativeMethodProvider
    {
        private const string NativeLibraryName = "FSvolLibInterop";

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool CalculateVolume(ref VolumeCalculationOptions_C options,
                                                            ref TreeMeasurment_C tree,
                                                            out IntPtr treeOutput,
                                                            out IntPtr error);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool CalculateVolumeWithMerchRules(ref VolumeCalculationOptions_C options,
            ref TreeMeasurment_C tree,
            ref MerchRules_C merchRules,
            out IntPtr treeOutput,
            out IntPtr error);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetVolumeEquationNumber(ref VolumeCalculationOptions_C options,
            IntPtr outBuffer,
            int bufferLen);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetHeightAtDiameter([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
            ref TreeMeasurment_C tree,
            double diameter);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern double GetDiameterAtHeight([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
            ref TreeMeasurment_C tree,
            double height);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetNumberOfLogs(ref VolumeCalculationOptions_C options,
            ref TreeMeasurment_C tree);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GetVersion(IntPtr outBuffer, int bufferLen);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_tree_output_c(IntPtr value);

        [DllImport(NativeLibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_error_info_c(IntPtr value);

        public static INativeMethods GetNativeMethods()
        {
            return new NativeMethods(
                CalculateVolume,
                CalculateVolumeWithMerchRules,
                GetVolumeEquationNumber,
                GetHeightAtDiameter,
                GetDiameterAtHeight,
                GetNumberOfLogs,
                GetVersion,
                free_tree_output_c,
                free_error_info_c);
        }

        INativeMethods INativeMethodProvider.GetNativeMethods()
        {
            return GetNativeMethods();
        }
    }
}
