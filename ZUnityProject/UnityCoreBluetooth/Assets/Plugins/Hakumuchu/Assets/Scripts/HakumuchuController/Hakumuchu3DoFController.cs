using UnityEngine;
#if UNITY_EDITOR_OSX || UNITY_IOS
using UnityCoreBluetoothFramework;
#endif

namespace Hakumuchu.HakumuchuController
{
    public class Hakumuchu3DoFController : MonoBehaviour
    {
#if UNITY_EDITOR_OSX || UNITY_IOS
        [SerializeField]
        private bool IsRightHand = true;

        [SerializeField]
        private bool Recentered = false;

        private Hakumuchu.DayDreamController.DayDreamControllerAnalyzer analyzer = new Hakumuchu.DayDreamController.DayDreamControllerAnalyzer();

        public Quaternion Orientation => analyzer.magnet.ValueAsQuaternion;
        public Quaternion MirrorOrientation => analyzer.magnet.ValueAsMirrorQuaternion;
        public Vector3 Gyro => analyzer.gyro.Value;

        public Hakumuchu.DayDreamController.Components.TouchPad.State touchPad => analyzer.touchPad.Value;

        public bool IsConnected = false;

        // Use this for initialization
        void Start()
        {
            UnityCoreBluetooth.CreateSharedInstance();

            UnityCoreBluetooth.Shared.OnUpdateState((string state) =>
            {
                Debug.Log("state: " + state);
                if (state != "poweredOn") return;
                UnityCoreBluetooth.Shared.StartScan();
            });

            UnityCoreBluetooth.Shared.OnDiscoverPeripheral((UnityCBPeripheral peripheral) =>
            {
                if (peripheral.name != "")
                    Debug.Log("discover peripheral name: " + peripheral.name);
                if (peripheral.name != "Daydream controller") return;

                UnityCoreBluetooth.Shared.StopScan();
                UnityCoreBluetooth.Shared.Connect(peripheral);
            });

            UnityCoreBluetooth.Shared.OnConnectPeripheral((UnityCBPeripheral peripheral) =>
            {
                Debug.Log("connected peripheral name: " + peripheral.name);
                IsConnected = true;
                peripheral.discoverServices();
            });

            UnityCoreBluetooth.Shared.OnDiscoverService((UnityCBService service) =>
            {
                Debug.Log("discover service uuid: " + service.uuid);
                if (service.uuid != "FE55") return;
                service.discoverCharacteristics();
            });


            UnityCoreBluetooth.Shared.OnDiscoverCharacteristic((UnityCBCharacteristic characteristic) =>
            {
                string uuid = characteristic.uuid;
                string usage = characteristic.propertis[0];
                Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
                if (usage != "notify") return;
                characteristic.setNotifyValue(true);
            });

            UnityCoreBluetooth.Shared.OnUpdateValue((UnityCBCharacteristic characteristic, byte[] data) =>
            {
                analyzer.UpdateBytes(data);
            });

            analyzer.buttons.OnUpdateValue((newValue) => {
                this.Recentered = newValue.app;
                if (this.Recentered) analyzer.magnet.ReCenter();
            });

            UnityCoreBluetooth.Shared.StartCoreBluetooth();
        }

        void Update()
        {
            this.transform.rotation = analyzer.Value.MagnetAsQuaternion;
        }

        void OnDestroy()
        {
            UnityCoreBluetooth.ReleaseSharedInstance();
        }
#else
        public Quaternion Orientation = Quaternion.identity;
        public Quaternion MirrorOrientation = Quaternion.identity;
        public Vector3 Gyro = Vector3.zero;
#endif
    }
}
