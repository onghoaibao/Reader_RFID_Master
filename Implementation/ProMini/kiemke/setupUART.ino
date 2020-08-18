#include "Header.h"

void setupRFID() {
  RFID.begin(115200);
  pinMode(RX_RFID, INPUT);
  pinMode(TX_RFID, OUTPUT);
}

void readDataRFID() {
  while (RFID.available() > 0) {
    if (dataReader.length() <= 26) {
      char c = RFID.read();
      String s = hexToASCII((int)c);
      dataReader += s;
      //Serial.println(s);
    }
    else {
      //Serial.println("Data Reader 1: " + dataReader.substring(4) + "  len: " + String(dataReader.length()));
      //dataReader = "";
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
  if (sOut.length() == 2) {
    return sOut;
  }
  sOut = sOut.substring(sOut.length() - 2, sOut.length());
  return sOut;
}
