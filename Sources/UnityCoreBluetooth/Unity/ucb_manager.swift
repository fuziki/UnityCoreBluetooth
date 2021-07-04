//
//  ucb_manager.swift
//  
//
//  Created by fuziki on 2021/07/04.
//

import CoreBluetooth
import Foundation


//MARK:- manager
@_cdecl("ucb_manager_shared_instantiate")
public func ucb_manager_shared_instantiate() {
    UnityCoreBluetoothManager.shared.startCoreBluetooth()
}

@_cdecl("ucb_manager_shared_release")
public func ucb_manager_shared_release() {
    UnityCoreBluetoothManager.shared.stopCoreBluetooth()
}

@_cdecl("ucb_manager_shared_startScan")
public func ucb_manager_shared_startScan() {
    UnityCoreBluetoothManager.shared.startScan()
}

@_cdecl("ucb_manager_shared_stopScan")
public func ucb_manager_shared_stopScan() {
    UnityCoreBluetoothManager.shared.stopScan()
}

@_cdecl("ucb_manager_shared_connectWithPeripheral")
public func ucb_manager_shared_connectWithPeripheral(_ peripheral: UnsafePointer<CBPeripheral>) {
    UnityCoreBluetoothManager.shared.connect(peripheral: peripheral.pointee)
}

//MARK:- register callback
@_cdecl("ucb_manager_shared_register_onUpdateState")
public func ucb_manager_shared_register_onUpdateState(_ handler: @escaping @convention(c) (UnsafePointer<CChar>?) -> Void) {
    UnityCoreBluetoothManager.shared.onUpdateStateHandler = { (state: String) in
        handler((state as NSString).utf8String)
    }
}

@_cdecl("ucb_manager_shared_register_onDiscoverPeripheral")
public func ucb_manager_shared_register_onDiscoverPeripheral(_ handler: @escaping @convention(c) (UnsafePointer<CBPeripheral>) -> Void) {
    UnityCoreBluetoothManager.shared.onDiscoverPeripheralHandler = { (peripheral: CBPeripheral) in
        let ptr = Unmanaged.passUnretained(peripheral).toOpaque()
        handler(ptr.bindMemory(to: CBPeripheral.self, capacity: 1))
    }
}

@_cdecl("ucb_manager_shared_register_onConnectPeripheral")
public func ucb_manager_shared_register_onConnectPeripheral(_ handler: @escaping @convention(c) (UnsafePointer<CBPeripheral>) -> Void) {
    UnityCoreBluetoothManager.shared.onConnectPeripheralHandler = { (peripheral: CBPeripheral) in
        let ptr = Unmanaged.passUnretained(peripheral).toOpaque()
        handler(ptr.bindMemory(to: CBPeripheral.self, capacity: 1))
    }
}

@_cdecl("ucb_manager_shared_register_onDiscoverService")
public func ucb_manager_shared_register_onDiscoverService(_ handler: @escaping @convention(c) (UnsafePointer<CBService>) -> Void) {
    UnityCoreBluetoothManager.shared.onDiscoverServicelHandler = { (service: CBService) in
        let ptr = Unmanaged.passUnretained(service).toOpaque()
        handler(ptr.bindMemory(to: CBService.self, capacity: 1))
    }
}

@_cdecl("ucb_manager_shared_register_onDiscoverCharacteristic")
public func ucb_manager_shared_register_onDiscoverCharacteristic(_ handler: @escaping @convention(c) (UnsafePointer<CBCharacteristic>) -> Void) {
    UnityCoreBluetoothManager.shared.onDiscoverCharacteristiclHandler = { (characteristic: CBCharacteristic) in
        let ptr = Unmanaged.passUnretained(characteristic).toOpaque()
        handler(ptr.bindMemory(to: CBCharacteristic.self, capacity: 1))
    }
}

@_cdecl("ucb_manager_shared_register_onUpdateValue")
public func ucb_manager_shared_register_onUpdateValue(_ handler: @escaping @convention(c) (UnsafePointer<CBCharacteristic>, UnsafePointer<UInt8>, CLong) -> Void) {
    UnityCoreBluetoothManager.shared.onUpdateValueHandler = { (characteristic: CBCharacteristic, data: Data) in
        data.withUnsafeBytes { (unsafeBytes) in
            let bytes = unsafeBytes.bindMemory(to: UInt8.self).baseAddress!
            let ptr = Unmanaged.passUnretained(characteristic).toOpaque()
            handler(ptr.bindMemory(to: CBCharacteristic.self, capacity: 1), bytes, data.count)
        }
    }
}
