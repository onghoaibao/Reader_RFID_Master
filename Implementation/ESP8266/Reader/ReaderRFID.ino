#include "headerFile.h"

void initReaderRFID(){
  readerSerial.begin(115200);
  pinMode(D2, INPUT);
  pinMode(D3, OUTPUT);  
}
