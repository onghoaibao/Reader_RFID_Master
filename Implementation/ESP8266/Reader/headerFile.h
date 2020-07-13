#ifndef __HEADER__
#define __HEADER__

#include "Arduino.h"
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <ESP8266WebServer.h>
#include <FS.h>
#include <EEPROM.h>
#include <SoftwareSerial.h>
#include <ThingSpeak.h>

#define SIZE 60
#define SIZE_SSID 30
#define SIZE_PASS 20
#define ADDRESS_SSID_WIFI 0 
#define ADDRESS_PASS_WIFI 35

void serialFlush();
String hexToASCII(unsigned long n);

// Setup wifi and init thingspeak
void setupWiFi();
void handleClientServer();
void client_Sendata();

//
typedef struct Nodes
{
   String sNodes_Data;
   struct Nodes *next;
} Node;
void append(Node** head, String sData);
void deleteList();
bool isElementInStruct(Node* head_ref, String str);
void displayStruct();
void appendWithoutRepeat(String str);

// FILE FS
boolean saveFile(String fileName, const char* content, uint16_t len);
String getStringFile(String fileName);
void removeContentFile(String fileName);
void listALlFile(String path);
String read200Line();

//EEPROM
void savePASSWifi(String pass);
void saveSSIDWifi(String ssid);
void accessWifi(String ssid, String pass);
String getSSID_WIFI();
String getPASS_WIFI();
void clearEEPROM(int _address, int _size);
String readEEPROM(int _address, int _size);
void writeEEPROM(String str, int _address, int _SIZE_);

#endif
