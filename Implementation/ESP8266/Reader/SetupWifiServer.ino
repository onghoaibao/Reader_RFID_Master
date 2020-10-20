#include "headerFile.h"

void setupWiFi() {
  //read200Line();
  Serial.println("---- Dang cai dat Wifi ---- ");
  Serial.println("Connecting to ");
  Serial.println(ssid);

  String _ssid_ = getSSID_WIFI();
  String _pass_ = getPASS_WIFI();
  _ssid_ = "BoBa2_4GHz";
  _pass_ = "Thang12@";

  WiFi.mode(WIFI_AP_STA);
  WiFi.softAP(NAME_WIFI, PASS_WFI);
  WiFi.begin(_ssid_, _pass_);
  int timeout = 0;
  while (WiFi.status() != WL_CONNECTED)
  {
    if (timeout == 40) {
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

  sv.on("/connectWifi", [] {
    String ssid = String(sv.arg("wifi"));
    String pass = String(sv.arg("pass"));
    accessWifi(ssid, pass);
    Serial.println(String("Wifi: ") + ssid + " len: " + String(ssid.length()));
    Serial.println(String("Pass: ") + pass + " len: " + String(pass.length()));
    getSSID_WIFI();
    getPASS_WIFI();
    sv.send(200, "text/html", "<h1>Wifi Is Connecting</h1>");
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
  sv.begin();
}

void handleClientServer() {
  sv.handleClient();
}

int t = 10;
void client_Sendata(String code) {
  WiFiClientSecure client;
  client.setInsecure();
  if (!client.connect(__host__, __httpsPort__))
  {
    Serial.println("connection failed");
    client.setInsecure();
    return;
  }
  String url = "/macros/s/AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0/exec?code=" + code;
  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + __host__ + "\r\n" +
               "Connection: close\r\n\r\n");
  Serial.print("Requesting URL: ");
  Serial.println(url);
  delay(2000);
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

String getAllDeviveFromDataBase() {
  WiFiClientSecure client;
  client.setInsecure();
  if (!client.connect(__host__, __httpsPort__))
  {
    Serial.println("connection failed");
    client.setInsecure();
    return "connection failed";
  }
//  String url = "/macros/s/AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0/exec?a=1&b=2";
//
//  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
//               "Host: " + __host__ + "\r\n" +
//               "Connection: close\r\n\r\n");
//  Serial.print("Requesting URL: ");
//  Serial.println(url);
//  delay(2000);
//  String section = "header";
  String sOut = "";
  long t = 0;
//  while (client.available() > 0) {
//    sOut = client.readStringUntil('\r');
//    Serial.print(sOut);
//    t++;
//    delay(1);
//    if (t >= 5000) {
//      break;
//    }
//  }
//  Serial.println();
//  Serial.println("closing connection");
//  delay(5000);
  String googleRedirHost = "script.googleusercontent.com";
  String sRe = "/macros/echo?user_content_key=HpAc6wP1WiiRLN5nHY-HcdHtFcneVifOBpd_1aQ4vAujRO6cVnQVRByyopx6q1_37pRY_hNP5w43YRZr6pILORy0pmLJ3Gslm5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnK_8q91JtUDH5ZB6y0798jIGepnT8qi0Fl7OArSVACEmWHkHCeXN3DMQZxWVHugsHxcmFEa6jifgB2pr_kvfBqk&lib=MJA1klEy-fKauGI90ScVSmlGg4jaJxtap";
  client.print(String("GET ") + sRe + " HTTP/1.1\r\n" +
               "Host: " + googleRedirHost + "\r\n" +
               "Connection: close\r\n\r\n");
  Serial.println(sRe);
  delay(2000);
  t = 0;    
  sOut = "";         
  while (client.available() > 0) {
    sOut = client.readStringUntil('\r');
    Serial.print(sOut);
    t++;
    delay(1);
    if (t >= 5000) {
      break;
    }
  }
            
  return sOut;
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
  String sBetween1 = "";
  for (int i = 0; i < 3; i++) {
    int num = WiFi.scanNetworks();
    for (int i = 0; i < num; i++) {
      String sSSID = WiFi.SSID(i).c_str();
      if (sBetween1.indexOf(sSSID) == -1)
        sBetween1 += String("<option value='") + sSSID + String("'>") + sSSID + String("</option>") + "\n";
    }
  }
  return sBetween1;
}
