//
//  UCBCharacteristicProperties.swift
//
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth

enum UCBCharacteristicProperties: String, CaseIterable {
    case broadcast
    case read
    case writeWithoutResponse
    case write
    case notify
    case indicate
    case authenticatedSignedWrites
    case extendedProperties
    case notifyEncryptionRequired
    case indicateEncryptionRequired
    public var toCBCharacteristicProperties: CBCharacteristicProperties {
        switch self {
        case .broadcast:
            return CBCharacteristicProperties.broadcast
        case .read:
            return CBCharacteristicProperties.read
        case .writeWithoutResponse:
            return CBCharacteristicProperties.writeWithoutResponse
        case .write:
            return CBCharacteristicProperties.write
        case .notify:
            return CBCharacteristicProperties.notify
        case .indicate:
            return CBCharacteristicProperties.indicate
        case .authenticatedSignedWrites:
            return CBCharacteristicProperties.authenticatedSignedWrites
        case .extendedProperties:
            return CBCharacteristicProperties.extendedProperties
        case .notifyEncryptionRequired:
            return CBCharacteristicProperties.notifyEncryptionRequired
        case .indicateEncryptionRequired:
            return CBCharacteristicProperties.indicateEncryptionRequired
        }
    }
}
