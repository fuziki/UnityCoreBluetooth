#include <M5StickCPlus.h>
#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEServer.h>
#include <BLE2902.h>

#define SERVICE_UUID        "FE55"
#define CHARACTERISTIC_UUID "beb5483e-36e1-4688-b7f5-ea07361b26a8"
#define SERVER_NAME         "M5StickC"

/* ================ BLE ================ */
BLEServer* pServer = NULL;
BLECharacteristic* pCharacteristic = NULL;
bool deviceConnected = false;

class MyServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* pServer) {
      M5.Lcd.fillScreen(BLACK);
      M5.Lcd.setCursor(0, 0);
      M5.Lcd.setTextSize(2);
      M5.Lcd.println("connect");
      deviceConnected = true;
    };
    void onDisconnect(BLEServer* pServer) {
      M5.Lcd.setCursor(0, 0);
      M5.Lcd.setTextSize(2);
      M5.Lcd.println("disconnect");
      deviceConnected = false;
    }
};

class MyCallbacks: public BLECharacteristicCallbacks {
  void onRead(BLECharacteristic *pCharacteristic) {
    M5.Lcd.println("read");
    std::string value = pCharacteristic->getValue();
    M5.Lcd.println(value.c_str());
  }
  void onWrite(BLECharacteristic *pCharacteristic) {
    std::string value = pCharacteristic->getValue();
    M5.Lcd.setCursor(0, 160);
    M5.Lcd.setTextSize(3);
    M5.Lcd.println("written");
    M5.Lcd.setTextSize(5);
    M5.Lcd.println(value.c_str());
  }
};

/* ================ Properties ================ */
int counter = 0;

/* ================ Arduino ================ */
void setup() {
  M5.begin();
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.setTextSize(2);
  M5.Lcd.println("Start!");

  setupBle();
}

void loop() {
  if (deviceConnected) {
    if(M5.BtnB.wasPressed()) {
      pCharacteristic->setValue(counter);
      pCharacteristic->notify();
      M5.Lcd.setCursor(0, 60);
      M5.Lcd.setTextSize(3);
      M5.Lcd.println("notify");
      M5.Lcd.setTextSize(5);
      M5.Lcd.println(counter);
      counter += 1;
    }
  }
  M5.update();
}

/* ================ BLE ================ */
void setupBle() {
  BLEDevice::init(SERVER_NAME);
  BLEServer *pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());
  BLEService *pService = pServer->createService(SERVICE_UUID);
  pCharacteristic = pService->createCharacteristic(
                                         CHARACTERISTIC_UUID,
                                         BLECharacteristic::PROPERTY_READ |
                                         BLECharacteristic::PROPERTY_WRITE |
                                         BLECharacteristic::PROPERTY_WRITE_NR |
                                         BLECharacteristic::PROPERTY_NOTIFY |
                                         BLECharacteristic::PROPERTY_INDICATE
                                       );
  pCharacteristic->setCallbacks(new MyCallbacks());
  pCharacteristic->addDescriptor(new BLE2902());

  pService->start();
  BLEAdvertising *pAdvertising = pServer->getAdvertising();
  pAdvertising->start();  
}
