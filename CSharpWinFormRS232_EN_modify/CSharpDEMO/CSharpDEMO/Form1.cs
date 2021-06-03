using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharpDEMO;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using System.IO;
namespace CSharpDEMO
{
    public partial class Form1 : Form
    {
        Thread thread1;
        public int comHandler = int.MinValue;
        public int DeviceAddress = 0;       
        public IcodeSliTAG CurTag = null;
        public bool Running = false;
        public string strResult="";
        private string strStatus = "";
        public bool checkComm()
        {
            if (comHandler == int.MinValue)
            {
                lblStatus.Text = "Com is not open";
                //MessageBox.Show("Com is not open", "Error");
                return false;
            }
            return true;
        }

        public void setNULL()
        {
            comHandler = int.MinValue;
        }

        //显示命令执行结果
        private void showStatue(int Code)
        {
            string msg = "";
            switch (Code)
            {
                case 0x00:
                    msg = "Command succeed.....";
                    break;
                case 0x01:
                    msg = "Command failed.....";
                    break;
                case 0x02:
                    msg = "Checksum error.....";
                    break;
                case 0x03:
                    msg = "Not selected COM port.....";
                    break;
                case 0x04:
                    msg = "Reply time out.....";
                    break;
                case 0x05:
                    msg = "Check sequence error.....";
                    break;
                case 0x07:
                    msg = "Check sum error.....";
                    break;
                case 0x0A:
                    msg = "The parameter value out of range.....";
                    break;
                case 0x80:
                    msg = "Command OK.....";
                    break;
                case 0x81:
                    msg = "Command FAILURE.....";
                    break;
                case 0x82:
                    msg = "Reader reply time out error.....";
                    break;
                case 0x83:
                    msg = "The card does not exist.....";
                    break;
                case 0x84:
                    msg = "The data is error.....";
                    break;
                case 0x85:
                    msg = "Reader received unknown command.....";
                    break;
                case 0x87:
                    msg = "Error.....";
                    break;
                case 0x89:
                    msg = "The parameter of the command or the format of the command error.....";
                    break;
                case 0x8A:
                    msg = "Some error appear in the card InitVal process.....";
                    break;
                case 0x8B:
                    msg = "Get the wrong snr during anticollison loop.....";
                    break;
                case 0x8C:
                    msg = "The authentication failure.....";
                    break;
                case 0x8F:
                    msg = "Reader received unknown command.....";
                    break;
                case 0x90:
                    msg = "The card do not support this command.....";
                    break;
                case 0x91:
                    msg = "The foarmat of the command error.....";
                    break;
                case 0x92:
                    msg = "Do not support option mode.....";
                    break;
                case 0x93:
                    msg = "The block do not exist.....";
                    break;
                case 0x94:
                    msg = "The object have been locked.....";
                    break;
                case 0x95:
                    msg = "The lock operation do not success.....";
                    break;
                case 0x96:
                    msg = "The operation do not success.....";
                    break;
            }
            strStatus=msg + "\r\n";
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void btn_UL_Halt_Click(object sender, EventArgs e)
        {
            if (!checkComm())
                return;
            int nRet = Reader.MF_Halt(comHandler, DeviceAddress);

            //textResponse.Text += "命令执行成功。\r\n";
            showStatue(nRet);
        }



        //------------------------------------------------------------------------------------------------------------


        private void btn_sys_API_OpenComm_Click(object sender, EventArgs e)
        {
            API_OpenCom();
        }
        private void btn_sys_API_CloseComm_Click(object sender, EventArgs e)
        {
            API_CloseCom();
        }
        private void API_OpenCom()
        {          
            byte[] com = new byte[10];
            int br = Convert.ToInt32(Baudrate.Text);
            UTF8Encoding u = new UTF8Encoding();
            com = u.GetBytes(comCOM.Text);
            comHandler = Reader.API_OpenComm(com, br);
            if (comHandler == 0)
            {
                //MessageBox.Show("Failed to open the Com", "Error");
                lblStatus.Text = "Failed to open the Com";
                //btn_sys_API_OpenComm.Enabled = true;
                //btn_sys_API_CloseComm.Enabled = false;
                setNULL();
            }
            else
            {
                //btn_sys_API_OpenComm.Enabled = false;
                //btn_sys_API_CloseComm.Enabled = true;
                chkReader.Checked = true;
            }
        }
        private void API_CloseCom()
        {
            chkReader.Checked = false;
            chkReader_CheckedChanged(this, new EventArgs());
            int result = Reader.API_CloseComm(comHandler);
            if (result == 0)
            {
                //btn_sys_API_OpenComm.Enabled = true;
                //btn_sys_API_CloseComm.Enabled = false;
                setNULL();
            }
            else
            {
                //MessageBox.Show("Failed to close the Com", "Error");
                lblStatus.Text = "Failed to close the Com";
                //btn_sys_API_OpenComm.Enabled = false;
                //btn_sys_API_CloseComm.Enabled = true;
            }
        }      
        private void btnThread1_Click(object sender, EventArgs e)
        {
            Running = true;
            thread1 = new Thread(polling);
            thread1.Start();
        }
        private void polling()
        {
            while (Running)
            {
                FlushTag();
                if (txtResult.Text != strResult && Running)
                {
                    lblStatus.set_text(strStatus);
                    txtResult.set_text(strResult);//&&Running to avoid thread.Join waiting forever
                }
            }

        }
        private void FlushTag()
        {
            try
            {
                IcodeSliTAG[] tags;
                tags = Inventory();
                Thread.Sleep(50);
                if (tags != null)
                {
                    if (tags.Count() == 1)
                    {
                        if (CurTag != null)//前回に読み取ったTagの情報
                        {
                            for (int i = 0; i < 8; i++)
                            {//UIDが違う時もイベントが起きるようにする
                                if (CurTag.UID[i] != tags[0].UID[i])
                                {
                                    CurTag = null;
                                    break;//UID is different
                                }
                            }                            
                        }
                        if (CurTag == null)
                        {
                            CurTag = tags[0];                           
                        }
                        strResult = "";
                        //strResult += "DFSID: " + CurTag.DSFID.ToString() + "\r\n";
                        strResult += toHexString("タッグ: ", CurTag.UID, 0, CurTag.UID.Length);
                        //データ更新
                        CurTag.data = tags[0].data;
                        strResult += "データ: " + Reader.ReadAsString(CurTag.data, 0, txtDataType.Text.Length);// toHexString("Data: ", CurTag.data, 0, CurTag.data.Length);
                    }
                    else//複数見つかってしまった場合　検出不能とする
                    {
                        strResult = "タッグ: >=" + tags.Length.ToString() + "個";
                        if (CurTag != null) CurTag = null;
                    }
                }
                else
                {
                    strResult = "タッグ: 無し";
                }
            }
            catch
            {

            }
            
        }
        private IcodeSliTAG[] Inventory()
        {
            List<IcodeSliTAG> Tags = new List<IcodeSliTAG>();
            //ポットの情報を確認
            if (!checkComm())
                return null;
            byte[] Cardnumber = new byte[1];
            byte[] pBuffer = new byte[256];
            
            /* the getable
            card number is relate to the output rate of the module antenna, commonly can read 2~6 card
            within anticollision */
            int nRet = Reader.ISO15693_Inventory(comHandler, DeviceAddress, Cardnumber, pBuffer);

            //showStatue(nRet);
            if (nRet != 0)
            {
                
                return null;
            }
            else
            {
                //タッグを見つけた
                for (int i = 0; i < Convert.ToInt32(Cardnumber[0]); i++)
                {
                    IcodeSliTAG Tag = new IcodeSliTAG();
                    byte[] uid_buffer = new byte[8];
                    //[0]->flag; [1]->DSFID; [2~]->UID
                    Tag.DSFID = pBuffer[i * 10 + 1];//[1]->DSFID
                    Array.Copy(pBuffer, i * 10 + 2, uid_buffer, 0, 8);//[2~]->UID
                    Tag.UID = uid_buffer;                  
                    Tags.Add(Tag);                   
                }            
                for(int i = 0; i < Tags.Count; i++)
                {
                    //各タッグのデータを取得
                    if (!checkComm())
                        return null;
                    byte flags = 0x22;//0x22 read with uid mode
                    byte blk_add =Convert.ToByte( txt_blk_add.Text);//[0]->flag; [1~2]->uid; [3~?]: data
                    byte num_blk = Convert.ToByte(Math.Ceiling(txtDataType.Text.Length/4.0));//number of blocks will be read
                    byte[] uid = Tags[i].UID;                  
                    byte[] buffer = new byte[256];
                    int n;
                    if (flags == 0x42)
                        n = 5;
                    else
                        n = 4;
                    //タッグのデータを取得
                    nRet = Reader.API_ISO15693Read(comHandler, DeviceAddress, flags, blk_add, num_blk, uid, buffer);
                    //showStatue(nRet);
                    if (nRet != 0)
                    {
                        //showStatue(buffer[0]);
                    }
                    else
                    {                   
                        Tags[i].data = new byte[n*num_blk];
                        Array.Copy(buffer, 1, Tags[i].data, 0, n * num_blk);                     
                    }
                }
                return Tags.ToArray();
            }
        }
        private string toHexString(string text, byte[] data, int s, int e)
        {
            string result = "";
            //非负转换
            for (int i = 0; i < e; i++)
            {
                if (data[s + i] < 0)
                    data[s + i] = Convert.ToByte(Convert.ToInt32(data[s + i]) + 256);
            }
            result += text;
            for (int i = 0; i < e; i++)
            {
                result += data[s + i].ToString("X2") + " ";
            }
            result += "\r\n";
            return result;
        }
        private void label88_Click(object sender, EventArgs e)
        {

        }

        private void LenOfTrans_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkReader_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReader.Checked == true)
            {
                if (!checkComm())
                {
                    chkReader.Checked = false;
                    return;
                }                
                Running = true;
                thread1 = new Thread(polling);
                thread1.Start();
            }
            else
            {
                if (thread1 != null && thread1.IsAlive == true)
                {
                    //thread1.Abort();
                    Running = false;
                    thread1.Join();
                }
                txtResult.set_text("");
            }
        }
        private static ManualResetEvent mre = new ManualResetEvent(false);
        /// <summary>
        /// split text to certain sizes; for example: ChunksUpto("123456",4) will return string[] {"1234", "56"}; 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxChunkSize"></param>
        /// <returns></returns>
        static IEnumerable<string> ChunksUpto(string str, int maxChunkSize)
        {
            for (int i = 0; i < str.Length; i += maxChunkSize)
                yield return str.Substring(i, Math.Min(maxChunkSize, str.Length - i));
        }
        private void btn_Change_Click(object sender, EventArgs e)
        {
            if (CurTag == null) return;
            if (thread1 != null && thread1.IsAlive)
            {
                Running = false;
                thread1.Join();
            }
            if (!checkComm())
                return;
            byte flags = 0x02;//write without uid
            byte blk_add = Convert.ToByte(txt_blk_add.Text);//
            byte num_blk =(byte)(Math.Ceiling(txtDataType.Text.Length/4.0));//1 block contains 4 bytes, 1 char equal 1 byte(ASCII encoding)
            byte[] uid = new byte[8];
            byte[] data = new byte[256];          
            string[] str = ChunksUpto(txtWriteData.Text, 4).ToArray();//split text to array of certain size of 4 elements
            for(int i=0; i < str.Length ; i++)
            {
                Reader.WriteString2data(data, str[i], i*4);
            }
            int nRet = Reader.API_ISO15693Write(comHandler, DeviceAddress, flags, blk_add, num_blk, uid, data);
            showStatue(nRet);
            if (nRet != 0)
            {
                showStatue(data[0]);
            }
            Running = true;
            thread1 = new Thread(polling);
            thread1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter file = File.CreateText(@"settings.st"))
            {
                file.Write(comCOM.Text + ",");
                file.Write(Baudrate.Text);
            }
            if (thread1 != null && thread1.IsAlive)
            {
                Running = false;
                if(!thread1.Join(500))thread1.Abort();


            }          
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] PortList = System.IO.Ports.SerialPort.GetPortNames();
            comCOM.Items.Clear();
            foreach (string PortName in PortList)
            {
                comCOM.Items.Add(PortName);
            }
            try
            {
                using (StreamReader file = File.OpenText(@"settings.st"))
                {
                    string str = file.ReadLine();
                    string[] a = str.Split(',');
                    if (a.Length == 2)
                    {
                        comCOM.Text = a[0];
                        Baudrate.Text = a[1];
                    }
                }
            }
            catch
            {
            }
            if (checkComm()) API_CloseCom();
            API_OpenCom();
        }

        private void comCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thread1 != null && thread1.IsAlive)
            {
                Running = false;
                thread1.Join();
            }
            if(checkComm()) API_CloseCom();
            API_OpenCom();
        }

        private void Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (thread1 != null && thread1.IsAlive)
            {
                Running = false;
                thread1.Join();
            }
            if (checkComm()) API_CloseCom();
            API_OpenCom();
        }
    }
    
}

