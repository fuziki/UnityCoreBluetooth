//
//  ucb_peripheral.swift
//  
//
//  Created by fuziki on 2021/07/04.
//

import CoreBluetooth
import Foundation

//MARK:- peripheral
@_cdecl("ucb_peripheral_getName")
public func ucb_peripheral_getName(_ peripheral: UnsafePointer<CBPeripheral>) -> UnsafePointer<CChar>? {
    let nsStr = (peripheral.pointee.name ?? "") as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_peripheral_discoverServicesWithPeripheral")
public func ucb_peripheral_discoverServicesWithPeripheral(_ peripheral: UnsafePointer<CBPeripheral>) {
    peripheral.pointee.discoverServices(nil)
}
