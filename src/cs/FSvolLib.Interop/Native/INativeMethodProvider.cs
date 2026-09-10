using System;
using System.Collections.Generic;
using System.Text;

namespace FSvolLib.Interop.Native;

public interface INativeMethodProvider
{
    INativeMethods GetNativeMethods();
}
