#include "Header.h"
String dataReader = "";


void setup() {
  // put your setup code here, to run once:
  delay(2000);
  Serial.begin(112500);
  setupBLE103();
}
String cmd = "";
long i =0;
void loop() {
  delay(2000);
  i+=1;
  sendDataToPoint("ABCDEABCDEABCDEABCDEAB" + String(i));
}
