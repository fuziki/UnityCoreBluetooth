using System;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetooth
{
    public class CoreBluetoothCharacteristic
    {
        private IntPtr nativePtr;
        public CoreBluetoothCharacteristic(IntPtr ptr)
        {
            this.nativePtr = ptr;
        }

        private string _uuid = null;
        public string uuid
        {
            get
            {
                if (_uuid == null) _uuid = NativeInterface.UcbCharacteristic.ucb_characteristic_getUuid(nativePtr);
                return _uuid;
            }
        }

        private string[] _propertis = null;
        public string[] propertis
        {
            get
            {
                if (_propertis == null) _propertis = new string[] { NativeInterface.UcbCharacteristic.ucb_characteristic_getPropertis(nativePtr) };
                return _propertis;
            }
        }

        public void setNotifyValue(bool enable)
        {
            NativeInterface.UcbCharacteristic.ucb_characteristic_setNotify(nativePtr, enable);
        }
    }
}
#endif
