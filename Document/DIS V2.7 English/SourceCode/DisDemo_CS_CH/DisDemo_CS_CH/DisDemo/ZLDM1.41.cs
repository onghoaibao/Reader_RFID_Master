using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DisDemo
{
    class ZLDM141
    {
        public static ushort m_DevCnt;    // 搜索到的设备数量
        public static byte m_SelectedDevNo = 0;
        public static int PARAM_DEV_LOCAL_IP = 2;
        public static int PARAM_DEV_LOCAL_PORT = 3;
        public static int PARAM_WORK_MODE = 256;
        public static int PARAM_IP_MODE = 257;
        public static int PARAM_NET_MASK = 268;
        public static int PARAM_GATEWAY = 269;
        public static int PARAM_DEST_IP = 277;
        public static int PARAM_DEST_PORT = 275;
        public static int PARAM_BAUNDRATE = 264;
        public static int PARAM_DATA_BITS = 263;
        public static int PARAM_PARITY = 262;

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_StartSearchDev")]
        public static extern UInt16 StartSearchDev();

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_GetDevID")]
        public static extern IntPtr GetDevID(int devIndex);

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_GetDevParamString")]
        public static extern IntPtr GetDevParamString(IntPtr id, int paramType);

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_SetDevParamString")]
        public static extern IntPtr SetDevParamString(IntPtr id, IntPtr NewParam, int paramType);

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_SetDevParamExcute")]
        public static extern int SetDevParamExcute(IntPtr id);

        [DllImport("ZlDevManage.dll", EntryPoint = "ZLDM_SetDevParamInt")]
        public static extern int SetDevParamInt(IntPtr id, int NewParam, int paramType);
    }
}
