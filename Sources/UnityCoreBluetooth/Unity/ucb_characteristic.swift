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
    let nsStr = characteristic.pointee.uuid.uuidString as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_getPropertis")
public func ucb_characteristic_getPropertis(_ characteristic: UnsafePointer<CBCharacteristic>) -> UnsafePointer<CChar>? {
    let nsStr = characteristic.pointee.propertyString as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_characteristic_setNotify")
public func ucb_characteristic_setNotify(_ characteristic: UnsafePointer<CBCharacteristic>, _ enable: Bool) {
    characteristic.pointee.service.peripheral.setNotifyValue(enable, for: characteristic.pointee)
}
