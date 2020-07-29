#include "Header.h"
String dataReader = "";


void setup() {
  // put your setup code here, to run once:
  delay(2000);
  Serial.begin(112500);
  setupBLE103();
  //setupRFID();
}
String cmd = "";
int i =0;
void loop() {
  delay(1000);
  i+=1;
  String a = String("ABCDEFABCDEFABCDEFABCDEF") + String(i);
  sendDataToPoint(a);
  //readDataRFID();
}
