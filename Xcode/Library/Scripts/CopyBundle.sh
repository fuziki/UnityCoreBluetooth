rm -r ${OUTPUT_PATH}
rm -r ${SCRIPTS_OUTPUT_PATH}
mkdir -p ${OUTPUT_PATH}
mkdir -p ${SCRIPTS_OUTPUT_PATH}
cp -r ${TARGET_BUILD_DIR}/${PRODUCT_NAME}.bundle/. ${OUTPUT_PATH}/${PRODUCT_NAME}.bundle
cd ${PROJECT_DIR}/Library/Platforms/macOS/Bundle/cs_maker
./cs_maker ${PROJECT_NAME} ${TARGET_NAME} ${PRODUCT_NAME}
cp ${PROJECT_DIR}/Library/Platforms/macOS/Bundle/${TARGET_NAME}.cs ${SCRIPTS_OUTPUT_PATH}/${TARGET_NAME}.cs
