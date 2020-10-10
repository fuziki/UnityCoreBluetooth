//
//  NativePlugin.h
//  PROJECT_NAME
//
//  Created by AUTHOR on YYYY/MM/DD.
//

#ifndef NativePlugin_h
#define NativePlugin_h

#import <CoreBluetooth/CoreBluetooth.h>

#ifdef TARGET_IOS
#import <UnityCoreBluetooth/UnityCoreBluetooth-Swift.h>
#elif TARGET_MACOS
#import <UnityCoreBluetooth_bundle-Swift.h>
#elif TARGET_MACOS_FRAMEWORK
#import <UnityCoreBluetooth_macOS/UnityCoreBluetooth_macOS-Swift.h>
#endif

typedef void (*OnUpdateStateHandler) (const char* state);
typedef void (*OnDiscoverPeripheralHandler) (CBPeripheral* peripheral);
typedef void (*OnConnectPeripheralHandler) (CBPeripheral* peripheral);
typedef void (*OnDiscoverServiceHandler) (CBService* service);
typedef void (*OnDiscoverCharacteristicHandler) (CBCharacteristic* characteristic);
typedef void (*OnUpdateValueHandler) (CBPeripheral* peripheral, CBCharacteristic* characteristic, unsigned char* data, long length);

#ifdef __cplusplus
extern "C" {
#endif
    int add_one(int num);

void unityCoreBluetooth_onUpdateState(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateStateHandler handler);
void unityCoreBluetooth_onDiscoverPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverPeripheralHandler handler);
void unityCoreBluetooth_onConnectPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnConnectPeripheralHandler handler);
void unityCoreBluetooth_onDiscoverService(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverServiceHandler handler);
void unityCoreBluetooth_onDiscoverCharacteristic(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverCharacteristicHandler handler);
void unityCoreBluetooth_onUpdateValue(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateValueHandler handler);

UnityCoreBluetooth* unityCoreBluetooth_init();
void unityCoreBluetooth_release(UnityCoreBluetooth* unityCoreBluetooth);

void unityCoreBluetooth_startCoreBluetooth(UnityCoreBluetooth* unityCoreBluetooth);
void unityCoreBluetooth_startScan(UnityCoreBluetooth* unityCoreBluetooth);
void unityCoreBluetooth_stopScan(UnityCoreBluetooth* unityCoreBluetooth);
void unityCoreBluetooth_connect(UnityCoreBluetooth* unityCoreBluetooth, CBPeripheral* peripheral);
void unityCoreBluetooth_clearPeripherals(UnityCoreBluetooth* unityCoreBluetooth);

const char* cbPeripheral_name(CBPeripheral* peripheral);
void cbPeripheral_discoverServices(CBPeripheral* peripheral);

const char* cbService_uuid(CBService* service);
void cbService_discoverCharacteristic(CBService* service);

const char* cbCharacteristic_uuid(CBCharacteristic* characteristic);
const char* cbCharacteristic_propertyString(CBCharacteristic* characteristic);
void cbCharacteristic_setNotifyValue(CBCharacteristic* characteristic, bool enable);

#ifdef __cplusplus
}
#endif

#endif /* NativePlugin_h */
