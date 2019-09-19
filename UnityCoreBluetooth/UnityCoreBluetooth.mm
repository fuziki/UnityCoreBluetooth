//
//  CoreBluetoothUnityMacOS.m
//  CoreBluetoothUnityMacOS
//
//  Created by fuziki on 2019/09/02.
//  Copyright © 2019 fuziki.factory. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

#ifdef UnityCoreBluetoothIOS
#import <UnityCoreBluetoothIOS/UnityCoreBluetoothIOS-Swift.h>
#elif libUnityCoreBluetoothIOS
#import <libUnityCoreBluetoothIOS-Swift.h>
#elif UnityCoreBluetoothMacOS
#import <UnityCoreBluetoothMacOS-Swift.h>
#endif

typedef void (*OnUpdateStateHandler) (const char* state);
typedef void (*OnDiscoverPeripheralHandler) (CBPeripheral* peripheral);
typedef void (*OnConnectPeripheralHandler) (CBPeripheral* peripheral);
typedef void (*OnDiscoverServiceHandler) (CBService* service);
typedef void (*OnDiscoverCharacteristicHandler) (CBCharacteristic* characteristic);
typedef void (*OnUpdateValueHandler) (CBCharacteristic* characteristic, unsigned char* data, long length);

extern "C" {
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
}

void unityCoreBluetooth_onUpdateState(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateStateHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onUpdateStateWithHandler: ^(NSString* state) {
        handler([state UTF8String]);
    }];
}

void unityCoreBluetooth_onDiscoverPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverPeripheralHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onDiscoverPeripheralWithHandler: ^(CBPeripheral* peripheral) {
        handler(peripheral);
    }];
}

void unityCoreBluetooth_onConnectPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnConnectPeripheralHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onConnectPeripheralWithHandler: ^(CBPeripheral* peripheral) {
        handler(peripheral);
    }];
}

void unityCoreBluetooth_onDiscoverService(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverServiceHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onDiscoverServiceWithHandler: ^(CBService* service) {
        handler(service);
    }];
}

void unityCoreBluetooth_onDiscoverCharacteristic(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverCharacteristicHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onDiscoverCharacteristicWithHandler: ^(CBCharacteristic* characteristic) {
        handler(characteristic);
    }];
}

void unityCoreBluetooth_onUpdateValue(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateValueHandler handler) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth onUpdateValueWithHandler: ^(CBCharacteristic* characteristic, NSData* data) {
        NSUInteger length = [data length];
        unsigned char* ptrDest = (unsigned char*)malloc(length);
        memcpy(ptrDest, [data bytes], length);
        handler(characteristic, ptrDest, length);
        free(ptrDest);
    }];
}

UnityCoreBluetooth* unityCoreBluetooth_init() {
    UnityCoreBluetooth* unityCoreBluetooth = [[UnityCoreBluetooth alloc] init];
    CFRetain((CFTypeRef)unityCoreBluetooth);
    return unityCoreBluetooth;
}

void unityCoreBluetooth_release(UnityCoreBluetooth* unityCoreBluetooth) {
    CFRelease((CFTypeRef)unityCoreBluetooth);
}

void unityCoreBluetooth_startCoreBluetooth(UnityCoreBluetooth* unityCoreBluetooth) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth startCoreBluetooth];
}

void unityCoreBluetooth_startScan(UnityCoreBluetooth* unityCoreBluetooth) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth startScan];
}

void unityCoreBluetooth_stopScan(UnityCoreBluetooth* unityCoreBluetooth) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth stopScan];
}

void unityCoreBluetooth_connect(UnityCoreBluetooth* unityCoreBluetooth, CBPeripheral* peripheral) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth connectWithPeripheral: peripheral];
}

void unityCoreBluetooth_clearPeripherals(UnityCoreBluetooth* unityCoreBluetooth) {
    if (unityCoreBluetooth == nil) return;
    [unityCoreBluetooth clearPeripherals];
}

const char* cbPeripheral_name(CBPeripheral* peripheral) {
    NSString* name = peripheral.name != nil ? peripheral.name: @"";
    const char* str = [name UTF8String];
    char* retStr = (char*)malloc(strlen(str) + 1);
    strcpy(retStr, str);
    retStr[strlen(str)] = '\0';
    return retStr;
}

void cbPeripheral_discoverServices(CBPeripheral* peripheral) {
    [peripheral discoverServices: nil];
}

const char* cbService_uuid(CBService* service) {
    NSString* name = service.UUID.UUIDString != nil ? service.UUID.UUIDString: @"";
    const char* str = [name UTF8String];
    char* retStr = (char*)malloc(strlen(str) + 1);
    strcpy(retStr, str);
    retStr[strlen(str)] = '\0';
    return retStr;
}

void cbService_discoverCharacteristic(CBService* service) {
    [service.peripheral discoverCharacteristics:nil forService:service];
}

const char* cbCharacteristic_uuid(CBCharacteristic* characteristic) {
    NSString* name = characteristic.UUID.UUIDString != nil ? characteristic.UUID.UUIDString: @"";
    const char* str = [name UTF8String];
    char* retStr = (char*)malloc(strlen(str) + 1);
    strcpy(retStr, str);
    retStr[strlen(str)] = '\0';
    return retStr;
}

const char* cbCharacteristic_propertyString(CBCharacteristic* characteristic) {
    const char* str = [characteristic.propertyString UTF8String];
    char* retStr = (char*)malloc(strlen(str) + 1);
    strcpy(retStr, str);
    retStr[strlen(str)] = '\0';
    return retStr;
}

void cbCharacteristic_setNotifyValue(CBCharacteristic* characteristic, bool enable) {
    [characteristic.service.peripheral setNotifyValue: enable forCharacteristic: characteristic];
}
