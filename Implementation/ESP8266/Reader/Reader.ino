#include "headerFile.h"
#define ENABLE false


String dataReader = "";
long timeout = 0;
bool st = false;

void setup() {
  delay(1000);
  Serial.begin(115200);
  Serial.println("\n-------------------\n");
  initSim800l();
  initReaderRFID();
  SPIFFS.begin();
  setupWiFi();
  Serial.println("\n------ Hello ------");
}

int sStatus = 1;

void loop() {
  // put your main code here, to run repeatedly:
  handleClientServer();
  while (readerSerial.available() > 0) {
    if (dataReader.length() <= 28) {
      char c = readerSerial.read();
      dataReader += hexToASCII(c);
    }
    else {
      Serial.println("Data Reader 1: " + dataReader + "  len: " + String(dataReader.length()));
      char c = readerSerial.read();
    }
    timeout = 0;
  }

  delay(1);
  timeout++;
  if (dataReader.length() == 30 && dataReader != "") {
    dataReader = dataReader.substring(6, dataReader.length());
    appendWithoutRepeat(dataReader);
    //displayStruct();
    if (st) {
      st = false;
      timeout = 0;
    }
    dataReader = "";
    timeout = 0;
  }
  
  if (timeout == 30000 && ENABLE) {
    Node* head_data_temp = head_data;
    while (head_data_temp != NULL) {
      String data = head_data_temp->sNodes_Data;
       //getStatusMaThietBi(data);
      Serial.println("sStatus: " + sStatus);    
      if(sStatus == 1){
        client_Sendata(data, "true");
        sStatus = 0;
      }
      else{
        client_Sendata(data, "false");
        sStatus = 1;
      }
      Serial.println("data send: " + data);
      head_data_temp = head_data_temp->next;
      delay(2000);
    }
    
    //Serial.println("Before del: " );
    
    //displayStruct();
    deleteList();
    //Serial.println("After del: " );
    //displayStruct();
    st = true;
    serialFlush();
    timeout = 0;
  }
}
