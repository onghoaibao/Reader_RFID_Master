#include "headerFile.h"

void append(Node** head, String sData)
{
  Node* newNode = new Node;
  Node* last = *head;
  newNode->sNodes_Data = sData;
  newNode->next = NULL;
  if (*head == NULL) {
    *head = newNode;
    return;
  }
  while (last->next != NULL)
    last = last->next;
  last->next = newNode;
  return;
}

void deleteList() {
  Node* next;
  while (head_data != NULL)
  {
    next = head_data->next;
    free(head_data);
    head_data = next;
  }
  head_data = NULL;
}

void addDataToStruct(String sData) {
  append(&head_data, sData);
}

void displayStruct() {
  Node* head_data_temp = head_data;
  while (head_data_temp != NULL) {
    String data = head_data_temp->sNodes_Data;
    Serial.println("data: " + data);
    head_data_temp = head_data_temp->next;
  }
}

void appendWithoutRepeat(String str){
  Node* head_data_temp = head_data;
  while (head_data_temp != NULL) {
    String data = head_data_temp->sNodes_Data;
    //Serial.println("data: " + data);
    if(str.indexOf(data) != -1 || data.indexOf(str) != -1){
      return;
    }
    head_data_temp = head_data_temp->next;
  }
  addDataToStruct(str);
}

bool isElementInStruct(String str) {
  Node* head_data_temp = head_data;
  while (head_data_temp != NULL) {
    String sDT = head_data_temp->sNodes_Data;
    //Serial.println("sDT: " + sDT);
    if (str == sDT) {
      return true;
    }
    head_data_temp = head_data_temp->next;
  }
  return false;
}
