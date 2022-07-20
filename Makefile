BUILD_DIR=Build
TARGET_NAME=UnityCoreBluetooth

framework:
	swift package generate-xcodeproj --skip-extra-files
	xcodebuild \
		-project ${TARGET_NAME}.xcodeproj \
		-scheme ${TARGET_NAME}-Package \
		-configuration Release \
		-sdk iphoneos \
		ENABLE_BITCODE=YES \
		BITCODE_GENERATION_MODE=bitcode \
		OTHER_CFLAGS=-fembed-bitcode \
		BUILD_LIBRARY_FOR_DISTRIBUTION=YES \
		CONFIGURATION_BUILD_DIR=${BUILD_DIR}

xcframework: framework
	xcodebuild -create-xcframework \
		-framework ${BUILD_DIR}/${TARGET_NAME}.framework \
		-debug-symbols $(CURDIR)/${BUILD_DIR}/${TARGET_NAME}.framework.dSYM \
		-debug-symbols $(CURDIR)/${BUILD_DIR}/*.bcsymbolmap \
		-output ${BUILD_DIR}/${TARGET_NAME}.xcframework

bundle:
	xcodebuild \
		-workspace UnityCoreBluetooth.xcworkspace \
		-scheme mcUnityCoreBluetooth-release \
		-configuration Release \
		-sdk macosx \
		CONFIGURATION_BUILD_DIR=${BUILD_DIR}

lint:
	xcrun --sdk macosx \
		swift run --package-path ./tools swiftlint \
		--config tools/.swiftlint.yml \
		autocorrect --format
	xcrun --sdk macosx \
		swift run --package-path ./tools swiftlint \
		--config tools/.swiftlint.yml
