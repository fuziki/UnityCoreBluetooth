using System;
using System.Runtime.InteropServices;
using AOT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleUser : MonoBehaviour {

//    private static event UnityCoreBluetooth.OnUpdateValueHandler onUpdateValueHandler;

    private UnityCoreBluetooth unityCoreBluetooth;

    // Use this for initialization
    void Start()
    {

        unityCoreBluetooth = UnityCoreBluetooth.Shared;

        unityCoreBluetooth.OnUpdateState((string state) =>
        {
            Debug.Log("state: " + state);
            if (state != "poweredOn") return;
            if (unityCoreBluetooth == null)
            {
                Debug.Log("unityCoreBluetooth is null");
                return;
            }
            unityCoreBluetooth.StartScan();
        });

        unityCoreBluetooth.OnDiscoverPeripheral((IntPtr peripheral) =>
        {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            string n = p.name;
            if (n != "")
                Debug.Log("discover peripheral name: " + n);

            if (n != "Daydream controller") return;

            if (this == null) return;

            unityCoreBluetooth.StopScan();
            unityCoreBluetooth.Connect(p);
        });

        unityCoreBluetooth.OnConnectPeripheral((IntPtr peripheral) =>
        {
            UnityCBPeripheral p = new UnityCBPeripheral(peripheral);
            string n = p.name;
            Debug.Log("connected peripheral name: " + n);
            p.discoverServices();
        });

        unityCoreBluetooth.OnDiscoverService((IntPtr service) =>
        {
            UnityCBService s = new UnityCBService(service);
            string uuid = s.uuid;
            Debug.Log("discover service uuid: " + uuid);
            if (uuid != "FE55") return;
            s.discoverCharacteristics();
        });


        unityCoreBluetooth.OnDiscoverCharacteristic((IntPtr characteristic) =>
        {
            UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
            string uuid = c.uuid;
            string usage = c.propertis[0];
            Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
            if (usage != "notify") return;
            c.setNotifyValue(true);
        });

        //onUpdateValueHandler += hoge;

        //        unityCoreBluetooth.OnUpdateValue(onUpdateValueHandler);

        //UnityCoreBluetooth.OnUpdateValueHandler2Event += delegate (IntPtr characteristic, IntPtr data, long length)
        //{
        //    UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
        //    string uuid = c.uuid;
        //    byte[] result = new byte[length];
        //    Marshal.Copy(data, result, 0, (int)length);
        //    if (this == null)
        //    {
        //        Debug.Log("this is null");
        //        return;
        //    }
        //    for (int i = 0; i < 20; i++)
        //        this.value[i] = result[i];
        //    flag = true;

        //};

        unityCoreBluetooth.OnUpdateValue((UnityCBCharacteristic characteristic, byte[] data) =>
        {
            if (this == null)
            {
                Debug.Log("this is null");
                return;
            }
            this.value = data;
            this.flag = true;
        });
        //unityCoreBluetooth.OnUpdateValue((IntPtr characteristic, IntPtr data, long length) =>
        //{
        //    UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
        //    string uuid = c.uuid;
        //    byte[] result = new byte[length];
        //    Marshal.Copy(data, result, 0, (int)length);
        //    if (this == null)
        //    {
        //        Debug.Log("this is null");
        //        return;
        //    }
        //    for (int i = 0; i < 20; i++)
        //        this.value[i] = result[i];
        //    flag = true;

        //    //Debug.Log("update characteristic uuid: " + uuid + ", data: " + BitConverter.ToString(result));
        //});

        unityCoreBluetooth.StartCoreBluetooth();
    }

    private bool flag = false;

    private void hoge(IntPtr characteristic, IntPtr data, long length)
    {
        UnityCBCharacteristic c = new UnityCBCharacteristic(characteristic);
        string uuid = c.uuid;
        byte[] result = new byte[length];
        Marshal.Copy(data, result, 0, (int) length);
        if (this == null)
        {
            Debug.Log("this is null");
            return;
        }
        for (int i = 0; i< 20; i++)
            this.value[i] = result[i];
        flag = true;
            //Debug.Log("update characteristic uuid: " + uuid + ", data: " + BitConverter.ToString(result));
    }

    private byte[] value = new byte[20];
	
	// Update is called once per frame
	void Update () {

        if (flag == false) return;

        Debug.Log("value: " + BitConverter.ToString(value));
    }

    void OnDestroy()
    {
        UnityCoreBluetooth.ReleaseSharedInstance();
//        UnityCoreBluetooth.OnUpdateValueHandler2Event -= hoge;


        //        unityCoreBluetooth.OnUpdateValue(null);
        //        onUpdateValueHandler -= hoge;
    }
}
