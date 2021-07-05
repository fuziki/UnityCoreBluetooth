using System;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth
{
    public class CoreBluetoothService
    {
        private IntPtr nativePtr;
        public CoreBluetoothService(IntPtr ptr)
        {
            this.nativePtr = ptr;
        }

        private string _uuid = null;
        public string uuid
        {
            get
            {
                if (_uuid == null) _uuid = NativeInterface.UcbService.ucb_service_getUuid(nativePtr);
                return _uuid;
            }
        }

        public void discoverCharacteristics()
        {
            NativeInterface.UcbService.ucb_service_discoverCharacteristic(nativePtr);
        }
    }
}
#endif
