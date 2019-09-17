using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCBCharacteristic {

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern string cbCharacteristic_uuid(IntPtr characteristic);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern string cbCharacteristic_propertyString(IntPtr characteristic);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void cbCharacteristic_setNotifyValue(IntPtr characteristic, bool enable);


    private IntPtr nativePtr;
    public UnityCBCharacteristic(IntPtr ptr)
    {
        this.nativePtr = ptr;
    }

    public string uuid
    {
        get
        {
            return cbCharacteristic_uuid(nativePtr);
        }
    }

    public string[] propertis
    {
        get
        {
            return new string[] { cbCharacteristic_propertyString(nativePtr) };
        }
    }

    public void setNotifyValue(bool enable)
    {
        cbCharacteristic_setNotifyValue(nativePtr, enable);
    }

}
