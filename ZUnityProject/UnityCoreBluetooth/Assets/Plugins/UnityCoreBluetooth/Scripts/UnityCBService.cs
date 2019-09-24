using System;
using System.Runtime.InteropServices;

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

    private string _uuid = null;
    public string uuid
    {
        get
        {
            if (_uuid == null) _uuid = cbService_uuid(nativePtr);
            return _uuid;
        }
    }

    public void discoverCharacteristics()
    {
        cbService_discoverCharacteristic(nativePtr);
    }


}
