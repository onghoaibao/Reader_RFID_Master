using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Resources;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;
using System.Configuration;


namespace DisDemo
{


    public partial class MainWindow : Form
    {
        [DllImport("ws2_32.dll")]
        public static extern Int32 WSAStartup(UInt16 wVersionRequested, ref  WSADATA wsaData);
        [DllImport("ws2_32.dll")]
        public static extern Int32 WSACleanup();

        static List<EPC_data> Tag_data = new List<EPC_data>();
        static bool bNewTag = false;
        static int nItemNo = 0; // ��¼��Ҫ���µ�EPC��������
        int RWBank = 0;// ��¼ѡ��Ķ�д���򼰿�ʼ��������ַ�����ڸ��¿ؼ�
        int addStart = 0;
        int addEnd = 0;

        int counts = 1;

        bool bConnected = false; // ���ӱ�־����ί�е��첽�̸߳�д

        /// <summary>
        /// ����һ�εĿ��ر���
        /// </summary>
        bool isReadOnce = true;

        public Dis.HANDLE_FUN f = new Dis.HANDLE_FUN(HandleData);
        // �����ַ�����Դ
        static ResourceManager[] rmArray = new ResourceManager[2]{  
                                                    new ResourceManager("DisDemo.SimpChinese", typeof(MainWindow).Assembly),
                                                    new ResourceManager("DisDemo.English", typeof(MainWindow).Assembly)};
        static ResourceManager rm = rmArray[0];

        static byte deviceNo = 0;
        // ���������豸��ί��
        public delegate void DeleConnectDev(byte[] ip, int CommPort, int PortOrBaudRate);
        // ���ݲ���ʱ���������¼�������ListView�ؼ�
        public delegate void UpdateControlEventHandler();
        public static event UpdateControlEventHandler UpdateControl;
        private static SynchronizationContext m_SyncContext = null;
        public MainWindow()
        {
            InitializeComponent();

            UpdateControl = new UpdateControlEventHandler(UpdateListView);  //����UpdateControl�¼�
            // ������̸߳��´��ڿؼ�
            Control.CheckForIllegalCrossThreadCalls = false;

            rbNet.Checked = true;
            textBoxPort.Text = "4196";



            // �����ݱ�
            listView.Columns.Add("No", 45, HorizontalAlignment.Center);
            listView.Columns.Add("EPC/TID", 260, HorizontalAlignment.Center);
            listView.Columns.Add("Count", 60, HorizontalAlignment.Center);
            listView.Columns.Add("AntNo", 80, HorizontalAlignment.Center);
            listView.Columns.Add("DevNo", 80, HorizontalAlignment.Center);


            // ��ʼ������ҳ��ؼ�
            InitCommParamControl();
            InitAccessTagControl();
            InitParamSetControl();
            InitWigginsTakePlaceValueControl();
            DisableAccessTagButton(false);


            cbbLangSwitch.SelectedIndex = 1;// 0; // Ĭ������
            cbbLangSwitch_SelectedIndexChanged(null, null);

            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnStartReadData.Enabled = false;
            btnReadOnce.Enabled = false;
            btnStopReadData.Enabled = false;
            comboBoxSerialCommPort.Visible = false;

            //2017.2.21 ZW ���������������б��
            cmb_rates.Visible = false;

            cbbWorkMode.SelectedIndex = 1;
            cbbCommMode.SelectedIndex = 0;
            cboBand.SelectedIndex = 0;
            cbbFreqModeUS.SelectedIndex = 0;
            cboFreqModeEU.SelectedIndex = 0;
            cboReadSpeed.SelectedIndex = 1;
            rbAsc.Checked = true;



            btnUpdate_Click(null, null);
            //  rbReadSingleTag.Enabled = true;
            //  rbReadMultiTag.Enabled = false;
            // ����Ĭ�ϵĽ�����ʾѡ��
            m_SyncContext = SynchronizationContext.Current;

        }

        private void InitParamSetControl()
        {
            // Τ��Э��
            cbbWiegandProtocol.Items.Add("Wiegand26");
            cbbWiegandProtocol.Items.Add("Wiegand34");
            cbbWiegandProtocol.Items.Add("Wiegand32");
            cbbWiegandProtocol.Items.Add("Wiegand66");
            // Ƶ��
            for (int i = 0; i < 50; ++i)
            {
                cbbFreqPointUS.Items.Add(string.Format("{0}-{1:F1}Mhz", i + 1, 902.5 + i * 0.5));
            }
            // Ƶ��
            for (int i = 0; i < 5; ++i)
            {
                cboFreqPointEU.Items.Add(string.Format("{0}-{1:F1}Mhz", i + 1, 865.5 + i * 0.5));
            }
        }

        private void InitWigginsTakePlaceValueControl()
        {
            // Τ��ȡλֵ
            for (int i = 0; i < 62; i++)
            {
                cboWigginsTakePlaceValue.Items.Add(i);
            }
        }

        private void InitAccessTagControl()
        {
            // ��д����
            cbbRWBank.Items.Add("RFU");
            cbbRWBank.Items.Add("EPC");
            cbbRWBank.Items.Add("TID");
            cbbRWBank.Items.Add("User");

            // ��������
            cbbLockBank.Items.Add("Kill");
            cbbLockBank.Items.Add("Access");
            cbbLockBank.Items.Add("EPC");
            cbbLockBank.Items.Add("TID");
            cbbLockBank.Items.Add("User");
        }

        private void DisableAccessTagButton(bool bEnabled)
        {
            // ��ǩ����ҳ�水ť
            btnFastWrite.Enabled = bEnabled;
            btnReadData.Enabled = bEnabled;
            btnWriteData.Enabled = bEnabled;
            btnLockTag.Enabled = bEnabled;
            btnUnlockTag.Enabled = bEnabled;
            btnKillTag.Enabled = bEnabled;
            btnInitTag.Enabled = bEnabled;
            // ��������ҳ�水ť
            btnReadWorkMode.Enabled = bEnabled;
            btnSetWorkMode.Enabled = bEnabled;
            btnReadCommMode.Enabled = bEnabled;
            btnSetCommMode.Enabled = bEnabled;
            btnReadFreq.Enabled = bEnabled;
            btnSetFreq.Enabled = bEnabled;
            //��������ҳ��
            btnTagAuth.Enabled = bEnabled;
            btnModifyAuthPwd.Enabled = bEnabled;
            btnSetBeep.Enabled = bEnabled;
            btnSetRelayTime.Enabled = bEnabled;
            btnSetUsbFormat.Enabled = bEnabled;
            btnReadBeep.Enabled = bEnabled;

            button3.Enabled = bEnabled;
            button1.Enabled = bEnabled;
            button4.Enabled = bEnabled;
            btnSetUsbFormat.Enabled = bEnabled;

            AutoAuthorizationBt.Enabled = bEnabled;
        }

        // ���´����б�
        public void GetSerialPortList(ref ComboBox comboBoxSP)
        {
            comboBoxSP.Items.Clear();
            comboBoxSP.Items.AddRange(SerialPort.GetPortNames());
            if (comboBoxSP.Items.Count > 0)
            {
                comboBoxSP.SelectedIndex = 0;
            }
        }

        bool bWSAInit = false;
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // ��ô����б�
            GetSerialPortList(ref comboBoxSerialCommPort);
            WSADATA wsaData = new WSADATA();

            if (!bWSAInit)
            {
                bWSAInit = true;
                WSAStartup(0x0202, ref wsaData);
            }
            // �����豸�����IP�б�
            ZLDM141.m_DevCnt = ZLDM141.StartSearchDev();

            comboBoxIP.Items.Clear();

