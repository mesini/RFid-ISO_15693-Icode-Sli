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
using System.IO;
using System.Diagnostics;
namespace CSharpDEMO
{
    public partial class Form1 : Form
    {
        Thread thread1=null;       
        public int comHandler = int.MinValue;
        public int DeviceAddress = 0;       
        public IcodeSliTAG CurTag = null;
        public volatile int Running = 0;
        public bool enable = false;
        public string strResult="";
        private string strStatus = "";
        static object syncObject = new object();
        public int reset_flag;
        public int retry;
        public bool checkComm()
        {
            if (comHandler == int.MinValue)
            {
                //lblStatus.Text = "Com is not open";
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
            strStatus=msg;
        }

        public Form1()
        {
            InitializeComponent();
            
        }



        //------------------------------------------------------------------------------------------------------------


        private int API_OpenCom()
        {
            try
            {
                byte[] com = new byte[10];
                int br = Convert.ToInt32(Baudrate.Text);
                UTF8Encoding u = new UTF8Encoding();
                com = u.GetBytes(comCOM.Text);
                comHandler = Reader.API_OpenComm(com, br);
                if (comHandler == 0)
                {

                    lblStatus.Text = "Failed to open the Com";

                    setNULL();
                    return -1;
                }
                else
                {
                    API_ControlLED();
                    byte[] VersionNum = new byte[256];
                    if (GetVersionNum(VersionNum)) return 1;
                    else return 2;
                }
            }
            catch
            {
                return -1;
            }
            
        }
        private bool API_CloseCom()
        {
            try
            {
                chkReader.Checked = false;
                int result = Reader.API_CloseComm(comHandler);
                if (result == 0)
                {
                    setNULL();
                    return true;
                }
                else
                {
                    //lblStatus.Text = "Failed to close the Com";    
                    return false;
                }
            }
            catch
            {
                return false;
            }
            
        }
        private void API_ControlLED(int ledcycle=10, int ledtimes=3)
        {
            if (!checkComm())
                return;
            int freq = ledcycle;// Convert.ToInt32(ledcycle.Text);
            int duration = ledtimes;// Convert.ToInt32(ledtimes.Text);
            byte[] buffer = new byte[1];
            int result = Reader.API_ControlLED(comHandler, DeviceAddress, freq, duration, buffer);
        }
        private bool GetVersionNum(byte[] VersionNum)
        {
            if (!checkComm())
                return false;            
            int result = Reader.GetVersionNum(comHandler, DeviceAddress, VersionNum);
            if (result == 0) return true;
            else return false;
        }
        private void polling()
        {
            while (true)
            {
                try
                {
                    switch (Running)
                    {
                        case 1://running
                            if (lblResult.Text != strResult) lblResult.set_text(strResult);//
                            if (lblStatus.Text != strStatus) lblStatus.set_text(strStatus);

                            lock (syncObject)
                            {
                                FlushTag();
                            }
                            break;
                        case 2://pause
                            strResult = "リーダー無効です";
                            if (enable == false) strStatus = "接続異常です";
                            else strStatus = "";
                            if (lblResult.Text != strResult) lblResult.set_text(strResult);//
                            if (lblStatus.Text != strStatus) lblStatus.set_text(strStatus);                            
                            break;
                        case 3://abort
                            return;
                        default:
                            break;
                    }
                    Thread.Sleep(1000);
                }
                catch
                {

                }
               
            }            
        }
        private void FlushTag()
        {
            try
            {
                //Thread.Sleep(1000);
                IcodeSliTAG[] tags;
                tags = Inventory();                
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
            try
            {
                /* the getable
           card number is relate to the output rate of the module antenna, commonly can read 2~6 card
           within anticollision */
                int nRet = Reader.ISO15693_Inventory(comHandler, DeviceAddress, Cardnumber, pBuffer);

                //showStatue(nRet);
                if (nRet != 0)
                {
                    if (pBuffer[0] != 0) showStatue(pBuffer[0]);
                    return null;
                }
                else
                {
                    strStatus = "";
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
                    for (int i = 0; i < Tags.Count; i++)
                    {
                        //各タッグのデータを取得
                        if (!checkComm())
                            return null;
                        byte flags = 0x22;//0x22 read with uid mode
                        byte blk_add = Convert.ToByte(txt_blk_add.Text);//[0]->flag; [1~2]->uid; [3~?]: data
                        byte num_blk = Convert.ToByte(Math.Ceiling(txtDataType.Text.Length / 4.0));//number of blocks will be read
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
                            if (buffer[0] != 0) showStatue(buffer[0]);
                        }
                        else
                        {
                            strStatus = "";
                            Tags[i].data = new byte[n * num_blk];
                            Array.Copy(buffer, 1, Tags[i].data, 0, n * num_blk);
                        }
                    }
                    return Tags.ToArray();
                }
            }
            catch
            {
                return null;
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

        private void chkReader_CheckedChanged(object sender, EventArgs e)
        {
            if (chkReader.Checked == true)
            {
                if(enable == true) Running = 1;
                else
                {
                    chkReader.Checked=false;
                }
            }
            if (chkReader.Checked == false)
            {
                Running = 2;
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
            try
            {
                if (CurTag == null) return;               
                if (!checkComm())
                    return;
                //pause();
                Running = 2;
                lock (syncObject)
                {
                    byte flags = 0x02;//write without uid
                    byte blk_add = Convert.ToByte(txt_blk_add.Text);//
                    byte num_blk = (byte)(Math.Ceiling(txtDataType.Text.Length / 4.0));//1 block contains 4 bytes, 1 char equal 1 byte(ASCII encoding)
                    byte[] uid = new byte[8];
                    byte[] data = new byte[256];
                    string[] str = ChunksUpto(txtWriteData.Text, 4).ToArray();//split text to array of certain size of 4 elements
                    for (int i = 0; i < str.Length; i++)
                    {
                        Reader.WriteString2data(data, str[i], i * 4);
                    }
                    int nRet = Reader.API_ISO15693Write(comHandler, DeviceAddress, flags, blk_add, num_blk, uid, data);
                    showStatue(nRet);
                    if (nRet != 0)
                    {
                        showStatue(data[0]);
                    }
                }
                //resume();
                Running = 1;
            }
            catch
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Running = 3;
            thread1.Join();
            if (checkComm())
            {
                if (!API_CloseCom()) MessageBox.Show("ポートを閉じることができませんでした。。");
            }           
            try
            {
                using (StreamWriter file = File.CreateText(@"settings.st"))
                {
                    file.Write(comCOM.Text + ",");
                    file.Write(Baudrate.Text + ",");
                    file.Write(txtDataType.Text + ",");
                    file.Write(txt_blk_add.Text);
                }
            }
            catch
            {

            }
            //thread1.Abort();          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Running = 2;
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
                    if (a.Length == 4)
                    {
                        comCOM.Text = a[0];
                        Baudrate.Text = a[1];
                        txtDataType.Text = a[2];
                        txt_blk_add.Text = a[3];
                        btnConnect_Click(this.btnConnect, new EventArgs());
                    }                   
                }
            }
            catch
            {
                MessageBox.Show("ポットを選択してください。");
            }
            thread1 = new Thread(polling);
            thread1.Name = "AAAAAAAAA";
            thread1.Start();
        }

        private void comCOM_SelectedIndexChanged(object sender, EventArgs e)
        {
                   
        }

        private void Baudrate_SelectedIndexChanged(object sender, EventArgs e)
        {
                  
        }

        private void txtDataType_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            //pause();
            enable = false;
            Running = 2;
            if (checkComm())
            {
               if(!API_CloseCom())
                {
                    MessageBox.Show("ポートが異常です。");
                    return;
                }
            }
            int r = API_OpenCom();
            if (r==1)
            {
                enable = true;
                chkReader.Checked = true;
            }
            else if(r == -1)
            {
                MessageBox.Show("ポートが異常です。");
                return;
            }
            else
            {//リーダーが反応されてない、リセットを行う
               if( MessageBox.Show("リーダーをリセットしますか？", "接続異常です", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bool cancel = false;
                    int maxcount = 600;
                    int delay_ms = 100;
                    retry = 0;
                    reset_flag = -1;
                    Thread thread3 = new Thread(() => reset_flag = resetReader(ref cancel, ref retry, maxcount, delay_ms));
                    thread3.Start();
                    ResetForm form = new ResetForm(this, maxcount);
                    form.ShowDialog();
                    if (thread3.IsAlive)
                    {
                        cancel = true;
                        thread3.Join();
                    }
                    if (reset_flag == 1)
                    {
                        enable = true;
                        chkReader.Checked = true;
                        lblStatus.Text = "";
                        MessageBox.Show("リセット完了です");
                        return;
                    }
                    else
                    {
                        lblStatus.Text = "";
                        MessageBox.Show("リセット異常です");
                    }
                }
                chkReader.Checked = false;
            }
        }
        /// <summary>
        /// -1: timeout, 1: reset succesfully, 2: user cancel
        /// </summary>
        /// <param name="maxCount"></param>
        /// <param name="delay_ms"></param>
        /// <returns></returns>
        private int resetReader(ref bool cancel, ref int retry,int maxCount=600, int delay_ms=100)//~60 seconds
        {
            int finish = -1;
            retry = 0;
            string version = "";
            while (retry < maxCount)
            {
                byte[] VersionNum = new byte[256];	//version number
                bool r = GetVersionNum(VersionNum);
                if (r)
                {
                    finish = 1;//reset successfully
                    version = toHexString("Version", VersionNum, 1, 20);
                    return finish;//user cancel this procedure
                }
                Thread.Sleep(delay_ms);
                retry++;
                if (cancel)
                {
                    finish = 2;
                    return finish;//user cancel this procedure
                }
            }
            //this line if finish still -1 means time out
            finish = 3;
            return finish;
        }
    }
}

