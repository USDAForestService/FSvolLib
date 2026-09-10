using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace FSvolLib.Interop.Native.CModels;

[StructLayout(LayoutKind.Sequential)]
public struct ErrorInfo_C
{
    public int errorCode;
    [MarshalAs(UnmanagedType.LPStr)]
    public string errorMessage;
}
