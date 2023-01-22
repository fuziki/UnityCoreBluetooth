UNITY_APP=/Applications/Unity/Hub/Editor/2021.3.6f1/Unity.app
BUILD_DIR=Build
TARGET_NAME=UnityCoreBluetooth
UNITY_PROJECT_PATH=Examples/UnityExample
ASSET_PATH=Assets/Plugins/UnityCoreBluetooth/Plugins

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

bundle:
	xcodebuild \
		-workspace UnityCoreBluetooth.xcworkspace \
		-scheme mcUnityCoreBluetooth-release \
		-configuration Release \
		-sdk macosx \
		CONFIGURATION_BUILD_DIR=$(CURDIR)/${BUILD_DIR}

copy:
	cp -r ${BUILD_DIR}/${TARGET_NAME}.framework ${UNITY_PROJECT_PATH}/${ASSET_PATH}/iOS/
	cp -r ${BUILD_DIR}/mc${TARGET_NAME}.bundle ${UNITY_PROJECT_PATH}/${ASSET_PATH}/macOS/

package:
	${UNITY_APP}/Contents/MacOS/Unity \
		-exportPackage ${ASSET_PATH} $(CURDIR)/${BUILD_DIR}/${TARGET_NAME}.unitypackage \
		-projectPath ${UNITY_PROJECT_PATH} \
		-batchmode \
		-nographics \
		-quit
	echo exported ${BUILD_DIR}/${TARGET_NAME}.unitypackage

lint:
	xcrun --sdk macosx \
		swift run --package-path ./tools swiftlint \
		--config tools/.swiftlint.yml \
		autocorrect --format
	xcrun --sdk macosx \
		swift run --package-path ./tools swiftlint \
		--config tools/.swiftlint.yml
