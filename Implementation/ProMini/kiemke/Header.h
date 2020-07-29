#ifndef __HEADER__
#define __HEADER__

#include "Arduino.h"
#include <SoftwareSerial.h>

// define TX,RX pin for RFID
#define TX_RFID 4
#define RX_RFID 3
SoftwareSerial RFID(RX_RFID, TX_RFID);

// define TX,RX pin for BLE103
#define TX_BLE 8
#define RX_BLE 7
SoftwareSerial BLE103(RX_BLE, TX_BLE);

// setup UART
void setupRFID();
void setupBLE103();

//functional support RFID
void readDataRFID();
String hexToASCII(unsigned long n);

//
void sendCommandAT();
void readDataBLE();
void sendDataToPoint(String data);
#endif
