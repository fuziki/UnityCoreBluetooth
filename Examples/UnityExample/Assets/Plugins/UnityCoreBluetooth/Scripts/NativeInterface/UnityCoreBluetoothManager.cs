using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth.NativeInterface
{
    public class UnityCoreBluetoothManagerWrapper
    {
        // OnUpdateState ----------------------------------------------------------------------------------
        public Action<string> OnUpdateStateHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onUpdateState_delegate))]
        private static void OnUpdateStateCallback(string state)
        {
            Shared.OnUpdateStateHandler(state);
        }

        // OnDiscoverPeripheral ----------------------------------------------------------------------------
        public Action<IntPtr> OnDiscoverPeripheralHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onDiscoverPeripheral_delegate))]
        private static void OnDiscoverPeripheralCallback(IntPtr peripheral)
        {
            if (Shared.OnDiscoverPeripheralHandler == null) return;
            Shared.OnDiscoverPeripheralHandler(peripheral);
        }

        // OnConnectPeripheral -----------------------------------------------------------------------------
        public Action<IntPtr> OnConnectPeripheralHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onConnectPeripheral_delegate))]
        private static void OnConnectPeripheralCallback(IntPtr peripheral)
        {
            if (Shared.OnConnectPeripheralHandler == null) return;
            Shared.OnConnectPeripheralHandler(peripheral);
        }

        // OnDiscoverService ------------------------------------------------------------------------------
        public Action<IntPtr> OnDiscoverServiceHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onDiscoverService_delegate))]
        private static void OnDiscoverServiceCallback(IntPtr service)
        {
            Shared.OnDiscoverServiceHandler(service);
        }

        // OnDiscoverCharacteristic -----------------------------------------------------------------------
        public Action<IntPtr> OnDiscoverCharacteristicHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onUpdateState_delegate))]
        private static void OnDiscoverCharacteristicCallback(IntPtr characteristic)
        {
            Shared.OnDiscoverCharacteristicHandler(characteristic);
        }

        // OnUpdateValue ----------------------------------------------------------------------------------
        public Action<IntPtr, byte[]> OnUpdateValueHandler;

        [MonoPInvokeCallback(typeof(UcbManager.ucb_manager_shared_onUpdateState_delegate))]
        private static void OnUpdateValueCallback(IntPtr characteristic, IntPtr data, long length)
        {
            var c = new CoreBluetoothCharacteristic(characteristic);
            byte[] result = new byte[length];
            Marshal.Copy(data, result, 0, (int)length);

            Shared.OnUpdateValueHandler(characteristic, result);
        }

        private UnityCoreBluetoothManagerWrapper()
        {
            Debug.Log("UnityCoreBluetoothManager init");
            UcbManager.ucb_manager_shared_register_onUpdateState(OnUpdateStateCallback);
            UcbManager.ucb_manager_shared_register_onDiscoverPeripheral(OnDiscoverPeripheralCallback);
            UcbManager.ucb_manager_shared_register_onConnectPeripheral(OnConnectPeripheralCallback);
            UcbManager.ucb_manager_shared_register_onDiscoverService(OnDiscoverServiceCallback);
            UcbManager.ucb_manager_shared_register_onDiscoverCharacteristic(OnDiscoverCharacteristicCallback);
            UcbManager.ucb_manager_shared_register_onUpdateValue(OnUpdateValueCallback);
        }

        ~UnityCoreBluetoothManagerWrapper()
        {
            OnUpdateStateHandler = null;
            OnDiscoverPeripheralHandler = null;
            OnConnectPeripheralHandler = null;
            OnDiscoverServiceHandler = null;
            OnDiscoverCharacteristicHandler = null;
            OnUpdateValueHandler = null;
            Debug.Log("UnityCoreBluetoothManager deinit");
        }

        private static UnityCoreBluetoothManagerWrapper _shared;
        public static UnityCoreBluetoothManagerWrapper Shared
        {
            get
            {
                if (_shared == null) _shared = new UnityCoreBluetoothManagerWrapper();
                return _shared;
            }
        }

        public void Start()
        {
            UcbManager.ucb_manager_shared_instantiate();
        }

        public void Stop()
        {
            UcbManager.ucb_manager_shared_release();
        }

        public void StartScan()
        {
            UcbManager.ucb_manager_shared_startScan();
        }

        public void StopScan()
        {
            UcbManager.ucb_manager_shared_stopScan();
        }

        public void ConnectToPeripheral(IntPtr peripheral)
        {
            UcbManager.ucb_manager_shared_connectWithPeripheral(peripheral);
        }
    }
}
#endif
