#include "headerFile.h"
<<<<<<< HEAD

#define ENABLE_DEBUG false
String dataReader = "";
long timeout = 0;
bool st = false;

void setup() {
  delay(500);
  Serial.begin(115200);
  Serial.println("\n-------------------\n");
=======
String sCodeDeviceDataBase;

void setup() {
  delay(2000);
  Serial.begin(115200);
  Serial.println("\n-----------------------------------\n");
  initSim800l();
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a
  initReaderRFID();
  initSim800l();
  SPIFFS.begin();
  setupWiFi();
  delay(2000);
  //getAllDeviveFromDataBase();
  serialFlush();
  Serial.println("\n------ Hello ------");
<<<<<<< HEAD
  //initHTTPS();
=======
  delay(3000);
  while (getAllCodeDevice() == "") {
    delay(2000);
  };
  sCodeDeviceDataBase = sCodeDeviceDataBase.substring(2, sCodeDeviceDataBase.length() - 5);
  Serial.println("sCodeDeviceDataBase: " + sCodeDeviceDataBase);
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a
}

int sStatus = 1;

void loop() {
  // put your main code here, to run repeatedly:
  handleClientServer();
<<<<<<< HEAD
//  readDataRFID();
//  if (dataReader.length() == 30) {
//    dataReader = getData();
//    String s1 = dataReader.substring(6, 10);
//    String s2 = dataReader.substring(dataReader.length() - 4, dataReader.length());
//    if (s1 == s2) {
//      Serial.println("Data Reader IF: " + s1 + "  " + s2 + "  len: " + String(s1.length()) + " - " + String(s2.length()));
//      appendWithoutRepeat(s1);
//    }
//    dataReader = "";
//  }

  delay(3000);
  timeout++;
  client_Sendata("1121");
  handleClientServer();
}

String getData() {
  return dataReader.substring(4);
=======
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
>>>>>>> 51739e87d32bb54539b1557ff83b3ce071b6f19a
}
