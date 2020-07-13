#ifndef  _SERVER_H
#define _SERVER_H
#include "user_def.h"
#include "HPTypeDef.h"

#pragma pack(push,1)

typedef struct _client_info_
{
	char                ip[50];                  // IP
	unsigned short      port;                   //�ͻ��˶˿�
	unsigned char       isPass;                 //�����Ƿ���Ч
	unsigned long       id;                     //�ͻ���ΨһID
    unsigned char       state;                  //0 �Ͽ�   1 ����
}CLIENT_INFO,*pCLIENT_INFO;

typedef struct _dis_single_para_
{
	BOOL isEffective;
	unsigned char result;
	unsigned char Multiresult[256];
}DIS_SINGLE_PARA,*pDIS_SINGLE_PARA;

#pragma pack(pop)

DIS_SINGLE_PARA g_DisSinglePara;

typedef void (WINAPIV *Command)(unsigned long hDev, char* command,unsigned short type);	 
typedef void (WINAPIV *ClientConnect)(CLIENT_INFO client);             
typedef void (WINAPIV *QuencyTag)(unsigned long hDev,Tag data);	

/****************************************************
*Function:ServerStart
*Description:����TCP Server�����		
*Input:ip   ������IP
*      port �������˿�   
*      recvData  ѭ����ѯEPC�ص����� 
*      command   ��ͨ����Ӧ��ص�����
*      connect   �ͻ���������Ϣ�ص�����
*      product   ��Ʒ����   ��ǰû�õ�
*Output:
*Data:
*Remark:���ȵ���
****************************************************/
APP_DLL int    WINAPI ServerStart(char* ip,unsigned short port, QuencyTag recvData,Command command,ClientConnect connect,unsigned char product);

/****************************************************
*Function:ServerStart
*Description:��ȡ����������Ϣ  
*Input:error  ��������
*      errorDesc ������Ϣ 
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL void   WINAPI GetLastErrors(int* error,char* errorDesc);

/****************************************************
*Function:ServerStart
*Description:�رշ�����ͨ�����  
*Input:
*Output:0  �ɹ�
       -1  ʧ��
*Remark:
****************************************************/
APP_DLL int    WINAPI ServerClose(void);

/****************************************************
*Function:CloseClient
*Description:�ر���ĳ���ͻ��˵�����  
*Input:id ��������ͻ�������ID
*Output:
*Data:
*Remark:�ҹ�˾�豸�Ͽ����Ӻ���Զ���������һ�㲻����
****************************************************/
APP_DLL int    WINAPI CloseClient(unsigned long id);

/****************************************************
*Function:DIS_SetSinglePara_TcpServer
*Description:��������ͨ�����ò���ָ��  
*Input:id ��������ͻ�������ID
*      type ��Ϣ����
*      message ��Ϣ������
*      messageLen ��Ϣ�����ݳ���
*Output:0  �ɹ�
       -1  ʧ��
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_SetSinglePara_TcpServer(unsigned long id,unsigned char type,unsigned char message);

/****************************************************
*Function:DIS_GetSinglePara_TcpServer
*Description:��������ͨ�ò�ѯ����ָ��  
*Input:��ʱ����Ҫ δʵ��
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_GetSinglePara_TcpServer(unsigned long id,unsigned char type,unsigned char* result);

/****************************************************
*Function:DIS_BeginMultiInv_TcpServer
*Description:����������ʼ��ѯEPCָ��  
*Input:
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_StartMulti_TcpServer(unsigned long id);

/****************************************************
*Function:DIS_StopMultiInv_TcpServer
*Description:��������ֹͣ��ѯEPCָ��  
*Input:
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_StopMulti_TcpServer(unsigned long id);

//�ǽӿں���
int SetParaResult();

#endif