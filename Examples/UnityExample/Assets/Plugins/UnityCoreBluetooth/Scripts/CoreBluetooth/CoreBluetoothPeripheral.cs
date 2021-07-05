using System;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth
{
    public class CoreBluetoothPeripheral
    {
        public IntPtr nativePtr;
        public CoreBluetoothPeripheral(IntPtr ptr)
        {
            this.nativePtr = ptr;
        }

        private string _name = null;
        public string name
        {
            get
            {
                if (_name == null) _name = NativeInterface.UcbPeripheral.ucb_peripheral_getName(nativePtr);
                return _name;
            }
        }

        public void discoverServices()
        {
            NativeInterface.UcbPeripheral.ucb_peripheral_discoverServicesWithPeripheral(nativePtr);
        }
    }
}
#endif
