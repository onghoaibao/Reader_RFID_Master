#include "headerFile.h"

SoftwareSerial readerSerial(D3, D2); // RX, TX
String dataReader = "";
long timeout = 0;
bool st = false;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  SPIFFS.begin();
  readerSerial.begin(115200);
  pinMode(D3, INPUT);
  pinMode(D2, OUTPUT);
  Serial.println(); 
  setupWiFi();
  Serial.println("\n------ Hello ------");
}

void loop() {
  // put your main code here, to run repeatedly:
  handleClientServer();
  while (readerSerial.available() > 0) {
    if (dataReader.length() <= 28){
        char c = readerSerial.read();
        dataReader += hexToASCII(c);
    }
    else{
      //Serial.println("Data Reader: " + dataReader + "  len: " + String(dataReader.length()));
      char c = readerSerial.read(); 
    }
  }

  delay(1);
  timeout++;
  if (dataReader.length() == 30 && dataReader != "") {
    dataReader = dataReader.substring(6, dataReader.length());
    appendWithoutRepeat(dataReader);
    displayStruct();
    //Serial.println("Data Reader: " + dataReader);
    if(st){
      st = false;
      timeout = 0;
      //ThingSpeak.setField(1, dataReader);
      //client_Sendata();
    }
    dataReader = "";
  }
  if(timeout == 5000){
    Serial.println("Before del: " );
    displayStruct();
    deleteList();
    Serial.println("After del: " );
    displayStruct();
    st = true;
    serialFlush();
    timeout = 0;
  }
}
