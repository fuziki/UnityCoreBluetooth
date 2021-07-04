//
//  CBCharacteristic+propertyString.swift
//  
//
//  Created by fuziki on 2021/06/27.
//

import CoreBluetooth

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
