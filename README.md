# UnityCoreBluetooth
![Platform](https://img.shields.io/badge/platform-%20iOS%20%7C%20macOS%20-lightgrey.svg)
![Unity](https://img.shields.io/badge/unity-2018-green.svg)
![Xode](https://img.shields.io/badge/xcode-xcode11-green.svg)

* iOS & macOS Unity Bluetooth Native Plugin
* Generate Xcode projects by [UnityPluginXcodeTemplate](https://github.com/fuziki/UnityPluginXcodeTemplate)

## Download Unity Package
* [Release v0.1.0](https://github.com/fuziki/UnityCoreBluetooth/releases/tag/v0.1.0)

## Requirements
* [xcode11+](https://apps.apple.com/jp/app/xcode/id497799835?mt=12)
* [Unity 2018+](https://unity.com/)
* [xcodegen](https://github.com/yonaskolb/XcodeGen)
* [rust](https://www.rust-lang.org/)  (if change script)


## video
<blockquote class="twitter-tweet"><p lang="ja" dir="ltr">UnityエディタとiOSで動くBluetoothのプラグイン作った<a href="https://twitter.com/hashtag/unity?src=hash&amp;ref_src=twsrc%5Etfw">#unity</a> <a href="https://t.co/EeHz0iqcmL">pic.twitter.com/EeHz0iqcmL</a></p>&mdash; ふじき (@fzkqi) <a href="https://twitter.com/fzkqi/status/1176485721445556224?ref_src=twsrc%5Etfw">September 24, 2019</a></blockquote> <script async src="https://platform.twitter.com/widgets.js" charset="utf-8"></script>

## 目的
UnityにはBluetoothで通信する機能がないです。
iOSでdaydreamのコントローラを使いたかったので、プラグインを自作しました。
Unity Editorにも対応しており、editorで実際に接続して実装して、実機は動作確認だけという開発が可能です。

## 導入
* Unity Packageを配布しています。

## 組み込む
Daydreamのコントローラの生データを取得します

### Daydreamコントローラについて
下記の条件のcharacteristicに接続すると、コントローラの生データを取得可能です。
BLEデバイスは複数のserviceを持っており、serviceは複数のcharacteristicを持っています。characteristicごとに決められた機能が提供されており、今回はcharacteristic uuidは同名のidが複数存在するので、notifyのusageのcharacteristicを使って生データを受け取ります。

| 項目 | 接続する端末の条件 |
|--|--|
| デバイス名 | Daydream controller |
| service uuid | FE55 |
| characteristic usage | notify |

### UnityCoreBluetoothを使う

#### 1. シングルトンインスタンスの生成
UnityCoreBluetoothはシングルトンで使用します。
bluetooth機能がpowerOnになったら、BLEデバイスのスキャンを開始させます。
コールバックの設定が全て終了したら、StartCoreBluetoothを呼び出して開始します。

```cs

        UnityCoreBluetooth.CreateSharedInstance();
        UnityCoreBluetooth.Shared.OnUpdateState((string state) =>
        {
            Debug.Log("state: " + state);
            if (state != "poweredOn") return;
            UnityCoreBluetooth.Shared.StartScan();
        });
        //~~中略~~
        UnityCoreBluetooth.Shared.StartCoreBluetooth();
```

#### 2.  接続したいデバイス名のデバイスを見つけたら、接続する

```cs
        UnityCoreBluetooth.Shared.OnDiscoverPeripheral((UnityCBPeripheral peripheral) =>
        {
            if (peripheral.name != "")
                Debug.Log("discover peripheral name: " + peripheral.name);
            if (peripheral.name != "Daydream controller") return;

            UnityCoreBluetooth.Shared.StopScan();
            UnityCoreBluetooth.Shared.Connect(peripheral);
        });
```

#### 3. デバイスに接続したら、サービスを探す。

```cs
        UnityCoreBluetooth.Shared.OnConnectPeripheral((UnityCBPeripheral peripheral) =>
        {
            Debug.Log("connected peripheral name: " + peripheral.name);
            peripheral.discoverServices();
        });
```

#### 4. 対象のuuidのサービスが見つかったら、characteristicを探す。

```cs
        UnityCoreBluetooth.Shared.OnDiscoverService((UnityCBService service) =>
        {
            Debug.Log("discover service uuid: " + service.uuid);
            if (service.uuid != "FE55") return;
            service.discoverCharacteristics();
        });
```

#### 5.  usage がnotifyのcharacteristicが見つかったら、通知を有効にする
通知を有効にすることで、daydreamコントローラから連続して生データを受け取ることが可能になります。

```cs
        UnityCoreBluetooth.Shared.OnDiscoverCharacteristic((UnityCBCharacteristic characteristic) =>
        {
            string uuid = characteristic.uuid;
            string usage = characteristic.propertis[0];
            Debug.Log("discover characteristic uuid: " + uuid + ", usage: " + usage);
            if (usage != "notify") return;
            characteristic.setNotifyValue(true);
        });
```

#### 6. characteristicから通知があったら、データを受け取る
※ リアルタイムで受け取れるのですが、メインスレッド保証ではないです。

```cs
        UnityCoreBluetooth.Shared.OnUpdateValue((UnityCBCharacteristic characteristic, byte[] data) =>
        {
            this.value = data;
            this.flag = true;
        });
```

#### 7. シングルトンインスタンスの破棄

```cs
    void OnDestroy()
    {
        UnityCoreBluetooth.ReleaseSharedInstance();
    }
```
