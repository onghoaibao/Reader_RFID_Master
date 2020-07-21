#include "headerFile.h"

//macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?
//maThietBi=ABCDEFGHsCDE
//&properties_name=ohbao
//&properties_sx=1234
//&trangthai=Tot
//&dvMuon=phong1
//&ngayTra=12-12-2020
//&lsdc=A1
//&hientai=Kho

String temp_url = "/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?";

void serialFlush() {
  while (readerSerial.available() > 0) {
    char t = readerSerial.read();
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

String getURL(String maThietBi, String vitri) {
  String url = (temp_url + "maThietBi=" + maThietBi + "&vitri=" + vitri);
  return url;
}
