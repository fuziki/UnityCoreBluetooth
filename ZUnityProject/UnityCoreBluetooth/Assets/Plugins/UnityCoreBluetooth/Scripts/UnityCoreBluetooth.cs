using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCoreBluetooth
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnUpdateStateHandler(string state);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnDiscoverPeripheralHandler(IntPtr peripheral);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnConnectPeripheralHandler(IntPtr peripheral);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnDiscoverServiceHandler(IntPtr service);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnDiscoverCharacteristicHandler(IntPtr characteristic);



    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onUpdateState(IntPtr unityCoreBluetooth, OnUpdateStateHandler handler);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onDiscoverPeripheral(IntPtr unityCoreBluetooth, OnDiscoverPeripheralHandler handler);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onConnectPeripheral(IntPtr unityCoreBluetooth, OnConnectPeripheralHandler handler);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onDiscoverService(IntPtr unityCoreBluetooth, OnDiscoverServiceHandler handler);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onDiscoverCharacteristic(IntPtr unityCoreBluetooth, OnDiscoverCharacteristicHandler handler);



    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void unityCoreBluetooth_onUpdateValue_delegate(IntPtr characteristic, IntPtr data, long length);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_onUpdateValue(IntPtr unityCoreBluetooth, unityCoreBluetooth_onUpdateValue_delegate handler);

    private static event Action<UnityCBCharacteristic, byte[]> onUpdateValueHandler;

    [MonoPInvokeCallback(typeof(unityCoreBluetooth_onUpdateValue_delegate))]
    private static void staticOnUpdateValue(IntPtr characteristic, IntPtr data, long length)
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



    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern IntPtr unityCoreBluetooth_init();

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_release(IntPtr unityCoreBluetooth);


    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_startCoreBluetooth(IntPtr unityCoreBluetooth);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_startScan(IntPtr unityCoreBluetooth);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_stopScan(IntPtr unityCoreBluetooth);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_connect(IntPtr unityCoreBluetooth, IntPtr peripheral);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void unityCoreBluetooth_clearPeripherals(IntPtr unityCoreBluetooth);

    private IntPtr nativePtr;


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

    private UnityCoreBluetooth()
    {
        nativePtr = unityCoreBluetooth_init();
        unityCoreBluetooth_onUpdateValue(nativePtr, staticOnUpdateValue);
    }

    ~UnityCoreBluetooth()
    {
        UnityCoreBluetooth.onUpdateValueHandler = null;
        Debug.Log("UnityCoreBluetooth deinit");
        unityCoreBluetooth_stopScan(nativePtr);
        unityCoreBluetooth_release(nativePtr);
    }


    public void OnUpdateState(OnUpdateStateHandler handler)
    {
        unityCoreBluetooth_onUpdateState(nativePtr, handler);
    }

    public void OnDiscoverPeripheral(OnDiscoverPeripheralHandler handler)
    {
        unityCoreBluetooth_onDiscoverPeripheral(nativePtr, handler);
    }

    public void OnConnectPeripheral(OnConnectPeripheralHandler handler)
    {
        unityCoreBluetooth_onConnectPeripheral(nativePtr, handler);
    }

    public void OnDiscoverService(OnDiscoverServiceHandler handler)
    {
        unityCoreBluetooth_onDiscoverService(nativePtr, handler);
    }

    public void OnDiscoverCharacteristic(OnDiscoverCharacteristicHandler handler) 
    {
        unityCoreBluetooth_onDiscoverCharacteristic(nativePtr, handler);
    }

    //public void OnUpdateValue(OnUpdateValueHandler handler)
    //{
    //    unityCoreBluetooth_onUpdateValue(nativePtr, handler);
    //}


