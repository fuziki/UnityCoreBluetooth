//
//  ucb_service.swift
//  
//
//  Created by fuzki on 2021/07/04.
//

import CoreBluetooth
import Foundation


//MARK:- service
@_cdecl("ucb_service_getUuid")
public func ucb_service_getUuid(_ service: UnsafePointer<CBService>) -> UnsafePointer<CChar>? {
    let service = Unmanaged<CBService>.fromOpaque(service).takeUnretainedValue()
    let nsStr = service.uuid.uuidString as NSString
    return nsStr.utf8String
}

@_cdecl("ucb_service_discoverCharacteristic")
public func ucb_service_discoverCharacteristic(_ service: UnsafePointer<CBService>) {
    let service = Unmanaged<CBService>.fromOpaque(service).takeUnretainedValue()
    service.peripheral.discoverCharacteristics(nil, for: service)
}
