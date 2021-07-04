EXPORT_DIRECTORY = .

framework:
	swift package generate-xcodeproj --skip-extra-files
	xcodebuild -project UnityVideoCreator.xcodeproj -scheme UnityVideoCreator-Package -configuration Release -sdk iphoneos CONFIGURATION_BUILD_DIR=Build
