//
//  CdeclUnityCoreBluetooth.swift
//  
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth
import Foundation

//MARK:- manager
@_cdecl("ucb_manager_instantiate")
public func ucb_manager_instantiate() {
}

@_cdecl("ucb_manager_release")
public func ucb_manager_release() {
}

@_cdecl("ucb_manager_startScan")
public func ucb_manager_startScan() {
}

@_cdecl("ucb_manager_stopScan")
public func ucb_manager_stopScan() {
}

@_cdecl("ucb_manager_connectWithPeripheral")
public func ucb_manager_connectWithPeripheral(_ peripheral: UnsafePointer<CBPeripheral>) {
}

@_cdecl("ucb_manager_discoverServicesWithPeripheral")
public func ucb_manager_discoverServicesWithPeripheral(_ peripheral: UnsafePointer<CBPeripheral>) {
}

@_cdecl("ucb_manager_discoverServicesWithPeripheral")
public func ucb_manager_discoverCharacteristicWithService(_ service: UnsafePointer<CBService>) {
}

//MARK:- register
@_cdecl("ucb_manager_register_onUpdateState")
public func ucb_manager_register_onUpdateState(_ handler: @convention(c) (UnsafePointer<CChar>) -> Void) {
}

@_cdecl("ucb_manager_register_onDiscoverPeripheral")
public func ucb_manager_register_onDiscoverPeripheral(_ handler: @convention(c) (UnsafePointer<CBPeripheral>) -> Void) {
}

@_cdecl("ucb_manager_register_onConnectPeripheral")
public func ucb_manager_register_onConnectPeripheral(_ handler: @convention(c) (UnsafePointer<CBPeripheral>) -> Void) {
}

@_cdecl("ucb_manager_register_onDiscoverService")
public func ucb_manager_register_onDiscoverService(_ handler: @convention(c) (UnsafePointer<CBService>) -> Void) {
}

@_cdecl("ucb_manager_register_onDiscoverCharacteristic")
public func ucb_manager_register_onDiscoverCharacteristic(_ handler: @convention(c) (UnsafePointer<CBCharacteristic>) -> Void) {
}

@_cdecl("ucb_manager_register_onUpdateValue")
public func ucb_manager_register_onUpdateValue(_ handler: @convention(c) (UnsafePointer<CBCharacteristic>,
                                                                          UnsafePointer<UInt8>,
                                                                          CLong) -> Void) {
}

//MARK:- peripheral
@_cdecl("ucb_peripheral_getName")
public func ucb_peripheral_getName(_ peripheral: UnsafePointer<CBPeripheral>) -> UnsafePointer<CChar>? {
    let nsStr = "str" as NSString
    return nsStr.utf8String
}

//MARK:- service
@_cdecl("ucb_service_getUuid")
public func ucb_service_getUuid(_ service: UnsafePointer<CBService>) -> UnsafePointer<CChar>? {
    let nsStr = "str" as NSString
    return nsStr.utf8String
}

//MARK:- characteristic
@_cdecl("ucb_characteristic_getUuid")
public func ucb_characteristic_getUuid(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let nsStr = "str" as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_getPropertis")
public func ucb_characteristic_getPropertis(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let nsStr = "str" as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_setNotify")
public func ucb_characteristic_setNotify(_ characteristic: UnsafePointer<CBCharacteristic>, _ enable: Bool) {
}
