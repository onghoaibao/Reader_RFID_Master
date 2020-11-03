#include "headerFile.h"
String sCodeDeviceDataBase;

void setup() {
  delay(2000);
  Serial.begin(115200);
  Serial.println("\n-----------------------------------\n");
  initSim800l();
  initReaderRFID();
  SPIFFS.begin();
  setupWiFi();
  Serial.println("\n------ Hello ------");
  delay(3000);
  while (getAllCodeDevice() == "") {
    delay(2000);
  };
  sCodeDeviceDataBase = sCodeDeviceDataBase.substring(2, sCodeDeviceDataBase.length() - 5);
  Serial.println("sCodeDeviceDataBase: " + sCodeDeviceDataBase);
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
      Serial.println("Data Reader IF: " + s1 + " = " + s2 + "  len: " + String(s1.length()) + " - " + String(s2.length()));
      if (sCodeDeviceDataBase.indexOf(s1) != -1 && !isElementInStruct(s1)) {
        appendWithoutRepeat(s1);
      }
    }
    dataReader = "";
  }
  //delayTimeOut(20000);

  if (timeout == 20000 && true) {
    Node* head_data_temp = head_data;
    while (head_data_temp != NULL && 1) {
      String data = head_data_temp->sNodes_Data;
      String st = client_Sendata(data);
      Serial.println("sCode: " + data + "  sStatus: " + st);
      head_data_temp = head_data_temp->next;
      delay(2000);
    }

    //    Serial.println("Before del: " );
    //    displayStruct();
    deleteList();
    //    Serial.println("After del: " );
    //    displayStruct();
    //    st = true;
    serialFlush();
    timeout = 0;
  }
  delay(1);
  timeout++;
}

void delayTimeOut(int t) {
  if (timeout >= t) {
    getAllCodeDevice();
    //deleteList();
    timeout = 0;
  }
}
