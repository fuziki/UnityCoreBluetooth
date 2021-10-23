//
//  UnityCoreBluetooth.swift
//  ExampleMacOS
//
//  Created by fuziki on 2019/09/09.
//  Copyright © 2019 fuziki.factory. All rights reserved.
//

import CoreBluetooth
import Foundation

internal class UnityCoreBluetoothManager: NSObject {
    public static let shared: UnityCoreBluetoothManager = UnityCoreBluetoothManager()

    public var onErrorMessageHandler: ((_ message: String) -> Void)?
    public var onUpdateStateHandler: ((_ state: String) -> Void)?
    public var onDiscoverPeripheralHandler: ((_ peripheral: CBPeripheral) -> Void)?
    public var onConnectPeripheralHandler: ((_ peripheral: CBPeripheral) -> Void)?
    public var onDiscoverServicelHandler: ((_ service: CBService) -> Void)?
    public var onDiscoverCharacteristiclHandler: ((_ characteristic: CBCharacteristic) -> Void)?
    public var onUpdateValueHandler: ((_ characteristic: CBCharacteristic, _ value: Data) -> Void)?

    private var manager: CBCentralManager?
    private var peripherals: [String: CBPeripheral] = [:]

    override public init() {
        super.init()
    }

    public func startCoreBluetooth() {
        manager = CBCentralManager(delegate: self, queue: nil)
    }

    public func stopCoreBluetooth() {
        peripherals.removeAll()
        manager = nil
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

extension UnityCoreBluetoothManager: CBCentralManagerDelegate {
    public func centralManagerDidUpdateState(_ central: CBCentralManager) {
        if let state = UCBManagerState(state: central.state) {
            self.onUpdateStateHandler?(state.rawValue)
        }
    }

    public func centralManager(_ central: CBCentralManager, didDiscover peripheral: CBPeripheral, advertisementData: [String: Any], rssi RSSI: NSNumber) {
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

extension UnityCoreBluetoothManager: CBPeripheralDelegate {
    public func peripheral(_ peripheral: CBPeripheral, didDiscoverCharacteristicsFor service: CBService, error: Error?) {
        for c in service.characteristics ?? [] {
            self.onDiscoverCharacteristiclHandler?(c)
        }
    }

    public func peripheral(_ peripheral: CBPeripheral, didUpdateValueFor characteristic: CBCharacteristic, error: Error?) {
        self.onUpdateValueHandler?(characteristic, characteristic.value ?? Data())
    }
}
