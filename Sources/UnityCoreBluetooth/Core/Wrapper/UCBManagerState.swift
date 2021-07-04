//
//  File.swift
//  
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth

enum UCBManagerState: String {
    case unknown
    case resetting
    case unsupported
    case unauthorized
    case poweredOff
    case poweredOn
    public init?(state: CBManagerState) {
        switch state {
        case .unknown:
            self = .unknown
        case .resetting:
            self = .resetting
        case .unsupported:
            self = .unsupported
        case .unauthorized:
            self = .unauthorized
        case .poweredOff:
            self = .poweredOff
        case .poweredOn:
            self = .poweredOn
        @unknown default:
            return nil
        }
    }
}
