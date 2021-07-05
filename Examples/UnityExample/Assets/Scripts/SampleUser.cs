using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR_OSX || UNITY_IOS
using UnityCoreBluetooth;

public class SampleUser : MonoBehaviour {

    public Text text;

    private CoreBluetoothManager manager;

    // Use this for initialization
    void Start()
    {
        manager = CoreBluetoothManager.Shared;

        manager.OnUpdateState((string state) =>
        {
            Debug.Log("state: " + state);
            if (state != "poweredOn") return;
            manager.StartScan();
        });

        manager.OnDiscoverPeripheral((CoreBluetoothPeripheral peripheral) =>
        {
            if (peripheral.name != "")
                Debug.Log("discover peripheral name: " + peripheral.name);
            if (peripheral.name != "Daydream controller") return;

            manager.StopScan();
            manager.ConnectToPeripheral(peripheral);
        });

        manager.OnConnectPeripheral((CoreBluetoothPeripheral peripheral) =>
        {
            Debug.Log("connected peripheral name: " + peripheral.name);
            peripheral.discoverServices();
        });

        manager.OnDiscoverService((CoreBluetoothService service) =>
        {
            Debug.Log("discover service uuid: " + service.uuid);
            if (service.uuid != "FE55") return;
            service.discoverCharacteristics();
        });


        manager.OnDiscoverCharacteristic((CoreBluetoothCharacteristic characteristic) =>
        {
            string uuid = characteristic.uuid;
            string usage = characteristic.propertis[0];
            Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
            if (usage != "notify") return;
            characteristic.setNotifyValue(true);
        });

        manager.OnUpdateValue((CoreBluetoothCharacteristic characteristic, byte[] data) =>
        {
            this.value = data;
            this.flag = true;
        });
        manager.Start();
    }

    private bool flag = false;
    private byte[] value = new byte[20];
	
	// Update is called once per frame
	void Update () {
        if (flag == false) return;
        text.text = BitConverter.ToString(value);
    }

    void OnDestroy()
    {
        manager.Stop();
    }
}
#endif