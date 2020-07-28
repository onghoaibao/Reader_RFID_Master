#include "Header.h"

void setupRFID() {
  RFID.begin(112500);
}



void readDataRFID() {
  while (RFID.available() > 0) {
    if (dataReader.length() <= 28) {
      char c = RFID.read();
      dataReader += hexToASCII(c);
      Serial.print(c);
    }
    else {
      Serial.println("Data Reader 1: " + dataReader + "  len: " + String(dataReader.length()));
      char c = RFID.read();
    }
  }
}

void RFIDFlush() {
  while (RFID.available() > 0) {
    char t = RFID.read();
  }
}

String hexToASCII(unsigned long n) {
  int base = 16;
  char buf[8 * sizeof(long) + 1]; // Assumes 8-bit chars plus zero byte.
  char *str = &buf[sizeof(buf) - 1];

  *str = '\0';
  do {
    unsigned long m = n;
    n /= 16;
    char c = m - base * n;
    *--str = c < 10 ? c + '0' : c + 'A' - 10;
  } while (n);

  String sOut = "";
  if (strlen(str) == 1) {
    sOut += (char)('0');
  }
  sOut += String(str);

  return sOut;
}
