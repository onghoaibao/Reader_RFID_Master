#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>

const char* NAME_WIFI_ESP8266 = "ESP_Manager";
const char* PASS_WIFI_ESP8266  = "123456789";


const char* host = "script.google.com";
const int httpsPort = 443; //the https port is same

const char *ssid = "ohbao"; 
const char *password = "123456xX";  
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

  const int httpPort = 443;
  if (!client.connect(host, httpPort)) 
  {
    Serial.println("connection failed");
    return;
  }

  t++;
  
  url = "/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?tag=ohbao&value=1233";

  
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
