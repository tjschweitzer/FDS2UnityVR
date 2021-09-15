using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pixyz.OptimizeSDK.Native
{
    public static partial class NativeInterface
    {
#if PXZ_CUSTOM_DLL_PATH
        private const string PiXYZOptimizeSDK_dll = "PiXYZOptimizeSDK";
        private const string memcpy_dll = "msvcrt.dll";
#endif
    }
}