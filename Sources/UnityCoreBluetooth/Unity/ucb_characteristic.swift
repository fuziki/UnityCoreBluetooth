//
//  ucb_characteristic.swift
//
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth
import Foundation

// MARK: - characteristic
@_cdecl("ucb_characteristic_getUuid")
public func ucb_characteristic_getUuid(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    let nsStr = characteristic.uuid.uuidString as NSString
    let str = nsStr.utf8String!
    let len = strlen(str) + 1
    let ptr = UnsafeMutablePointer<CChar>.allocate(capacity: len)
    ptr.initialize(from: str, count: len)
    return UnsafePointer(ptr)
}

@_cdecl("ucb_characteristic_getPropertis")
public func ucb_characteristic_getPropertis(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    let nsStr = characteristic.propertyString as NSString
    let str = nsStr.utf8String!
    let len = strlen(str) + 1
    let ptr = UnsafeMutablePointer<CChar>.allocate(capacity: len)
    ptr.initialize(from: str, count: len)
    return UnsafePointer(ptr)
}

@_cdecl("ucb_characteristic_write")
public func ucb_characteristic_write(_ characteristic: UnsafePointer<CBCharacteristic>,
                                     _ value: UnsafePointer<UInt8>?, _ len: CLong) {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    let peripheral: CBPeripheral = characteristic.service.peripheral
    let data = Data(bytes: value!, count: len)
    peripheral.writeValue(data, for: characteristic, type: .withoutResponse)
}

@_cdecl("ucb_characteristic_setNotify")
public func ucb_characteristic_setNotify(_ characteristic: UnsafePointer<CBCharacteristic>, _ enable: Bool) {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    characteristic.service.peripheral.setNotifyValue(enable, for: characteristic)
}
