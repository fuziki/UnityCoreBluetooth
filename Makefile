EXPORT_DIRECTORY = .

framework:
	swift package generate-xcodeproj --skip-extra-files
	xcodebuild -project UnityCoreBluetooth.xcodeproj -scheme UnityCoreBluetooth-Package -configuration Release -sdk iphoneos CONFIGURATION_BUILD_DIR=Build
