# UnityCoreBluetooth
![Platform](https://img.shields.io/badge/platform-%20iOS%20%7C%20macOS%20-lightgrey.svg)
![Unity](https://img.shields.io/badge/unity-2022-green.svg)
![Xode](https://img.shields.io/badge/xcode-xcode14-green.svg)

iOS & macOS [Unity Native Plugin](https://docs.unity3d.com/Manual/NativePlugins.html) for [Core Bluetooth framework](https://developer.apple.com/documentation/corebluetooth)

# Example: Get Raw Value from Daydream Controller

<img src="docs/videos/daydream.gif" width="800">  

# Requirements
## Unity
* Unity 2021.X
* Unity 2022.X
* Unity 2022.2+ (Apple Silicon)

## Platforms
* iOS 14+
* macOS 12+ (Apple Silicon and Intel)

# Installation
Download UnityCoreBluetooth.unitypackage from [Releases](https://github.com/fuziki/UnityCoreBluetooth/releases) and install it in your project.

## Set Info.plist

Set **privacy - bluetooth always usage description** in Info.plist.  
You can also set it in PostProcessBuild.  

```c#
public class PostXcodeBuild
{
    [PostProcessBuild]
    public static void SetXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS) return;
        var plistPath = pathToBuiltProject + "/Info.plist";
        var plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
        var rootDict = plist.root;
        rootDict.SetString("NSBluetoothAlwaysUsageDescription", "use ble");
        File.WriteAllText(plistPath, plist.WriteToString());
    }
}
```

# Features
- Peripheral
  - [x] Get name
  - [x] Search services
- Service
  - [x] Get uuid
  - [x] Search characteristics
- Characteristic
  - [x] Get uuid
  - [x] Get properties
  - [x] Write value
  - [ ] Read value
  - [x] Receive notify
  
# Usage
## Example (Get Raw Value from Daydream Controller)

SeeMore [SampleUser.cs](Examples/UnityExample/Assets/Scripts/SampleUser.cs)

### About Daydream Controller

Get raw value form Daydream controller

| Property | Target |
|--|--|
| Peripheral Name | Daydream controller |
| Service UUID | FE55 |
| Characteristic Usage | notify |

### 1. Get CoreBluetoothManager instance

Get shared CoreBluetoothManager instance.

```c#
manager = CoreBluetoothManager.Shared;
```

### 2. Start Scan On PowerOn

```c#
manager.OnUpdateState((string state) =>
{
    Debug.Log("state: " + state);
    if (state != "poweredOn") return;
    manager.StartScan();
});
```

### 3. Discover And Connect Daydream Controller

```c#
manager.OnDiscoverPeripheral((CoreBluetoothPeripheral peripheral) =>
{
    if (peripheral.name != "")
        Debug.Log("discover peripheral name: " + peripheral.name);
    if (peripheral.name != "Daydream controller") return;

    manager.StopScan();
    manager.ConnectToPeripheral(peripheral);
});
```

### 4. Discover Services And Characteristic On Connected

```c#
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
```

### 5.  Enable Notify

```c#
manager.OnDiscoverCharacteristic((CoreBluetoothCharacteristic characteristic) =>
{
    string uuid = characteristic.uuid;
    string usage = characteristic.propertis[0];
    Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
    if (usage != "notify") return;
    characteristic.setNotifyValue(true);
});
```

### 6.  Handle Notify Value

```c#
manager.OnUpdateValue((CoreBluetoothCharacteristic characteristic, byte[] data) =>
{
    this.value = data;
    this.flag = true;
});
```

### 7.  Start CoreBluetoothManager

```c#
manager.Start();
```

### 8.  Write Value

```c#
byte[] value = { 0x64, 0x68 };
characteristic.Write(value);
```

# Examples
* [Show Detail](Examples/)

## [UnityExample](Examples/UnityExample)
* Example for Unity
* Unity Version
  * check [Examples/UnityExample/ProjectSettings/ProjectVersion.txt](Examples/UnityExample/ProjectSettings/ProjectVersion.txt)
* Work on Unity Editor And iOS device
* Show raw value from Daydream controller or M5StickC

## [M5Peripheral](Examples/M5Peripheral)
* Peripheral Example for M5StickC (Plus)
* Support write and notify

## NativeExamples
### [DaydreamExample iOS](Examples/NativeExamples/DaydreamExample%20iOS)
* Native Examample for iOS
* Show raw value from Daydream controller

### [DaydreamExample macOS](Examples/NativeExamples/DaydreamExample%20macOS)
* Native Examample for macOS
* Show raw value from Daydream controller
