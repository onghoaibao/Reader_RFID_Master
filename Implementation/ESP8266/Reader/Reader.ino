#include "headerFile.h"
#define ENABLE false

SoftwareSerial readerSerial(D2, D3); // RX, TX
String dataReader = "";
long timeout = 0;
bool st = false;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  SPIFFS.begin();
  readerSerial.begin(115200);
  pinMode(D2, INPUT);
  pinMode(D3, OUTPUT);
  Serial.println();
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
      //Serial.print(c);
    }
    else {
      //Serial.println():
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
