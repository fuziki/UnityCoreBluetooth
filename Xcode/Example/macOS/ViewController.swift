//
//  ViewController.swift
//  PROJECT_NAME
//
//  Created by AUTHOR on YYYY/MM/DD.
//

import Cocoa

class ViewController: NSViewController {

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view.

        let a = add_one(32)
        print("a: \(a)")
    }

    override var representedObject: Any? {
        didSet {
        // Update the view, if already loaded.
        }
    }
}
