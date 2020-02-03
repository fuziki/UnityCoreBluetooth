//
//  AddOne.swift
//  PROJECT_NAME
//
//  Created by AUTHOR on YYYY/MM/DD.
//

import Foundation

@objcMembers
public class AddOne: NSObject {
    public static func add(num: Int) -> Int {
        return num + 1
    }

    public static func getOne() -> Int {
        return 1
    }
}

public func add_two(num: Int) -> Int {
    return num + 2
}
