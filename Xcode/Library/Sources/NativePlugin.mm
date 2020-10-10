//
//  NativePlugin.mm
//  PROJECT_NAME
//
//  Created by AUTHOR on YYYY/MM/DD.
//

#include <stdio.h>

//#import IMPORT_SWIFT_HEADER
#import "NativePlugin.h"

int add_one(int num) {
    return (int)[AddOne addWithNum: num];
}

void unityCoreBluetooth_onUpdateState(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateStateHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onUpdateStateWithHandler: ^(NSString* state) {
            handler([state UTF8String]);
        }];
    }
}

void unityCoreBluetooth_onDiscoverPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverPeripheralHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onDiscoverPeripheralWithHandler: ^(CBPeripheral* peripheral) {
            handler(peripheral);
        }];
    }
}

void unityCoreBluetooth_onConnectPeripheral(UnityCoreBluetooth* unityCoreBluetooth, OnConnectPeripheralHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onConnectPeripheralWithHandler: ^(CBPeripheral* peripheral) {
            handler(peripheral);
        }];
    }
}

void unityCoreBluetooth_onDiscoverService(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverServiceHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onDiscoverServiceWithHandler: ^(CBService* service) {
            handler(service);
        }];
    }
}

void unityCoreBluetooth_onDiscoverCharacteristic(UnityCoreBluetooth* unityCoreBluetooth, OnDiscoverCharacteristicHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onDiscoverCharacteristicWithHandler: ^(CBCharacteristic* characteristic) {
            handler(characteristic);
        }];
    }
}

void unityCoreBluetooth_onUpdateValue(UnityCoreBluetooth* unityCoreBluetooth, OnUpdateValueHandler handler) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth onUpdateValueWithHandler: ^(CBPeripheral* peripheral, CBCharacteristic* characteristic, NSData* data) {
            NSUInteger length = [data length];
            unsigned char* ptrDest = (unsigned char*)malloc(length);
            memcpy(ptrDest, [data bytes], length);
            handler(peripheral, characteristic, ptrDest, length);
            free(ptrDest);
        }];
    }
}

UnityCoreBluetooth* unityCoreBluetooth_init() {
    @autoreleasepool {
        UnityCoreBluetooth* unityCoreBluetooth = [[UnityCoreBluetooth alloc] init];
        CFRetain((CFTypeRef)unityCoreBluetooth);
        return unityCoreBluetooth;
    }
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
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth stopScan];
    }
}

void unityCoreBluetooth_connect(UnityCoreBluetooth* unityCoreBluetooth, CBPeripheral* peripheral) {
    @autoreleasepool {
        if (unityCoreBluetooth == nil) return;
        [unityCoreBluetooth connectWithPeripheral: peripheral];
    }
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

