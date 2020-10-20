#include "headerFile.h"

#define ENABLE_DEBUG false
String dataReader = "";
long timeout = 0;
bool st = false;

void setup() {
  delay(500);
  Serial.begin(115200);
  Serial.println("\n-------------------\n");
  initReaderRFID();
  initSim800l();
  SPIFFS.begin();
  setupWiFi();
  Serial.println("\n------ Hello ------");
  delay(2000);
  //getAllDeviveFromDataBase();
  serialFlush();
}

int sStatus = 1;

void loop() {
  // put your main code here, to run repeatedly:
  handleClientServer();
  readDataRFID();
  if (dataReader.length() == 30) {
    dataReader = getData();
    String s1 = dataReader.substring(6, 10);
    String s2 = dataReader.substring(dataReader.length() - 4, dataReader.length());
    if (s1 == s2) {
      Serial.println("Data Reader IF: " + s1 + "  " + s2 + "  len: " + String(s1.length()) + " - " + String(s2.length()));
      appendWithoutRepeat(s1);
    }
    dataReader = "";
  }

  delay(1);
  timeout++;
  //  if (dataReader.length() == 30 && dataReader != "") {
  //    dataReader = dataReader.substring(6, dataReader.length());
  //    appendWithoutRepeat(dataReader);
  //    //displayStruct();
  //    handleClientServer();
  //    if (st) {
  //      st = false;
  //      timeout = 0;
  //    }
  //    dataReader = "";
  //    timeout = 0;
  //    handleClientServer();
  //  }

  //  if (timeout == 30000 && ENABLE_DEBUG) {
  //    handleClientServer();
  //    Node* head_data_temp = head_data;
  //    while (head_data_temp != NULL) {
  //      handleClientServer();
  //      String data = head_data_temp->sNodes_Data;
  //      client_Sendata(data);
  //      head_data_temp = head_data_temp->next;
  //      delay(2000);
  //    }
  //
  //    //Serial.println("Before del: " );
  //
  //    //displayStruct();
  //    deleteList();
  //    //Serial.println("After del: " );
  //    //displayStruct();
  //    st = true;
      
  //    timeout = 0;
  //  }
  handleClientServer();
}

String getData() {
  return dataReader.substring(4);
}
