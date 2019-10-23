using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

#if UNITY_EDITOR_OSX || UNITY_IOS

namespace UnityCoreBluetoothFramework
{
    public class LIBRARY
    {
#if UNITY_EDITOR_OSX
        public const string NAME = "UnityCoreBluetoothMacOS";
#elif UNITY_IOS
        public const string NAME = "__Internal";
#endif
    }
    public class UnityCoreBluetooth
    {
        //unityCoreBluetooth_onUpdateState ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onUpdateState_delegate(string state);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onUpdateState(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onUpdateState_delegate handler);

        private static event Action<string> onUpdateStateHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onUpdateValue_delegate))]
        private static void onUpdateStateCallback(string state)
        {
            UnityCoreBluetooth.onUpdateStateHandler(state);
        }

        public void OnUpdateState(Action<string> handler)
        {
            UnityCoreBluetooth.onUpdateStateHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_onDiscoverPeripheral ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onDiscoverPeripheral_delegate(IntPtr peripheral);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onDiscoverPeripheral(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onDiscoverPeripheral_delegate handler);

        private static event Action<UnityCBPeripheral> onDiscoverPeripheralHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onDiscoverPeripheral_delegate))]
        private static void onDiscoverPeripheralCallback(IntPtr peripheral)
        {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            UnityCoreBluetooth.onDiscoverPeripheralHandler(p);
        }

        public void OnDiscoverPeripheral(Action<UnityCBPeripheral> handler)
        {
            UnityCoreBluetooth.onDiscoverPeripheralHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_onConnectPeripheral ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onConnectPeripheral_delegate(IntPtr peripheral);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onConnectPeripheral(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onConnectPeripheral_delegate handler);

        private static event Action<UnityCBPeripheral> onConnectPeripheralHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onConnectPeripheral_delegate))]
        private static void onConnectPeripheralCallback(IntPtr peripheral)
        {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            UnityCoreBluetooth.onConnectPeripheralHandler(p);
        }

        public void OnConnectPeripheral(Action<UnityCBPeripheral> handler)
        {
            UnityCoreBluetooth.onConnectPeripheralHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_onDiscoverService ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onDiscoverService_delegate(IntPtr service);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onDiscoverService(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onDiscoverService_delegate handler);

        private static event Action<UnityCBService> onDiscoverServiceHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onDiscoverService_delegate))]
        private static void onDiscoverServiceCallback(IntPtr service)
        {
            UnityCBService s = new UnityCBService(service);
            UnityCoreBluetooth.onDiscoverServiceHandler(s);
        }

        public void OnDiscoverService(Action<UnityCBService> handler)
        {
            UnityCoreBluetooth.onDiscoverServiceHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_onDiscoverCharacteristic ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onDiscoverCharacteristic_delegate(IntPtr characteristic);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onDiscoverCharacteristic(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onDiscoverCharacteristic_delegate handler);

        private static event Action<UnityCBCharacteristic> onDiscoverCharacteristicHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onDiscoverCharacteristic_delegate))]
        private static void onDiscoverCharacteristicCallback(IntPtr characteristic)
        {
            UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
            UnityCoreBluetooth.onDiscoverCharacteristicHandler(c);
        }

        public void OnDiscoverCharacteristic(Action<UnityCBCharacteristic> handler)
        {
            UnityCoreBluetooth.onDiscoverCharacteristicHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_onUpdateValue ----------------------------------------------------------------------------------
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void unityCoreBluetooth_onUpdateValue_delegate(IntPtr characteristic, IntPtr data, long length);

        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_onUpdateValue(IntPtr unityCoreBluetooth,
            unityCoreBluetooth_onUpdateValue_delegate handler);

        private static event Action<UnityCBCharacteristic, byte[]> onUpdateValueHandler;

        [MonoPInvokeCallback(typeof(unityCoreBluetooth_onUpdateValue_delegate))]
        private static void onUpdateValueCallback(IntPtr characteristic, IntPtr data, long length)
        {
            UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
            byte[] result = new byte[length];
            Marshal.Copy(data, result, 0, (int)length);
            UnityCoreBluetooth.onUpdateValueHandler(c, result);
        }

        public void OnUpdateValue(Action<UnityCBCharacteristic, byte[]> handler)
        {
            UnityCoreBluetooth.onUpdateValueHandler = handler;
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_startCoreBluetooth ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_startCoreBluetooth(IntPtr unityCoreBluetooth);
        public void StartCoreBluetooth()
        {
            unityCoreBluetooth_startCoreBluetooth(nativePtr);
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_startScan ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_startScan(IntPtr unityCoreBluetooth);
        public void StartScan()
        {
            unityCoreBluetooth_startScan(nativePtr);
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_stopScan ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_stopScan(IntPtr unityCoreBluetooth);
        public void StopScan()
        {
            unityCoreBluetooth_stopScan(nativePtr);
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_connect ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_connect(IntPtr unityCoreBluetooth, IntPtr peripheral);
        public void Connect(UnityCBPeripheral peripheral)
        {
            unityCoreBluetooth_connect(nativePtr, peripheral.nativePtr);
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_clearPeripherals ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_clearPeripherals(IntPtr unityCoreBluetooth);
        public void ClearPeripherals()
        {
            unityCoreBluetooth_clearPeripherals(nativePtr);
        }
        //--------------------------------------------------------------------------------------------------------------------

        // native pointer
        private IntPtr nativePtr;
        //

        //unityCoreBluetooth_init ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern IntPtr unityCoreBluetooth_init();

        private UnityCoreBluetooth()
        {
            nativePtr = unityCoreBluetooth_init();
            unityCoreBluetooth_onUpdateState(nativePtr, UnityCoreBluetooth.onUpdateStateCallback);
            unityCoreBluetooth_onDiscoverPeripheral(nativePtr, UnityCoreBluetooth.onDiscoverPeripheralCallback);
            unityCoreBluetooth_onConnectPeripheral(nativePtr, UnityCoreBluetooth.onConnectPeripheralCallback);
            unityCoreBluetooth_onDiscoverService(nativePtr, UnityCoreBluetooth.onDiscoverServiceCallback);
            unityCoreBluetooth_onDiscoverCharacteristic(nativePtr, UnityCoreBluetooth.onDiscoverCharacteristicCallback);
            unityCoreBluetooth_onUpdateValue(nativePtr, UnityCoreBluetooth.onUpdateValueCallback);
            Debug.Log("UnityCoreBluetooth init");
        }
        //--------------------------------------------------------------------------------------------------------------------

        //unityCoreBluetooth_release ----------------------------------------------------------------------------------
        [DllImport(LIBRARY.NAME)]
        private static extern void unityCoreBluetooth_release(IntPtr unityCoreBluetooth);

        ~UnityCoreBluetooth()
        {
            UnityCoreBluetooth.onUpdateStateHandler = null;
            UnityCoreBluetooth.onDiscoverPeripheralHandler = null;
            UnityCoreBluetooth.onConnectPeripheralHandler = null;
            UnityCoreBluetooth.onDiscoverServiceHandler = null;
            UnityCoreBluetooth.onDiscoverCharacteristicHandler = null;
            UnityCoreBluetooth.onUpdateValueHandler = null;
            unityCoreBluetooth_stopScan(nativePtr);
            unityCoreBluetooth_release(nativePtr);
            Debug.Log("UnityCoreBluetooth deinit");
        }
        //--------------------------------------------------------------------------------------------------------------------

        //C# Singleton Shared Instance ----------------------------------------------------------------------------------
        private static UnityCoreBluetooth _shared;
        public static UnityCoreBluetooth Shared
        {
            get
            {
                if (_shared == null) _shared = new UnityCoreBluetooth();
                return _shared;
            }
        }

        public static void CreateSharedInstance()
        {
            _shared = new UnityCoreBluetooth();
        }

        public static void ReleaseSharedInstance()
        {
            _shared = null;
        }
        //--------------------------------------------------------------------------------------------------------------------
    }
}
#endif
