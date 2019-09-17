using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCBService {

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern string cbService_uuid(IntPtr service);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void cbService_discoverCharacteristic(IntPtr service);

    private IntPtr nativePtr;
    public UnityCBService(IntPtr ptr)
    {
        this.nativePtr = ptr;
    }

    public string uuid
    {
        get
        {
            return cbService_uuid(nativePtr);
        }
    }

    public void discoverCharacteristics()
    {
        cbService_discoverCharacteristic(nativePtr);
    }


}
