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
  while (RFID.available() > 0) {
    char t = RFID.read();
  }
}

String getURL(String maThietBi, String vitri) {
  String url = (temp_url + "maThietBi=" + maThietBi + "&vitri=" + vitri);
  return url;
}
