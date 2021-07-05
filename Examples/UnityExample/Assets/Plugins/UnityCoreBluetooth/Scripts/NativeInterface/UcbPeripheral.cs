using System;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth.NativeInterface
{
    public class UcbPeripheral
    {
        [DllImport(ImportConfig.TargetName)]
        public static extern string ucb_peripheral_getName(IntPtr peripheral);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_peripheral_discoverServicesWithPeripheral(IntPtr peripheral);
    }
}
#endif