using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixyz.ImportSDK.Native
{
    public static partial class NativeInterface
    {
#if PXZ_CUSTOM_DLL_PATH
        private const string PiXYZImportSDK_dll = "PiXYZImportSDK";
        private const string memcpy_dll = "msvcrt.dll";
#endif
    }
}