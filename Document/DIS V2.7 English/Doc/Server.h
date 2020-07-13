#ifndef  _SERVER_H
#define _SERVER_H
#include "user_def.h"
#include "HPTypeDef.h"

#pragma pack(push,1)

typedef struct _client_info_
{
	char                ip[50];                  // IP
	unsigned short      port;                   //客户端端口
	unsigned char       isPass;                 //连接是否有效
	unsigned long       id;                     //客户端唯一ID
    unsigned char       state;                  //0 断开   1 连接
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
*Description:开启TCP Server服务端		
*Input:ip   服务器IP
*      port 服务器端口   
*      recvData  循环查询EPC回调函数 
*      command   普通命令应答回调函数
*      connect   客户端连接信息回调函数
*      product   产品类型   当前没用到
*Output:
*Data:
*Remark:最先调用
****************************************************/
APP_DLL int    WINAPI ServerStart(char* ip,unsigned short port, QuencyTag recvData,Command command,ClientConnect connect,unsigned char product);

/****************************************************
*Function:ServerStart
*Description:获取操作错误信息  
*Input:error  错误类型
*      errorDesc 错误信息 
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL void   WINAPI GetLastErrors(int* error,char* errorDesc);

/****************************************************
*Function:ServerStart
*Description:关闭服务器通信组件  
*Input:
*Output:0  成功
       -1  失败
*Remark:
****************************************************/
APP_DLL int    WINAPI ServerClose(void);

/****************************************************
*Function:CloseClient
*Description:关闭与某个客户端的连接  
*Input:id 服务器与客户端连接ID
*Output:
*Data:
*Remark:我公司设备断开连接后会自动连接所以一般不调用
****************************************************/
APP_DLL int    WINAPI CloseClient(unsigned long id);

/****************************************************
*Function:DIS_SetSinglePara_TcpServer
*Description:分离器件通用设置参数指令  
*Input:id 服务器与客户端连接ID
*      type 消息类型
*      message 消息体数据
*      messageLen 消息体数据长度
*Output:0  成功
       -1  失败
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_SetSinglePara_TcpServer(unsigned long id,unsigned char type,unsigned char message);

/****************************************************
*Function:DIS_GetSinglePara_TcpServer
*Description:分离器件通用查询参数指令  
*Input:暂时不需要 未实现
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_GetSinglePara_TcpServer(unsigned long id,unsigned char type,unsigned char* result);

/****************************************************
*Function:DIS_BeginMultiInv_TcpServer
*Description:分离器件开始查询EPC指令  
*Input:
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_StartMulti_TcpServer(unsigned long id);

/****************************************************
*Function:DIS_StopMultiInv_TcpServer
*Description:分离器件停止查询EPC指令  
*Input:
*Output:
*Data:
*Remark:
****************************************************/
APP_DLL int    WINAPI DIS_StopMulti_TcpServer(unsigned long id);

//非接口函数
int SetParaResult();

#endif