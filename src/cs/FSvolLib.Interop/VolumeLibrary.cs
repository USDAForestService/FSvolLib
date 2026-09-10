using FSvolLib.Interop.Native;
using FSvolLib.Interop.Native.CModels;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop;


public static class VolumeLibrary
{
    static INativeMethods NativeMethods { get; } = NativeMethodProvider.GetNativeMethods();
    public static string GetVersion()
    {
        byte[] buffer = new byte[16];
        unsafe
        {
            fixed (byte* p = buffer)
            {
                NativeMethods.GetVersion((IntPtr)p, buffer.Length);
            }
        }

        int nullIndex = Array.IndexOf(buffer, (byte)0);
        if (nullIndex < 0)
        {
            nullIndex = buffer.Length;
        }

        return Encoding.UTF8.GetString(buffer, 0, nullIndex);
    }

    public static FsvolResult CalculateVolume(VolumeCalculationOptions options, TreeMeasurment treeMeasurment)
    {
        var optionsC = new VolumeCalculationOptions_C(options);
        var treeMeasurmentC = new TreeMeasurment_C(treeMeasurment);

        return CalculateVolume(optionsC, treeMeasurmentC);
    }

    public static string GetVolumeEquationNumber(VolumeCalculationOptions options)
    {
        var optionsC = new VolumeCalculationOptions_C(options);

        byte[] buffer = new byte[16];

        unsafe
        {
            fixed (byte* p = buffer)
            {
                NativeMethods.GetVolumeEquationNumber(ref optionsC, (IntPtr)p, buffer.Length);
            }
        }

        int nullIndex = Array.IndexOf(buffer, (byte)0);
        if (nullIndex < 0)
        {
            nullIndex = buffer.Length;
        }

        return Encoding.UTF8.GetString(buffer, 0, nullIndex);
    }


    private static FsvolResult CalculateVolume(VolumeCalculationOptions_C options, TreeMeasurment_C tree)
    {
        var nativeOptions = options;
        var result = new FsvolResult();

        try
        {
            bool succeeded = NativeMethods.CalculateVolume(ref nativeOptions, ref tree, out IntPtr treeOutputPtr, out IntPtr errorPtr);

            if (errorPtr != IntPtr.Zero)
            {
                var error = Marshal.PtrToStructure<ErrorInfo_C>(errorPtr);
                result.ErrorMessage = error.errorMessage;

                NativeMethods.free_error_info_c(errorPtr);
            }

            if (treeOutputPtr != IntPtr.Zero)
            {
                var nativeTreeOutput = Marshal.PtrToStructure<TreeOutput_C>(treeOutputPtr);
                result.TreeOutput = new TreeOutput(nativeTreeOutput);

                NativeMethods.free_tree_output_c(treeOutputPtr);
            }

            result.Success = succeeded;
            return result;
        }
        finally
        {
            if (nativeOptions.volumeEquationNumberOverride != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(nativeOptions.volumeEquationNumberOverride);
            }

            if (nativeOptions.ecoRegion != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(nativeOptions.ecoRegion);
            }
        }
    }




    private static VolumeCalculationOptions_C CreateVolumeCalculationOptions(int fiaCode, int region, int forest, int district, int primaryProduct, int secondaryProduct, string volumeEquationNumberOverride = "", string ecoRegion = "")
    {
        var options = new VolumeCalculationOptions_C
        {
            fiaCode = fiaCode,
            region = region,
            forest = forest,
            district = district,
            primaryProduct = primaryProduct,
            secondaryProduct = secondaryProduct,
            volumeCalculationOptions = 0,
            auxFlag = 0,
            volumeEquationNumberOverride = string.IsNullOrEmpty(volumeEquationNumberOverride) ? IntPtr.Zero : Marshal.StringToCoTaskMemUTF8(volumeEquationNumberOverride),
            ecoRegion = string.IsNullOrEmpty(ecoRegion) ? IntPtr.Zero : Marshal.StringToCoTaskMemUTF8(ecoRegion)
        };

        return options;
    }

    public static TreeMeasurment_C CreateTreeMeasurement(double dbh, double totalHeight)
    {
        return new TreeMeasurment_C
        {
            dbh = dbh,
            totalHeight = totalHeight,
            merchHeightUnit = 0,
            isLive = true,
            formClass = 80,
            stems = 1,
            referenceHeight = 0.0,
            merchHeightSaw = 0.0,
            merchHeightNonsaw = 0.0
        };
    }

}

public sealed class FsvolResult
{
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public TreeOutput? TreeOutput { get; set; }
}

