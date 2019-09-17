using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleUser : MonoBehaviour {

    private UnityCoreBluetooth unityCoreBluetooth;

    // Use this for initialization
    void Start () {

        unityCoreBluetooth = new UnityCoreBluetooth();

        unityCoreBluetooth.OnUpdateState((string state) => {
            Debug.Log("state: " + state);
            if (state != "poweredOn") return;
            if (unityCoreBluetooth == null)
            {
                Debug.Log("unityCoreBluetooth is null");
                return;
            }
            unityCoreBluetooth.StartScan();
        });

        unityCoreBluetooth.OnDiscoverPeripheral((IntPtr peripheral) => {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            string n = p.name;
            Debug.Log("discover peripheral name: " + n);

            if (n != "Daydream controller") return;

            unityCoreBluetooth.StopScan();
            unityCoreBluetooth.Connect(p);
        });

        unityCoreBluetooth.OnConnectPeripheral((IntPtr peripheral) => {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            string n = p.name;
            Debug.Log("connected peripheral name: " + n);
            p.discoverServices();
        });

        unityCoreBluetooth.OnDiscoverService((IntPtr service) => {
            UnityCBService s = new UnityCBService(service);
            string uuid = s.uuid;
            Debug.Log("discover service uuid: " + uuid);
            if (uuid != "FE55") return;
            s.discoverCharacteristics();
        });


        unityCoreBluetooth.OnDiscoverCharacteristic((IntPtr characteristic) => {
            UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
            string uuid = c.uuid;
            string usage = c.propertis[0];
            Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
            if (usage != "notify") return;
            c.setNotifyValue(true);
        });


        unityCoreBluetooth.OnUpdateValue((IntPtr characteristic, IntPtr data, long length) => {
            UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
            string uuid = c.uuid;
            byte[] result = new byte[length];
            Marshal.Copy(data, result, 0, (int)length);
            Debug.Log("update characteristic uuid: " + uuid + ", data: " + BitConverter.ToString(result));
        });

        unityCoreBluetooth.StartCoreBluetooth();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
