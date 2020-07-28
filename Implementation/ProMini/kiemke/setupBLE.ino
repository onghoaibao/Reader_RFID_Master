#include "Header.h"

void setupBLE103() {
  pinMode(9, OUTPUT);
  digitalWrite(9, 1);
  BLE103.begin(57600);
  delay(2000);
  modeSlave();
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
