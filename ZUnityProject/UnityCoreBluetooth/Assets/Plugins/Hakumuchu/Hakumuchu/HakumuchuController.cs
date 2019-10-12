using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCoreBluetoothFramework;


public class HakumuchuController : MonoBehaviour {

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
//            Debug.Log("update value: " + newValue);
            this.Recentered = newValue.app;
            if (this.Recentered) analyzer.magnet.ReCenter();
        });

        UnityCoreBluetooth.Shared.StartCoreBluetooth();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = analyzer.magnet.ValueAsQuaternion;
    }

    private Hakumuchu.DayDreamController.DayDreamControllerAnalyzer analyzer = new Hakumuchu.DayDreamController.DayDreamControllerAnalyzer();

    void OnDestroy()
    {
        UnityCoreBluetooth.ReleaseSharedInstance();
    }

    public bool IsRightHand = false;

    public bool Recentered = false;

    public Quaternion Orientation
    {
        get
        {
            return analyzer.magnet.ValueAsQuaternion;
        }
    }

    public Vector3 Gyro
    {
        get
        {
            return analyzer.gyro.Value;
        }
    }

}
