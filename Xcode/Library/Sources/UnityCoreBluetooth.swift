//
//  UnityCoreBluetooth.swift
//  ExampleMacOS
//
//  Created by fuziki on 2019/09/09.
//  Copyright © 2019 fuziki.factory. All rights reserved.
//

import CoreBluetooth
import Foundation

fileprivate enum UCBManagerState: String {
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

fileprivate enum UCBCharacteristicProperties: String, CaseIterable {
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

extension CBCharacteristic {
    @objc
    public var propertyString: String {
        var ret: [UCBCharacteristicProperties] = []
        for property in UCBCharacteristicProperties.allCases {
            if self.properties.rawValue & property.toCBCharacteristicProperties.rawValue != 0 {
                ret.append(property)
            }
        }
        return ret.map({$0.rawValue}).joined(separator: ",")
    }
}


@objcMembers
public class UnityCoreBluetooth: NSObject {
    private var onErrorMessageHandler: ((_ message: String) -> Void)? = nil
    public func onErrorMessage(handler: @escaping (_ message: String) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onErrorMessageHandler = handler
        }
    }
    
    private var onUpdateStateHandler: ((_ state: String) -> Void)? = nil
    public func onUpdateState(handler: @escaping (_ state: String) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onUpdateStateHandler = handler
        }
    }
    
    private var onDiscoverPeripheralHandler: ((_ peripheral: CBPeripheral) -> Void)? = nil
    public func onDiscoverPeripheral(handler: @escaping (_ peripheral: CBPeripheral) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onDiscoverPeripheralHandler = handler
        }
    }
    
    private var onConnectPeripheralHandler: ((_ peripheral: CBPeripheral) -> Void)? = nil
    public func onConnectPeripheral(handler: @escaping (_ peripheral: CBPeripheral) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onConnectPeripheralHandler = handler
        }
    }
    
    private var onDiscoverServicelHandler: ((_ service: CBService) -> Void)? = nil
    public func onDiscoverService(handler: @escaping (_ services: CBService) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onDiscoverServicelHandler = handler
        }
    }
    
    private var onDiscoverCharacteristiclHandler: ((_ characteristic: CBCharacteristic) -> Void)? = nil
    public func onDiscoverCharacteristic(handler: @escaping (_ characteristic: CBCharacteristic) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onDiscoverCharacteristiclHandler = handler
        }
    }
    
    private var onUpdateValueHandler: ((_ characteristic: CBCharacteristic, _ value: Data) -> Void)? = nil
    public func onUpdateValue(handler: @escaping (_ characteristic: CBCharacteristic, _ value: Data) -> Void) {
        DispatchQueue.main.async { [weak self] in
            self?.onUpdateValueHandler = handler
        }
    }
    
    private var manager: CBCentralManager?
    private var peripherals: [String: CBPeripheral] = [:]
    
    override public init() {
        super.init()
    }
    
    public func startCoreBluetooth() {
        manager = CBCentralManager(delegate: self, queue: nil)
    }
    
    public func startScan() {
        if let manager = self.manager, manager.isScanning == false {
            manager.scanForPeripherals(withServices: nil, options: nil)
        }
    }
    
    public func stopScan() {
        manager?.stopScan()
    }
    
    public func connect(peripheral: CBPeripheral) {
        if let p = peripherals[peripheral.identifier.uuidString] {
            manager?.connect(p, options: nil)
        }
    }
    
    public func clearPeripherals() {
        peripherals.removeAll()
    }
}

extension UnityCoreBluetooth: CBCentralManagerDelegate {
    public func centralManagerDidUpdateState(_ central: CBCentralManager) {
        if let state = UCBManagerState(state: central.state) {
            self.onUpdateStateHandler?(state.rawValue)
        }
    }
    
    public func centralManager(_ central: CBCentralManager, didDiscover peripheral: CBPeripheral, advertisementData: [String : Any], rssi RSSI: NSNumber) {
        peripherals[peripheral.identifier.uuidString] = peripheral
        self.onDiscoverPeripheralHandler?(peripheral)
    }
    
    public func centralManager(_ central: CBCentralManager, didConnect peripheral: CBPeripheral) {
        peripheral.delegate = self
        self.onConnectPeripheralHandler?(peripheral)
    }
    
    public func centralManager(_ central: CBCentralManager, didFailToConnect peripheral: CBPeripheral, error: Error?) {
        self.onErrorMessageHandler?("connection failed")
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverServices error: Error?) {
        for s in peripheral.services ?? [] {
            self.onDiscoverServicelHandler?(s)
        }
    }
}

extension UnityCoreBluetooth: CBPeripheralDelegate {
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverCharacteristicsFor service: CBService, error: Error?) {
        for c in service.characteristics ?? [] {
            self.onDiscoverCharacteristiclHandler?(c)
        }
    }
    
    public func peripheral(_ peripheral: CBPeripheral, didUpdateValueFor characteristic: CBCharacteristic, error: Error?) {
        self.onUpdateValueHandler?(characteristic, characteristic.value ?? Data())
    }
}
