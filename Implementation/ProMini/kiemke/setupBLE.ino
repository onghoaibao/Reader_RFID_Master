#include "Header.h"
const byte rs = 6;
void setupBLE103() {
  pinMode(rs, OUTPUT);
  digitalWrite(rs, 1);
  BLE103.begin(57600);
  delay(2000);
  modeSlave();
  //iBeaconMode();
}

void readDataBLE() {
  delay(100);
  while (BLE103.available() > 0) {
    char c = BLE103.read();
    Serial.print(c);
  }
}

void sendDataToPoint(String data){
  BLE103.print(data);
}

void enableATCommand() {
  BLE103.print("+++a");
  delay(10);
  readDataBLE();
}

void sendCommandAT(String cmd) {
  BLE103.print(cmd);
  BLE103.write(0x0D);
  BLE103.write(0x0A);
  delay(100);
  readDataBLE();
}

void resetBLE() {
  digitalWrite(9, 0);
  delay(10);
  digitalWrite(9, 1);
  delay(100);
  readDataBLE();
}

// IBEACON mode
String IBEACON_COMMAND = "AT+IBEACON=B9407F30F5F8466EAFF925556B57FE6D,1234,5467,175";

void iBeaconMode() {
  BLE103.print("+++a");
  delay(1000);
  sendCommandAT("AT+MODE=B");
  delay(1000);
  sendCommandAT("AT+Z");
  delay(1000);
  BLE103.print("+++a");
  delay(1000);
  sendCommandAT(IBEACON_COMMAND);
  delay(1000);
  sendCommandAT("AT+Z");
  delay(1000);
}

/*-------------------- SLAVE MODE --------------------*/
// Config Slave mode
void modeSlave() {
  resetBLE();
  enableATCommand();
  delay(200);
  sendCommandAT("AT+NAME=Medical_BLE");
  sendCommandAT("AT+PASSEN=OFF");
  sendCommandAT("AT+MODE=S");
  sendCommandAT("AT+Z");
}
