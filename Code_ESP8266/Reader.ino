#include <SoftwareSerial.h>
#include <ESP8266WiFi.h>

SoftwareSerial readerSerial(D3, D2); // RX, TX

void setup() {
  // put your setup code here, to run once:
  Serial.begin(115200);
  delay(100);
  readerSerial.begin(115200);
  pinMode(D3, INPUT);
  pinMode(D2, OUTPUT);

  Serial.println("\n------ Hello ------");
}

String dataReader = "";

void loop() {
  // put your main code here, to run repeatedly:
  while (readerSerial.available() > 0) {
    if (dataReader.length() >= 36) {
      Serial.print("Data Reader: " + dataReader);
      dataReader = ""    }

    char c = readerSerial.read();
    dataReader += hexToASCII(c);

    Serial.print(hexToASCII(c));
    Serial.print(" - ");

  }

  delay(300);
  Serial.println();
  if (dataReader.indexOf("00CE205024861109019304D97149FF") != -1) {
    Serial.print("Data Reader: " + dataReader);
    dataReader = "";
  }
}


String hexToASCII(unsigned long n)
{
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
