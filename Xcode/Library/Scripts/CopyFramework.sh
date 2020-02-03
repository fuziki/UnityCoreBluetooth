mkdir -p ${OUTPUT_PATH}
cp -r ${TARGET_BUILD_DIR}/${TARGET_NAME}.framework/. ${OUTPUT_PATH}/${TARGET_NAME}.framework
cp ${PROJECT_DIR}/Library/Platforms/iOS/template.framework.meta ${OUTPUT_PATH}/${TARGET_NAME}.framework.meta
