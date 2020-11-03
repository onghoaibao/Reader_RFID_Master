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
  Serial.println("---- Dang Khoi Dong Module WIFI ----");
  Serial.println("Connecting to ");

  String _ssid_ = getSSID_WIFI();
  String _pass_ = getPASS_WIFI();

  WiFi.mode(WIFI_AP_STA);
  WiFi.softAP(NAME_WIFI, PASS_WFI);
  WiFi.begin("ohbao", "123456xX");
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
    + String(number3) + "</h1>");
  });

  sv.begin();
}

void handleClientServer() {
  sv.handleClient();
}

int t = 10;
String client_Sendata(String maThietbi) {
  const char* host = "script.google.com";
  const char* googleRedirHost = "script.googleusercontent.com";
  const int httpsPort = 443;
  HTTPSRedirect client(httpsPort);
  String url = "/macros/s/AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0/exec?code=" + maThietbi;
  const char* fingerprint = "F0 5C 74 77 3F 6B 25 D7 3B 66 4D 43 2F 7E BC 5B E9 28 86 AD";
  for (int i = 0; i < 5; i++) {
    bool isConnect = client.connect(host, httpsPort);
    if (isConnect) {
      break;
    }
    else {
      Serial.println("Connection failed. Retrying…");
    }
    delay(1000);
  }
  Serial.println("url: " + url);
  client.printRedir(url, host, googleRedirHost);
  return client.getResponseData();
}

String getAllCodeDevice() {
  const char* host = "script.google.com";
  const char* googleRedirHost = "script.googleusercontent.com";
  const int httpsPort = 443;
  HTTPSRedirect client(httpsPort);
  String url = "/macros/s/AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0/exec?a=1&b=2";
  const char* fingerprint = "F0 5C 74 77 3F 6B 25 D7 3B 66 4D 43 2F 7E BC 5B E9 28 86 AD";
  for (int i = 0; i < 5; i++) {
    bool isConnect = client.connect(host, httpsPort);
    if (isConnect) {
      break;
    }
    else {
      Serial.println("Connection failed. Retrying…");
    }
    delay(1000);
  }
  client.printRedir(url, host, googleRedirHost);
  sCodeDeviceDataBase = client.getResponseData();
  return sCodeDeviceDataBase;
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
