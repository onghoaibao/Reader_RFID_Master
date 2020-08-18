#ifndef __HEADER__
#define __HEADER__

#include "Arduino.h"
#include <SoftwareSerial.h>

// define TX,RX pin for RFID
#define TX_RFID 10
#define RX_RFID 11
SoftwareSerial RFID(RX_RFID, TX_RFID);

// define TX,RX pin for BLE103
#define TX_BLE 3
#define RX_BLE 2
SoftwareSerial BLE103(RX_BLE, TX_BLE);

// setup UART
void setupRFID();
void setupBLE103();

//functional RFID
void readDataRFID();
String hexToASCII(unsigned long n);

//function 
void sendCommandAT();
void readDataBLE();
void sendDataToPoint(String data);

//Data structure
typedef struct Nodes{
   String sNodes_Data;
   struct Nodes *next;
} Node;
Node* head_data = NULL;

bool isElementInStruct(String str);
void deleteList();
void append(Node** head, String sData);

#endif
