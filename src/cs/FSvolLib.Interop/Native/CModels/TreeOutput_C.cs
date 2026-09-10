using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

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

    public static LogOutput_C[] ReadLogs(IntPtr logPointer, IntPtr logCount)
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
