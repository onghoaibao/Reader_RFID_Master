#ifndef _DISDLL_H
#define _DISDLL_H

#ifdef APP_EXPORT
#define APP_DLL extern "C" _declspec(dllexport)
#else
#define APP_DLL extern "C" _declspec(dllimport)
#endif  // APP_EXPORT
//-----------------------------------------------------------
#define RECV_BUFF_SIZE				(1024*16)		// ���ջ�������С
#define SEND_BUFF_SIZE				(1024)			// ���ͻ�������С
#define MAX_RECV_BUFFSIZE			(8192)			// �����ջ�����
#define SEND_START_CODE				0xA0			// ����������ʼ��
#define RES_PACK_STARTCODE			0xE4			// ������Ӧ֡��ʼ��
#define INFO_PACK_STARTCODE			0xE0			// ��Ϣ֡��ʼ��
#define HEAD_LENGTH					4				// �����ͷ����
#define PACKAGE_LEN					17				// ��������֡����
#define MAX_TAG_NUM					50				// һ�ν��յ����֡��
//--------------------����������---------------------------------------

#define DIS_MULTI_BEGIN				0x65			// ���ã���ʼ����Ѱ��
#define DIS_MULTI_END				0xA8			// ֹͣѰ��
#define DIS_READ_VERSION			0x6A			// ��ȡ�豸�汾��
#define DIS_SET_SINGLE_PARA			0x60			// ���õ�������
#define DIS_GET_SINGLE_PARA			0x61			// ��ȡ��������
#define DIS_SET_MULTI_PARA			0x62			// ���ö������
#define DIS_GET_MULTI_PARA			0x63			// ��ȡ�������
#define DIS_READ_SINGLE_TAG			0x82			// ʶ���ű�ǩ
#define DIS_READ_TAG_DATA			0x80			// ����ǩָ����������
#define DIS_WRITE_TAG_DATA			0x81			// д��ǩָ������
#define DIS_WRITE_TAG_MULTI_WORD	0xAB			// д����
#define DIS_FAST_WRITE				0x9C			// ��д��ǩ��EPC���
#define DIS_TID_BY_EPC				0xAA			// ����EPC��TID
#define DIS_INIT_TAG				0x99			// ��ʼ����ǩ
#define DIS_BEEP_CTRL				0xB0			// ���ö�������
#define DIS_LOCK_TAG				0xA5			// ������ǩ
#define DIS_UNLOCK_TAG				0xA6			// ������ǩ
#define DIS_KILL_TAG				0x86			// ���ٱ�ǩ
#define DIS_SET_BAUDRATE			0xA9			// ���ò�����
//#define DIS_STOP_WORK				0x50			// ֹͣ����
#define DIS_SET_RELAY					0xB1		// ���ü̵���
#define DIS_SET_AUTHERPWD	   0x9B			// ������Ȩ����
#define DIS_TAG_AUTHER  0x9A			//  ����Ȩ����


//----------------------������Ϣ---------------------------------------

#define INVALID_COM_PORT			 -2				// ��Ч���ں�
#define INVALID_HANDLE_COM			 -3				// ��Ч���ھ��
#define INVALID_HOST_PORTORBAUDRATE	 -4				// ��Ч�˿ںŻ�����
#define INVALID_IPADDRESS			 -5				// ��ЧIP��ַ
#define INVALID_POINTER				 -6				// ��Чָ��
#define INVALID_PARAMETER			 -7				// ��Ч����
#define ERROR_OPEN_COM				 -8				// �򿪴��ڳ���
#define ERROR_SOCKET_INIT			 -9				// socket��ʼ������
#define ERROR_SET_COMMBUFFER		 -10			// �������绺������
#define ERROR_NOT_CONNECT			 -11			// �豸δ����
#define ERROR_SET_DCB				 -12			// ����DCB����
#define ERROR_COM_SEND				 -13			// ���ڷ��ͳ���
#define ERROR_COM_RECV				 -14			// ���ڽ��ճ���
#define ERROR_CREATE_SOCKET			 -15			// ����socket����
#define ERROR_SOCKET_CONNECT		 -16			// socket���ӳ���
#define ERROR_SOCKET_SEND			 -17			// socket���ͳ���
#define ERROR_SOCKET_RECV			 -18			// socket���ճ���
#define ERROR_UNKNOWN				 -19			// δ֪����

