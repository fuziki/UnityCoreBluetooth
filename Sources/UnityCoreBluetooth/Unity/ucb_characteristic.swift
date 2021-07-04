//
//  ucb_characteristic.swift
//  
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth
import Foundation

//MARK:- characteristic
@_cdecl("ucb_characteristic_getUuid")
public func ucb_characteristic_getUuid(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    let nsStr = characteristic.uuid.uuidString as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_getPropertis")
public func ucb_characteristic_getPropertis(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    let nsStr = characteristic.propertyString as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_setNotify")
public func ucb_characteristic_setNotify(_ characteristic: UnsafePointer<CBCharacteristic>, _ enable: Bool) {
    let characteristic = Unmanaged<CBCharacteristic>.fromOpaque(characteristic).takeUnretainedValue()
    characteristic.service.peripheral.setNotifyValue(enable, for: characteristic)
}
