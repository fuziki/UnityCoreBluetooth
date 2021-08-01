using System;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth.NativeInterface
{
    public class UcbCharacteristic
    {
        [DllImport(ImportConfig.TargetName)]
        public static extern string ucb_characteristic_getUuid(IntPtr characteristic);

        [DllImport(ImportConfig.TargetName)]
        public static extern string ucb_characteristic_getPropertis(IntPtr characteristic);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_characteristic_setNotify(IntPtr characteristic, bool enable);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_characteristic_write(IntPtr characteristic, byte[] value, long len);
    }
}
#endif