//------------------------����������������-----------------------------
#define LOCK_USER					  0				// �û���
#define LOCK_TID					  1				// TID��
#define LOCK_EPC					  2				// EPC��
#define LOCK_ACCESS					  3				// ������
#define LOCK_KILL					  4				// �����
#define LOCK_ALL					  5				// ȫ������

//--------------------------������-------------------------------------
#define BR_9600						  0
#define BR_19200						  1
#define BR_38400						  2
#define BR_57600						  3
#define BR_115200					  4
//--------------------------����ģʽ-----------------------------------
#define WM_MASTER_SLAVE	1
#define WM_TIMING					2
#define WM_TRIG						3
//--------------------------Τ��Э��-----------------------------------
#define Wiegand26							1
#define Wiegand34							2
#define Wiegand32							3
//--------------------------ͨ��ģʽ-----------------------------------

#define MODE_RS485					  1
#define MODE_Wiegend				  2
#define MODE_RS232					  3
//--------------------------������������ַ��---------------------------
#define ADD_USERCODE							0x64
#define ADD_POWER									0x65
#define ADD_WORKMODE						0x70
#define ADD_TIME_INTERVAL					0x71
#define ADD_COMM_MODE						0x72
#define ADD_WIEGAND_PROTO				0x73
#define ADD_WIEGAND_PULSEWIDTH 0x74
#define ADD_WIEGAND_PULSECYCLE	0x75
#define ADD_NEIGHJUDGE_TIME			0x7A
#define ADD_NEIGHJUDGE_SET				0x7B
#define ADD_TRIG_SWITCH						0x80
#define ADD_TRIG_MODE							0x81
#define ADD_TRIG_DELAYTIME				0x84
#define ADD_BAUD_RATE							0x85
#define ADD_SINGLE_OR_MULTI_TAG 0x87
#define ADD_ANT_MODE						   0x89
#define ADD_ANT_SET							  0x8A
#define ADD_FREQUENCY_SET			  0x90
#define ADD_FREQUENCY_PARA_92	  0x92
#define ADD_FREQUENCY_PARA_93	  0x93
#define ADD_FREQUENCY_PARA_94	  0x94
#define ADD_FREQUENCY_PARA_95	  0x95
#define ADD_FREQUENCY_PARA_96    0x96
#define ADD_FREQUENCY_PARA_97    0x97
#define ADD_FREQUENCY_PARA_98    0x98
#define ADD_SERIAL								  0x34
#define ADD_DELAYER_TIME				  0xC6

#define MASK_BIT_0								  0x1
#define MASK_BIT_1								  0x2
#define MASK_BIT_2								  0x4
#define MASK_BIT_3								 0x8
#define MASK_BIT_4								  0x10
#define MASK_BIT_5								  0x20
#define MASK_BIT_6								  0x40
#define MASK_BIT_7								  0x80





typedef struct _PACKAGE
{
	BYTE startcode;  // ��ʼ��
	BYTE len;		 // ����
	BYTE cmd;		 // ������
	BYTE usercode;	 // �豸��
	BYTE data[100];	 // ����
	BYTE bcc;		 // У����
}PACKAGE, *PPACKAGE;
//---------------------------�ص�����----------------------------------------------------------------------------------------
typedef void (CALLBACK * HANDLE_FUN)(BYTE* pData, int length);