            for (byte i = 0; i < ZLDM141.m_DevCnt; ++i)
            {
                comboBoxIP.Items.Add(Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(i), ZLDM141.PARAM_DEV_LOCAL_IP)));
            }
            if (ZLDM141.m_DevCnt > 0)
            {
                comboBoxIP.SelectedIndex = 0;
            }
        }

        // ί��ִ�е����Ӻ������ɹ����޸ı�־��ֹͣ��ʱ��
        private void ConnectDevice(byte[] ip, int CommPort, int PortOrBaudRate)
        {
            if (0 == Dis.DeviceInit(ip, CommPort, PortOrBaudRate))
            {
                labelVersion.Text = rm.GetString("strMsgInitFailure");
                return;
            }
            if (0 == Dis.DeviceConnect())
            {
                return;
            }
            for (int i = 0; i < 2; ++i)
            { Dis.StopWork(deviceNo); }
            int mainVer = 0, minSer = 0;
            Dis.GetDeviceVersion(deviceNo, out mainVer, out minSer);
            if (mainVer > 7)
            {
                radioButton12.Checked = true;
            }
            else
            {
                radioButton11.Checked = true;
            }
            labelVersion.Text = "Version:" + string.Format("{0}.{1}", mainVer, minSer);
            if (labelVersion.Text != "Version:0.0")
            {
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
                btnStartReadData.Enabled = false;
                btnReadOnce.Enabled = true;
                rbReadSingleTag.Enabled = true;
                rbReadMultiTag.Enabled = true;
                rbReadSingleTag.Checked = true;
                DisableAccessTagButton(true);
                bConnected = true;

                comboBoxSerialCommPort.Enabled = false;
                cmb_rates.Enabled = false;
                comboBoxIP.Enabled = false;
                textBoxPort.Enabled = false;

                timerConnect.Stop();// ���ӳɹ���������ʱ��
            }
            else
            {
                labelVersion.Text = rm.GetString("strMsgConnectFailure");
            }
        }

        // ��ʱ����������ָ��ʱ����û��������ӣ���ִ�д˺���
        private void timerConnect_Tick(object sender, EventArgs e)
        {
            if (!bConnected)
            {
                labelVersion.Text = rm.GetString("strMsgConnectFailure");
                timerConnect.Stop();
            }
        }

        /// <summary>
        /// ͨ������
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            byte[] ip = new byte[32];
            int CommPort = 0;
            int PortOrBaudRate = 0;
            if (rbNet.Checked) // TCP/IP
            {
                if ((!Regex.IsMatch(comboBoxIP.Text, "^[0-9.]+$")) || comboBoxIP.Text.Length < 7 || comboBoxIP.Text.Length > 15)
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidIPAdd"));
                    return;
                }
                ip = Encoding.ASCII.GetBytes(comboBoxIP.Text); // ��string���byte����
                if (!Regex.IsMatch(textBoxPort.Text, "^[0-9]+$") || textBoxPort.Text.Length > 5)
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidPort"));
                    return;
                }
                PortOrBaudRate = Int32.Parse(textBoxPort.Text);
            }
            else // SerialCommPort
            {
                if (comboBoxSerialCommPort.SelectedIndex >= 0)
                {
                    CommPort = int.Parse(comboBoxSerialCommPort.Text.Trim("COM".ToCharArray()));
                    //PortOrBaudRate = 9600;
                    if (cmb_rates.Text == "")
                    {
                        MessageBox.Show(rm.GetString("strMsgSelectRates"));
                    }
                    else
                    {
                        PortOrBaudRate = Convert.ToInt32(cmb_rates.Text);
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("strMsgSelectPort"));
                    return;
                }
            }
            if (!Regex.IsMatch(tbDeviceNo.Text, "^[0-9]+$"))
            {
                MessageBox.Show(rm.GetString("strMsgNotDigit"));
                return;
            }
            if (int.Parse(tbDeviceNo.Text) > 255)
            {
                MessageBox.Show(rm.GetString("strMsgDevNoValid"));
                return;
            }
            deviceNo = Byte.Parse(tbDeviceNo.Text);

            // ʹ��ί���첽�߳�ִ�����ӣ�ͬʱ������ʱ�����ȴ�
            DeleConnectDev dcd = new DeleConnectDev(ConnectDevice);
            dcd.BeginInvoke(ip, CommPort, PortOrBaudRate, null, null);
            bConnected = false;
            timerConnect.Interval = 3000; // �ȴ�3��ʱ��
            timerConnect.Start();
        }

        // �ص�����
        static string epc;
        static byte state = 1;
        public static void HandleData(IntPtr pData, int length)
        {
            if (!isStartMuliti)
                return;
            string epc = "";
            byte[] data = new byte[256];
            Marshal.Copy(pData, data, 0, length);
            if (state == 1)
            {
                for (int i = 1; i < length - 2; ++i)
                {
                    epc += string.Format("{0:X2} ", data[i]);
                }
                bNewTag = true;
                lock (Tag_data)
                {
                    for (int i = 0; i < Tag_data.Count; ++i)
                    {
                        if (epc == Tag_data[i].epc)
                        {
                            Tag_data[i].count++;
                            Tag_data[i].epc = epc;
                            Tag_data[i].antNo = data[13];
                            Tag_data[i].devNo = data[0];
                            bNewTag = false;     // �����±�ǩ
                            nItemNo = i;         //��¼��������ֵ�����ڸ���listView��
                            break;
                        }
                    }
                    if (bNewTag)
                    {
                        EPC_data epcdata = new EPC_data();
                        epcdata.epc = epc;
                        epcdata.antNo = data[13];// data[length - 1];
                        epcdata.devNo = data[0]; //data[length - 2];
                        epcdata.count = 1;
                        Tag_data.Add(epcdata);
                    }
                }
                UpdateControl(); // �������ݲ���������listView
            }
            else if (state == 2)
            {
                for (int i = 2; i < length - 2; ++i)
                {
                    epc += string.Format("{0:X2} ", data[i]);
                }
                bNewTag = true;
                for (int i = 0; i < Tag_data.Count; ++i)
                {
                    if (epc == Tag_data[i].epc)
                    {
                        Tag_data[i].count++;
                        Tag_data[i].antNo = data[length - 2];   
                        Tag_data[i].devNo = data[0];
                        bNewTag = false;     // �����±�ǩ
                        nItemNo = i;             //��¼��������ֵ�����ڸ���listView��
                        break;
                    }
                }
                if (bNewTag)
                {
                    EPC_data epcdata = new EPC_data();
                    epcdata.epc = epc;
                    epcdata.antNo = data[length - 2];
                    if (epcdata.antNo == 12)
                    {
                        ;
                    }
                    epcdata.devNo = data[0];
                    epcdata.count = 1;
                    Tag_data.Add(epcdata);
                }
                UpdateControl(); // �������ݲ���������listView
            }

        }

        private void UpdateListEPC(object state)
        {
            UpdateEPC updateEPC = state as UpdateEPC;

        }

        public static void WriteFile(string strTxt, string path)
        {
            using (StreamWriter wlog = File.AppendText(path))
            {
                wlog.Write("{0}", strTxt);
                wlog.Write(wlog.NewLine);
            }
        }

        public static void WriteFileSeparator(string strTxt, string path)
        {
            using (StreamWriter wlog = File.AppendText(path))
            {
                wlog.Write("{0}", strTxt);
            }
        }

        private void UpdateListView()
        {
            if (rbAsc.Checked)
            {
                Tag_data.Sort();
            }
            else if (rbDesc.Checked)
            {
                Tag_data.Sort();
                Tag_data.Reverse();
            }
            try
            {
                bool isNewData = true;
                labelCount.Text = (int.Parse(labelCount.Text) + 1).ToString();              
                lock (Tag_data)
                {
                    lock (listView.Items)
                    {
                        foreach (EPC_data epc in Tag_data)
                        {
                            isNewData = true;
                            for (int index = 0; index < listView.Items.Count; index++)
                            {
                                if (listView.Items[index].SubItems[1].Text == epc.epc)
                                {
                                    listView.Items[index].SubItems[2].Text = epc.count.ToString();
                                    listView.Items[index].SubItems[3].Text = epc.antNo.ToString();
                                    listView.Items[index].SubItems[4].Text = epc.devNo.ToString();
                                    isNewData = false;
                                    break;
                                }
                            }
                            if (isNewData)
                            {
                                 // ���¶�ȡ����
                                labelTagCount.Text = (int.Parse(labelTagCount.Text) + 1).ToString();// ���±�ǩ����
                                ListViewItem lvi = new ListViewItem();
                                int no = Tag_data.Count;
                                lvi.Text = no.ToString();
                                lvi.SubItems.Add(epc.epc);
                                lvi.SubItems.Add(epc.count.ToString());
                                lvi.SubItems.Add(epc.antNo.ToString());
                                lvi.SubItems.Add(epc.devNo.ToString());
                                listView.Items.Add(lvi);
                                if (cbSaveFile.Checked)
                                {
                                    WriteFile(epc.epc, "tag.txt");
                                }
                            }
                        }
                    }
                }
                /* if (!updateEPC.isNewData) // ���±�ǩ�����¶�Ӧ��Ķ�ȡ���������ߺš��豸�ŵ�
                 {
                     labelCount.Text = (int.Parse(labelCount.Text) + 1).ToString();
                     listView.Items[nItemNo].SubItems[1].Text = updateEPC.Tag_data[nItemNo].epc;
                     listView.Items[nItemNo].SubItems[2].Text = updateEPC.Tag_data[nItemNo].count.ToString();
                     listView.Items[nItemNo].SubItems[3].Text = updateEPC.Tag_data[nItemNo].antNo.ToString();
                     listView.Items[nItemNo].SubItems[4].Text = updateEPC.Tag_data[nItemNo].devNo.ToString();
                 }
                 else // �±�ǩ
                 {
                     labelCount.Text = (int.Parse(labelCount.Text) + 1).ToString(); // ���¶�ȡ����
                     labelTagCount.Text = (int.Parse(labelTagCount.Text) + 1).ToString();// ���±�ǩ����
                     ListViewItem lvi = new ListViewItem();
                     int no = Tag_data.Count;
                     lvi.Text = no.ToString();
                     lvi.SubItems.Add(updateEPC.Tag_data[no - 1].epc);
                     lvi.SubItems.Add(updateEPC.Tag_data[no - 1].count.ToString());
                     lvi.SubItems.Add(updateEPC.Tag_data[no - 1].antNo.ToString());
                     lvi.SubItems.Add(updateEPC.Tag_data[no - 1].devNo.ToString());
                     listView.Items.Add(lvi);
                 }*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("���ڲ���������������رգ������´򿪣�", "��ʾ");
            }
        }

        private void UpdateLV()
        {
            try
            {
                listView.BeginUpdate();
                listView.Items.Clear();
                for (int i = 1; i <= Tag_data.Count; ++i)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = i.ToString();
                    lvi.SubItems.Add(Tag_data[i - 1].epc);
                    lvi.SubItems.Add(Tag_data[i - 1].count.ToString());
                    lvi.SubItems.Add(Tag_data[i - 1].antNo.ToString());
                    lvi.SubItems.Add(Tag_data[i - 1].devNo.ToString());
                    listView.Items.Add(lvi);
                }
                listView.EndUpdate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// ��ʼ����Ѱ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            Dis.GetSingleParameter(deviceNo, Dis.ADDR_12BYTE_FLAG, out m_EPCMode);
            if (m_EPCMode == 0)
                state = 2;
            else
                state = 1;
            labelVersion.Text = "";
            Dis.BeginMultiInv(deviceNo, f);
            btnStopReadData.Enabled = true;
            btnStartReadData.Enabled = false;
            btnDisconnect.Enabled = false;

            rbReadSingleTag.Enabled = false;
            btnClearListView.Enabled = false;
            isStartMuliti = true;
        }

        /// <summary>
        /// ֹͣ����Ѱ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            Dis.StopInv(deviceNo);
            btnStartReadData.Enabled = true;
            btnStopReadData.Enabled = false;
            btnDisconnect.Enabled = true;

            rbReadSingleTag.Enabled = true;
            btnClearListView.Enabled = true;

            isStartMuliti = false;
        }

        /// <summary>
        /// ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {

            Tag_data.Clear();

            listView.Items.Clear();
            labelTagCount.Text = "0";
            labelCount.Text = "0";
        }

        /// <summary>
        /// ͨ�ŶϿ�
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Dis.ResetReader(deviceNo);
            Dis.DeviceDisconnect();
            Dis.DeviceUninit();
            labelVersion.Text = "";
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
            btnStartReadData.Enabled = false;
            btnReadOnce.Enabled = false;
            btnStopReadData.Enabled = false;
            bConnected = false;

            comboBoxSerialCommPort.Enabled = true;
            cmb_rates.Enabled = true;
            comboBoxIP.Enabled = true;
            textBoxPort.Enabled = true;
        }

        /// <summary>
        /// ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonAsc_CheckedChanged(object sender, EventArgs e)
        {
            Tag_data.Sort();
            UpdateLV();
        }

        /// <summary>
        /// ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonDesc_CheckedChanged(object sender, EventArgs e)
        {
            Tag_data.Sort();
            Tag_data.Reverse();
            UpdateLV();
        }

        /// <summary>
        /// ����ͨ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonSP_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSerialCommPort.Visible = true;
            comboBoxIP.Visible = false;
            textBoxPort.Visible = false;
            //labCommPort.Visible = false;

            //2017.2.20 ZW ���������ʵ������б��
            cmb_rates.Visible = true;

            lbl_ip.Text = rm.GetString("strCommPort");
            labCommPort.Text = rm.GetString("strDatas");

            cmb_rates.SelectedIndex = 0;
        }

        /// <summary>
        /// ����ͨ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonNet_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSerialCommPort.Visible = false;
            comboBoxIP.Visible = true;
            textBoxPort.Visible = true;
            labCommPort.Visible = true;

            //2017.2.21 ZW ���������ʵ������б��
            cmb_rates.Visible = false;

            labCommPort.Text = rm.GetString("strLabCommPort");
            lbl_ip.Text = rm.GetString("strIP");
        }

        private void SetCommParam_Enter(object sender, EventArgs e)
        {
        }

        private void InitCommParamControl()
        {
            comboBoxNetMode.Items.Add("TCP Server");
            comboBoxNetMode.Items.Add("TCP Client");
            comboBoxNetMode.Items.Add("UDP");
            comboBoxNetMode.Items.Add("UDP Group");

            comboBoxIPMode.Items.Add("Static");
            comboBoxIPMode.Items.Add("Dynamic");

            comboBoxBaudRate.Items.Add("1200");
            comboBoxBaudRate.Items.Add("2400");
            comboBoxBaudRate.Items.Add("4800");
            comboBoxBaudRate.Items.Add("7200");
            comboBoxBaudRate.Items.Add("9600");
            comboBoxBaudRate.Items.Add("14400");
            comboBoxBaudRate.Items.Add("19200");
            comboBoxBaudRate.Items.Add("28800");
            comboBoxBaudRate.Items.Add("38400");
            comboBoxBaudRate.Items.Add("57600");
            comboBoxBaudRate.Items.Add("76800");
            comboBoxBaudRate.Items.Add("115200");
            comboBoxBaudRate.Items.Add("230400");

            #region  2017-2-21 ZW �������ý���������������
            cmb_rate.Items.Add("9600");
            cmb_rate.Items.Add("19200");
            cmb_rate.Items.Add("38400");
            cmb_rate.Items.Add("57600");
            cmb_rate.Items.Add("115200");
            #endregion

            #region 2017.2.21 ZW ��������������
            cmb_rates.Items.Add("9600");
            cmb_rates.Items.Add("19200");
            cmb_rates.Items.Add("38400");
            cmb_rates.Items.Add("57600");
            cmb_rates.Items.Add("115200");
            #endregion

            comboBoxDataBits.Items.Add("8");
            comboBoxDataBits.Items.Add("7");
            comboBoxDataBits.Items.Add("6");
            comboBoxDataBits.Items.Add("5");

            comboBoxCheckBits.Items.Add("None");
            comboBoxCheckBits.Items.Add("Odd");
            comboBoxCheckBits.Items.Add("Even");
            comboBoxCheckBits.Items.Add("Marked");
            comboBoxCheckBits.Items.Add("Space");

            cbbRelayTime.Visible = false;
            lblCloseTime.Visible = false;
            rdoOpen1.Visible = false;
            rdoOpen2.Visible = false;
            rdoClose1.Visible = false;
            rdoClose2.Visible = false;
        }

        /// <summary>
        /// �����豸
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchDev_Click(object sender, EventArgs e)
        {
            lvZl.Items.Clear();
            ZLDM141.m_DevCnt = ZLDM141.StartSearchDev();
            for (byte i = 0; i < ZLDM141.m_DevCnt; ++i)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = (i + 1).ToString();
                lvi.SubItems.Add(Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(i), ZLDM141.PARAM_DEV_LOCAL_IP)));
                lvi.SubItems.Add(Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(i), ZLDM141.PARAM_DEV_LOCAL_PORT)).ToString());
                // string port = Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(i), ZLDM141.PARAM_DEV_LOCAL_PORT));
                lvi.SubItems.Add(Marshal.PtrToStringAnsi(ZLDM.GetDevID(i)));
                lvZl.Items.Add(lvi);
            }
            if (ZLDM141.m_DevCnt > 0)
            {
                //  UpdateCommParamControl(); // ����ҳ��ؼ�           
                lvZl.FocusedItem = lvZl.Items[0];// ���õ�һ��Ϊ������
            }

        }

        // ��С��ת��
        public static UInt16 ReverseByte(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        /// <summary>
        /// �༭�豸
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyDev_Click(object sender, EventArgs e)
        {
            if (lvZl.SelectedItems != null)
                listViewDev_ItemActivate(sender, e);
        }

        /// <summary>
        /// ����Ĭ�ϲ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultParams_Click(object sender, EventArgs e)
        {
            comboBoxNetMode.SelectedIndex = 0;
            comboBoxIPMode.SelectedIndex = 0;
            textBoxIPAdd.Text = "192.168.1.200";
            textBoxNetMask.Text = "255.255.255.0";
            textBoxPortNo.Text = "4196";
            textBoxGateway.Text = "192.168.1.1";
            textBoxDestIP.Text = "192.168.1.100";
            textBoxDestPort.Text = "4196";
            comboBoxBaudRate.SelectedIndex = 4;
            comboBoxDataBits.SelectedIndex = 0;
            comboBoxCheckBits.SelectedIndex = 0;
        }

        /// <summary>
        /// ����ͨ�Ų���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetParams_Click(object sender, EventArgs e)
        {
            if (-1 == comboBoxNetMode.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgSelectNetMode"));
                return;
            }
            if (-1 == comboBoxIPMode.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgSelectIPMode"));
                return;
            }
            if (-1 == comboBoxBaudRate.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgSelectBaudRate"));
                return;
            }
            if (-1 == comboBoxDataBits.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgSelectDataBits"));
                return;
            }
            if (-1 == comboBoxDataBits.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgSelectParity"));
                return;
            }
            // ���IP��ַ
            if (!IsValidIP(textBoxIPAdd.Text))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidIP"));
                return;
            }
            // �������
            if (!IsValidIP(textBoxNetMask.Text))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidMask"));
                return;
            }
            // �������
            if (!IsValidIP(textBoxGateway.Text))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidGateway"));
                return;
            }
            // ���Ŀ��IP
            if (!IsValidIP(textBoxDestIP.Text))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidDestIP"));
                return;
            }
            // ���˿ں�
            int value = int.Parse(textBoxPortNo.Text);
            if (value < 1000 || value > 65535)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidPort"));
                return;
            }
            value = int.Parse(textBoxDestPort.Text);
            if (value < 1000 || value > 65535)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidDestPort"));
                return;
            }
            //  ushort port = ReverseByte(ushort.Parse(textBoxPortNo.Text));
            //  ushort destport = ReverseByte(ushort.Parse(textBoxDestPort.Text));

            //  byte[] ip = new byte[32];
            // byte[] netmask = new byte[32];
            // byte[] gateway = new byte[32];
            // byte[] destip = new byte[32];
            //  ip = Encoding.ASCII.GetBytes(textBoxIPAdd.Text);
            //  netmask = Encoding.ASCII.GetBytes(textBoxNetMask.Text);
            //  gateway = Encoding.ASCII.GetBytes(textBoxGateway.Text);
            //   destip = Encoding.ASCII.GetBytes(textBoxDestIP.Text);

            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), comboBoxNetMode.SelectedIndex, ZLDM141.PARAM_WORK_MODE);
            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), comboBoxIPMode.SelectedIndex, ZLDM141.PARAM_IP_MODE);
            ZLDM141.SetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), Marshal.StringToHGlobalAnsi(textBoxIPAdd.Text), ZLDM141.PARAM_DEV_LOCAL_IP);
            ZLDM141.SetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), Marshal.StringToHGlobalAnsi(textBoxNetMask.Text), ZLDM141.PARAM_NET_MASK);
            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), int.Parse(textBoxPortNo.Text), ZLDM141.PARAM_DEV_LOCAL_PORT);
            ZLDM141.SetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), Marshal.StringToHGlobalAnsi(textBoxGateway.Text), ZLDM141.PARAM_GATEWAY);
            ZLDM141.SetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), Marshal.StringToHGlobalAnsi(textBoxDestIP.Text), ZLDM141.PARAM_DEST_IP);
            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), int.Parse(textBoxDestPort.Text), ZLDM141.PARAM_DEST_PORT);
            // ZLDM.SetIPMode(ZLDM.m_SelectedDevNo, (byte)comboBoxIPMode.SelectedIndex);
            // ZLDM.SetIP(ZLDM.m_SelectedDevNo, ip);
            // ZLDM.SetNetMask(ZLDM.m_SelectedDevNo, netmask);
            //  ZLDM.SetPort(ZLDM.m_SelectedDevNo, port);
            //  ZLDM.SetGateWay(ZLDM.m_SelectedDevNo, gateway);

            //  ZLDM.SetDestName(ZLDM.m_SelectedDevNo, destip);
            //   ZLDM.SetDestPort(ZLDM.m_SelectedDevNo, destport);

            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), comboBoxBaudRate.SelectedIndex, ZLDM141.PARAM_BAUNDRATE);
            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), comboBoxDataBits.SelectedIndex, ZLDM141.PARAM_DATA_BITS);
            ZLDM141.SetDevParamInt(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), comboBoxCheckBits.SelectedIndex, ZLDM141.PARAM_PARITY);
            //  ZLDM.SetBaudrateIndex(ZLDM.m_SelectedDevNo, (byte)comboBoxBaudRate.SelectedIndex);
            //  ZLDM.SetDataBits(ZLDM.m_SelectedDevNo, (byte)comboBoxDataBits.SelectedIndex);
            //   ZLDM.SetParity(ZLDM.m_SelectedDevNo, (byte)comboBoxCheckBits.SelectedIndex);

            //   bool res = ZLDM.SetParam(ZLDM.m_SelectedDevNo);
            int res = ZLDM141.SetDevParamExcute(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo));
            if (res == 1)
                MessageBox.Show(rm.GetString("strMsgSucceedSetCommParam"));
            else
                MessageBox.Show(rm.GetString("strMsgFailedSetCommParam"));
        }
        private bool IsValidIP(string strIP)
        {
            // �ȼ�����޷������ַ�
            if (!Regex.IsMatch(strIP, "^[0-9.]+$"))
            {
                return false;
            }
            // �ټ�����ݷ�Χ�Ƿ����
            string[] strNumArray = strIP.Split('.');
            if (strNumArray.Length != 4)
            {
                return false;
            }
            int n = 0;
            for (int i = 0; i < 4; ++i)
            {
                n = int.Parse(strNumArray[i]);
                if (n > 255)
                {
                    return false;
                }
            }
            return true;
        }

        private void listViewDev_ItemActivate(object sender, EventArgs e)
        {
            if (lvZl.Items.Count > 0)
            {
                ZLDM141.m_SelectedDevNo = (byte)(lvZl.Items.IndexOf(lvZl.FocusedItem));
                UpdateCommParamControl();
            }
        }

        private void UpdateCommParamControl()
        {
            if (ZLDM141.m_DevCnt > 0)
            {
                comboBoxNetMode.SelectedIndex = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_WORK_MODE));
                comboBoxIPMode.SelectedIndex = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_IP_MODE));
                textBoxIPAdd.Text = Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_DEV_LOCAL_IP));
                textBoxNetMask.Text = Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_NET_MASK));
                textBoxPortNo.Text = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_DEV_LOCAL_PORT)).ToString();
                textBoxGateway.Text = Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_GATEWAY));
                textBoxDestIP.Text = Marshal.PtrToStringAnsi(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_DEST_IP));
                textBoxDestPort.Text = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_DEST_PORT)).ToString();
                comboBoxBaudRate.SelectedIndex = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_BAUNDRATE));
                comboBoxDataBits.SelectedIndex = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_DATA_BITS));
                comboBoxCheckBits.SelectedIndex = Marshal.ReadInt32(ZLDM141.GetDevParamString(ZLDM141.GetDevID(ZLDM141.m_SelectedDevNo), ZLDM141.PARAM_PARITY));
            }
        }

        /// <summary>
        /// ��д���ݲ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbRWBank_SelectedIndexChanged(object sender, EventArgs e)
        {   // ���ݲ�������ȷ����Ч����ʼ��ַ
            if (cbbRWBank.SelectedIndex == 0) // ������
            {
                RWBank = 0;
                addStart = 0;
                addEnd = 3;
            }
            else if (cbbRWBank.SelectedIndex == 1) // EPC��
            {
                RWBank = 1;
                addStart = 2;
                addEnd = 7;
            }
            else if (cbbRWBank.SelectedIndex == 2) // TID
            {
                RWBank = 2;
                addStart = 0;
                addEnd = 5;
            }
            else if (cbbRWBank.SelectedIndex == 3) // User
            {
                RWBank = 3;
                addStart = 0;
                addEnd = 31;
                //addEnd = 128;
            }
            cbbStartAdd.Items.Clear();
            for (int i = addStart; i <= addEnd; ++i)
            {
                cbbStartAdd.Items.Add(i.ToString());
            }
            cbbStartAdd.SelectedIndex = 0;

        }

        /// <summary>
        /// ��ʼ��ַ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbStartAdd_SelectedIndexChanged(object sender, EventArgs e)
        { // ������ʼ��ַ��ȷ������ѡ��
            int nItem = cbbStartAdd.SelectedIndex; // ȡ����ʼ��ַ����ֵ
            int maxLength = addEnd - addStart - nItem + 1;
            cbbLength.Items.Clear();
            for (int i = 1; i <= maxLength; ++i)
            {
                cbbLength.Items.Add(i.ToString());
            }
            cbbLength.SelectedIndex = maxLength - 1;
        }

        /// <summary>
        /// ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearData_Click(object sender, EventArgs e)
        {
            tbRWData.Text = "";
            counts = 1;
            tbRWData.Focus();
        }

        /// <summary>
        /// ��ȡ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadData_Click(object sender, EventArgs e)
        {
            try
            {
                int RWBank = -1;
                int StartAdd = -1;
                int Length = -1;
                labResult.Text = "";
                if ((RWBank = cbbRWBank.SelectedIndex) == -1)
                {
                    MessageBox.Show(rm.GetString("strMsgSelectRWBank"));
                    return;
                }
                StartAdd = int.Parse(cbbStartAdd.Text);
                Length = int.Parse(cbbLength.Text);
                byte[] byteArray = new byte[256];
                tbRWData.Text = "";
                if (checkBox9.Checked)
                {
                    if (1 == Dis.ReadEpcByAnt(deviceNo, (byte)RWBank, (byte)StartAdd, Length, GetAnt(), byteArray))
                    {
                        for (int i = 3; i < 2 * Length + 3; i++) // ǰ�棳���ֽ�Ϊ����Ĳ���
                        {
                            tbRWData.Text += string.Format("{0:X2} ", byteArray[i]);
                        }
                        labResult.Text = rm.GetString("strMsgSussecReadData");
                    }

                }
                else
                {
                    if (1 == Dis.ReadTagData(deviceNo, (byte)RWBank, (byte)StartAdd, (byte)Length, byteArray))
                    {
                        for (int i = 3; i < 2 * Length + 3; i++) // ǰ�棳���ֽ�Ϊ����Ĳ���
                        {
                            tbRWData.Text += string.Format("{0:X2} ", byteArray[i]);
                        }
                        labResult.Text = rm.GetString("strMsgSussecReadData");
                    }
                    else
                    {
                        labResult.Text = rm.GetString("strMsgFailedReadData");
                    }
                }
            }
            catch
            {
                labResult.Text = rm.GetString("strMsgFailedReadData");
            }

        }



        private byte GetAnt()
        {
            if (radioButton10.Checked)
                return 1;
            if (radioButton9.Checked)
                return 2;
            if (radioButton4.Checked)
                return 3;
            if (radioButton3.Checked)
                return 4;
            if (radioButton8.Checked)
                return 5;
            if (radioButton7.Checked)
                return 6;
            if (radioButton6.Checked)
                return 7;
            if (radioButton5.Checked)
                return 8;
            return 0;
        }

        /// <summary>
        /// д��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWriteData_Click(object sender, EventArgs e)
        {
            int RWBank = -1;
            int StartAdd = -1;
            int Length = -1;
            labResult.Text = "";
            if ((RWBank = cbbRWBank.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgSelectRWBank"));
                return;
            }
            StartAdd = int.Parse(cbbStartAdd.Text);
            Length = int.Parse(cbbLength.Text);

            string strData = tbRWData.Text.Replace(" ", "");// ȥ�ո�
            if (strData.Length % 4 != 0 || strData.Length / 4 != Length)
            {
                MessageBox.Show(rm.GetString("strMsgLengthDiff"));
                return;
            }
            if (!IsHexCharacter(strData))
            {
                MessageBox.Show(rm.GetString("strMsgPwdInvalidChar"));
                return;
            }

            byte[] byteArray = new byte[256];
            for (int i = 0; i < 2 * Length; ++i)
            {
                byteArray[i] = Convert.ToByte(strData.Substring(2 * i, 2), 16);
            }
            if (checkBox10.Checked)
            {
                if (1 == Dis.WriteEpcByAnt(deviceNo, (byte)RWBank, (byte)StartAdd, (byte)Length, GetAnt(), 01, byteArray))
                {
                    labResult.Text = rm.GetString("strMsgSucceedWrite");
                }
                else
                {
                    labResult.Text = rm.GetString("strMsgFailedWrite");
                }
            }
            else
            {
                 /*if (1 == Dis.WriteTagData(deviceNo, (byte)RWBank, (byte)StartAdd, (byte)Length, byteArray))
                 {
                     labResult.Text = rm.GetString("strMsgSucceedWrite");
                 }
                 else
                 {
                     labResult.Text = rm.GetString("strMsgFailedWrite");
                 }*/
                if (1 == Dis.WriteTagMultiWord(deviceNo, (byte)RWBank, (byte)StartAdd, (byte)Length, byteArray))
                {
                    labResult.Text = rm.GetString("strMsgSucceedWrite");
                }
                else
                {
                    labResult.Text = rm.GetString("strMsgFailedWrite");
                }
            }
        }
        private bool IsHexCharacter(string str)
        {
            return Regex.IsMatch(str, "^[0-9A-Fa-f]+$");
        }

        /// <summary>
        /// ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLockTag_Click(object sender, EventArgs e)
        {
            int lockBank = -1;
            labResult.Text = "";

            string strpwd = tbLockAccessPwd.Text.Replace(" ", "");
            if (strpwd.Length != 8)
            {
                MessageBox.Show(rm.GetString("strMsgPwdMustEight"));
                return;
            }
            if (!IsHexCharacter(strpwd))
            {
                MessageBox.Show(rm.GetString("strMsgPwdInvalidChar"));
                return;
            }
            if ((lockBank = cbbLockBank.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgSelecOprBank"));
                return;
            }

            byte[] pwd = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                pwd[i] = Convert.ToByte(strpwd.Substring(i * 2, 2), 16); // ���ַ������Ӵ�תΪ16���Ƶ�8λ�޷�������
            }
            if (1 == Dis.LockTag(deviceNo, (byte)lockBank, pwd))
            {
                labResult.Text = rm.GetString("strMsgSucceedLock");
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedLock");
            }
            return;
        }

        /// <summary>
        /// ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnKillTag_Click(object sender, EventArgs e)
        {
            string strKillPwd = tbKillKillPwd.Text.Replace(" ", "");
            if (strKillPwd.Length != 8)
            {
                MessageBox.Show(rm.GetString("strMsgPwdMustEight"));
                return;
            }
            if (!IsHexCharacter(strKillPwd))
            {
                MessageBox.Show(rm.GetString("strMsgPwdInvalidChar"));
                return;
            }
            byte[] byteAccessPwd = new byte[4];
            byte[] byteKillPwd = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                byteKillPwd[i] = Convert.ToByte(strKillPwd.Substring(i * 2, 2), 16);
            }
            if (1 == Dis.KillTag(deviceNo, byteKillPwd))
            {
                labResult.Text = rm.GetString("strMsgSucceedDes");
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedDes");
            }
        }
        bool IsDecNumber(string str)
        {
            return Regex.IsMatch(str, "^[0-9]+$");
        }

        /// <summary>
        ///  Ƶ�ʼ�������������
        ///  2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetFreq_Click(object sender, EventArgs e)
        {
            //  ����豸��
            if (!int.TryParse(tbNewDevNo.Text, out m_DeviceNo))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidDevNo"));
                return;
            }
            else
            {
                if (m_DeviceNo > 255)
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidDevNo"));
                    return;
                }
            }
            // ��鹦��ֵ
            if (!int.TryParse(tbPower.Text, out m_Power))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                return;
            }
            else
            {
                //if (m_Power<100)
                //{
                //    //MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                //    //return;

                //    DialogResult drs = MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                //    if (drs == DialogResult.OK)
                //    {
                //        m_Power=0;
                //        btnReadFreq_Click(sender,e);
                //        return;
                //    }
                //}
                //else if (m_Power > 150)
                //{
                //    //MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                //    //return;
                //    DialogResult drt = MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                //    if (drt == DialogResult.OK)
                //    {
                //        m_Power = 0;
                //        btnReadFreq_Click(sender, e);
                //        return;
                //    }
                //}
                if (m_Power < 1 || m_Power > 150)
                {
                    DialogResult drs = MessageBox.Show(rm.GetString("strMsgInvalidPower"));
                    if (drs == DialogResult.OK)
                    {
                        m_Power = 0;
                        btnReadFreq_Click(sender, e);
                        return;
                    }
                }
            }
            // ��鵥���࿨ģʽ
            if ((m_ReadingMode = cbbSingOrMulti.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidSingleOrMulti"));
                return;
            }
            if ((m_EPCMode = epcModeCB.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("StrFormatError"));
                return;
            }
            if ((m_Band = cboBand.SelectedIndex) == 0 || (m_Band = cboBand.SelectedIndex) == 2)                 //����   ����     
            {
                // ���Ƶ��ģʽ
                if ((m_FreqMode = cbbFreqModeUS.SelectedIndex) == -1)
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidFreqMode"));
                    return;
                }
                // ���Ƶ��
                else if (cbbFreqModeUS.SelectedIndex == 1)
                 {
                     if ((m_FreqPoint = cbbFreqPointUS.SelectedIndex) == -1)
                     return;
                 }
            }
            else if ((m_Band = cboBand.SelectedIndex) == 1)             //ŷ��
            {
                // ���Ƶ��ģʽ
                if ((m_FreqMode = cboFreqModeEU.SelectedIndex) == -1)
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidFreqMode"));
                    return;
                }
                else if (cboFreqModeEU.SelectedIndex == 1)
                {
                    //2017-03-29  ZW  ����ŷ���¶�Ƶ���õ��ж�
                    if ((m_FreqPoint = cboFreqPointEU.SelectedIndex) == -1)
                    {
                        MessageBox.Show(rm.GetString("strMsgInvalidFreqPoint"));
                        return;
                    }
                }
            }


            if (m_FreqMode > 0) // ��Ƶģʽ
            {
                m_FreqMode = m_FreqPoint + 1; // д��ֵΪƵ������
            }
            int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_USERCODE, (byte)m_DeviceNo);
            Dis.SetSingleParameter(deviceNo, Dis.ADDR_12BYTE_FLAG, (byte)m_EPCMode);
            if (m_EPCMode == 0)
                 state = 2;
            else
                 state = 1;
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_POWER, (byte)m_Power);
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_SINGLE_OR_MULTI_TAG, (byte)m_ReadingMode);
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_FREQUENCY_SET, (byte)m_FreqMode);
            if (state == 1)
                res *= Dis.SetSingleParameter(0, Dis.ADD_BAND_SET, (byte)m_Band);
            else
                res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_FRQUENCY_AREA, (byte)m_Band);//�����ĸ����ҵĹ������� 2016-04-25 hz

            byte[] fp = new byte[8];
            if (cboBand.SelectedIndex == 0 || cboBand.SelectedIndex == 2)//����
            {
                // �������Ƶ��
                m_fp[0] = cbFp1.Checked ? 1 : 0;
                m_fp[1] = cbFp2.Checked ? 1 : 0;
                m_fp[2] = cbFp3.Checked ? 1 : 0;
                m_fp[3] = cbFp4.Checked ? 1 : 0;
                m_fp[4] = cbFp5.Checked ? 1 : 0;
                m_fp[5] = cbFp6.Checked ? 1 : 0;
                m_fp[6] = cbFp7.Checked ? 1 : 0;
                m_fp[7] = cbFp8.Checked ? 1 : 0;
                m_fp[8] = cbFp9.Checked ? 1 : 0;
                m_fp[9] = cbFp10.Checked ? 1 : 0;

                m_fp[10] = cbFp11.Checked ? 1 : 0;
                m_fp[11] = cbFp12.Checked ? 1 : 0;
                m_fp[12] = cbFp13.Checked ? 1 : 0;
                m_fp[13] = cbFp14.Checked ? 1 : 0;
                m_fp[14] = cbFp15.Checked ? 1 : 0;
                m_fp[15] = cbFp16.Checked ? 1 : 0;
                m_fp[16] = cbFp17.Checked ? 1 : 0;
                m_fp[17] = cbFp18.Checked ? 1 : 0;
                m_fp[18] = cbFp19.Checked ? 1 : 0;
                m_fp[19] = cbFp20.Checked ? 1 : 0;

                m_fp[20] = cbFp21.Checked ? 1 : 0;
                m_fp[21] = cbFp22.Checked ? 1 : 0;
                m_fp[22] = cbFp23.Checked ? 1 : 0;
                m_fp[23] = cbFp24.Checked ? 1 : 0;
                m_fp[24] = cbFp25.Checked ? 1 : 0;
                m_fp[25] = cbFp26.Checked ? 1 : 0;
                m_fp[26] = cbFp27.Checked ? 1 : 0;
                m_fp[27] = cbFp28.Checked ? 1 : 0;
                m_fp[28] = cbFp29.Checked ? 1 : 0;
                m_fp[29] = cbFp30.Checked ? 1 : 0;

                m_fp[30] = cbFp31.Checked ? 1 : 0;
                m_fp[31] = cbFp32.Checked ? 1 : 0;
                if (cboBand.SelectedIndex == 0)
                {
                    m_fp[32] = cbFp33.Checked ? 1 : 0;
                    m_fp[33] = cbFp34.Checked ? 1 : 0;
                    m_fp[34] = cbFp35.Checked ? 1 : 0;
                    m_fp[35] = cbFp36.Checked ? 1 : 0;
                    m_fp[36] = cbFp37.Checked ? 1 : 0;
                    m_fp[37] = cbFp38.Checked ? 1 : 0;
                    m_fp[38] = cbFp39.Checked ? 1 : 0;
                    m_fp[39] = cbFp40.Checked ? 1 : 0;

                    m_fp[40] = cbFp41.Checked ? 1 : 0;
                    m_fp[41] = cbFp42.Checked ? 1 : 0;
                    m_fp[42] = cbFp43.Checked ? 1 : 0;
                    m_fp[43] = cbFp44.Checked ? 1 : 0;
                    m_fp[44] = cbFp45.Checked ? 1 : 0;
                    m_fp[45] = cbFp46.Checked ? 1 : 0;
                    m_fp[46] = cbFp47.Checked ? 1 : 0;
                    m_fp[47] = cbFp48.Checked ? 1 : 0;
                    m_fp[48] = cbFp49.Checked ? 1 : 0;
                    m_fp[49] = cbFp50.Checked ? 1 : 0;
                }
                else if (cboBand.SelectedIndex == 2)
                {
                    m_fp[32] = 0;
                    m_fp[33] = 0;
                    m_fp[34] = 0;
                    m_fp[35] = 0;
                    m_fp[36] = 0;
                    m_fp[37] = 0;
                    m_fp[38] = 0;
                    m_fp[39] = 0;

                    m_fp[40] = 0;
                    m_fp[41] = 0;
                    m_fp[42] = 0;
                    m_fp[43] = 0;
                    m_fp[44] = 0;
                    m_fp[45] = 0;
                    m_fp[46] = 0;
                    m_fp[47] = 0;
                    m_fp[48] = 0;
                    m_fp[49] = 0;
                 }
                


                fp[0] += (byte)m_fp[0];
                fp[0] += (byte)(m_fp[1] << 1);
                fp[0] += (byte)(m_fp[2] << 2);
                fp[0] += (byte)(m_fp[3] << 3);
                fp[0] += (byte)(m_fp[4] << 4);
                fp[0] += (byte)(m_fp[5] << 5);
                fp[0] += (byte)(m_fp[6] << 6);
                fp[0] += (byte)(m_fp[7] << 7);

                fp[1] += (byte)m_fp[8];
                fp[1] += (byte)(m_fp[9] << 1);
                fp[1] += (byte)(m_fp[10] << 2);
                fp[1] += (byte)(m_fp[11] << 3);
                fp[1] += (byte)(m_fp[12] << 4);
                fp[1] += (byte)(m_fp[13] << 5);
                fp[1] += (byte)(m_fp[14] << 6);
                fp[1] += (byte)(m_fp[15] << 7);

                fp[2] += (byte)m_fp[16];
                fp[2] += (byte)(m_fp[17] << 1);
                fp[2] += (byte)(m_fp[18] << 2);
                fp[2] += (byte)(m_fp[19] << 3);
                fp[2] += (byte)(m_fp[20] << 4);
                fp[2] += (byte)(m_fp[21] << 5);
                fp[2] += (byte)(m_fp[22] << 6);
                fp[2] += (byte)(m_fp[23] << 7);

                fp[3] += (byte)m_fp[24];
                fp[3] += (byte)(m_fp[25] << 1);
                fp[3] += (byte)(m_fp[26] << 2);
                fp[3] += (byte)(m_fp[27] << 3);
                fp[3] += (byte)(m_fp[28] << 4);
                fp[3] += (byte)(m_fp[29] << 5);
                fp[3] += (byte)(m_fp[30] << 6);
                fp[3] += (byte)(m_fp[31] << 7);

                fp[4] += (byte)m_fp[32];
                fp[4] += (byte)(m_fp[33] << 1);
                fp[4] += (byte)(m_fp[34] << 2);
                fp[4] += (byte)(m_fp[35] << 3);
                fp[4] += (byte)(m_fp[36] << 4);
                fp[4] += (byte)(m_fp[37] << 5);
                fp[4] += (byte)(m_fp[38] << 6);
                fp[4] += (byte)(m_fp[39] << 7);

                fp[5] += (byte)m_fp[40];
                fp[5] += (byte)(m_fp[41] << 1);
                fp[5] += (byte)(m_fp[42] << 2);
                fp[5] += (byte)(m_fp[43] << 3);
                fp[5] += (byte)(m_fp[44] << 4);
                fp[5] += (byte)(m_fp[45] << 5);
                fp[5] += (byte)(m_fp[46] << 6);
                fp[5] += (byte)(m_fp[47] << 7);

                fp[6] += (byte)m_fp[48];
                fp[6] += (byte)(m_fp[49] << 1);
            }
            else if (cboBand.SelectedIndex == 1) //ŷ��
            {
                // �������Ƶ��
                m_EU[0] = chkFp1.Checked ? 1 : 0;
                m_EU[1] = chkFp2.Checked ? 1 : 0;
                m_EU[2] = chkFp3.Checked ? 1 : 0;
                m_EU[3] = chkFp4.Checked ? 1 : 0;
                m_EU[4] = chkFp5.Checked ? 1 : 0;
                m_EU[5] = chkFp6.Checked ? 1 : 0;
                m_EU[6] = chkFp7.Checked ? 1 : 0;
                m_EU[7] = chkFp8.Checked ? 1 : 0;
                m_EU[8] = chkFp9.Checked ? 1 : 0;
                m_EU[9] = chkFp10.Checked ? 1 : 0;
                m_EU[10] = chkFp11.Checked ? 1 : 0;
                m_EU[11] = chkFp12.Checked ? 1 : 0;

                fp[0] += (byte)m_EU[0];
                fp[0] += (byte)(m_EU[1] << 1);
                fp[0] += (byte)(m_EU[2] << 2);
                fp[0] += (byte)(m_EU[3] << 3);
                fp[0] += (byte)(m_EU[4] << 4);
                fp[0] += (byte)(m_EU[5] << 5);
                fp[0] += (byte)(m_EU[6] << 6);
                fp[0] += (byte)(m_EU[7] << 7);

                fp[1] += (byte)m_EU[8];
                fp[1] += (byte)(m_EU[9] << 1);
                fp[1] += (byte)(m_EU[10] << 2);
                fp[1] += (byte)(m_EU[11] << 3);

            }
            res *= Dis.SetMultiParameters(deviceNo, Dis.ADD_FREQUENCY_PARA_92, 7, fp);
            if (0 != res)
            {
                MessageBox.Show(rm.GetString("strMsgSucceedSetFreq"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetFreq"));
            }

        }


        private void OtherOpreation_Enter_Fun(object sender, EventArgs e)
        {
            if (isStartMuliti)
            {
                ;
            }
            else
            {
                btnReadBeep_Click(null, null);
                button3_Click_1(null, null);
                button4_Click(null, null);
            }

        }
        /// <summary>
        /// ����ģʽ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetWorkMode_Click(object sender, EventArgs e)
        {
            // ����ģʽ
            if ((m_WorkMode = cbbWorkMode.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidWordMode"));
                return;
            }
            if (cbbWorkMode.SelectedIndex == 1)
            {
                // ��ʱ����
                if (!int.TryParse(tbTiming.Text, out m_TimingInterval))
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidTimingParam"));
                    return;
                }
                else
                {
                    if (m_TimingInterval < 2 || m_TimingInterval > 100)
                    {
                        MessageBox.Show(rm.GetString("strMsgInvalidTimingParam"));
                        return;
                    }
                }
            }

            if (cbbWorkMode.SelectedIndex == 2)
            {
                // ������ʱ����

                if (!int.TryParse(tbDelay.Text, out m_DelayTime))
                {
                    MessageBox.Show(rm.GetString("strMsgInvalidDelayTime"));
                    return;
                }
                else
                {
                    if (m_DelayTime < 0 || m_DelayTime > 300)
                    {
                        MessageBox.Show(rm.GetString("strMsgInvalidDelayTime"));
                        return;
                    }
                }
            }
            // �������ز���
            if ((m_TrigSwitch = cbbTrigSwitch.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidTrigSwitch"));
                return;
            }

            // �����б�ʱ��
            if (!int.TryParse(tbNeighJudge.Text, out m_AdjaDisTime))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidNJ"));
                return;
            }
            else
            {
                if (m_AdjaDisTime > 255 || m_AdjaDisTime < 0)
                {
                    MessageBox.Show(rm.GetString("stMsgNJValid"));
                    return;
                }
            }
            // �����б�
            if (cbAjaDisc.Checked)
            {
                m_AdjaDis = 1;
            }
            else
            {
                m_AdjaDis = 2;
            }
            //int readSpeed =(cboReadSpeed.Text == null || cboReadSpeed.Text == "") ? 1 : int.Parse(cboReadSpeed.Text);
            int readSpeed = (cboReadSpeed.Text == "10") ? 0 : 1;
            // д�����ֵ
            int bRes = Dis.SetSingleParameter(deviceNo, Dis.ADD_WORKMODE, (byte)(m_WorkMode + 1));    // ����ģʽ
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_TIME_INTERVAL, (byte)m_TimingInterval);  // ��ʱ����
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_TRIG_DELAYTIME, (byte)m_DelayTime);      // �����ӳ�ʱ��
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_TRIG_SWITCH, (byte)m_TrigSwitch);        // ���ùܽ�0
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_TRIG_MODE, 0x1);                         // �ܽ�0���óɸߵ�ƽģʽ��0Ϊ�͵�ƽ
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_NEIGHJUDGE_TIME, (byte)m_AdjaDisTime);   // �����б�ʱ��
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_NEIGHJUDGE_SET, (byte)m_AdjaDis);        // ���������б�
            bRes *= Dis.SetSingleParameter(deviceNo, Dis.ADD_READSPEED, (byte)readSpeed);             //���õ�λ�����ٶ� 2016-04-26 hz

            if (0 != bRes)
            {
                MessageBox.Show(rm.GetString("strMsgSucceedSetWorkMode"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetWorkMode"));
            }
        }

        private void cbbLangSwitch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //// ������������
            cbbLockBank.Items.Clear();
            cbbWorkMode.Items.Clear();
            cbbTrigSwitch.Items.Clear();
            cbbCommMode.Items.Clear();
            cbbSingOrMulti.Items.Clear();
            cbbFreqModeUS.Items.Clear();
            cboFreqModeEU.Items.Clear();
            cbbBeepControl.Items.Clear();
            cbbUsbFormat.Items.Clear();
            cboBand.Items.Clear();
            epcModeCB.Items.Clear();

            if (cbbLangSwitch.SelectedIndex == 0)
            {
                // ���ܲ�����
                cbbLockBank.Items.Add("�û���");
                cbbLockBank.Items.Add("TID");
                cbbLockBank.Items.Add("EPC");
                cbbLockBank.Items.Add("����������");
                cbbLockBank.Items.Add("���������");
                cbbLockBank.Items.Add("ȫ������");
                // ����ģʽ
                cbbWorkMode.Items.Add("����ģʽ");
                cbbWorkMode.Items.Add("��ʱģʽ");
                cbbWorkMode.Items.Add("����ģʽ");
                // ����ģʽ
                cbbTrigSwitch.Items.Add("��");
                cbbTrigSwitch.Items.Add("�ߵ�ƽ����");

                // ͨ�ŷ�ʽ
                cbbCommMode.Items.Add("RS485/RJ45");
                cbbCommMode.Items.Add("Τ��");
                cbbCommMode.Items.Add("RS232");
                // ����ģʽ
                cbbSingOrMulti.Items.Add("����ǩ");
                cbbSingOrMulti.Items.Add("���ǩ");
                // Ƶ������
                cbbFreqModeUS.Items.Add("��Ƶ");
                cbbFreqModeUS.Items.Add("��Ƶ");

                cboFreqModeEU.Items.Add("��Ƶ");
                cboFreqModeEU.Items.Add("��Ƶ");
                // Ƶ�ʱ���
                cboBand.Items.Add("����");
                cboBand.Items.Add("ŷ��");
                cboBand.Items.Add("����");
                //cboBand.Items.Add("����");
                // ������������
                cbbBeepControl.Items.Add("�ر�");
                cbbBeepControl.Items.Add("����Beep");
                cbbBeepControl.Items.Add("ֻ��һ��");
                // USB�����ʽ
                cbbUsbFormat.Items.Add("Τ��26(1�ֽ�+2�ֽ�)��8λ10������");
                cbbUsbFormat.Items.Add("Τ��26(3�ֽ�)��8λʮ������");
                cbbUsbFormat.Items.Add("00+3�ֽ�16����");
                cbbUsbFormat.Items.Add("4�ֽ�16����");
                cbbUsbFormat.Items.Add("12�ֽ�16����");
                cbbUsbFormat.Items.Add("12�ֽ�TID");
                cbbUsbFormat.Items.Add("0000+ǰ3�ֽ�16����");
                cbbUsbFormat.Items.Add("ԭʼ����(��֤ԭʼ������Ascii)");

                epcModeCB.Items.Add("�ǹ̶�����");
                epcModeCB.Items.Add("12�ֽ�");
            }
            else
            {
                cbbLockBank.Items.Add("User");
                cbbLockBank.Items.Add("EPC");
                cbbLockBank.Items.Add("AccessPwd");
                cbbLockBank.Items.Add("KillPwd");
                cbbLockBank.Items.Add("All");
                // ����ģʽ
                cbbWorkMode.Items.Add("Master/Slave");
                cbbWorkMode.Items.Add("Timing");
                cbbWorkMode.Items.Add("Trigger");
                // ����ģʽ
                cbbTrigSwitch.Items.Add("Off");
                cbbTrigSwitch.Items.Add("High level");

                // ͨ�ŷ�ʽ
                cbbCommMode.Items.Add("RS485/RJ45");
                cbbCommMode.Items.Add("Wiegand");
                cbbCommMode.Items.Add("RS232");
                // ����ģʽ
                cbbSingOrMulti.Items.Add("Single Tag");
                cbbSingOrMulti.Items.Add("Multi Tag");
                // Ƶ������
                cbbFreqModeUS.Items.Add("Freq Hopping");
                cbbFreqModeUS.Items.Add("Fixed Freq");

                cboFreqModeEU.Items.Add("Freq Hopping");
                cboFreqModeEU.Items.Add("Fixed Freq");

                // Ƶ�ʱ���
                cboBand.Items.Add("US Band");
                cboBand.Items.Add("EU Band");
                cboBand.Items.Add("Korea Band");
                //cboBand.Items.Add("Chinese Band");

                // ������������
                cbbBeepControl.Items.Add("Off");
                cbbBeepControl.Items.Add("Continuous beep");
                cbbBeepControl.Items.Add("Only once");
                // USB�����ʽ
                cbbUsbFormat.Items.Add("Wiegand26 (1B+2B) 8bits(Dec) ");
                cbbUsbFormat.Items.Add("Wiegand26 3B 8bits(Dec)");
                cbbUsbFormat.Items.Add("00+3Bytes(Hex)");
                cbbUsbFormat.Items.Add("4Bytes(Hex)");
                cbbUsbFormat.Items.Add("12Bytes(Hex)");
                cbbUsbFormat.Items.Add("12Bytes(TID)");
                cbbUsbFormat.Items.Add("0000+3Bytes(Front) ");
                cbbUsbFormat.Items.Add("Raw data (ensure that the original data is Ascii)");

                epcModeCB.Items.Add("Non-standard length EPC");
                epcModeCB.Items.Add("Standard 12-byte EPC");
            }
            // ������ѡ��������ַ���
            // if (System.Threading.Thread.CurrentThread.CurrentCulture.Name == "zh-CN")
            // {
            //    cbbLangSwitch.SelectedIndex = 0;
            // }
            //  else
            //  {
            //      cbbLangSwitch.SelectedIndex = 1;
            //  }
            rm = rmArray[cbbLangSwitch.SelectedIndex];
            // Tab��ǩҳ����
            General.Text = rm.GetString("strTpGeneral");
            TagAccess.Text = rm.GetString("strTpTagAccess");
            SetCommParam.Text = rm.GetString("strTpSetCommParam");
            SetReaderParam.Text = rm.GetString("strTpSetReaderParam");
            OtherOpreation.Text = rm.GetString("strTpOtherOpr");

            // ��ͷ����
            listView.Columns[0].Text = rm.GetString("strLvHeadNo");
            listView.Columns[1].Text = rm.GetString("strLvHeadEPC");
            listView.Columns[2].Text = rm.GetString("strLvHeadCount");
            listView.Columns[3].Text = rm.GetString("strLvHeadAntNo");
            listView.Columns[4].Text = rm.GetString("strLvHeadDevNo");
            lvZl.Columns[0].Text = rm.GetString("strZlHeadNo");
            lvZl.Columns[1].Text = rm.GetString("strZlHeadIP");
            lvZl.Columns[2].Text = rm.GetString("strZlHeadPort");
            lvZl.Columns[3].Text = rm.GetString("strZlHeadMAC");

            //������������Ϣ
            if (rbSerialPort.Checked)
            {
                lbl_ip.Text = rm.GetString("strCommPort");
                labCommPort.Text = rm.GetString("strDatas");
            }
            else
            {
                labCommPort.Text = rm.GetString("strLabCommPort");
                lbl_ip.Text = rm.GetString("strIP");
            }
            // ��ť����
            btnConnect.Text = rm.GetString("strBtnConnect");
            btnDisconnect.Text = rm.GetString("strBtnDisconnect");
            btnUpdate.Text = rm.GetString("strBtnUpdate");
            btnStartReadData.Text = rm.GetString("strBtnStartReadData");
            btnStopReadData.Text = rm.GetString("strBtnStopReadData");
            btnClearListView.Text = rm.GetString("strBtnClearListView");
            btnReadOnce.Text = rm.GetString("strBtnReadOnce");
            btnDefaultParams.Text = rm.GetString("strBtnDefaultParams");
            btnKillTag.Text = rm.GetString("strBtnKillTag");
            btnModifyDev.Text = rm.GetString("strBtnModifyDev");
            btnLockTag.Text = rm.GetString("strBtnLock");
            btnUnlockTag.Text = rm.GetString("strBtnUnlockTag");
            btnInitTag.Text = rm.GetString("strBtnInitTag");
            btnReadData.Text = rm.GetString("strBtnReadTagData");
            btnSearchDev.Text = rm.GetString("strBtnSearchDev");
            btnSetFreq.Text = rm.GetString("strBtnSetDevNo");
            btnSetWorkMode.Text = rm.GetString("strBtnSetNeighJudge");
            btnSetParams.Text = rm.GetString("strBtnSetParams");
            btnWriteData.Text = rm.GetString("strBtnWriteTagData");
            btnClearData.Text = rm.GetString("strBtnClearEditData");
            btnFastWrite.Text = rm.GetString("strBtnFastWrite");
            btnClearFWData.Text = rm.GetString("strBtnClearFWData");
            btnReadWorkMode.Text = rm.GetString("strBtnReadWorkMode");
            btnDefaultWorkMode.Text = rm.GetString("strBtnDefaultWorkMode");
            btnSetWorkMode.Text = rm.GetString("strBtnSetWorkMode");
            btnReadCommMode.Text = rm.GetString("strBtnReadCommMode");
            btnDefaultCommMode.Text = rm.GetString("strBtnDefaultCommMode");
            btnSetCommMode.Text = rm.GetString("strBtnSetCommMode");
            btnReadFreq.Text = rm.GetString("strBtnReadFreq");
            btnSetFreq.Text = rm.GetString("strBtnSetFreq");
            btnDefaultFreq.Text = rm.GetString("strBtnDefaultFreq");
            btnClearFreq.Text = rm.GetString("strBtnClearFreq");
            btnTagAuth.Text = rm.GetString("strBtnTagAuth");
            btnModifyAuthPwd.Text = rm.GetString("strBtnModAuthPwd");
            btnSetBeep.Text = rm.GetString("strBtnSetBeep");
            btnSetUsbFormat.Text = rm.GetString("strBtnSetUsbFormat");
            btnReadBeep.Text = rm.GetString("btnReadBeep");
            rdoOpen1.Text = rm.GetString("rdoOpen1");
            rdoClose1.Text = rm.GetString("rdoClose1");
            rdoOpen2.Text = rm.GetString("rdoOpen2");
            rdoClose2.Text = rm.GetString("rdoClose2");
            chkZD.Text = rm.GetString("chkZD");
            //lbl_ip.Text = rm.GetString("");
            btn_connRead.Text = rm.GetString("strBtnConnRead");
            btn_stoptimer.Text = rm.GetString("strStopRead");
            button2.Text = rm.GetString("strSwitch");
            //�����˵�
            ����ToolStripMenuItem.Text = rm.GetString("saveStr");
            radioButton11.Text = rm.GetString("strSingleReadMode");
            radioButton12.Text = rm.GetString("strMultiReadMode1");
            //��д����
            checkBox9.Text = rm.GetString("strReadByAnt");
            checkBox10.Text = rm.GetString("strWriteByAnt");
            radioButton10.Text = rm.GetString("chkAnt1");
            radioButton9.Text = rm.GetString("chkAnt2");
            radioButton4.Text = rm.GetString("chkAnt3");
            radioButton3.Text = rm.GetString("chkAnt4");
            radioButton8.Text = rm.GetString("chkAnt5");
            radioButton7.Text = rm.GetString("chkAnt6");
            radioButton6.Text = rm.GetString("chkAnt7");
            radioButton5.Text = rm.GetString("chkAnt8");
            // RadioButton��ʾ
            rbReadSingleTag.Text = rm.GetString("strRbReadSingleTag");
            rbReadMultiTag.Text = rm.GetString("strRbReadMultiTag");
            rbAsc.Text = rm.GetString("strRbAsc");
            rbDesc.Text = rm.GetString("strRbDesc");
            rbNet.Text = rm.GetString("strRbNet");
            rbSerialPort.Text = rm.GetString("strRbSerialPort");
            label3.Text = rm.GetString("strWeigand");
            radioButton1.Text = rm.GetString("strWeigand21");
            radioButton2.Text = rm.GetString("strWeigand30");
            // CheckBox��ʾ

            // GroupBox˵������
            gbCommMode.Text = rm.GetString("strGbCommMode");
            gbReadMode.Text = rm.GetString("strGbReadMode");
            gbAdvancedOpr.Text = rm.GetString("strGbAdvanceOpr");
            gbWorkMode.Text = rm.GetString("strGbDevParams");
            gbFastWrite.Text = rm.GetString("strGbFastWrite");
            gbNetParams.Text = rm.GetString("strGbNetParams");
            gbRWData.Text = rm.GetString("strGbRWData");
            gbSPParams.Text = rm.GetString("strGbSPParams");
            gbCommModeParam.Text = rm.GetString("strGbCommModeParam");
            gbFreq.Text = rm.GetString("strGbFreq");
            gbTagAuth.Text = rm.GetString("strGbTagAuth");
            gbBeepControl.Text = rm.GetString("strGbBeepControl");
            gbUsbFormat.Text = rm.GetString("strGbUsbFormat");

            cbAjaDisc.Text = rm.GetString("strChkAjaDis");
            cbSaveFile.Text = rm.GetString("strChkSaveFile");

            grpAntSet.Text = rm.GetString("grpAntSet");
            chkAnt1.Text = rm.GetString("chkAnt1");
            chkAnt2.Text = rm.GetString("chkAnt2");
            chkAnt3.Text = rm.GetString("chkAnt3");
            chkAnt4.Text = rm.GetString("chkAnt4");
            chkAnt5.Text = rm.GetString("chkAnt5");
            chkAnt6.Text = rm.GetString("chkAnt6");
            chkAnt7.Text = rm.GetString("chkAnt7");
            chkAnt8.Text = rm.GetString("chkAnt8");
            btnAntRead.Text = rm.GetString("btnAntRead");
            btnAntSet.Text = rm.GetString("btnAntSet");

            // Label��ǩ
            labBaudRate.Text = rm.GetString("strLabBaudRate");
            labCheckBits.Text = rm.GetString("strLabCheckBits");
            labDataBits.Text = rm.GetString("strLabDataBits");
            labDestIP.Text = rm.GetString("strLabDestIP");
            labDeviceNo.Text = rm.GetString("strLabDeviceNo");
            labDestPort.Text = rm.GetString("strLabDestPort");
            labDestroyPwd.Text = rm.GetString("strLabDestrlyPwd");
            labDevNo.Text = rm.GetString("strLabDevNo");
            labGateway.Text = rm.GetString("strLabGateway");
            labIPAdd.Text = rm.GetString("strLabIPAdd");
            labIPMode.Text = rm.GetString("strLabIPMode");
            labLength.Text = rm.GetString("strLabLength");
            labLockAccessPwd.Text = rm.GetString("strLabLockAccessPwd");
            labLockBank.Text = rm.GetString("strLabLockBank");
            labMask.Text = rm.GetString("strLabMask");
            labNetMode.Text = rm.GetString("strLabNetMode");
            labPort.Text = rm.GetString("strLabPort");
            labPromotion.Text = rm.GetString("strLabPromotion");
            labOprBank.Text = rm.GetString("strLabRWBank");
            labData.Text = rm.GetString("strLabRWData");
            labStartAdd.Text = rm.GetString("strLabStartAdd");
            labReadCount.Text = rm.GetString("strLabCount");
            labTagCount.Text = rm.GetString("strLabTagCount");
            labFWData.Text = rm.GetString("strLabFWData");
            labFWPromo.Text = rm.GetString("strLabFWPromotion");
            labWorkMode.Text = rm.GetString("strLabWorkMode");
            labTimingParam.Text = rm.GetString("strLabTimingParam");
            labTimingUnit.Text = rm.GetString("strLabTimingUnit");
            labTrigSwitch.Text = rm.GetString("strLabTrigSwitch");
            labTrigParam.Text = rm.GetString("strLabTrigParam");
            labDelayUnit.Text = rm.GetString("strLabDelayUnit");
            labCommMode.Text = rm.GetString("strLabCommMode");
            labPulseWidth.Text = rm.GetString("strLabPulseWidth");
            labPulseWidthUnit.Text = rm.GetString("strLabPulseWidthUnit");
            labPulseCycle.Text = rm.GetString("strLabPulseCycle");
            labPulseCycleUnit.Text = rm.GetString("strLabPulseCycleUnit");
            labWiegandProtocol.Text = rm.GetString("strLabWiegandProtocol");
            labPower.Text = rm.GetString("strLabPower");
            labSingleOrMulti.Text = rm.GetString("strLabReadMode");
            labFreq.Text = rm.GetString("strLabFreqSet");
            lblFreq.Text = rm.GetString("strLabFreqSet");
            labAuthPwd.Text = rm.GetString("strLabAuthPwd");
            labNewAuthPwd.Text = rm.GetString("strLabNewAuthPwd");
            lblWigginsTakePlaceValue.Text = rm.GetString("lblWigginsTakePlaceValue");
            lblReadVoice.Text = rm.GetString("lblReadVoice");

            //2017-2-21 ZW ������������Ӣ��
            lbl_rate.Text = rm.GetString("strLabBaudRate");
            epcModeCB.SelectedIndex = 1;
            // �����л���������½ǽ����ʾ��
            labResult.Text = "";
            labelVersion.Text = "";
            labSetParam.Text = "";
            button3.Text = rm.GetString("strBtnReadTagData");
            button4.Text = rm.GetString("strBtnReadTagData");
            //�̵�������
            GopRelayControl.Text = rm.GetString("GopRelayControl");
            rdoOpenRelay.Text = rm.GetString("rdoOpenRelay");
            rdoCloseRelay.Text = rm.GetString("rdoCloseRelay");
            lblCloseTime.Text = rm.GetString("lblCloseTime");
            btnSetRelayTime.Text = rm.GetString("btnSetRelayTime");
            AdvancedAccessBtn.Text = rm.GetString("advancedAccessStr");
            AutoAuthorizationBt.Text = rm.GetString("startAutoAuthorization");
            AutoAuthorizationBt.Text = rm.GetString("startAutoAuthorization");
            label1.Text = rm.GetString("AuthorizationResult");
            authorizationLb.Text = rm.GetString("NoneAuthorizationTag");
            label2.Text = rm.GetString("strLabRWBank");
            button1.Text = rm.GetString("strBtnSetAlive");
            EpcFormatLB.Text = rm.GetString("strFormatEPC");

            button5.Text = rm.GetString("strGetAuthorizationCode");
            AuthorizationSuccess = rm.GetString("authorizationSuccess");
            NoneAuthorizationTag = rm.GetString("NoneAuthorizationTag");
            if (bathReadWrite != null)
            {
                bathReadWrite.ChangeLanguage(rm);
            }

        }


        /// <summary>
        /// Ѱ��һ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadOnce_Click(object sender, EventArgs e)
        {
            isReadOnce = true;
            labelVersion.Text = "";
            byte[] outData = new byte[256];
            byte[] antNo = new byte[16];
            byte[] res = new byte[256]; // ����HandleData�����Ľ����
            int dv = 0;
            Dis.GetSingleParameter(deviceNo, 0x64, out dv);// ��һ���ֽ����豸��
            res[0] = (byte)dv;

            if (Dis.ReadSingleTag(deviceNo, outData, antNo) == 0)
            {
                for (int i = 0; i < outData[1] + 2; i++)
                {
                    res[i] = outData[i];
                }
                res[outData[1] + 2] = antNo[0];// �����ֽ������ߺ�
                //res[outData[0]  �豸��
                InsertTag(res, outData[1] + 2);
            }
            else
            {
                isReadOnce = false;
                //nub -= 1;
                //if (nub == 0)
                labelVersion.Text = rm.GetString("strMsgFailedReadData");
            }
        }
        // �ѱ�ǩ���ݲ��뵽�������
        static void InsertTag(byte[] data, int length)
        {
            string epc = "";
            for (int i = 2; i < length; ++i)
            {
                epc += string.Format("{0:X2} ", data[i]);
            }
            bNewTag = true;
            lock (Tag_data)
            {
                for (int i = 0; i < Tag_data.Count; ++i)
                {
                    if (epc == Tag_data[i].epc)
                    {
                        Tag_data[i].count++;
                        Tag_data[i].antNo = data[length];
                        Tag_data[i].devNo = data[0];
                        bNewTag = false;     // �����±�ǩ
                        nItemNo = i;             //��¼��������ֵ�����ڸ���listView��
                        break;
                    }
                }
                if (bNewTag)
                {
                    EPC_data epcdata = new EPC_data();
                    epcdata.epc = epc;
                    epcdata.antNo = data[length];
                    epcdata.devNo = data[0];
                    epcdata.count = 1;
                    Tag_data.Add(epcdata);
                }
            }
            UpdateControl(); // �������ݲ���������listView
        }

        /// <summary>
        /// ��������
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbReadMultiTag_CheckedChanged(object sender, EventArgs e)
        {
            btnStartReadData.Enabled = true;
            btnReadOnce.Enabled = false;
        }

        /// <summary>
        /// ����ʶ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbReadSingleTag_CheckedChanged(object sender, EventArgs e)
        {
            btnStartReadData.Enabled = false;
            btnReadOnce.Enabled = true;
        }

        /// <summary>
        /// ��д
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFastWrite_Click(object sender, EventArgs e)
        {
            string strData = tbFWData.Text.Replace(" ", "");
            if (strData.Length == 0)
            {
                MessageBox.Show(rm.GetString("strMsgDataNotEmpty"));
                return;
            }
            if (!Regex.IsMatch(strData, "^[0-9A-Fa-f]+$"))
            {
                MessageBox.Show(rm.GetString("strMsgPwdInvalidChar"));
                return;
            }
            if (strData.Length % 4 != 0)
            {
                MessageBox.Show(rm.GetString("strMsgDataMustFourTimes"));
                return;
            }
            int leng = strData.Length / 2;
            byte[] byteArray = new byte[64];
            for (int i = 0; i < leng; ++i)
            {
                byteArray[i] = Convert.ToByte(strData.Substring(2 * i, 2), 16);
            }
            string strEpc = strData;
            if (1 == Dis.FastWriteTag(deviceNo, byteArray, (byte)(leng / 2)))
            {
                labResult.Text = rm.GetString("strMsgSucceedWrite");
                if (chkZD.Checked)//�Զ���1 2016-04-08 hz
                {
                    strEpc = PageFun.AddStr(strData, 1, 0);
                    for (int i = 0; i < leng - 1; ++i)
                    {
                        // strEpc += string.Format("{0:X2} ", byteArray[i]);
                        strEpc = PageFun.AddStr(strData, 1, 0);
                    }
                }
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedWrite");
            }
            for (int i = 0; i < leng; ++i)
            {
                byteArray[i] = Convert.ToByte(strEpc.Substring(2 * i, 2), 16);
            }
            strEpc = "";
            for (int i = 0; i < leng; ++i)
            {
                strEpc += string.Format("{0:X2} ", byteArray[i]);
            }
            tbFWData.Text = strEpc;
        }

        /// <summary>
        /// ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFWData_Click(object sender, EventArgs e)
        {
            tbFWData.Text = "";
            tbFWData.Focus();
        }

        /// <summary>
        /// ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnlockTag_Click(object sender, EventArgs e)
        {
            labResult.Text = "";
            string strpwd = tbLockAccessPwd.Text.Replace(" ", "");
            if (strpwd.Length != 8)
            {
                MessageBox.Show(rm.GetString("strMsgPwdMustEight"));
                return;
            }
            if (!IsHexCharacter(strpwd))
            {
                MessageBox.Show(rm.GetString("strMsgPwdInvalidChar"));
                return;
            }

            byte[] pwd = new byte[4];
            for (int i = 0; i < 4; ++i)
            {
                pwd[i] = Convert.ToByte(strpwd.Substring(i * 2, 2), 16); // ���ַ������Ӵ�תΪ16���Ƶ�8λ�޷�������
            }
            int unlockBank = cbbLockBank.SelectedIndex;
            if (-1 == unlockBank)
            {
                MessageBox.Show(rm.GetString("strMsgSelecOprBank"));
                return;
            }
            if (1 == Dis.UnlockTag(deviceNo, (byte)unlockBank, pwd))
            {
                labResult.Text = rm.GetString("strMsgSucceedUnlock");
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedUnlock");
            }
        }

        /// <summary>
        /// ��ʼ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInitTag_Click(object sender, EventArgs e)
        {
            labResult.Text = "";
            //int res = Dis.InitializeTag(deviceNo);
            //MessageBox.Show(res.ToString());
            //return;
            if (1 == Dis.InitializeTag(deviceNo))
            {
                labResult.Text = rm.GetString("strMsgSucceedInit");
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedInit");
            }
        }

        // ����ģʽ��������
        int m_WorkMode = 0;
        int m_TimingInterval = 0;
        int m_DelayTime = 0;
        int m_TrigSwitch = 0;
        int m_AdjaDis = 0;
        int m_AdjaDisTime = 0;

        // ͨ��ģʽ��������
        int m_CommMode = 0;
        int m_WiegandProto = 0;
        int m_PulseWidth = 0;
        int m_PulseCycle = 0;
        int m_WiegandValue = 0;

        #region 2017-2-21  ZW ��������
        int m_Rate = 0;
        #endregion

        // Ƶ�ʼ�������������
        int m_DeviceNo = 0;
        int m_Power = 0;
        int m_ReadingMode = 0;
        int m_EPCMode = 0;
        int m_FreqMode = 0;
        int m_FreqPoint = 0;
        int m_Band = 0;
        int[] m_fp = new int[50];
        int[] m_EU = new int[12];

        // �����ٶ�
        int m_ReadSpeed = 1;

        // �̵�������
        int m_Relay_C_O = 0;
        int m_Relay_Time = 0;

        //����
        int ant = 0;
        int p_ant = 0;
        int[] a_fp = new int[8];

        /// <summary>
        /// ����ģʽ��ȡ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadWorkMode_Click(object sender, EventArgs e)
        {
            try
            {
                Dis.GetSingleParameter(deviceNo, Dis.ADD_WORKMODE, out m_WorkMode);
                Dis.GetSingleParameter(deviceNo, Dis.ADD_TIME_INTERVAL, out m_TimingInterval);
                Dis.GetSingleParameter(deviceNo, Dis.ADD_TRIG_DELAYTIME, out m_DelayTime);
                Dis.GetSingleParameter(deviceNo, Dis.ADD_TRIG_SWITCH, out m_TrigSwitch);
                Dis.GetSingleParameter(deviceNo, Dis.ADD_NEIGHJUDGE_TIME, out m_AdjaDisTime);                    // �����б�ʱ��
                Dis.GetSingleParameter(deviceNo, Dis.ADD_NEIGHJUDGE_SET, out m_AdjaDis);                                // �����б�
                Dis.GetSingleParameter(deviceNo, Dis.ADD_READSPEED, out m_ReadSpeed); //��ȡ�����ٶ� 2016-04-26 hz

                cbbWorkMode.SelectedIndex = m_WorkMode - 1;
                //    // OnCbnSelchangeComboWorkmode();

                tbDelay.Text = m_DelayTime.ToString();
                tbTiming.Text = m_TimingInterval.ToString();
                tbNeighJudge.Text = m_AdjaDisTime.ToString();
                cbbTrigSwitch.SelectedIndex = 0x1 & m_TrigSwitch;
                int readspeed = 0;
                if (m_ReadSpeed == 1)
                    readspeed = 0;
                //else if (m_ReadSpeed == 10)
                else if (m_ReadSpeed == 0)
                    readspeed = 1;
                else
                    readspeed = 1;
                cboReadSpeed.SelectedIndex = readspeed;
                if (1 == m_AdjaDis)
                {
                    cbAjaDisc.Checked = true;
                }
                else
                {
                    cbAjaDisc.Checked = false;
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
            }

        }

        /// <summary>
        /// ����ģʽĬ�ϲ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultWorkMode_Click(object sender, EventArgs e)
        {
            cbbWorkMode.SelectedIndex = 1;
        }

        /// <summary>
        /// ͨ�ŷ�ʽ��ȡ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadCommMode_Click(object sender, EventArgs e)
        {
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_COMM_MODE, out m_CommMode);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PROTO, out m_WiegandProto);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PULSEWIDTH, out m_PulseWidth);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PULSECYCLE, out m_PulseCycle);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_WIEGAND_VALUE, out m_WiegandValue);

                //2017.2.21  ZW �����������ʲ���
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_BAUD_RATE, out m_Rate);
                if (0 != res)
                {
                    cbbCommMode.SelectedIndex = m_CommMode - 1;
                    cbbWiegandProtocol.SelectedIndex = (m_WiegandProto == 255 ? 1 : m_WiegandProto) - 1;
                    tbPulseWidth.Text = m_PulseWidth.ToString();
                    tbPulseCycle.Text = m_PulseCycle.ToString();
                    cboWigginsTakePlaceValue.SelectedIndex = m_WiegandValue > 61 ? 61 : m_WiegandValue;

                    //2017-04-24 �������ʵ���ʾ
                    if (m_Rate == 255)
                    {
                    }
                    else
                    {
                        cmb_rate.SelectedIndex = m_Rate;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
            }
        }

        /// <summary>
        /// ͨ�ŷ�ʽĬ�ϲ���
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultCommMode_Click(object sender, EventArgs e)
        {
            cbbCommMode.SelectedIndex = 2;
        }

        /// <summary>
        /// ͨ�ŷ�ʽ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetCommMode_Click(object sender, EventArgs e)
        {
            // ͨ�ŷ�ʽ
            if ((m_CommMode = cbbCommMode.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidCommMode"));
                return;
            }
            // Τ��Э��
            if ((m_WiegandProto = cbbWiegandProtocol.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidWiegandProto"));
                return;
            }
            // Τ��������
            if (!int.TryParse(tbPulseWidth.Text, out m_PulseWidth))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidPulseWidth"));
                return;
            }
            // ��������
            if (!int.TryParse(tbPulseCycle.Text, out m_PulseCycle))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidPulseCycle"));
                return;
            }
            // Τ��ȡλֵ
            if ((m_WiegandValue = cboWigginsTakePlaceValue.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidWiegandValue"));
                return;
            }

            #region 2016-10-24  ZW ���������ж�
            if (cbbCommMode.SelectedIndex == 2)
            {
                if ((m_Rate = cmb_rate.SelectedIndex) == -1)
                {
                    MessageBox.Show(rm.GetString("strSetRate"));
                    return;
                }
            }
            #endregion

            int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_COMM_MODE, (byte)(m_CommMode + 1));
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PROTO, (byte)(m_WiegandProto + 1));
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PULSEWIDTH, (byte)m_PulseWidth);
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_WIEGAND_PULSECYCLE, (byte)m_PulseCycle);
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_WIEGAND_VALUE, (byte)m_WiegandValue);

            //2017.2.21  ZW �����������ʲ���
            res *= Dis.SetSingleParameter(deviceNo, Dis.ADD_BAUD_RATE, (byte)m_Rate);

            if (0 != res)
            {
                MessageBox.Show(rm.GetString("strMsgSucceedSetCommMode"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetCommMode"));
            }
        }

        /// <summary>
        /// Ƶ�ʼ�����������ȡ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadFreq_Click(object sender, EventArgs e)
        {
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_USERCODE, out m_DeviceNo);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_POWER, out m_Power);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_SINGLE_OR_MULTI_TAG, out m_ReadingMode);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_FREQUENCY_SET, out m_FreqPoint); // Ƶ��ģʽ
                Dis.GetSingleParameter(deviceNo, Dis.ADDR_12BYTE_FLAG, out m_EPCMode); // Ƶ��ģʽ
                byte[] fp = new byte[10];
                res *= Dis.GetMultiParameters(deviceNo, Dis.ADD_FREQUENCY_PARA_92, 7, fp);
                if (state == 1)
                    res *= Dis.GetSingleParameter(0, Dis.ADD_BAND_SET, out m_Band);
                else
                    res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_FRQUENCY_AREA, out m_Band);//��ȡ�ĸ����ҵĹ������� 2016-04-25 hz
                if (m_EPCMode<2)
                epcModeCB.SelectedIndex = m_EPCMode;
                m_fp[0] = fp[3] & Dis.MASK_BIT_0;
                m_fp[1] = fp[3] & Dis.MASK_BIT_1;
                m_fp[2] = fp[3] & Dis.MASK_BIT_2;
                m_fp[3] = fp[3] & Dis.MASK_BIT_3;
                m_fp[4] = fp[3] & Dis.MASK_BIT_4;
                m_fp[5] = fp[3] & Dis.MASK_BIT_5;
                m_fp[6] = fp[3] & Dis.MASK_BIT_6;
                m_fp[7] = fp[3] & Dis.MASK_BIT_7;

                m_fp[8] = fp[4] & Dis.MASK_BIT_0;
                m_fp[9] = fp[4] & Dis.MASK_BIT_1;
                m_fp[10] = fp[4] & Dis.MASK_BIT_2;
                m_fp[11] = fp[4] & Dis.MASK_BIT_3;
                m_fp[12] = fp[4] & Dis.MASK_BIT_4;
                m_fp[13] = fp[4] & Dis.MASK_BIT_5;
                m_fp[14] = fp[4] & Dis.MASK_BIT_6;
                m_fp[15] = fp[4] & Dis.MASK_BIT_7;

                m_fp[16] = fp[5] & Dis.MASK_BIT_0;
                m_fp[17] = fp[5] & Dis.MASK_BIT_1;
                m_fp[18] = fp[5] & Dis.MASK_BIT_2;
                m_fp[19] = fp[5] & Dis.MASK_BIT_3;
                m_fp[20] = fp[5] & Dis.MASK_BIT_4;
                m_fp[21] = fp[5] & Dis.MASK_BIT_5;
                m_fp[22] = fp[5] & Dis.MASK_BIT_6;
                m_fp[23] = fp[5] & Dis.MASK_BIT_7;

                m_fp[24] = fp[6] & Dis.MASK_BIT_0;
                m_fp[25] = fp[6] & Dis.MASK_BIT_1;
                m_fp[26] = fp[6] & Dis.MASK_BIT_2;
                m_fp[27] = fp[6] & Dis.MASK_BIT_3;
                m_fp[28] = fp[6] & Dis.MASK_BIT_4;
                m_fp[29] = fp[6] & Dis.MASK_BIT_5;
                m_fp[30] = fp[6] & Dis.MASK_BIT_6;
                m_fp[31] = fp[6] & Dis.MASK_BIT_7;

                m_fp[32] = fp[7] & Dis.MASK_BIT_0;
                m_fp[33] = fp[7] & Dis.MASK_BIT_1;
                m_fp[34] = fp[7] & Dis.MASK_BIT_2;
                m_fp[35] = fp[7] & Dis.MASK_BIT_3;
                m_fp[36] = fp[7] & Dis.MASK_BIT_4;
                m_fp[37] = fp[7] & Dis.MASK_BIT_5;
                m_fp[38] = fp[7] & Dis.MASK_BIT_6;
                m_fp[39] = fp[7] & Dis.MASK_BIT_7;

                m_fp[40] = fp[8] & Dis.MASK_BIT_0;
                m_fp[41] = fp[8] & Dis.MASK_BIT_1;
                m_fp[42] = fp[8] & Dis.MASK_BIT_2;
                m_fp[43] = fp[8] & Dis.MASK_BIT_3;
                m_fp[44] = fp[8] & Dis.MASK_BIT_4;
                m_fp[45] = fp[8] & Dis.MASK_BIT_5;
                m_fp[46] = fp[8] & Dis.MASK_BIT_6;
                m_fp[47] = fp[8] & Dis.MASK_BIT_7;

                m_fp[48] = fp[9] & Dis.MASK_BIT_0;
                m_fp[49] = fp[9] & Dis.MASK_BIT_1;


                if (0 == m_FreqPoint) // ��Ϊ��Ƶģʽ
                {
                    m_FreqMode = 0;// ��0��ΪƵ������
                }
                else if (m_FreqPoint > 0)��// �ǣ�Ϊ��Ƶģʽ
                {
                    m_FreqMode = 1;
                }
                if (0 != res)
                {
                    tbNewDevNo.Text = m_DeviceNo.ToString();
                    tbPower.Text = m_Power.ToString();
                    cbbSingOrMulti.SelectedIndex = m_ReadingMode;
                  //  cboBand.SelectedIndex = (m_Band != 0 && m_Band != 1) ? 0 : m_Band;
                    cboBand.SelectedIndex = m_Band>2?0:m_Band;
                    if (cboBand.SelectedIndex == 0 || cboBand.SelectedIndex == 2)
                    {
                        cbbFreqModeUS.SelectedIndex = m_FreqMode;
                        if (m_FreqPoint > 0)
                            cbbFreqPointUS.SelectedIndex = m_FreqPoint - 1;

                        cbFp1.Checked = m_fp[0] == 0 ? false : true;
                        cbFp2.Checked = m_fp[1] == 0 ? false : true;
                        cbFp3.Checked = m_fp[2] == 0 ? false : true;
                        cbFp4.Checked = m_fp[3] == 0 ? false : true;
                        cbFp5.Checked = m_fp[4] == 0 ? false : true;
                        cbFp6.Checked = m_fp[5] == 0 ? false : true;
                        cbFp7.Checked = m_fp[6] == 0 ? false : true;
                        cbFp8.Checked = m_fp[7] == 0 ? false : true;
                        cbFp9.Checked = m_fp[8] == 0 ? false : true;
                        cbFp10.Checked = m_fp[9] == 0 ? false : true;

                        cbFp11.Checked = m_fp[10] == 0 ? false : true;
                        cbFp12.Checked = m_fp[11] == 0 ? false : true;
                        cbFp13.Checked = m_fp[12] == 0 ? false : true;
                        cbFp14.Checked = m_fp[13] == 0 ? false : true;
                        cbFp15.Checked = m_fp[14] == 0 ? false : true;
                        cbFp16.Checked = m_fp[15] == 0 ? false : true;
                        cbFp17.Checked = m_fp[16] == 0 ? false : true;
                        cbFp18.Checked = m_fp[17] == 0 ? false : true;
                        cbFp19.Checked = m_fp[18] == 0 ? false : true;
                        cbFp20.Checked = m_fp[19] == 0 ? false : true;

                        cbFp21.Checked = m_fp[20] == 0 ? false : true;
                        cbFp22.Checked = m_fp[21] == 0 ? false : true;
                        cbFp23.Checked = m_fp[22] == 0 ? false : true;
                        cbFp24.Checked = m_fp[23] == 0 ? false : true;
                        cbFp25.Checked = m_fp[24] == 0 ? false : true;
                        cbFp26.Checked = m_fp[25] == 0 ? false : true;
                        cbFp27.Checked = m_fp[26] == 0 ? false : true;
                        cbFp28.Checked = m_fp[27] == 0 ? false : true;
                        cbFp29.Checked = m_fp[28] == 0 ? false : true;
                        cbFp30.Checked = m_fp[29] == 0 ? false : true;

                        cbFp31.Checked = m_fp[30] == 0 ? false : true;
                        cbFp32.Checked = m_fp[31] == 0 ? false : true;
                        cbFp33.Checked = m_fp[32] == 0 ? false : true;
                        cbFp34.Checked = m_fp[33] == 0 ? false : true;
                        cbFp35.Checked = m_fp[34] == 0 ? false : true;
                        cbFp36.Checked = m_fp[35] == 0 ? false : true;
                        cbFp37.Checked = m_fp[36] == 0 ? false : true;
                        cbFp38.Checked = m_fp[37] == 0 ? false : true;
                        cbFp39.Checked = m_fp[38] == 0 ? false : true;
                        cbFp40.Checked = m_fp[39] == 0 ? false : true;

                        cbFp41.Checked = m_fp[40] == 0 ? false : true;
                        cbFp42.Checked = m_fp[41] == 0 ? false : true;
                        cbFp43.Checked = m_fp[42] == 0 ? false : true;
                        cbFp44.Checked = m_fp[43] == 0 ? false : true;
                        cbFp45.Checked = m_fp[44] == 0 ? false : true;
                        cbFp46.Checked = m_fp[45] == 0 ? false : true;
                        cbFp47.Checked = m_fp[46] == 0 ? false : true;
                        cbFp48.Checked = m_fp[47] == 0 ? false : true;
                        cbFp49.Checked = m_fp[48] == 0 ? false : true;
                        cbFp50.Checked = m_fp[49] == 0 ? false : true;
                    }
                    else if (cboBand.SelectedIndex == 1)
                    {
                        cboFreqModeEU.SelectedIndex = m_FreqMode;
                        if (m_FreqPoint > 0)
                            cboFreqPointEU.SelectedIndex = m_FreqPoint - 1;

                        chkFp1.Checked = m_fp[0] == 0 ? false : true;
                        chkFp2.Checked = m_fp[1] == 0 ? false : true;
                        chkFp3.Checked = m_fp[2] == 0 ? false : true;
                        chkFp4.Checked = m_fp[3] == 0 ? false : true;
                        chkFp5.Checked = m_fp[4] == 0 ? false : true;

                        chkFp6.Checked = m_fp[5] == 0 ? false : true;
                        chkFp7.Checked = m_fp[6] == 0 ? false : true;
                        chkFp8.Checked = m_fp[7] == 0 ? false : true;
                        chkFp9.Checked = m_fp[8] == 0 ? false : true;
                        chkFp10.Checked = m_fp[9] == 0 ? false : true;
                        chkFp11.Checked = m_fp[10] == 0 ? false : true;
                        chkFp12.Checked = m_fp[11] == 0 ? false : true;
                    }
                }
                else
                {
                    MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
            }

        }

        /// <summary>
        ///  Ƶ�ʼ���������Ĭ�ϲ���
        ///  2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDefaultFreq_Click(object sender, EventArgs e)
        {
            cboBand.SelectedIndex = 0;
            tbNewDevNo.Text = "0";
            tbPower.Text = "150";
            cbbSingOrMulti.SelectedIndex = 0;
            cbbFreqModeUS.SelectedIndex = 0;
            cbbFreqPointUS.SelectedIndex = 0;
            epcModeCB.SelectedIndex = 0;
            cbFp1.Checked = false;
            cbFp2.Checked = false;
            cbFp3.Checked = false;
            cbFp4.Checked = false;
            cbFp5.Checked = false;
            cbFp6.Checked = false;
            cbFp7.Checked = false;
            cbFp8.Checked = false;
            cbFp9.Checked = false;
            cbFp10.Checked = false;

            cbFp11.Checked = false;
            cbFp12.Checked = false;
            cbFp13.Checked = false;
            cbFp14.Checked = false;
            cbFp15.Checked = false;
            cbFp16.Checked = false;
            cbFp17.Checked = false;
            cbFp18.Checked = false;
            cbFp19.Checked = false;
            cbFp20.Checked = false;

            cbFp21.Checked = false;
            cbFp22.Checked = false;
            cbFp23.Checked = false;
            cbFp24.Checked = false;
            cbFp25.Checked = false;
            cbFp26.Checked = false;
            cbFp27.Checked = false;
            cbFp28.Checked = false;
            cbFp29.Checked = false;
            cbFp30.Checked = false;

            cbFp31.Checked = false;
            cbFp32.Checked = false;
            cbFp33.Checked = false;
            cbFp34.Checked = false;
            cbFp35.Checked = false;
            cbFp36.Checked = false;
            cbFp37.Checked = false;
            cbFp38.Checked = false;
            cbFp39.Checked = false;
            cbFp40.Checked = false;

            cbFp41.Checked = true;
            cbFp42.Checked = true;
            cbFp43.Checked = true;
            cbFp44.Checked = true;
            cbFp45.Checked = true;
            cbFp46.Checked = true;
            cbFp47.Checked = true;
            cbFp48.Checked = true;
            cbFp49.Checked = true;
            cbFp50.Checked = true;
        }

        /// <summary>
        ///  Ƶ�ʼ������������Ƶ��
        ///  2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearFreq_Click(object sender, EventArgs e)
        {
            cbFp1.Checked = false;
            cbFp2.Checked = false;
            cbFp3.Checked = false;
            cbFp4.Checked = false;
            cbFp5.Checked = false;
            cbFp6.Checked = false;
            cbFp7.Checked = false;
            cbFp8.Checked = false;
            cbFp9.Checked = false;
            cbFp10.Checked = false;

            cbFp11.Checked = false;
            cbFp12.Checked = false;
            cbFp13.Checked = false;
            cbFp14.Checked = false;
            cbFp15.Checked = false;
            cbFp16.Checked = false;
            cbFp17.Checked = false;
            cbFp18.Checked = false;
            cbFp19.Checked = false;
            cbFp20.Checked = false;

            cbFp21.Checked = false;
            cbFp22.Checked = false;
            cbFp23.Checked = false;
            cbFp24.Checked = false;
            cbFp25.Checked = false;
            cbFp26.Checked = false;
            cbFp27.Checked = false;
            cbFp28.Checked = false;
            cbFp29.Checked = false;
            cbFp30.Checked = false;

            cbFp31.Checked = false;
            cbFp32.Checked = false;
            cbFp33.Checked = false;
            cbFp34.Checked = false;
            cbFp35.Checked = false;
            cbFp36.Checked = false;
            cbFp37.Checked = false;
            cbFp38.Checked = false;
            cbFp39.Checked = false;
            cbFp40.Checked = false;

            cbFp41.Checked = false;
            cbFp42.Checked = false;
            cbFp43.Checked = false;
            cbFp44.Checked = false;
            cbFp45.Checked = false;
            cbFp46.Checked = false;
            cbFp47.Checked = false;
            cbFp48.Checked = false;
            cbFp49.Checked = false;
            cbFp50.Checked = false;

            chkFp1.Checked = false;
            chkFp2.Checked = false;
            chkFp3.Checked = false;
            chkFp4.Checked = false;
            chkFp5.Checked = false;
            chkFp6.Checked = false;
            chkFp7.Checked = false;
            chkFp8.Checked = false;
            chkFp9.Checked = false;
            chkFp10.Checked = false;
            chkFp11.Checked = false;
            chkFp12.Checked = false;
        }

        /// <summary>
        /// ����ģʽ������/��ʱ/������
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbWorkMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbNeighJudge.Visible = cbAjaDisc.Checked;
            switch (cbbWorkMode.SelectedIndex)
            {
                case 0: // ����
                    {
                        labTimingParam.Visible = false;
                        labTimingUnit.Visible = false;
                        labDelayUnit.Visible = false;
                        labTrigParam.Visible = false;
                        labTrigSwitch.Visible = false;
                        tbTiming.Visible = false;
                        tbDelay.Visible = false;
                        cbbTrigSwitch.Visible = false;
                        cboReadSpeed.Visible = false;
                    }
                    break;
                case 1:// ��ʱ
                    {
                        labTimingParam.Visible = true;
                        labTimingUnit.Visible = true;
                        labDelayUnit.Visible = false;
                        labTrigParam.Visible = false;
                        labTrigSwitch.Visible = false;
                        tbTiming.Visible = true;
                        tbDelay.Visible = false;
                        cbbTrigSwitch.Visible = false;
                        cboReadSpeed.Visible = true;
                        cboReadSpeed.SelectedIndex = 1;
                        tbTiming.Text = "20";
                    }
                    break;
                case 2: // ����
                    {
                        labTimingParam.Visible = false;
                        labTimingUnit.Visible = false;
                        labDelayUnit.Visible = true;
                        labTrigParam.Visible = true;
                        labTrigSwitch.Visible = true;
                        tbTiming.Visible = false;
                        tbDelay.Visible = true;
                        cbbTrigSwitch.Visible = true;
                        cboReadSpeed.Visible = false;

                        tbTiming.Text = "5";
                        tbDelay.Text = "10";
                        cbAjaDisc.Checked = false;
                        tbNeighJudge.Text = "5";
                    }
                    break;
            }

        }

        /// <summary>
        /// ͨ�ŷ�ʽ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbCommMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbCommMode.SelectedIndex)
            {
                case -1:
                case 0: // RS485/RJ45
                    {
                        #region 2017-4-24  ZW �����������ʵ���ʾ
                        lbl_rate.Visible = true;
                        cmb_rate.Visible = true;
                        cmb_rate.SelectedIndex = 0;

                        labWiegandProtocol.Visible = false;
                        labPulseWidth.Visible = false;
                        labPulseCycle.Visible = false;
                        labPulseWidthUnit.Visible = false;
                        labPulseCycleUnit.Visible = false;
                        tbPulseWidth.Visible = false;
                        tbPulseCycle.Visible = false;
                        cbbWiegandProtocol.Visible = false;
                        lblWigginsTakePlaceValue.Visible = false;
                        cboWigginsTakePlaceValue.Visible = false;
                        #endregion

                    }
                    break;
                case 2: // RS232
                    {
                        lbl_rate.Visible = true;
                        cmb_rate.Visible = true;
                        cmb_rate.SelectedIndex = 0;

                        labWiegandProtocol.Visible = false;
                        labPulseWidth.Visible = false;
                        labPulseCycle.Visible = false;
                        labPulseWidthUnit.Visible = false;
                        labPulseCycleUnit.Visible = false;
                        tbPulseWidth.Visible = false;
                        tbPulseCycle.Visible = false;
                        cbbWiegandProtocol.Visible = false;
                        lblWigginsTakePlaceValue.Visible = false;
                        cboWigginsTakePlaceValue.Visible = false;
                    }
                    break;
                case 1: // Wiegand
                    {
                        lbl_rate.Visible = false;
                        cmb_rate.Visible = false;

                        labWiegandProtocol.Visible = true;
                        labPulseWidth.Visible = true;
                        labPulseCycle.Visible = true;
                        labPulseWidthUnit.Visible = true;
                        labPulseCycleUnit.Visible = true;
                        tbPulseWidth.Visible = true;
                        tbPulseCycle.Visible = true;
                        cbbWiegandProtocol.Visible = true;
                        lblWigginsTakePlaceValue.Visible = true;
                        cboWigginsTakePlaceValue.Visible = true;

                        cbbWiegandProtocol.SelectedIndex = 0;
                        tbPulseWidth.Text = "10";
                        tbPulseCycle.Text = "16";
                        cboWigginsTakePlaceValue.SelectedIndex = 9;
                    }
                    break;
            }
        }

        /// <summary>
        /// Ƶ������
        /// </summary>
        /// <param name="bVisibled"></param>
        private void ShowFreqPointEU(bool bVisibled)
        {
            chkFp1.Visible = bVisibled;
            chkFp2.Visible = bVisibled;
            chkFp3.Visible = bVisibled;
            chkFp4.Visible = bVisibled;
            chkFp5.Visible = bVisibled;

            chkFp6.Visible = bVisibled;
            chkFp7.Visible = bVisibled;
            chkFp8.Visible = bVisibled;
            chkFp9.Visible = bVisibled;
            chkFp10.Visible = bVisibled;
            chkFp11.Visible = bVisibled;
            chkFp12.Visible = bVisibled;
        }

        /// <summary>
        /// Ƶ������
        /// </summary>
        /// <param name="bVisibled"></param>
        private void ShowFreqPoint(bool bVisibled)
        {
            cbFp1.Visible = bVisibled;
            cbFp2.Visible = bVisibled;
            cbFp3.Visible = bVisibled;
            cbFp4.Visible = bVisibled;
            cbFp5.Visible = bVisibled;
            cbFp6.Visible = bVisibled;
            cbFp7.Visible = bVisibled;
            cbFp8.Visible = bVisibled;
            cbFp9.Visible = bVisibled;
            cbFp10.Visible = bVisibled;

            cbFp11.Visible = bVisibled;
            cbFp12.Visible = bVisibled;
            cbFp13.Visible = bVisibled;
            cbFp14.Visible = bVisibled;
            cbFp15.Visible = bVisibled;
            cbFp16.Visible = bVisibled;
            cbFp17.Visible = bVisibled;
            cbFp18.Visible = bVisibled;
            cbFp19.Visible = bVisibled;
            cbFp20.Visible = bVisibled;

            cbFp21.Visible = bVisibled;
            cbFp22.Visible = bVisibled;
            cbFp23.Visible = bVisibled;
            cbFp24.Visible = bVisibled;
            cbFp25.Visible = bVisibled;
            cbFp26.Visible = bVisibled;
            cbFp27.Visible = bVisibled;
            cbFp28.Visible = bVisibled;
            cbFp29.Visible = bVisibled;
            cbFp30.Visible = bVisibled;

            cbFp31.Visible = bVisibled;
            cbFp32.Visible = bVisibled;
            cbFp33.Visible = bVisibled;
            cbFp34.Visible = bVisibled;
            cbFp35.Visible = bVisibled;
            cbFp36.Visible = bVisibled;
            cbFp37.Visible = bVisibled;
            cbFp38.Visible = bVisibled;
            cbFp39.Visible = bVisibled;
            cbFp40.Visible = bVisibled;

            cbFp41.Visible = bVisibled;
            cbFp42.Visible = bVisibled;
            cbFp43.Visible = bVisibled;
            cbFp44.Visible = bVisibled;
            cbFp45.Visible = bVisibled;
            cbFp46.Visible = bVisibled;
            cbFp47.Visible = bVisibled;
            cbFp48.Visible = bVisibled;
            cbFp49.Visible = bVisibled;
            cbFp50.Visible = bVisibled;
        }

        /// <summary>
        /// Ƶ������
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbFreqMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbbFreqModeUS.SelectedIndex)
            {
                case -1:
                case 0: // ��Ƶ
                    {
                        cbbFreqPointUS.Visible = false;
                        
                        if (cboBand.SelectedIndex == 2)
                        {
                            ShowFreqPoint(true);
                            ShowFreqKoreaPoint();
                        }
                        else
                        {
                            ShowFreqPoint(true);
                            ShowFreqUSPoint();
                        }
                    }
                    break;
                case 1:// ��Ƶ
                    {
                        cbbFreqPointUS.Visible = true;
                        
                        if (cboBand.SelectedIndex == 2)
                        {
                            ShowFreqPoint(false);
                            ShowFreqKoreaFixPoint();
                        }
                        else
                        {
                            ShowFreqPoint(false);
                            ShowFixUSPoint();
                        }
                    }
                    break;
            }
        }

        private void ShowFixUSPoint()
        {
            cbbFreqPointUS.Items.Clear();
            for (int i = 0; i < 50; ++i)
            {
                cbbFreqPointUS.Items.Add(string.Format("{0}-{1:F1}Mhz", i + 1, 902.5 + i * 0.5));
            }
        }

        private void ShowFreqUSPoint()
        {

            cbFp1.Text = "902.5";
            cbFp2.Text = "903.0";
            cbFp3.Text = "903.5";
            cbFp4.Text = "904.0";
            cbFp5.Text = "904.5";
            cbFp6.Text = "905.0";
            cbFp7.Text = "905.5";
            cbFp8.Text = "906.0";
            cbFp9.Text = "906.5";
            cbFp10.Text = "907.0";

            cbFp11.Text = "907.5";
            cbFp12.Text = "908.0";
            cbFp13.Text = "908.5";
            cbFp14.Text = "909.0";
            cbFp15.Text = "909.5";
            cbFp16.Text = "910.0";
            cbFp17.Text = "910.5";
            cbFp18.Text = "911.0";
            cbFp19.Text = "911.5";
            cbFp20.Text = "912.0";

            cbFp21.Text = "912.5";
            cbFp22.Text = "913.0";
            cbFp23.Text = "913.5";
            cbFp24.Text = "914.0";
            cbFp25.Text = "914.5";
            cbFp26.Text = "915.0";
            cbFp27.Text = "915.5";
            cbFp28.Text = "916.0";
            cbFp29.Text = "916.5";
            cbFp30.Text = "917.0";

            cbFp31.Text = "917.0";
            cbFp32.Text = "918.0";
        }

        private void ShowFreqKoreaFixPoint()
        {
            cbbFreqPointUS.Items.Clear();
            for (int i = 0; i < 32; ++i)
            {
                cbbFreqPointUS.Items.Add(string.Format("{0}-{1:F1}Mhz", i + 1, 917.1 + i * 0.2));
            }
        }

        private void ShowFreqKoreaPoint()
        {
            cbFp1.Visible = true;
          
            cbFp2.Visible = true;
            cbFp3.Visible = true;
            cbFp4.Visible = true;
            cbFp5.Visible = true;
            cbFp6.Visible = true;
            cbFp7.Visible = false;
            cbFp8.Visible = false;
            cbFp9.Visible = false;
            cbFp10.Visible = false;

            cbFp11.Visible = false;
            cbFp12.Visible = false;
            cbFp13.Visible = false;
            cbFp14.Visible = false;
            cbFp15.Visible = false;
            cbFp16.Visible = false;
            cbFp17.Visible = false;
            cbFp18.Visible = false;
            cbFp19.Visible = false;
            cbFp20.Visible = false;

            cbFp21.Visible = false;
            cbFp22.Visible = false;
            cbFp23.Visible = false;
            cbFp24.Visible = false;
            cbFp25.Visible = false;
            cbFp26.Visible = false;
            cbFp27.Visible = false;
            cbFp28.Visible = false;
            cbFp29.Visible = false;
            cbFp30.Visible = false;

            cbFp31.Visible = false;
            cbFp32.Visible = false;
            cbFp33.Visible = false;
            cbFp34.Visible = false;
            cbFp35.Visible = false;
            cbFp36.Visible = false;
            cbFp37.Visible = false;
            cbFp38.Visible = false;
            cbFp39.Visible = false;
            cbFp40.Visible = false;

            cbFp41.Visible = false;
            cbFp42.Visible = false;
            cbFp43.Visible = false;
            cbFp44.Visible = false;
            cbFp45.Visible = false;
            cbFp46.Visible = false;
            cbFp47.Visible = false;
            cbFp48.Visible = false;
            cbFp49.Visible = false;
            cbFp50.Visible = false;


            cbFp1.Text = "917.3";
            cbFp2.Text = "917.9";
            cbFp3.Text = "918.5";
            cbFp4.Text = "919.1";
            cbFp5.Text = "919.7";
            cbFp6.Text = "920.3";
            cbFp7.Text = "918.3";
            cbFp16.Text = "920.1";
            cbFp17.Text = "920.3";
            cbFp18.Text = "920.5";
            cbFp19.Text = "920.7";
            cbFp20.Text = "920.9";

            cbFp21.Text = "921.1";
            cbFp22.Text = "921.3";
            cbFp23.Text = "921.5";
            cbFp24.Text = "921.7";
            cbFp25.Text = "921.9";
            cbFp26.Text = "922.1";
            cbFp27.Text = "922.3";
            cbFp28.Text = "922.5";
            cbFp29.Text = "922.7";
            cbFp30.Text = "922.9";

            cbFp31.Text = "923.1";
            cbFp32.Text = "923.3";
        }

        private void SetReaderParam_Enter(object sender, EventArgs e)
        {
            if (isStartMuliti)
            {
                ;
            }
            else
            {
                btnReadWorkMode_Click(null, null);
                btnReadCommMode_Click(null, null);
                btnReadFreq_Click(null, null);
                btnAntRead_Click(null, null);
                btnReadWorkMode_Click(null, null);
            }

        }
        private void cbAjaDisc_CheckedChanged(object sender, EventArgs e)
        {
            tbNeighJudge.Visible = cbAjaDisc.Checked;
        }

        /// <summary>
        /// ��ǩ����
        /// ��ǩ��Ȩ
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTagAuth_Click(object sender, EventArgs e)
        {
            if (0 != Dis.TagAuther(deviceNo))
            {
                MessageBox.Show(rm.GetString("strMsgSucceedAuth"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedAuth"));
            }
        }

        /// <summary>
        /// ������������
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetBeep_Click(object sender, EventArgs e)
        {
            if (-1 == cbbBeepControl.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidBeepControl"));
                return;
            }
            if (0 != Dis.BeepCtrl(deviceNo, (byte)cbbBeepControl.SelectedIndex))
            {
                MessageBox.Show(rm.GetString("strMsgSucceedBeepControl"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedBeepControl"));
            }
        }

        /// <summary>
        /// USB�����ʽ����
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetUsbFormat_Click(object sender, EventArgs e)
        {
            if (-1 == cbbUsbFormat.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidUsbFormat"));
                return;
            }
            if (0 != Dis.SetSingleParameter(deviceNo, Dis.ADD_DOUBLE_USB_DATA, (byte)cbbUsbFormat.SelectedIndex))
            {
                MessageBox.Show(rm.GetString("strMsgSucceedSetUsbFormat"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetUsbFormat"));
            }
        }

        /// <summary>
        /// ��ǩ����
        /// �޸���Ȩ��
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyAuthPwd_Click(object sender, EventArgs e)
        {
            string strAuthPwd = tbAuthPwd.Text.Replace(" ", "");
            string strNewAuthPwd = tbNewAuthPwd.Text.Replace(" ", "");
            if (tbAuthPwd.Text.Length != 4 || tbNewAuthPwd.Text.Length != 4)
            {
                MessageBox.Show(rm.GetString("strMsgPwdMustFour"));
                return;
            }
            if (!Regex.IsMatch(tbAuthPwd.Text, "^[0-9A-Fa-f]+$") || !Regex.IsMatch(tbNewAuthPwd.Text, "^[0-9A-Fa-f]+$"))
            {
                MessageBox.Show(rm.GetString("strMsgInvalidChar"));
                return;
            }

            byte[] oldPwd = new Byte[2];
            byte[] newPwd = new Byte[2];

            for (int i = 0; i < 2; i++)
            {
                oldPwd[i] = Convert.ToByte(strAuthPwd.Substring(i * 2, 2), 16);
                newPwd[i] = Convert.ToByte(strNewAuthPwd.Substring(i * 2, 2), 16);
            }

            byte[] readPwd = new Byte[2];
            Dis.GetAutherPwd(deviceNo, readPwd);
            if (readPwd[0] == oldPwd[0] && readPwd[1] == oldPwd[1])
            {
                if (0 != Dis.SetAutherPwd(deviceNo, newPwd))
                {
                    MessageBox.Show(rm.GetString("strMsgSuccedSetAuthPwd"));
                }
                else
                {
                    MessageBox.Show(rm.GetString("strMsgFailedSetAuthPwd"));
                }
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgInvalidOldPwd"));
                return;
            }

        }

        /// <summary>
        /// �̵�������
        /// 2016-02-20
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoOpenRelay_Click(object sender, EventArgs e)
        {
            //if (rdoOpenRelay.Checked)
            //{
            //    Dis.SetRelay(0, 1);
            //}
            //int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, (byte)1);
            cbbRelayTime.Visible = true;
            lblCloseTime.Visible = true;
            rdoOpen1.Visible = false;
            rdoOpen2.Visible = false;
            rdoClose1.Visible = false;
            rdoClose2.Visible = false;
        }

        /// <summary>
        /// �̵�������
        /// 2016-02-20
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoCloseRelay_CheckedChanged(object sender, EventArgs e)
        {
            //if (rdoCloseRelay.Checked)
            //{
            //    Dis.SetRelay(0, 0);
            //}
            //int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, (byte)0);
            cbbRelayTime.Visible = false;
            lblCloseTime.Visible = false;
            rdoOpen1.Visible = true;
            rdoOpen2.Visible = true;
            rdoClose1.Visible = true;
            rdoClose2.Visible = true;
        }

        /// <summary>
        /// ��������������
        /// 2016-02-20
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetRelayTime_Click(object sender, EventArgs e)
        {
            try
            {
                byte time = 0;
                int pValue = 0;
                if (rdoOpenRelay.Checked == true) //�̵�������ģʽ-����
                {
                    if (!int.TryParse(cbbRelayTime.Text, out pValue))
                    {
                        MessageBox.Show(rm.GetString("StrMsgcbbRelayTime"));
                        return;
                    }
                    else
                    {
                        if (pValue < 0)
                        {
                            MessageBox.Show(rm.GetString("StrMsgcbbRelayTime"));
                            return;
                        }
                    }

                    int res = int.Parse(cbbRelayTime.Text);
                    time = (byte)res;
                    int result = Dis.SetSingleParameter(deviceNo, Dis.ADD_RELAY_TIME_DELAY, time);
                    result *= Dis.SetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, 1);

                    if (0 == result)
                    {
                        labResult.Text = rm.GetString("strMsgSucceedInvalidbtnSetRelayTime");
                    }
                    else
                    {
                        labResult.Text = rm.GetString("strMsgFailedInvalidbtnSetRelayTime");
                    }
                }
                else if (rdoCloseRelay.Checked == true) //�̵�������ģʽ -����
                {
                    int relay1 = -1;
                    int relay2 = -1;

                    //�̵���һ·
                    if (rdoOpen1.Checked == true)
                    {
                        relay1 = 1;
                    }
                    else if (rdoClose1.Checked == true)
                    {
                        relay1 = 0;
                    }
                    //�̵�����·
                    if (rdoOpen2.Checked == true)
                    {
                        relay2 = 3;
                    }
                    else if (rdoClose2.Checked == true)
                    {
                        relay2 = 2;
                    }
                    int result = Dis.SetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, 0);
                    result *= relay1 != -1 ? Dis.SetRelay((byte)deviceNo, (byte)relay1) : 0;
                    result *= relay2 != -1 ? Dis.SetRelay((byte)deviceNo, (byte)relay2) : 0;

                    if (0 == result)
                    {
                        labResult.Text = rm.GetString("strMsgSucceedInvalidbtnSetRelayTime");
                    }
                    else
                    {
                        labResult.Text = rm.GetString("strMsgFailedInvalidbtnSetRelayTime");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnSetRelayTime"));
            }
        }

        /// <summary>
        /// �ر�
        /// 2015-11-9
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(rm.GetString("MainWindowFormClosing"), rm.GetString("ClosePrompt"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }


            //����Excel 2015-11-9 hz
            //DialogResult result = MessageBox.Show(rm.GetString("SaveExcel"), rm.GetString("ClosePrompt"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //if (result == DialogResult.Yes)
            //{
            //    string fileName = "\\DisSave\\Dis" + DateTime.Now.ToString("yyyyMMdd")+".xlsx";
            //    int res = SaveExcel(null, Application.StartupPath + fileName);
            //    if (res == -1)
            //    {
            //        MessageBox.Show(rm.GetString("MsgSaveExcel"));
            //        return;
            //    }
            //    else
            //    {
            //        DialogResult reg = MessageBox.Show(rm.GetString("MsgOpenExcel"), rm.GetString("ClosePrompt"), MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            //        if (reg == DialogResult.Yes)
            //        {
            //            string strPath = Application.StartupPath + fileName;
            //            System.Diagnostics.Process.Start(strPath);
            //        }
            //    }
            //    e.Cancel = false;
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        /// <summary>
        /// SaveExcel
        /// hz
        /// 2015-11-9
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        private int SaveExcel(System.Data.DataTable dt, string filePath)
        {
            int reg = 0;
            try
            {
                //��Ҫ���ɵ�Excel�ļ�                
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine("<?xml version=\"1.0\"?>");
                writer.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
                //Excel��������ʼ
                writer.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                writer.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
                writer.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
                writer.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
                //writer.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40//">");
                writer.WriteLine(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
                writer.WriteLine(" <Author>Mrluo735</Author>");
                writer.WriteLine(string.Format("  <Created>{0}T{1}Z</Created>", DateTime.Now.ToString("yyyy-mm-dd"), DateTime.Now.ToString("HH:MM:SS")));
                writer.WriteLine(" <Company>Mrluo735</Company>");
                writer.WriteLine(" <Version>11.6408</Version>");
                writer.WriteLine(" </DocumentProperties>");
                writer.WriteLine(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                writer.WriteLine("  <WindowHeight>8955</WindowHeight>");
                writer.WriteLine("  <WindowWidth>11355</WindowWidth>");
                writer.WriteLine("  <WindowTopX>480</WindowTopX>");
                writer.WriteLine("  <WindowTopY>15</WindowTopY>");
                writer.WriteLine("  <ProtectStructure>False</ProtectStructure>");
                writer.WriteLine("  <ProtectWindows>False</ProtectWindows>");
                writer.WriteLine(" </ExcelWorkbook>");
                //Excel����������
                //��������ʽ
                writer.WriteLine("<Styles>");
                writer.WriteLine("<Style ss:ID=\"Default\" ss:Name=\"Normal\">");
                writer.WriteLine("  <Alignment ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
                //writer.WriteLine("  <Borders>");
                //writer.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                //writer.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                //writer.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/> ");
                //writer.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                //writer.WriteLine("  </Borders>");
                writer.WriteLine(string.Format("<Font ss:FontName=\"{0}\" x:CharSet=\"{1}\" ss:Size=\"{2}\"/>", "����", 134, 12));
                writer.WriteLine("  <Interior/>");
                writer.WriteLine("  <Protection/>");
                writer.WriteLine("</Style>");
                writer.WriteLine("  <Style ss:ID=\"s21\">");
                writer.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
                writer.WriteLine("  </Style>");
                writer.WriteLine("<Style ss:ID=\"BoldColumn\">");
                writer.WriteLine("  <Font ss:FontName=\"����\" ss:Bold=\"1\" ss:Size=\"14\"/>");
                writer.WriteLine("  <Borders>");
                writer.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("  </Borders>");
                writer.WriteLine("</Style>");
                writer.WriteLine("<Style ss:ID=\"DataColumn\">");
                writer.WriteLine("  <Font ss:FontName=\"����\" ss:WrapText=\"1\"/>");
                writer.WriteLine("  <Borders>");
                writer.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\"/>");
                writer.WriteLine("  </Borders>");
                writer.WriteLine("</Style>");
                //�ı���ʽ
                writer.WriteLine("<Style ss:ID=\"StringLiteral\">");
                writer.WriteLine("  <NumberFormat ss:Format=\"@\"/>");
                writer.WriteLine("</Style>");
                //��������ʽ
                writer.WriteLine("<Style ss:ID=\"Decimal\">");
                writer.WriteLine("  <NumberFormat ss:Format=\"0.00\"/>");
                writer.WriteLine("</Style>");
                //������ʽ
                writer.WriteLine("<Style ss:ID=\"Integer\">");
                writer.WriteLine("  <NumberFormat ss:Format=\"0\"/>");
                writer.WriteLine("</Style>");
                //������ʽ
                writer.WriteLine("<Style ss:ID=\"DateLiteral\">");
                writer.WriteLine("  <NumberFormat ss:Format=\"yyyy/mm/dd;@\"/>");
                writer.WriteLine("</Style>");
                writer.WriteLine(" </Styles>");

                int rows = dt.Rows.Count + 1;
                int cols = dt.Columns.Count;
                //��i��������
                writer.WriteLine(string.Format("<Worksheet ss:Name=\"{0}\">", dt.TableName));
                writer.WriteLine(string.Format("    <Table ss:ExpandedColumnCount=\"{0}\" ss:ExpandedRowCount=\"{1}\" x:FullColumns=\"1\"", cols.ToString(), rows.ToString()));
                writer.WriteLine("   x:FullRows=\"1\">");

                int width = 80;
                //ָ��ÿһ�еĿ��
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    if (c == 1) width = 245; else width = 80;
                    writer.WriteLine(string.Format("<Column ss:Index=\"{0}\" ss:AutoFitWidth=\"{1}\" ss:Width=\"{2}\"/> ", c + 1, 1, width));
                }

                //���ɱ���
                writer.WriteLine(string.Format("<Row ss:AutoFitHeight=\"{0}\" ss:Height=\"{1}\">", 0, 28.5));
                foreach (DataColumn eachCloumn in dt.Columns)
                {
                    //writer.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                    writer.Write("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">");
                    writer.Write(eachCloumn.ColumnName.ToString());
                    writer.WriteLine("</Data></Cell>");
                }
                writer.WriteLine("</Row>");

                //�������ݼ�¼
                foreach (DataRow eachRow in dt.Rows)
                {
                    writer.WriteLine("<Row ss:AutoFitHeight=\"0\">");
                    for (int currentRow = 0; currentRow != cols; currentRow++)
                    {
                        object[] getValue = ExcelContent(eachRow[currentRow]);

                        //writer.Write(string.Format("<Cell ss:StyleID=\"{0}\"><Data ss:Type=\"{1}\">", getValue[0], getValue[1]));
                        writer.Write("<Cell ss:StyleID=\"DataColumn\"><Data ss:Type=\"String\">");
                        writer.Write(getValue[2]);
                        writer.WriteLine("</Data></Cell>");
                    }
                    writer.WriteLine("</Row>");
                }
                writer.WriteLine("</Table>");
                writer.WriteLine("<WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                writer.WriteLine("<Selected/>");
                writer.WriteLine("<Panes>");
                writer.WriteLine("<Pane>");
                writer.WriteLine("  <Number>3</Number>");
                writer.WriteLine("  <ActiveRow>1</ActiveRow>");
                writer.WriteLine("</Pane>");
                writer.WriteLine("</Panes>");
                writer.WriteLine("<ProtectObjects>False</ProtectObjects>");
                writer.WriteLine("<ProtectScenarios>False</ProtectScenarios>");
                writer.WriteLine("</WorksheetOptions>");
                writer.WriteLine("</Worksheet>");

                writer.WriteLine("</Workbook>");
                writer.Close();
            }
            catch (Exception ex)
            {
                reg = -1;
            }
            return reg;
        }

        /// <summary>
        /// ����C#ֵ����ת����Excelֵ
        /// 2015-11-9
        /// hz
        /// </summary>
        /// <param name="Value">ֵ</param>
        /// <returns>Excel��ʽ���������ͣ��ı�</returns>
        private static object[] ExcelContent(object Value)
        {
            object[] strValue = new object[3];
            System.Type rowType = Value.GetType();
            switch (rowType.ToString())
            {
                case "System.String":
                case "System.Guid":
                    string XMLstring = Value.ToString();
                    XMLstring = XMLstring.Trim();
                    XMLstring = XMLstring.Replace("&", "&");
                    XMLstring = XMLstring.Replace(">", ">");
                    XMLstring = XMLstring.Replace("<", "<");
                    strValue[0] = "StringLiteral";
                    strValue[1] = "String";
                    strValue[2] = XMLstring;
                    break;
                case "System.DateTime":
                    DateTime XMLDate = (DateTime)Value;
                    string XMLDatetoString = ""; //Excel Converted Date
                    //������ʱ��ת��Ϊ����yyyy-MM-ddTHH:mm:ss������Excel�еĸ�ʽ
                    XMLDatetoString = XMLDate.ToString(System.Globalization.DateTimeFormatInfo.CurrentInfo.SortableDateTimePattern);
                    strValue[0] = "DateLiteral";
                    strValue[1] = "DateTime";
                    if (XMLDate < Convert.ToDateTime("1900-1-1"))
                    {
                        strValue[0] = "StringLiteral";
                        strValue[1] = "String";
                        XMLDatetoString = string.Empty;
                    }
                    strValue[2] = XMLDatetoString;
                    break;
                case "System.Boolean":
                    strValue[0] = "StringLiteral";
                    strValue[1] = "String";
                    strValue[2] = Value.ToString();
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Byte":
                    strValue[0] = "Integer";
                    strValue[1] = "Number";
                    strValue[2] = Value.ToString();
                    break;
                case "System.Byte[]":
                    strValue[0] = "StringLiteral";
                    strValue[1] = "String";
                    strValue[2] = (byte[])Value;
                    break;
                case "System.Decimal":
                case "System.Double":
                    strValue[0] = "Decimal";
                    strValue[1] = "Number";
                    strValue[2] = Value.ToString();
                    break;
                case "System.DBNull":
                    strValue[0] = "StringLiteral";
                    strValue[1] = "String";
                    strValue[2] = "";
                    break;
                default:
                    throw (new Exception(rowType.ToString() + " not handled."));
            }
            return strValue;
        }

        private void txtTemperature2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //��ֹ�ո��
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //������
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //����Ƿ��ַ�
                }
            }
        }

        /// <summary>
        /// �պ�ʱ��
        /// 2016-01-08
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbbRelayTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //��ֹ�ո��
            if ((e.KeyChar == 0x2D) && (((ComboBox)sender).Text.Length == 0)) return;   //������
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((ComboBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //����Ƿ��ַ�
                }
            }
        }

        /// <summary>
        /// �����ٶ�
        /// 2016-01-08
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadSpeed_Click(object sender, EventArgs e)
        {
            if (cboReadSpeed.SelectedIndex == -1)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidReadSpeedSet"));
                return;
            }
            int readSpeed = cboReadSpeed.Text == "" ? 1 : int.Parse(cboReadSpeed.Text);
            int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_READSPEED, (byte)readSpeed);

            if (0 != res)
            {
                MessageBox.Show(rm.GetString("strMsgSucceedSetReadSpeed"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetReadSpeed"));
            }
        }

        /// <summary>
        /// ������ȡ
        /// 2016-01-08
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadBeep_Click(object sender, EventArgs e)
        {
            int buzzer;
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_BUZZER, out buzzer);
                if (res != 0)
                {
                    cbbBeepControl.SelectedIndex = buzzer;
                }
                else
                {
                    MessageBox.Show(rm.GetString("strGetBeepFailed"));
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strGetBeepFailed"));
            }
        }

        /// <summary>
        /// �ٶȶ�ȡ
        /// 2016-01-08
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadSpeed_Click_1(object sender, EventArgs e)
        {
            int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_READSPEED, out m_ReadSpeed);
            if (0 != res)
            {
                int readspeed = 0;
                if (m_ReadSpeed == 1)
                    readspeed = 0;
                else if (m_ReadSpeed == 10)
                    readspeed = 1;
                else
                    readspeed = 0;
                cboReadSpeed.SelectedIndex = readspeed;
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedReadCommMode"));
            }
        }

        /// <summary>
        /// �̵������ƶ�ȡ
        /// 2016-02-20
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReadRelayTime_Click(object sender, EventArgs e)
        {
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, out m_Relay_C_O);
                res *= Dis.GetSingleParameter(deviceNo, Dis.ADD_RELAY_TIME_DELAY, out m_Relay_Time);
                if (0 == res)
                {
                    if (m_Relay_C_O == 1)
                    {
                        rdoOpenRelay.Checked = true;
                        cbbRelayTime.Visible = true;
                        lblCloseTime.Visible = true;
                        rdoOpen1.Visible = false;
                        rdoOpen2.Visible = false;
                        rdoClose1.Visible = false;
                        rdoClose2.Visible = false;
                        cbbRelayTime.Text = m_Relay_Time.ToString();
                        labResult.Text = rm.GetString("strMsgSucceedInvalidbtnReadRelayTime");
                    }
                    else if (m_Relay_C_O == 0)
                    {
                        rdoCloseRelay.Checked = true;
                        cbbRelayTime.Visible = false;
                        lblCloseTime.Visible = false;
                        rdoOpen1.Visible = true;
                        rdoOpen2.Visible = true;
                        rdoClose1.Visible = true;
                        rdoClose2.Visible = true;
                        labResult.Text = rm.GetString("strMsgSucceedInvalidbtnReadRelayTime");
                    }
                }
                else
                {
                    labResult.Text = rm.GetString("strMsgFailedInvalidbtnReadRelayTime");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnReadRelayTime"));
            }
        }

        /// <summary>
        /// ����ѡȡ
        /// 2016-04-25
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboBand_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBand.SelectedIndex == 0)
            {
                pnlUSBand.Visible = true;
                pnlEUBand.Visible = false;
                cbbFreqMode_SelectedIndexChanged(null, null);
            }
            else if (cboBand.SelectedIndex == 1)
            {
                pnlEUBand.Visible = true;
                pnlUSBand.Visible = false;
                cboFreqModeEU_SelectedIndexChanged(null, null);
            }
            else if (cboBand.SelectedIndex == 2)
            {
                pnlUSBand.Visible = true;
                pnlEUBand.Visible = false;
                cbbFreqMode_SelectedIndexChanged(null, null);
            }

        }

        /// <summary>
        /// ŷ��Ƶ������
        /// 2016-4-25
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboFreqModeEU_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cboFreqModeEU.SelectedIndex)
            {
                case -1:
                case 0: // ��Ƶ
                    {
                        cboFreqPointEU.Visible = false;
                        ShowFreqPointEU(true);
                    }
                    break;
                case 1:// ��Ƶ
                    {
                        cboFreqPointEU.Visible = true;
                        ShowFreqPointEU(false);
                    }
                    break;
            }
        }
        /// <summary>
        /// ���߶�ȡ
        /// 2016-06-07
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAntRead_Click(object sender, EventArgs e)
        {
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_ANT_MODE, out ant);
                int res1 = Dis.GetSingleParameter(deviceNo, Dis.ADD_ANT_SET, out p_ant);

                a_fp[0] = p_ant & Dis.ANT_BIT_0;
                a_fp[1] = p_ant & Dis.ANT_BIT_1;
                a_fp[2] = p_ant & Dis.ANT_BIT_2;
                a_fp[3] = p_ant & Dis.ANT_BIT_3;
                a_fp[4] = p_ant & Dis.ANT_BIT_4;
                a_fp[5] = p_ant & Dis.ANT_BIT_5;
                a_fp[6] = p_ant & Dis.ANT_BIT_6;
                a_fp[7] = p_ant & Dis.ANT_BIT_7;

                chkAnt1.Checked = a_fp[0] == 0 ? false : true;
                chkAnt2.Checked = a_fp[1] == 0 ? false : true;
                chkAnt3.Checked = a_fp[2] == 0 ? false : true;
                chkAnt4.Checked = a_fp[3] == 0 ? false : true;
                chkAnt5.Checked = a_fp[4] == 0 ? false : true;
                chkAnt6.Checked = a_fp[5] == 0 ? false : true;
                chkAnt7.Checked = a_fp[6] == 0 ? false : true;
                chkAnt8.Checked = a_fp[7] == 0 ? false : true;
                if (res == 0 && res1 == 0)
                    MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnAntRead"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnAntRead"));
            }
        }

        /// <summary>
        /// ��������
        /// 2016-06-07
        /// hz
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAntSet_Click(object sender, EventArgs e)
        {
            try
            {
                // �����������
                a_fp[0] = chkAnt1.Checked ? 1 : 0;
                a_fp[1] = chkAnt2.Checked ? 1 : 0;
                a_fp[2] = chkAnt3.Checked ? 1 : 0;
                a_fp[3] = chkAnt4.Checked ? 1 : 0;
                a_fp[4] = chkAnt5.Checked ? 1 : 0;
                a_fp[5] = chkAnt6.Checked ? 1 : 0;
                a_fp[6] = chkAnt7.Checked ? 1 : 0;
                a_fp[7] = chkAnt8.Checked ? 1 : 0;

                int num = 0;
                for (int i = 0; i < a_fp.Length; i++)
                {
                    if (a_fp[i] == 1)
                        num += 1;
                }

                int p_ant = 0;
                //if (num < 2 && a_fp[0]==1)//�ж����߿�������
                //    ant = 1;
                //else
                //    ant = 4;
                if (num < 2)//�ж����߿�������
                    ant = 1;
                else
                    ant = num;
                int res = Dis.SetSingleParameter(deviceNo, Dis.ADD_ANT_MODE, (byte)ant);


                p_ant += (byte)a_fp[0];
                p_ant += (byte)(a_fp[1] << 1);
                p_ant += (byte)(a_fp[2] << 2);
                p_ant += (byte)(a_fp[3] << 3);
                p_ant += (byte)(a_fp[4] << 4);
                p_ant += (byte)(a_fp[5] << 5);
                p_ant += (byte)(a_fp[6] << 6);
                p_ant += (byte)(a_fp[7] << 7);

                int setRes = Dis.SetSingleParameter(deviceNo, Dis.ADD_ANT_SET, (byte)p_ant);

                if (res == 0 && setRes == 0)
                    MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnAntSet"));
                else
                    MessageBox.Show(rm.GetString("strMsgbtnAntSetSuccess"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(rm.GetString("strMsgFailedInvalidbtnAntSet"));
            }
        }


        /// <summary>
        /// ������ȡ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_connRead_Click(object sender, EventArgs e)
        {
            int RWBank = -1;
            int StartAdd = -1;
            int Length = -1;
            labResult.Text = "";
            if ((RWBank = cbbRWBank.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgSelectRWBank"));
                return;
            }
            if ((StartAdd = cbbStartAdd.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgSelectStartAdd"));
                return;
            }
            StartAdd = int.Parse(cbbStartAdd.Text);
            if ((Length = cbbLength.SelectedIndex) == -1)
            {
                MessageBox.Show(rm.GetString("strMsgSelectLength"));
                return;
            }
            Length = int.Parse(cbbLength.Text);
            byte[] byteArray = new byte[64];
            timerConnRead.Enabled = true;   //����������ȡ��ʱ��
            timerConnRead.Interval = 1500;

            #region ���ض����������д����ť
            btnReadData.Enabled = false;
            btnClearData.Enabled = false;
            btnWriteData.Enabled = false;
            btn_connRead.Enabled = false;
            #endregion

            //tbRWData.Text = "";
            if (1 == Dis.ReadTagData(deviceNo, (byte)RWBank, (byte)StartAdd, (byte)Length, byteArray))
            {
                string tags = string.Empty;
                for (int i = 3; i < 2 * Length + 3; i++) // ǰ�棳���ֽ�Ϊ����Ĳ���
                {
                    tags += string.Format("{0:X2} ", byteArray[i]);
                }
                if (tbRWData.Text == "")
                {
                    tbRWData.Text = counts + "." + " " + tags;
                    counts = counts + 1;
                    //tbRWData.Text = tags;
                }
                else
                {
                    //tbRWData.Text =tbRWData.Text +"\r"+"\n"+ tags;
                    tbRWData.Text = tbRWData.Text + "\r" + "\n" + counts + "." + " " + tags;
                    counts = counts + 1;
                }
                tbRWData.Focus();
                this.tbRWData.Select(this.tbRWData.TextLength, 0);//��궨λ���ı����
                this.tbRWData.ScrollToCaret();//��������괦
                labResult.Text = rm.GetString("strMsgSussecReadData");
            }
            else
            {
                labResult.Text = rm.GetString("strMsgFailedReadData");
            }
        }


        /// <summary>
        /// ��/д����
        /// ����������ʱ��
        /// 2017.2.23
        /// ZW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerConnRead_Tick(object sender, EventArgs e)
        {
            btn_connRead_Click(sender, e);
        }


        /// <summary>
        /// ��/д����
        /// ֹͣ����������ʱ��
        /// 2017.2.23
        /// ZW
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_stoptimer_Click(object sender, EventArgs e)
        {
            timerConnRead.Enabled = false;   //ֹͣ������ȡ��ʱ��

            #region ��ʾ�����������д����ť
            btnReadData.Enabled = true;
            btnClearData.Enabled = true;
            btnWriteData.Enabled = true;
            btn_connRead.Enabled = true;
            #endregion
        }


        /// <summary>
        /// TabContrl SelectIndex
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvZl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvZl.Items.Count > 0)
            {
                ZLDM141.m_SelectedDevNo = (byte)(lvZl.Items.IndexOf(lvZl.FocusedItem));
                UpdateCommParamControl();
            }
        }

        Thread autoAuthorizationThread;
        private void button1_Click(object sender, EventArgs e)
        {
            if (AutoAuthorizationBt.Text == rm.GetString("startAutoAuthorization"))
            {
                autoAuthorizationThread = new Thread(new ThreadStart(AuthorizationMethod));
                autoAuthorizationThread.Start();
                AutoAuthorizationBt.Text = rm.GetString("stopAutoAuthorization");
            }
            else if (AutoAuthorizationBt.Text == rm.GetString("stopAutoAuthorization"))
            {
                autoAuthorizationThread.Abort();
                AutoAuthorizationBt.Text = rm.GetString("startAutoAuthorization");
            }

        }


        string AuthorizationSuccess;
        string NoneAuthorizationTag;

        private void AuthorizationMethod()
        {
            while (autoAuthorizationThread.IsAlive)
            {
                if (0 != Dis.TagAuther(deviceNo))
                {
                    authorizationLb.Text = "��Ȩ�ɹ�";
                    authorizationLb.ForeColor = Color.Green;
                }
                else
                {
                    authorizationLb.Text = "������Ȩ��ǩ";
                    authorizationLb.ForeColor = Color.Black;
                }
                Thread.Sleep(500);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (-1 == MultiInvCB.SelectedIndex)
            {
                MessageBox.Show(rm.GetString("strMsgInvalidBeepControl"));
                return;
            }
            if (0 != Dis.SetSingleParameter(deviceNo, Dis.ADDR_EPC_TID_FLAG, (byte)MultiInvCB.SelectedIndex))
            {
                MessageBox.Show(rm.GetString("strMsgSucceedBeepControl"));
            }
            else
            {
                MessageBox.Show(rm.GetString("strMsgFailedSetUsbFormat"));
            }
        }

        BathReadWrite bathReadWrite;
        private static bool isStartMuliti = false;
        private void button2_Click(object sender, EventArgs e)
        {
            bathReadWrite = new BathReadWrite(deviceNo, rm);
            bathReadWrite.Show();
        }

        private void cbSaveFile_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.D0)
            {
                rbAsc.Visible = true;
                rbDesc.Visible = true;
                cbSaveFile.Visible = true;
                button5.Visible = true;
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.D1)
            {
                rbAsc.Visible = false;
                rbDesc.Visible = false;
                cbSaveFile.Visible = false;
                button5.Visible = false;
            }

        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (Tag_data)
            {
                /* foreach (EPC_data epc in Tag_data)
                 {
                     WriteFileSeparator(epc.epc.Replace(" ","")+",","epc.txt");
                 }*/
                SaveTxt();
            }
        }

        private void SaveTxt()
        {
            string fileName = "";
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "txt";
            saveDialog.Filter = "�ı��ļ�(*.txt)|*.txt";
            saveDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //������ȡ��
            foreach (EPC_data epc in Tag_data)
            {
                WriteFileSeparator(epc.epc.Replace(" ", "") + ",", saveFileName);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string str = wiegandTb.Text.ToString();
            str = str.PadLeft(8, '0');
            if (radioButton1.Checked)            //
            {
                tbRWData.Text = PageFun.weigandStrTostr1(str);
            }
            else if (radioButton2.Checked)         //
            {
                tbRWData.Text = PageFun.weigandStrTostr2(str);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void gbCommMode_Enter(object sender, EventArgs e)
        {

        }

        private void btnReadRelayTime_Click_1(object sender, EventArgs e)
        {
            int time;
            int state;
            int result = Dis.GetSingleParameter(deviceNo, Dis.ADD_RELAY_AUTOMATIC_CLOSE, out state);
            if (result != 0)
            {
                if (state == 0)          //����
                {
                    ;
                }
                else if (state == 1)          //����
                {
                    result *= Dis.GetSingleParameter(deviceNo, Dis.ADD_RELAY_TIME_DELAY, out time);
                    cbbRelayTime.Text = time.ToString();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*tbRWData.Text = "";
            counts = 1;
            tbRWData.Focus();
            byte[] HexData = new byte[4];
            byte[] writeData = new byte[12];
            int len = textBox1.Text.Length;
            string hexString = textBox1.Text;
            for (int i = 0; i < 4; ++i)
            {
                HexData[i] = Convert.ToByte(hexString.Substring(2 * i, 2), 16);
            }
            HexData.CopyTo(writeData, 8);
            for (int i = 0; i < 12; i++) // ǰ�棳���ֽ�Ϊ����Ĳ���
            {
                tbRWData.Text += string.Format("{0:X2} ", writeData[i]);
            }*/
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            state = 1;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            state = 2;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            int flag;
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADDR_EPC_TID_FLAG, out flag);
                if (res != 0)
                {
                    MultiInvCB.SelectedIndex = flag;
                }
                else
                {
                    MessageBox.Show(rm.GetString("strGetQuencyDataFormatFailed"));
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strGetQuencyDataFormatFailed"));
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            int flag;
            try
            {
                int res = Dis.GetSingleParameter(deviceNo, Dis.ADD_DOUBLE_USB_DATA, out flag);
                if (res != 0)
                {
                    cbbUsbFormat.SelectedIndex = flag;
                }
                else
                {
                    MessageBox.Show(rm.GetString("strGetDoubleUSBFailed"));
                }
            }
            catch
            {
                MessageBox.Show(rm.GetString("strGetDoubleUSBFailed"));
            }

        }



        private void tabControl_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ;
        }

        private void tabControl_Click(object sender, EventArgs e)
        {
            if (isStartMuliti)
            {
                /* if (tabControl.SelectedTab.Name != "TagAccess" || tabControl.SelectedTab.Name != "SetCommParam" || tabControl.SelectedTab.Name != "SetReaderParam" || tabControl.SelectedTab.Name != "OtherOpreation")
                 {
                     tabControl.SelectedIndex = 0;
                     DialogResult dr = MessageBox.Show(rm.GetString("strReadStopTips"), rm.GetString("Tips"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                     if (dr == DialogResult.OK)
                     {
                         tabControl.SelectTab(General);
                     }
                 }*/
                tabControl.SelectedIndex = 0;
                DialogResult dr = MessageBox.Show(rm.GetString("strReadStopTips"), rm.GetString("Tips"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] readPwd = new Byte[2];
            Dis.GetAutherPwd(deviceNo, readPwd);
            tbAuthPwd.Text = readPwd[0].ToString("X2") + readPwd[1].ToString("X2");
        }


    }

    public struct WSADATA
    {
        public short wVersion;
        public short wHighVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string szDescription;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 6)]
        public string szSystemStatus;
        [Obsolete] // Ignored for v2.0 and above 
        public int wMaxSockets;
        [Obsolete] // Ignored for v2.0 and above 
        public int wMAXUDPDG;
        public IntPtr dwVendorInfo;
    }

    public sealed class EPC_data : IComparable
    {
        public string epc;
        public int count;
        public int devNo;
        public byte antNo;
        int IComparable.CompareTo(object obj)
        {
            EPC_data temp = (EPC_data)obj;
            {
                return string.Compare(this.epc, temp.epc);
            }
        }
    }

    // ����������ݱ����˸������
    class ListViewNF : System.Windows.Forms.ListView
    {
        public ListViewNF()
        {
            // ����˫����
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            // Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }
    }
}