//    public delegate void OnUpdateValueHandler2(IntPtr characteristic, IntPtr data, long length);
    //private static event Action<UnityCBCharacteristic, byte[]> OnUpdateValueHandler2Event;

    //[MonoPInvokeCallback(typeof(OnUpdateValueHandler))]
    //private static void staticOnUpdateValue(IntPtr characteristic, IntPtr data, long length)
    //{
    //    UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
    //    byte[] result = new byte[length];
    //    Marshal.Copy(data, result, 0, (int)length);
    //    UnityCoreBluetooth.OnUpdateValueHandler2Event(c, result);
    //}

    //public void OnUpdateValue(Action<UnityCBCharacteristic, byte[]> handler)
    //{
    //    UnityCoreBluetooth.OnUpdateValueHandler2Event = handler;
    //    //UnityCoreBluetooth.OnUpdateValueHandler2Event += delegate (IntPtr characteristic, IntPtr data, long length)
    //    //{
    //    //    handler(characteristic, data, length);
    //    //};
    //    //unityCoreBluetooth_onUpdateValue(nativePtr, handler);
    //}


    public void StartCoreBluetooth()
    {
        unityCoreBluetooth_startCoreBluetooth(nativePtr);
    }

    public void StartScan()
    {
        unityCoreBluetooth_startScan(nativePtr);
    }

    public void StopScan()
    {
        unityCoreBluetooth_stopScan(nativePtr);
    }

    public void Connect(UnityCBPeripheral peripheral)
    {
        unityCoreBluetooth_connect(nativePtr, peripheral.nativePtr);
    }

    public void ClearPeripherals()
    {
        unityCoreBluetooth_clearPeripherals(nativePtr);
    }


/*    // Use this for initialization
    void Start()
    {
        unityCoreBluetooth = unityCoreBluetooth_init();

        unityCoreBluetooth_onUpdateState(unityCoreBluetooth, (string state) => {
            Debug.Log("state: " + state);
            if (state != "poweredOn") return;
            if (unityCoreBluetooth == null)
            {
                Debug.Log("unityCoreBluetooth is null");
                return;
            }
            unityCoreBluetooth_startScan(unityCoreBluetooth);
        });

        unityCoreBluetooth_onDiscoverPeripheral(unityCoreBluetooth, (IntPtr peripheral) => {
            string n = cbPeripheral_name(peripheral);
            Debug.Log("discover peripheral name: " + n);

            if (n != "Daydream controller") return;

            unityCoreBluetooth_stopScan(unityCoreBluetooth);

            unityCoreBluetooth_connect(unityCoreBluetooth, peripheral);
        });

        unityCoreBluetooth_onConnectPeripheral(unityCoreBluetooth, (IntPtr peripheral) => {
            string n = cbPeripheral_name(peripheral);
            Debug.Log("connected peripheral name: " + n);
            cbPeripheral_discoverServices(peripheral);
        });


        unityCoreBluetooth_onDiscoverService(unityCoreBluetooth, (IntPtr service) => {
            string uuid = cbService_uuid(service);
            Debug.Log("discover service uuid: " + uuid);
            if (uuid != "FE55") return;
            cbService_discoverCharacteristic(service);
        });


        
        unityCoreBluetooth_onDiscoverCharacteristic(unityCoreBluetooth, (IntPtr characteristic) => {
            string uuid = cbCharacteristic_uuid(characteristic);
            string usage = cbCharacteristic_propertyString(characteristic);
            Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
            if (usage != "notify") return;
            cbCharacteristic_setNotifyValue(characteristic, true);
        });


        unityCoreBluetooth_onUpdateValue(unityCoreBluetooth, (IntPtr characteristic, IntPtr data, long length) => {
            string uuid = cbCharacteristic_uuid(characteristic);
            byte[] result = new byte[length];
            Marshal.Copy(data, result, 0, (int)length);
            Debug.Log("update characteristic uuid: " + uuid + ", data: " + BitConverter.ToString(result));
        });



        unityCoreBluetooth_startCoreBluetooth(unityCoreBluetooth);
        Debug.Log("start CoreBluetoothUnityMacOS");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        unityCoreBluetooth_stopScan(unityCoreBluetooth);
        unityCoreBluetooth_release(unityCoreBluetooth);
    }*/
}
