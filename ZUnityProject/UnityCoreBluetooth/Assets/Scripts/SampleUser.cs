using System;
using UnityEngine;
using UnityCoreBluetoothFramework;
using UnityEngine.UI;

public class SampleUser : MonoBehaviour {

    public Text text;

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
            this.value = data;
            this.flag = true;
        });

        UnityCoreBluetooth.Shared.StartCoreBluetooth();
    }

    private bool flag = false;
    private byte[] value = new byte[20];
	
	// Update is called once per frame
	void Update () {
        if (flag == false) return;
        text.text = BitConverter.ToString(value);
        Debug.Log("value: " + BitConverter.ToString(value));
    }

    void OnDestroy()
    {
        UnityCoreBluetooth.ReleaseSharedInstance();
    }
}
