using System;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetoothFramework
{
    public class UnityCBPeripheral
    {
        [DllImport(UnityCoreBluetooth_bundle.IMPORT_TARGET)]
        private static extern string cbPeripheral_name(IntPtr peripheral);

        [DllImport(UnityCoreBluetooth_bundle.IMPORT_TARGET)]
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
}
#endif
