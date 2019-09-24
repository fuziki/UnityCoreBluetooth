using System;
using System.Runtime.InteropServices;

#if UNITY_EDITOR_OSX || UNITY_IOS
namespace UnityCoreBluetoothFramework
{
    public class UnityCBCharacteristic
    {
        [DllImport(LIBRARY.NAME)]
        private static extern string cbCharacteristic_uuid(IntPtr characteristic);

        [DllImport(LIBRARY.NAME)]
        private static extern string cbCharacteristic_propertyString(IntPtr characteristic);

        [DllImport(LIBRARY.NAME)]
        private static extern void cbCharacteristic_setNotifyValue(IntPtr characteristic, bool enable);

        private IntPtr nativePtr;
        public UnityCBCharacteristic(IntPtr ptr)
        {
            this.nativePtr = ptr;
        }

        private string _uuid = null;
        public string uuid
        {
            get
            {
                if (_uuid == null) _uuid = cbCharacteristic_uuid(nativePtr);
                return _uuid;
            }
        }

        private string[] _propertis = null;
        public string[] propertis
        {
            get
            {
                if (_propertis == null) _propertis = new string[] { cbCharacteristic_propertyString(nativePtr) };
                return _propertis;
            }
        }

        public void setNotifyValue(bool enable)
        {
            cbCharacteristic_setNotifyValue(nativePtr, enable);
        }
    }
}
#endif
