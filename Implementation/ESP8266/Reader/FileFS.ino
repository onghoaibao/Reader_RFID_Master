#include "headerFile.h"

const char* NAME_WIFI = "ESP_Manager";
const char* PASS_WFI  = "123456789";

boolean saveFile(String fileName, const char* content, uint16_t len) {
  SPIFFS.begin();
  File f = SPIFFS.open(String("/") + fileName, "a");
  if (!f) {
    f.close();
    return false;
  }
  else {
    f.write((const uint8_t*)content, len);
  }
  f.close();
  return true;
}

String getStringFile(String fileName) {
  SPIFFS.begin();
  File f = SPIFFS.open(String("/") + fileName, "r");
  if (!f) {
    f.close();
    return "";
  }
  String ret = f.readString();
  f.close();
  return ret;
}

void removeContentFile(String fileName) {
  SPIFFS.begin();
  File f = SPIFFS.open(String("/") + fileName, "w");
  f.close();
}

void listALlFile(String path){
  String str = "";
  Dir dir = SPIFFS.openDir("/");
  while (dir.next()) {
      str = dir.fileName();
      Serial.println("File name: " + String(str));
  }
}

void removeFile(String fileName){
  bool b = SPIFFS.remove(fileName);
  Serial.println("Remove File name " +  fileName + " -> " + String(b));
}

void writeEEPROM(String str, int _address, int _SIZE_){
  int _size = str.length();
  if(_size <= _SIZE_){
      clearEEPROM(_address, _SIZE_);   
      delay(20);
      for(int i=0; i<_size; i++){
        EEPROM.write( _address + i, str.charAt(i));
      }
      delay(20);
      EEPROM.commit();   
      return;
  }
  else{
    Serial.println("Cannot save data");
  }
}

String readEEPROM(int _address, int _size){
  EEPROM.begin(SIZE);
  delay(50);
  String _buffer = "";
  for(int i=0; i<_size; i++){
    char c = (char)EEPROM.read(_address + i);
    if(c == NULL){
       break;
    }
    _buffer += c;
  }
  Serial.println("Data of _buffer: " + _buffer + "  len: " + _buffer.length());
  return _buffer;
}

void clearEEPROM(int _address, int _size){
  EEPROM.begin(SIZE);
  for(int i=0; i<_size; i++){
    EEPROM.write(_address + i, (char)NULL);
  }
  EEPROM.commit();
  delay(50);
}


void saveSSIDWifi(String ssid){
  writeEEPROM(ssid, ADDRESS_SSID_WIFI, SIZE_SSID);
}

void saveNumberPhone(String number1, String number2, String number3){
  writeEEPROM(number1, ADDRESS_PHONE_1, 15);
  writeEEPROM(number2, ADDRESS_PHONE_2, 15);
  writeEEPROM(number3, ADDRESS_PHONE_3, 15);

  String n1 = readEEPROM(ADDRESS_PHONE_1, 15);
  String n2 = readEEPROM(ADDRESS_PHONE_2, 15);
  String n3 = readEEPROM(ADDRESS_PHONE_3, 15);
  Serial.println("n1= " + String(n1));
  Serial.println("n2= " + String(n2));
  Serial.println("n3= " + String(n3));
}

void savePASSWifi(String pass){
  writeEEPROM(pass, ADDRESS_PASS_WIFI, SIZE_PASS);
}

void accessWifi(String ssid, String pass){
  saveSSIDWifi(ssid);
  savePASSWifi(pass);
}

String getSSID_WIFI(){
  return readEEPROM(ADDRESS_SSID_WIFI, SIZE_SSID);
}

String getPASS_WIFI(){
  return readEEPROM(ADDRESS_PASS_WIFI, SIZE_PASS);
}

String read200Line(){
  File f = SPIFFS.open(String("/CodeDataRFID.txt"), "r");
  if (!f) {
    f.close();
    return "";
  }
  String sCode = "";
  int ret = f.read();
  int iCouter = 0;
  while(ret != -1 && iCouter < 200){   
    char c = (char)ret;
    sCode += c;
    if(c == '\n')
      iCouter+=1;
    ret = f.read();
  }
  Serial.println("sCode: " + sCode + "  len: " + String(sCode.length()/25));
  f.close();
  return sCode;
}
