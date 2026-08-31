using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop;

[StructLayout(LayoutKind.Sequential)]
public struct BiomassOutput_C
{
    public double aboveGroundTotal;
    public double branches;
    public double foliage;
    public double stumpWood;
    public double stumpBark;
    public double stemWoodTotal;
    public double stemBarkTotal;
    public double stemPrimaryWood;
    public double stemPrimaryBark;
    public double stemSecondaryWood;
    public double stemSecondaryBark;
    public double stemTipWood;
    public double stemTipBark;
    public double stemTopAndLimb;
}

[StructLayout(LayoutKind.Sequential)]
public struct LogOutput_C
{
    public int logNumber;
    public int product;
    public int isSecondary;
    public double smallEndDiameterActual;
    public double largeEndDiameterActual;
    public double smallEndDiameterScaled;
    public double largeEndDiameterScaled;
    public double length;
    public double heightToLargeEndDiameter;
    public double grossBoardFoot;
    public double grossCubicFoot;
    public double internationalBoardFoot;
    public double greenWeight;
    public double dryWeight;
}

[StructLayout(LayoutKind.Sequential)]
public struct TreeOutput_C
{
    public double grossBoardFootPrimary;
    public double grossBoardFootSecondary;
    public double grossCubicFootPrimary;
    public double grossCubicFootSecondary;
    public double grossInternationalBoardFoot;
    public double totalCubicFoot;
    public double stumpCubicFoot;
    public double tipCubicFoot;
    public double cordMerchantable;
    public double greenWeightPrimary;
    public double greenWeightSecondary;
    public double dryWeightPrimary;
    public double dryWeightSecondary;
    public BiomassOutput_C greenBio;
    public BiomassOutput_C dryBio;
    public double carbonContent;
    public IntPtr logs;
    public IntPtr logCount;
    public int numberOfLogs;
    public int errflag;
}

[StructLayout(LayoutKind.Sequential)]
public struct TreeMeasurment_C
{
    public double totalHeight;
    public double referenceHeight;
    public double merchHeightSaw;
    public double merchHeightNonsaw;
    public int merchHeightUnit;
    public double heightToFirstLiveLimb;
    public double heightToTopBroken;
    [MarshalAs(UnmanagedType.I1)]
    public bool isLive;
    public double dbh;
    public double drc;
    public double referenceDiameter;
    public double topBrokenDiameter;
    public int formClass;
    public double stumpHeightOverride;
    public double minTopDibSawOverride;
    public double minTopDibNonSawOverride;
    public double cull;
    public int decaycd;
    public double crownRatio;
    public int stems;
}

[StructLayout(LayoutKind.Sequential)]
public struct MerchRules_C
{
    public int evenOdd;
    public int segmentationOption;
    public double maxLogLength;
    public double minLogLength;
    public double minLengthTop;
    public double minTopDibSaw;
    public double minTopDibNonSaw;
    public double minMerchLength;
    public double stumpHeight;
    public double trim;
    public double barkThicknessRatio;
    public double doubleBarkThicknessAtBrestHeight;
    public double minimumBoardFootDiameter;
    [MarshalAs(UnmanagedType.I1)]
    public bool useCorrectedFactor;
}

[StructLayout(LayoutKind.Sequential)]
public struct VolumeCalculationOptions_C
{
    public int fiaCode;
    public int auxFlag;
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
}

[StructLayout(LayoutKind.Sequential)]
public struct ErrorInfo_C
{
    public int errorCode;
    public IntPtr errorMessage;
}

