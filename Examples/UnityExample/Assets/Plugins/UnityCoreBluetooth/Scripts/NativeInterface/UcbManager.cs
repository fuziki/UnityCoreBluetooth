using System;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth.NativeInterface
{
    public class UcbManager
    {
        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_instantiate();

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_release();

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_startScan();

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_stopScan();

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_connectWithPeripheral(IntPtr peripheral);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onUpdateState_delegate(string state);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onUpdateState(ucb_manager_shared_onUpdateState_delegate handler);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onDiscoverPeripheral_delegate(IntPtr peripheral);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onDiscoverPeripheral(ucb_manager_shared_onDiscoverPeripheral_delegate handler);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onConnectPeripheral_delegate(IntPtr peripheral);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onConnectPeripheral(ucb_manager_shared_onConnectPeripheral_delegate handler);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onDiscoverService_delegate(IntPtr service);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onDiscoverService(ucb_manager_shared_onDiscoverService_delegate handler);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onDiscoverCharacteristic_delegate(IntPtr characteristic);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onDiscoverCharacteristic(ucb_manager_shared_onDiscoverCharacteristic_delegate handler);

        // -------------------------------------------------------------------------------------------------

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ucb_manager_shared_onUpdateValue_delegate(IntPtr characteristic, IntPtr data, long length);

        [DllImport(ImportConfig.TargetName)]
        public static extern void ucb_manager_shared_register_onUpdateValue(ucb_manager_shared_onUpdateValue_delegate handler);
    }
}
#endif