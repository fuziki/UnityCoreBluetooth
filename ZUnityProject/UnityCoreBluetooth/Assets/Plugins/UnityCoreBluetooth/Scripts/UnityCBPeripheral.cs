using System;
using System.Runtime.InteropServices;

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

    private string _name = null;
    public string name
    {
        get
        {
            if (_name == null) _name = cbPeripheral_name(nativePtr);
            return _name;
        }
    }

    public void discoverServices()
    {
        cbPeripheral_discoverServices(nativePtr);
    }
}
