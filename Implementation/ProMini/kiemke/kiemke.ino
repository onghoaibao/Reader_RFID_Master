#include "Header.h"
String dataReader = "";

void setup() {
  // put your setup code here, to run once:
  delay(500);
  Serial.begin(9600);
  setupBLE103();
  delay(500);
  setupRFID();
  delay(500);
  Serial.println("------- Hello -------");
}
String cmd = "";
int i = 0;
void loop() {
  readDataRFID();
  if (dataReader.length() == 28) {
    dataReader = getData();
    String s1 = dataReader.substring(4, 8);
    String s2 = dataReader.substring(dataReader.length() - 4, dataReader.length());
    if (s1 == s2) {
      Serial.println("Data Reader IF: " + s1 + "  " + s2 + "  len: " + String(s1.length()) + " - " + String(s2.length()));
      bool b = isElementInStruct(s1);
      sendDataToPoint(s1);
      if (!b) {
        //sendDataToPoint(s1);
        //addDataToStruct(s1);
      }
    }
    dataReader = "";
  }
  delay(50);

//  sendDataToPoint("ABCD");
//  delay(1000);
}


String getData() {
  return dataReader.substring(4);
}