//--------------------------��������-------------------------------------------------------------------------------------------
APP_DLL void   _stdcall  PrintBuffer(BYTE* pBuf, int length);
APP_DLL int   _stdcall  GetErrorMessage();
/*-------------------------------------------------------------------------------
  1�����ƣ�GetErrorMessage
  2�����ܣ���ȡ�����
  3����������
  4������ֵ�������
********************************************************************************/
APP_DLL bool  _stdcall  DeviceInit(char* pHost, int CommMode, int PortOrBandRate);
/*-------------------------------------------------------------------------------
   1�����ƣ�deviceInit
   2�����ܣ�����������,ȷ�����ӷ�ʽ������ʼ���豸���ȫ�ֱ���		
   3��������pHost IP��ַ�ַ�����������ģʽʱ���Ըò���
			CommMode��ͨ�ŷ�ʽ��0��������ģʽ��1-9�Ǵ���ģʽ��ͬʱ�Ǵ��ں�
			PortOrBaudRate ͨ�Ŷ˿ںŻ����ʡ�
   4������ֵ
			TRUE   ��ʼ���ɹ�
			FALSE����ʼ��ʧ��
   5������ֵ
			 INVALID_IPADDRESS  			// ��ЧIP��ַ
			 INVALID_COMM_PORT		 		// ��Ч���ں�
			 INVALID_HOST_PORTORBANDRATE	// ��Ч�˿ںŻ�����
			 INVALID_POINTER		    	// ��ָ��
			 ERROR_SOCKET_INIT				// �����ʼ����
********************************************************************************/
APP_DLL bool   _stdcall  DeviceConnect();
/*------------------------------------------------------------------------------- 
   1�����ƣ�deviceConnect
�� 2�����ܣ������豸
   3����������
�� 4������ֵ��
			TRUE  ���ӳɹ�
			FALSE ����ʧ��
   5������ֵ
			ERROR_CREATE_SOCKET				// ����socket����
			ERROR_SOCKET_CONNECT			// socket���ӳ���
			INVALID_COM_PORT				// ��Ч���ں�
			ERROR_OPEN_COM					// �򿪴��ڳ���
/********************************************************************************/
APP_DLL bool   _stdcall  DeviceDisconnect();
/*------------------------------------------------------------------------------- 
�� 1�����ƣ�deviceDisconnect
�� 2�����ܣ��Ͽ��豸����
   3��������������
�� 4������ֵ��
			TRUE  ���ӳɹ�
			FALSE ����ʧ��
*********************************************************************************/
APP_DLL bool   _stdcall  DeviceUninit();
/*------------------------------------------------------------------------------- 
�� 1�����ƣ�deviceUnInit
�� 2�����ܣ��ָ���ʼֵ		
   3����������
   4������ֵ��
			TRUE  ���ӳɹ�
			FALSE ����ʧ��
*********************************************************************************/

