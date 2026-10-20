using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FSvolLib.Interop.Native;

internal static class StringExtentions
{
    public static IntPtr MarshalString(this string? str)
    {
        if (String.IsNullOrEmpty(str)) return IntPtr.Zero;

        // Get the exact number of bytes needed for UTF-8 + null-terminator
        int byteCount = Encoding.UTF8.GetByteCount(str);
        IntPtr ptr = Marshal.AllocCoTaskMem(byteCount + 1);

        try
        {
            // Encode the string directly into the allocated native memory
            unsafe
            {
                byte* pByte = (byte*)ptr.ToPointer();
                fixed (char* pChar = str)
                {
                    Encoding.UTF8.GetBytes(pChar, str.Length, pByte, byteCount);
                }
                pByte[byteCount] = 0; // Add null-terminator
            }
            return ptr;
        }
        catch
        {
            Marshal.FreeCoTaskMem(ptr);
            throw;
        }

        // simpler option but only avalible in net core
        //return string.IsNullOrEmpty(str) ? IntPtr.Zero : Marshal.StringToCoTaskMemUTF8(str);
    }
}
