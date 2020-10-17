#include "headerFile.h"

// declare variables for the google sheet
const char* NAME_WIFI_ESP8266 = "Medical_Wifi";
const char* PASS_WIFI_ESP8266  = "123456789";

String url = "/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?tag=ohbao&value=1233";

//Wifi
//String ssid =  " BoBa2_4GHz";
//String pass =  "Thang06@";

String bLogin = "";
const char* __host__ = "script.google.com";
const int __httpsPort__ = 443; //the https port is same


void setupWiFi() {
  //read200Line();
  Serial.println("Connecting to ");
  //  Serial.println(ssid);

  String _ssid_ = getSSID_WIFI();
  String _pass_ = getPASS_WIFI();

  WiFi.mode(WIFI_AP_STA);
  WiFi.softAP(NAME_WIFI, PASS_WFI);
  WiFi.begin(_ssid_, _pass_);
  int timeout = 0;
  while (WiFi.status() != WL_CONNECTED)
  {
    if (timeout == 30) {
      Serial.print("\nCannot Connect to BoBa2_4GHz Wifi\n");
      break;
    }
    delay(500);
    Serial.print(".");
    timeout++;
  }
  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  sv.on("/setupWifi", [] {
    String sBetween =  scanWifi();
    String sSetup = getStringFile("setupWifi.html");
    sSetup.replace("[###]", sBetween);
    sv.send(200, "text/html", sSetup);
  });

  sv.on("/setphone", [] {
    String sSetup = getStringFile("setphone.html");
    sv.send(200, "text/html", sSetup);
  });

  sv.on("/transFile", HTTP_ANY, [] {
    //Serial.println(String("Accept: ") + ++count);
    sv.send(200, "text/html",
    "<meta charset='utf-8'>"
    "<html>"
    "<head>"
    "<title>Truyen File Den ESP8266</title>"
    "</head>"
    "<body>"
    "<form method='POST' action='/transFile' enctype='multipart/form-data'>"
    "<h2>Config File login.html</h2>"
    "<input type='file' name='SelectFile'>"
    "<input type='submit' name='SubmitFile' value='Send File'>"
    "</form>"
    "<form method='POST' action='/transFile' enctype='multipart/form-data'>"
    "<h2>Config File setupWifi.html</h2>"
    "<input type='file' name='SelectFile'>"
    "<input type='submit' name='SubmitFile' value='Send File'>"
    "</form>"
    "<form method='POST' action='/transFile' enctype='multipart/form-data'>"
    "<h2>Config File setphone.html</h2>"
    "<input type='file' name='SelectFile'>"
    "<input type='submit' name='SubmitFile' value='Send File'>"
    "</form>"
    "</body>"
    "</html");
  }, [] {
    HTTPUpload& file = sv.upload();
    if (file.status == UPLOAD_FILE_START) {
      Serial.println("Start File: " + file.filename);
      removeContentFile(file.filename);
    }
    else if (file.status == UPLOAD_FILE_WRITE) {
      Serial.println("File dang ghi: " + file.filename);
      saveFile(file.filename, (const char*)file.buf, file.currentSize);
    }
  });

  sv.on("/connectWifi", [] {
    String ssid = String(sv.arg("wifi"));
    String pass = String(sv.arg("pass"));
    accessWifi(ssid, pass);
    Serial.println(String("Wifi: ") + ssid + " len: " + String(ssid.length()));
    Serial.println(String("Pass: ") + pass + " len: " + String(pass.length()));
    getSSID_WIFI();
    getPASS_WIFI();
    sv.send(200, "text/html", "<h1>Wifi Is Connecting To " + String(ssid) + "</h1>");
  });

  sv.on("/getphone", [] {
    String number1 = String(sv.arg("number1"));
    String number2 = String(sv.arg("number2"));
    String number3 = String(sv.arg("number3"));
    saveNumberPhone(number1, number2, number3);
    Serial.println(String("number1: ") + number1);
    Serial.println(String("number2: ") + number2);
    Serial.println(String("number3: ") + number3);
    sv.send(200, "text/html", "<h1>Installing number phone: " 
      + String(number1) + String(" - ")
      + String(number2) + String(" - ") 
      + String(number3)+ "</h1>");
  });

  sv.begin();
}

void handleClientServer() {
  sv.handleClient();
}

int t = 10;
void client_Sendata(String maThietbi, String pos) {
  WiFiClientSecure client;
  client.setInsecure();
  if (!client.connect(__host__, __httpsPort__)) {
    Serial.println("connection failed");
    client.setInsecure();
    return;
  }
  String url = "/macros/s/AKfycbxsMad8bqrkqXUkajFobCX3C9-BXrgkPXDoE1QJqN3tprFn7SIo/exec?maThietBi=" + maThietbi + "&vitri=" + pos + String(t);
  t += 4;
  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + __host__ + "\r\n" +
               "Connection: close\r\n\r\n");
  Serial.print("Requesting URL: ");
  Serial.println(url);
  delay(500);
  String section = "header";
  while (client.available())
  {
    String line = client.readStringUntil('\r');
    Serial.print(line);
  }
  Serial.println();
  Serial.println("closing connection");
  delay(2000);
}

String getStatusMaThietBi(String maThietbi) {
  WiFiClientSecure client;
  client.setInsecure();
  if (!client.connect(__host__, __httpsPort__))
  {
    Serial.println("connection failed");
    client.setInsecure();
    return "connection failed";
  }
  String url = "/macros/s/AKfycbxsMad8bqrkqXUkajFobCX3C9-BXrgkPXDoE1QJqN3tprFn7SIo/exec?maThietBi=" + maThietbi;

  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + __host__ + "\r\n" +
               "Connection: close\r\n\r\n");
  Serial.print("Requesting URL: ");
  Serial.println(url);
  delay(500);
  String section = "header";
  String sOut = "";
  long t = 0;
  while (client.available()) {
    sOut = client.readStringUntil('\r');
    Serial.print(sOut);
    t++;
    delay(1);
    if (t >= 2000) {
      break;
    }
  }
  Serial.println();
  Serial.println("closing connection");
  return sOut;
}

String scanWifi() {
  String sBetween = "";
  for (int y = 0; y < 3; y++) {
    int num = WiFi.scanNetworks();
    for (int i = 0; i < num; i++) {
      String sSSID = WiFi.SSID(i).c_str();
      if (sBetween.indexOf(sSSID) == -1)
        sBetween += String("<option value='") + sSSID + String("'>") + sSSID + String("</option>") + "\n";
    }
  }
  return sBetween;
}
