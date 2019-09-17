using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCBPeripheral {

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern string cbPeripheral_name(IntPtr peripheral);

    [DllImport("UnityCoreBluetoothMacOS")]
    private static extern void cbPeripheral_discoverServices(IntPtr peripheral);

    public IntPtr nativePtr;
    public UnityCBPeripheral(IntPtr ptr)
    {
        this.nativePtr = ptr;
    }

    public string name
    {
        get
        {
            return cbPeripheral_name(nativePtr);
        }
    }

    public void discoverServices()
    {
        cbPeripheral_discoverServices(nativePtr);
    }
}
