//
//  GameViewController.swift
//  DaydreamExample iOS
//
//  Created by fuziki on 2021/06/26.
//

import Foundation
import UIKit
import SceneKit

class GameViewController: UIViewController {

    @IBOutlet weak var gameView: SCNView!
    @IBOutlet weak var label: UILabel!

    var gameController: GameController!
    let bluetoothService = BluetoothService.shared

    override func viewDidLoad() {
        super.viewDidLoad()

        self.gameController = GameController(sceneRenderer: gameView)

        // Allow the user to manipulate the camera
        self.gameView.allowsCameraControl = true

        // Show statistics such as fps and timing information
        self.gameView.showsStatistics = true

        // Configure the view
        self.gameView.backgroundColor = UIColor.black

        // Add a tap gesture recognizer
        let tapGesture = UITapGestureRecognizer(target: self, action: #selector(handleTap(_:)))
        var gestureRecognizers = gameView.gestureRecognizers ?? []
        gestureRecognizers.insert(tapGesture, at: 0)
        self.gameView.gestureRecognizers = gestureRecognizers

        bluetoothService.onUpdateValue = { [weak self] (value: String) in
            DispatchQueue.main.async { [weak self] in
                self?.label.text = value
            }
        }

        bluetoothService.start()
    }

    @objc
    func handleTap(_ gestureRecognizer: UIGestureRecognizer) {
        // Highlight the tapped nodes
        let p = gestureRecognizer.location(in: gameView)
        gameController.highlightNodes(atPoint: p)
    }

    override var shouldAutorotate: Bool {
        return true
    }

    override var supportedInterfaceOrientations: UIInterfaceOrientationMask {
        if UIDevice.current.userInterfaceIdiom == .phone {
            return .allButUpsideDown
        } else {
            return .all
        }
    }

    override var prefersStatusBarHidden: Bool {
        return true
    }

}
