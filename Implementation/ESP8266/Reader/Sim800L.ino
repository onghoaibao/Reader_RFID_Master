#include "headerFile.h"

void initSim800l(){
  pinMode(D6, INPUT);
  pinMode(D7, OUTPUT);
  Serial.println("---- Dang cai dat module SIM800L---- ");
  SIM800L.begin(9600);
  delay(1000);
  //readInfoSim800l("AT");
  //Gsm_Init();
  //delay(2000);
  //sendMessage();
}

void sendAT_Sim800l(String cmd){
  SIM800L.print(cmd);
  SIM800L.write(0x0D);
  SIM800L.write(0x0A);
}

void readInfoSim800l(String cmd){
  sendAT_Sim800l(cmd);
  delay(500);
  while(SIM800L.available() > 0){
     char c = SIM800L.read();
     Serial.print(c);
  }
  delay(3000);
}

void readInfo(){
  delay(300);
  while(SIM800L.available() > 0){
     char c = SIM800L.read();
     Serial.print(c);
  }
}
  
void Gsm_Init()
{
  //readInfoSim800l("ATE0");                     // Tat che do phan hoi (Echo mode)
  readInfoSim800l("AT+CMGF=1");                // Chon che do TEXT Mode
  readInfoSim800l("AT+CNMI=2,2,0,0,0");              // Hien thi truc tiep noi dung tin nhan
}

void sendMessage(){
  String myphone = "0938303762";
  SIM800L.print("AT+CMGS=\"" + myphone + "\"");     // Lenh gui tin nhan
  SIM800L.write(0x0D);  
  SIM800L.write(0x0A); 
  delay(5000);  
  readInfo();// Cho ky tu '>' phan hoi ve 
  delay(2000);
  SIM800L.print("Thiet Bi Da Roi Khoi Kho !!!");                             // Gui noi dung
  delay(2000);  
  for(int i=0; i < 15; i++)
    readInfo();
  SIM800L.write(26);                            // Gui Ctrl+Z hay 26 de ket thuc noi dung tin nhan va gui tin di
  delay(3000);  
  for(int i=0; i < 15; i++)
    readInfo();


  delay(1000);  
}
 