public sealed class FsvolLibrary
{
    private static class NativeMethods
    {
        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool TryCalculateVolume(ref VolumeCalculationOptions_C options,
            ref TreeMeasurment_C tree,
            out IntPtr treeOutput,
            out IntPtr error);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CalculateVolumeWithMerchRules(ref VolumeCalculationOptions_C options,
            ref TreeMeasurment_C tree,
            ref MerchRules_C merchRules,
            out IntPtr treeOutput,
            out IntPtr error);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetVolumeEquationNumber(ref VolumeCalculationOptions_C options,
            IntPtr outBuffer,
            int bufferLen);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetHeightAtDiameter([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
            ref TreeMeasurment_C tree,
            double diameter);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetDiameterAtHeight([MarshalAs(UnmanagedType.LPUTF8Str)] string volEqNumber,
            ref TreeMeasurment_C tree,
            double height);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNumberOfLogs(ref VolumeCalculationOptions_C options,
            ref TreeMeasurment_C tree);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void GetVersion(IntPtr outBuffer, int bufferLen);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_tree_output_c(IntPtr value);

        [DllImport("FSvolLibInterop.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void free_error_info_c(IntPtr value);
    }

    public static string GetVersion()
    {
        byte[] buffer = new byte[256];
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

    public static FsvolResult TryCalculateVolume(VolumeCalculationOptions_C options, TreeMeasurment_C tree)
    {
        var nativeOptions = options;
        var result = new FsvolResult();

        try
        {
            bool succeeded = NativeMethods.TryCalculateVolume(ref nativeOptions, ref tree, out IntPtr treeOutputPtr, out IntPtr errorPtr);

            if (errorPtr != IntPtr.Zero)
            {
                result.Error = Marshal.PtrToStructure<ErrorInfo_C>(errorPtr);
                NativeMethods.free_error_info_c(errorPtr);
            }

            if (treeOutputPtr != IntPtr.Zero)
            {
                var nativeTreeOutput = Marshal.PtrToStructure<TreeOutput_C>(treeOutputPtr);
                result.TreeOutput = new TreeOutputDto
                {
                    grossBoardFootPrimary = nativeTreeOutput.grossBoardFootPrimary,
                    grossBoardFootSecondary = nativeTreeOutput.grossBoardFootSecondary,
                    grossCubicFootPrimary = nativeTreeOutput.grossCubicFootPrimary,
                    grossCubicFootSecondary = nativeTreeOutput.grossCubicFootSecondary,
                    grossInternationalBoardFoot = nativeTreeOutput.grossInternationalBoardFoot,
                    totalCubicFoot = nativeTreeOutput.totalCubicFoot,
                    stumpCubicFoot = nativeTreeOutput.stumpCubicFoot,
                    tipCubicFoot = nativeTreeOutput.tipCubicFoot,
                    cordMerchantable = nativeTreeOutput.cordMerchantable,
                    numberOfLogs = nativeTreeOutput.numberOfLogs,
                    errflag = nativeTreeOutput.errflag,
                    logs = ReadLogs(nativeTreeOutput.logs, nativeTreeOutput.logCount)
                };

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

    public static VolumeCalculationOptions_C CreateVolumeCalculationOptions(int fiaCode, int region, int forest, int district, int primaryProduct, int secondaryProduct, string volumeEquationNumberOverride = "", string ecoRegion = "")
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

    private static LogOutput_C[] ReadLogs(IntPtr logPointer, IntPtr logCount)
    {
        if (logPointer == IntPtr.Zero || logCount == IntPtr.Zero)
        {
            return Array.Empty<LogOutput_C>();
        }

        int count = (int)logCount;
        var logs = new LogOutput_C[count];

        for (int i = 0; i < count; i++)
        {
            IntPtr current = IntPtr.Add(logPointer, i * Marshal.SizeOf<LogOutput_C>());
            logs[i] = Marshal.PtrToStructure<LogOutput_C>(current);
        }

        return logs;
    }
}

public sealed class FsvolResult
{
    public bool Success { get; set; }
    public ErrorInfo_C? Error { get; set; }
    public TreeOutputDto? TreeOutput { get; set; }
}

public sealed class TreeOutputDto
{
    public double grossBoardFootPrimary { get; set; }
    public double grossBoardFootSecondary { get; set; }
    public double grossCubicFootPrimary { get; set; }
    public double grossCubicFootSecondary { get; set; }
    public double grossInternationalBoardFoot { get; set; }
    public double totalCubicFoot { get; set; }
    public double stumpCubicFoot { get; set; }
    public double tipCubicFoot { get; set; }
    public double cordMerchantable { get; set; }
    public int numberOfLogs { get; set; }
    public int errflag { get; set; }
    public LogOutput_C[] logs { get; set; } = Array.Empty<LogOutput_C>();
}