APP_DLL bool   _stdcall  ResetReader(unsigned char usercode);
/*------------------------------------------------------------------------------- 
	1������:  ResetReader
	2������:  ���ö�������������������״̬
	3������:  usercode  �豸��
	4������ֵ:
			TRUE ���ö������ɹ�
			FALSE ����ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  BeginMultiInv(unsigned char usercode, HANDLE_FUN fun_name  );
/*------------------------------------------------------------------------------- 
	1������:  BeginMultiInv
	2������:  ����Ѱ��
	3������:  fun_name �����߶���Ļص�����
						usercode  �豸��
	4������ֵ:
			TRUE �ɹ�����
			FALSE ����ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_POINTER				// ��Ч����ָ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  StopInv(unsigned char usercode);
/*------------------------------------------------------------------------------- 
	1������:  StopInv
	2������:  ֹͣѰ��
	3������:  usercode  �豸��
	4������ֵ:
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_POINTER				// ��Ч����ָ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  GetDeviceVersion(unsigned char usercode, int* pMainVerion, int* pSubVersion);
/*------------------------------------------------------------------------------- 
	1������:  GetDeviceVersion
	2������:  ��ȡ�豸�汾��
	3������: 
						usercode  �豸��
						pMainVersion ���汾�ŵ�ַ
						pSubVersion  ���汾�ŵ�ַ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_POINTER				// ��Ч����ָ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  GetSingleParameter(unsigned char usercode, unsigned char ParaAddr, unsigned char* pValue);
/*------------------------------------------------------------------------------- 
	1������:  GetSingleParameter
	2������:  ��ȡ��������
	3������: 
						usercode  �豸��
						ParaAddr��������ַ
						pValue  �������ֵ�ı�����ַ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_POINTER				// ��Ч����ָ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  SetSingleParameter(unsigned char usercode, unsigned char ParaAddr, unsigned char Value);
/*------------------------------------------------------------------------------- 
	1������:  SetSingleParameter
	2������:  ���õ�������
	3������:  
						usercode  �豸��
						ParaAddr��������ַ
						 Value  ����ֵ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  GetMultiParameters(unsigned char usercode, int addr, int NumOfPara, unsigned char* pParas);
/*------------------------------------------------------------------------------- 
	1������:  GetMultiParameters
	2������:  ��ȡ�������
	3������:  
							usercode  �豸��
							addr��������ַ
							NumOfPara ��������
							pParas  �������ֵ�Ļ�������ַ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  SetMultiParameters(unsigned char usercode,  int addr, int NumOfPara, unsigned char* pParas);
/*------------------------------------------------------------------------------- 
	1������:  SetMultiParameters
	2������:  ���ö������
	3������: 
							usercode  �豸��
							addr��������ַ
							NumOfPara ��������
							pParas  �������ֵ�Ļ�������ַ
	4������ֵ
			  TRUE �ɹ�
			  FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  ReadSingleTag(unsigned char& usercode,  unsigned char* pTagID, unsigned char* pAntNo);
/*------------------------------------------------------------------------------- 
	1������:  ReadSingleTag
	2������:  �����ű�ǩ
	3������:  
						usercode  �豸��
						pTagID��EPC��ַ
						 pAntNo �������ߺŵ�ַ
	4������ֵ
			  TRUE �ɹ�
			  FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  ReadTagData(unsigned char usercode,  unsigned char bank, unsigned char begin, unsigned char length, unsigned char* pOutData);
/*-------------------------------------------------------------------------------
	1������: ReadTagData
	2������: ��ȡ��ȡ��ǩָ�����������
	3������
			usercode  �豸��
			bank: Ҫ��ȡ����������
			begin:��ʼ��ַ
			length: Ҫ��ȡ������
			pOutData: �洢����buffer
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool  _stdcall   WriteTagData(unsigned char usercode, unsigned char bank, unsigned char begin, unsigned char length, unsigned char* pData);// 0x81
/*------------------------------------------------------------------------------- 
	1������: WriteTagData
	2������: д��ǩ��ָ������
	3������
			usercode  �豸��
			bank: д�����������
			begin:��ʼ��ַ
			length: д�������
			pData: �洢����buffer
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  WriteTagMultiWord(unsigned char usercode, unsigned char bank, unsigned char begin, unsigned char length, unsigned char* pData);// 0xAB
/*------------------------------------------------------------------------------- 
	1������: WriteTagMultiWord
	2������: д����ֽڵ�ָ������
	3������
			usercode  �豸��
			bank: д�����������
			begin:��ʼ��ַ
			length: д�������
			pData: �洢����buffer
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool  _stdcall   WriteTagSingleWord(unsigned char usercode, unsigned char bank, unsigned char begin, unsigned char data1, unsigned char data2);
/*------------------------------------------------------------------------------- 
	1������: WriteTagSingleWord
	2������: д���ֵ�ָ������
	3������
			usercode  �豸��
			bank: д�����������
			begin:��ʼ��ַ
			data1: ��һ�ֽ�
			data2: �ڶ��ֽ�
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/


APP_DLL bool   _stdcall  FastWriteTag(unsigned char usercode, unsigned char* pData, unsigned char length);
/*------------------------------------------------------------------------------- 
	1������: FastWriteTag
	2������: ��д��ǩEPC
	3������
			usercode  �豸��
			 pData : ��������ַ
			 length: ����������
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  ReadTIDByEpc(unsigned char usercode, const unsigned char* pEpc, unsigned char* pTID);
/*------------------------------------------------------------------------------- 
	1������: ReadTIDByEpc
	2������: ����EPC��TID
	3������
			usercode  �豸��
			 pEpc: EPC��������ַ
			 pTID: ����TID��������ַ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  InitializeTag(unsigned char usercode );
/*------------------------------------------------------------------------------- 
	1������: InitializeTag
	2������: ��ʼ����ǩ
	3������
			usercode  �豸��
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  BeepCtrl(unsigned char usercode, unsigned char status);
/*------------------------------------------------------------------------------- 
	1������: BeepCtrl
	2������: ���ö�������
	3������
			usercode  �豸��
			status ����ֵ  0  ȡ��
											1	 �򿪶�����Beep��
											2   ������һ��Beep��
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/


APP_DLL bool   _stdcall  LockTag(unsigned char usercode, unsigned char lockBank, unsigned char* pPassword);
/*------------------------------------------------------------------------------- 
	1������: LockTag
	2������: ������ǩ
	3������
			usercode  �豸��
			lockBank����������
			pPassword: ����
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_BANK					// ��Ч�����
			INVALID_OPCODE					// ��Ч������
			INVALID_PASSWORD				// ��Ч����
			ERROR_DEV_CONNECT				// �豸δ����
			ERROR_DEV_SEND					// �豸���ʹ�
			ERROR_DEV_RECV					// �豸���մ�
			ERROR_OPER_FAIL					// ����ʧ��
*********************************************************************************/
APP_DLL bool   _stdcall  UnlockTag(unsigned char usercode, unsigned char unlockBank, unsigned char* pPassword);
/*------------------------------------------------------------------------------- 
	1������: UnlockTag
	2������: ������ǩ
	3������
			usercode  �豸��
			unlockBank����������
			pPassword: ����
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_BANK					// ��Ч�����
			INVALID_OPCODE					// ��Ч������
			INVALID_PASSWORD				// ��Ч����
			ERROR_DEV_CONNECT				// �豸δ����
			ERROR_DEV_SEND					// �豸���ʹ�
			ERROR_DEV_RECV					// �豸���մ�
			ERROR_OPER_FAIL					// ����ʧ��
*********************************************************************************/
APP_DLL bool  _stdcall   KillTag(unsigned char usercode, unsigned char* pPassword);
/*------------------------------------------------------------------------------- 
	1������:  KillTag
	2������:  ���ٱ�ǩ
	3������
						usercode  �豸��
						pPassword�����������
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_BANK					// ��Ч�����
			INVALID_OPCODE					// ��Ч������
			INVALID_PASSWORD				// ��Ч����
			ERROR_DEV_CONNECT				// �豸δ����
			ERROR_DEV_SEND					// �豸���ʹ�
			ERROR_DEV_RECV					// �豸���մ�
			ERROR_OPER_FAIL					// ����ʧ��
*********************************************************************************/
APP_DLL bool   _stdcall  SetBaudRate(unsigned char usercode, unsigned char BaudRate);
/*------------------------------------------------------------------------------- 
	1������: SetBaudRate
	2������: ���ò�����
	3������
			usercode  �豸��
			BaudRate ����ֵ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			INVALID_PARAMETER			// ��Ч����
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  StopWork(unsigned char usercode );
/*------------------------------------------------------------------------------- 
	1������: StopWork
	2������: ֹͣ����
	3������
						usercode  �豸��
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  SetRelay(unsigned char usercode , unsigned char relayOnOff );
/*------------------------------------------------------------------------------- 
	1������: SetRelay
	2������: �򿪻�رռ̵���
	3������
						usercode  �豸��
						relayOnOff �̵���״̬
								0���ر�
								1��
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			INVALID_PARAMETER		 // ��Ч����
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/

APP_DLL bool   _stdcall  SetRelayTime(unsigned char usercode , unsigned char time );
/*------------------------------------------------------------------------------- 
	1������: SetRelay
	2������: �򿪻�رռ̵���
	3������
						usercode  �豸��
						time �̵����պ�ʱ�䣬��λΪ�롣��3��ֵ��1, 5, 10
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			INVALID_PARAMETER		 // ��Ч����
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  SetAutherPwd(unsigned char usercode , unsigned char* pPwd );
/*------------------------------------------------------------------------------- 
	1������: SetAutherPwd
	2������: ���ö�������Ȩ����
	3������
						usercode  �豸��
						pPwd ���2���ֽ���Ȩ����ĵ�ַ
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			INVALID_PARAMETER		 // ��Ч����
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  TagAuther(unsigned char usercode);
/*------------------------------------------------------------------------------- 
	1������: TagAuther
	2������: ����ǩ��֤��Ȩ
	3������
						usercode  �豸��
	4������ֵ
			TRUE �ɹ�
			FALSE ʧ��
	5������ֵ
			INVALID_HANDLE_COM			// ��Ч���ھ��
			ERROR_NOT_CONNECT			// �豸û������
			INVALID_PARAMETER		 // ��Ч����
			ERROR_COM_SEND				// ���ڷ��ͳ���
			ERROR_COM_RECV				// ���ڽ��ճ���
			ERROR_SOCKET_CONNECT		// �������ӳ���
			ERROR_SOCKET_SEND			// ���緢�ͳ���
			ERROR_SOCKET_RECV			// ������ճ���
			ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
APP_DLL bool   _stdcall  GetAutherPwd(unsigned char usercode , unsigned char* pPwd );
/*--------------------------------------------------------------------------------------------------------------------------------------
1������: GetAutherPwd
2������: ��ȡ��Ȩ����
3������
				usercode  �豸��
				pPwd ���2���ֽ���Ȩ����ĵ�ַ
4������ֵ
TRUE �ɹ�
FALSE ʧ��
5������ֵ
INVALID_HANDLE_COM			// ��Ч���ھ��
ERROR_NOT_CONNECT			// �豸û������
INVALID_PARAMETER		 // ��Ч����
ERROR_COM_SEND				// ���ڷ��ͳ���
ERROR_COM_RECV				// ���ڽ��ճ���
ERROR_SOCKET_CONNECT		// �������ӳ���
ERROR_SOCKET_SEND			// ���緢�ͳ���
ERROR_SOCKET_RECV			// ������ճ���
ERROR_UNKNOWN				// δ֪����
*********************************************************************************/
#endif  // _DISDLL_H