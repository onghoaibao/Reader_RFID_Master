#ifndef __HEADER__
#define __HEADER__

#include "Arduino.h"
#include <ESP8266WiFi.h>
//#include <ESP8266HTTPClient.h>
#include <ESP8266WebServer.h>
#include <FS.h>
#include <EEPROM.h>
#include <SoftwareSerial.h>
<<<<<<< HEAD
//#include <WiFiClientSecure.h>
#include "HTTPSRedirect.h"
=======
#include <WiFiClientSecure.h>
#include "HTTPSRedirect.h"

#define ENABLE false
String dataReader = "";
long timeout = 0;
bool st = false;
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a

// Init server and client
ESP8266WebServer sv(9090);

#define SIZE 200
#define SIZE_SSID 30
#define SIZE_PASS 20
#define ADDRESS_SSID_WIFI 0 
#define ADDRESS_PASS_WIFI 35
#define ADDRESS_PHONE_1   70
#define ADDRESS_PHONE_2   85
#define ADDRESS_PHONE_3   100

void serialFlush();
String hexToASCII(unsigned long n);

// declare variables for the google sheet
const char* NAME_WIFI_ESP8266 = "ESP_Manage";
const char* PASS_WIFI_ESP8266  = "123456789";

String ssid =  "";
String pass =  "";

String bLogin = "";
const char* __host__ = "script.google.com";
const int __httpsPort__ = 443; 
const char *GScriptId = "AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0";
const char* fingerprint = "";
String URL_HTTPS = String("/macros/s/") + GScriptId + "/exec?code=";
String payload_base =  "{\"command\": \"appendRow\", \"sheet_name\": \"Sheet1\", \"values\": ";
String payload = "";

void setupWiFi();
void handleClientServer();
<<<<<<< HEAD
void client_Sendata(String code);
void initHTTPS();
String getAllDeviveFromDataBase();
=======
void client_Sendata();
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a
String scanWifi();

//
typedef struct Nodes
{
   String sNodes_Data;
   struct Nodes *next;
} Node;

Node* head_data = NULL;

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
void saveNumberPhone(String number1, String number2, String number3);

//EEPROM
void savePASSWifi(String pass);
void saveSSIDWifi(String ssid);
void accessWifi(String ssid, String pass);
String getSSID_WIFI();
String getPASS_WIFI();
void clearEEPROM(int _address, int _size);
String readEEPROM(int _address, int _size);
void writeEEPROM(String str, int _address, int _SIZE_);


//Google Sheet
String getURL(String maThietBi, String properties_name, String properties_sx,
              String trangthai, String dvMuon, String ngayTra, String lsdc, String hientai);
String getStatusMaThietBi(String maThietbi);
String client_Sendata(String maThietbi);
String getAllCodeDevice();

// Sim800L
String arrPhone[3];
SoftwareSerial SIM800L(D6, D7); // RX, TX
void sendAT_Sim800l(String cmd);
void readInfoSim800l();
void sendMessage();

// Reader RFID
SoftwareSerial RFID(D2, D3); // RX, TX
<<<<<<< HEAD
void readDataRFID();
void initReaderRFID();
String hexToASCII(unsigned long n);
=======
void initReaderRFID();
String getData();
String hexToASCII(unsigned long n);
void readDataRFID();
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a

#endif
