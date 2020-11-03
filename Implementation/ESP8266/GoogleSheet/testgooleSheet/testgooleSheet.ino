#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>

const char* NAME_WIFI_ESP8266 = "ESP_Manage";
const char* PASS_WIFI_ESP8266  = "123456789";

const String host = "script.google.com";
const int httpsPort = 443; //the https port is same

//const char* ssid = "ohbao"; 
//const char* password = "123456xX";  

String ssid =  " BoBa2_4GHz";
String password =  "Thang06@";

String url;

void setup() 
{
  Serial.begin(115200);
  delay(100);
  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  WiFi.begin(ssid, password); 
  while (WiFi.status() != WL_CONNECTED) 
  {
    delay(500);
    Serial.print(".");
  }
 
  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  Serial.print("Netmask: ");
  Serial.println(WiFi.subnetMask());
  Serial.print("Gateway: ");
  Serial.println(WiFi.gatewayIP());
}

float t = 1999;
void loop() 
{
  Serial.print("connecting to ");
  Serial.println(host);
  WiFiClientSecure client;
  client.setInsecure();
  const int httpPort = 443;
  if (!client.connect(host, httpPort)){
    Serial.println("connection failed");
    client.setInsecure();
    return;
  }

  t++;
  url = "/macros/s/AKfycbxsMad8bqrkqXUkajFobCX3C9-BXrgkPXDoE1QJqN3tprFn7SIo/exec?maThietBi=E20050248611009026000FF2";

  client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" + 
               "Connection: close\r\n\r\n");

  Serial.print("Requesting URL: ");
  Serial.println(url);
  delay(500);
  String section="header";
  while(client.available())
  {
    String line = client.readStringUntil('\r');
    Serial.print(line);
  }
  Serial.println();
  Serial.println("closing connection");
  delay(6000);
}
