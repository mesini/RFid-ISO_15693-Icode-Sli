using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace CSharpDEMO
{
    class Reader
    {
        //-------------------------------------------------------------------------------------------
        [DllImport("function.dll")]
        public static extern int API_OpenComm([In]byte[] com, int Baudrate);

        [DllImport("function.dll")]
        public static extern int API_CloseComm(int commHandler);

        [DllImport("function.dll")]
        public static extern int API_SetDeviceAddress(int comHandler, int DeviceAddress, byte newAddr, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_SetBandrate(int comHandler, int DeviceAddress, byte newBaud, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_ControlBuzzer(int comHandler, int DeviceAddress, int freq, int duration, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_ControlLED(int comHandler, int DeviceAddress, int freq, int duration, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int GetVersionNum(int comHandler, int DeviceAddress, [In]byte[] VersionNum);

        [DllImport("function.dll")]
        public static extern int API_SetSerNum(int comHandler, int DeviceAddress, [In]byte[] newValue, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_GetSerNum(int comHandler, int DeviceAddress, [In]byte[] buffer);

        /*14443A-MF*/
        [DllImport("function.dll")]
        public static extern int API_PCDRead(int comHandler, int DeviceAddress,byte mode, byte blk_add, byte num_blk, [In]byte[] snr, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_PCDWrite(int comHandler, int DeviceAddress,byte mode, byte blk_add, byte num_blk, [In]byte[] snr, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int API_PCDInitVal(int comHandler, int DeviceAddress,byte mode, byte SectNum, [In]byte[] snr, [In]byte[] value);

        [DllImport("function.dll")]
        public static extern int API_PCDDec(int comHandler, int DeviceAddress,byte mode, byte SectNum, [In]byte[] snr, [In]byte[] value);

        [DllImport("function.dll")]
        public static extern int API_PCDInc(int comHandler, int DeviceAddress,byte mode, byte SectNum, [In]byte[] snr, [In]byte[] value);

        //[DllImport("function.dll")]
        //public static extern int MF_Request([In]byte[] commHandle, int DeviceAdddress, byte inf_mode, [In]byte[] Buffer);

        //[DllImport("function.dll")]
        //public static extern int MF_Select([In]byte[] commHandle, int DeviiceAddress, byte inf_mode, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int MF_Halt(int comHandler, int DeviceAddress);

        //[DllImport("function.dll")]
        //public static extern int MF_Anticoll([In]byte[] commHandle, int DeviceAddress, [In]byte[] snr, [In]byte[] status);

        //[DllImport("function.dll")]
        //public static extern int MF_Restore([In]byte[] commHandle, int DeviceAddress, byte mode, byte cardlength, [In]byte[] carddata);

        [DllImport("function.dll")]
        public static extern int GET_SNR(int comHandler, int DeviceAddress,int mode, int halt, [In]byte[] snr, [In]byte[] value);

        /*Ultralight*/
        [DllImport("function.dll")]
        public static extern int UL_Request(int comHandler, int DeviceAddress, byte mode, [In]byte[] snr);

        [DllImport("function.dll")]
        public static extern int UL_HLRead(int comHandler, int DeviceAddress, byte mode, byte blk_add, [In]byte[] snr, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int UL_HLWrite(int comHandler, int DeviceAddress, byte mode, byte blk_add, [In]byte[] snr, [In]byte[] buffer);

        /*ISO14443TypeB*/
        [DllImport("function.dll")]
        public static extern int RequestType_B(int comHandler, int DeviceAddress, [In]byte[] buffer);

        //[DllImport("function.dll")]
        //public static extern int TYPEB_SFZSNR(byte mode, byte halt, [In]byte[] snr, [In]byte[] value);

        [DllImport("function.dll")]
        public static extern int API_ISO14443TypeBTransCOSCmd(int comHandler, int DeviceAddress, [In]byte[] cmd, int cmdSize, [In]byte[] buffer);

        /*ISO15693*/

        [DllImport("function.dll")]
        public static extern int ISO15693_Inventory(int commHandle, int DeviceAddress, [In]byte[] Cardnumbers, [In]byte[] pBuffer);
        /*
           函数功能：此命令通过防冲突用于得到读卡区域内所有卡片的序列号（能得到的卡片数量与模块天线的输出功率有关，一般能对2~6卡进行防冲突）

           输入参数：
               Cardnumber       返回的卡的数量（一个字节）
               pBuffer          返回的数据（包括FLAG和DSFID和8*n个字节的卡号）

           输出参数：
               如果：操作成功
                   Cardnumber       返回的卡的数量（一个字节）
                   pBuffer          返回的数据（包括FLAG和DSFID和8*n个字节的卡号）
                        
               如果：操作失败，则*nrOfCard为错误代码

           返回值：
               0x00，操作成功，  
               0x01，操作失败
        */

        [DllImport("function.dll")]
        public static extern int API_ISO15693Read(int comHandler, int DeviceAddress, byte flags, byte blk_add, byte num_blk, [In]byte[] uid, [In]byte[] buffer);
        /*
            函数功能：
	            用来读取1个或多个扇区的值，如果要读每个块的安全位，
                将FLAGS中Option_flag置为1，即FLAG = 0X42，每个扇区将返回5个字节，包括1个表示安全状态字节和4个字节的块内容，这时候每次最多能读12个块。
                如果FLAG = 02，将只返回4字节的块内容，这时候每次最多能读63个块。

            输入参数：
                flags          0x02  不带uid
                               0x22    带uid
                               0x42  不带uid但是要读安全位
                blk_add,       要读的起始块号
                num_blk,       块的数量
                *uid           UID信息
                *buffer        返回值

            输出参数：
                操作成功,buffer[0]  返回的flag   buffer[1..N]  Data   
                操作失败，buffer[0]为错误代码

            返回值：
                0x00，操作成功，  
                0x01，操作失败
         */

        [DllImport("function.dll")]
        public static extern int API_ISO15693Write(int comHandler, int DeviceAddress, byte flag, byte blk_add, byte num_blk, [In]byte[] uid, [In]byte[] data);
        /*
            函数功能：  对一个块进行写操作（每次只能写一个块）

            输入参数：
                flags         0x02  不带uid
                              0x22    带uid
                              0x42  不带uid但是要读安全位
                blk_add,      要写的起始块号
                num_blk,      写的块的数量
                *uid          UID信息
                *data         返回值

            输出参数：
                如果：操作失败，则data[0]为错误代码

            返回值：
                0x00，操作成功，  
                0x01，操作失败
         */

        [DllImport("function.dll")]
        public static extern int ISO15693_GetSysInfo(int comHandler, int DeviceAddress, byte flag, [In]byte[] uid, [In]byte[] Buffer);

        [DllImport("function.dll")]
        public static extern int ISO15693_Lock(int comHandler, int DeviceAddress, byte flags, byte num_blk, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int ISO15693Select(int comHandler, int DeviceAddress, byte flags, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int WriteAFI(int comHandler, int DeviceAddress, byte flags, byte afi, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int LockAFI(int comHandler, int DeviceAddress, byte flags, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int WriteDSFID(int comHandler, int DeviceAddress, byte flags, byte DSFID, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int LockDSFID(int comHandler, int DeviceAddress, byte flags, [In]byte[] uid, [In]byte[] buffer);

        [DllImport("function.dll")]
        public static extern int ISO15693_GetMulSecurity(int comHandler, int DeviceAddress, byte flag, byte blkAddr, byte blkNum, [In]byte[] uid, [In]byte[] pBuffer);
        [DllImport("function.dll")]
        public static extern int ISO15693StayQuiet(int comHandler, int DeviceAddress, byte flags, [In]byte[] uid, [In]byte[] buffer);
        [DllImport("function.dll")]
        public static extern int ResetToReady(int comHandler, int DeviceAddress, byte flags, [In]byte[] uid, [In]byte[] buffer);
        [DllImport("function.dll")]
        public static extern int API_ISO15693TransCOSCmd(int comHandler, int DeviceAddress, [In]byte[] cmd, int cmdSize, [In]byte[] buffer);
        /// <summary>
        /// 文字列をBYTEに変換し指定した位置に格納する
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bytearray"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        public static  void WriteString2data(byte[] data,string str, int start, int len = 4)
        {
            if (data == null) return;
            int length = str.Length * 2;
            if (len > length) length = len;//どちらか大きいほう
            byte[] dat = new byte[length];
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            dat = asciiEncoding.GetBytes(str);
            for (int i = 0; i < len; i++)
            {
                if (dat.Length <= i)
                {
                    // data[start + i] = 32;// space Ascii
                    data[start + i] =0;// null ASCII
                }
                else
                {
                    data[start + i] = dat[i];
                }
            }
        }
        public static string ReadAsString(byte[] data,int start, int len)
        {
            if (data == null) return "";
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            byte[] dat = new byte[len];
            Buffer.BlockCopy(data, start, dat, 0, len);
            return asciiEncoding.GetString(dat);
        }
    }
    public class IcodeSliTAG
    {
        public byte DSFID = 0;
        public byte[] UID = new byte[8];
        public byte[] data;



        public IcodeSliTAG Clone()
        {
            IcodeSliTAG clone = new IcodeSliTAG();
            clone.data = new byte[256];
            Buffer.BlockCopy(data, 0, clone.data, 0, data.Length);
            Buffer.BlockCopy(UID, 0, clone.UID, 0, UID.Length);
            clone.DSFID = DSFID;
            return clone;
        }


        /// <summary>
        /// 文字列をBYTEに変換し指定した位置に格納する
        /// </summary>
        /// <param name="str"></param>
        /// <param name="bytearray"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        public void WriteString2data(string str, int start, int len = 4)
        {
            if (data == null) return;
            int length = str.Length * 2;
            if (len > length) length = len;//どちらか大きいほう
            byte[] dat = new byte[length];

            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();

            dat = asciiEncoding.GetBytes(str);

            for (int i = 0; i < len; i++)
            {
                if (dat.Length <= i)
                {
                    data[start + i] = 32;// space Ascii
                }
                else
                {
                    data[start + i] = dat[i];

                }



            }
        }
        /// <summary>
        /// IntをBYTEに変換し指定した位置に格納する
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bytearray"></param>
        /// <param name="start"></param>
        /// <param name="len"></param>
        public void WriteInt2data(int value, int start, int len = 4)
        {
            int val = value;
            data[start] = (byte)(val % 0x100); val /= 0x100;
            data[start + 1] = (byte)(val % 0x100); val /= 0x100;
            data[start + 2] = (byte)(val % 0x100); val /= 0x100;
            data[start + 3] = (byte)(val % 0x100);

        }

        /// <summary>
        /// データの特定の部位を文字列として読み出す
        /// </summary>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// 
        public string ReadAsString(int start, int len)
        {
            if (data == null) return "";
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            byte[] dat = new byte[len];
            Buffer.BlockCopy(data, start, dat, 0, len);
            return asciiEncoding.GetString(dat);
        }
        /// <summary>
        /// データの特定の部位をIntとして読み出す
        /// </summary>
        /// <param name="start"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public int ReadAsint(int start, int len = 4)
        {
            if (data == null) return 0;
            if (len > 4) return 0;
            byte[] dat = new byte[len];

            Buffer.BlockCopy(data, start, dat, 0, len);
            int val = dat[3] * 0x1000000 + dat[2] * 0x10000 + dat[1] * 0x100 + dat[0];
            return val;
        }

        public override string ToString()
        {
            return ToHexString(UID);
        }
        public static string ToHexString(byte[] bytes)
        {
            String hexString = String.Empty;
            for (int i = 0; i < bytes.Length; i++)
                hexString += byteHEX(bytes[i]);

            return hexString;
        }
        public static String byteHEX(Byte ib)
        {
            String _str = String.Empty;
            try
            {
                char[] Digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A',
                'B', 'C', 'D', 'E', 'F' };
                char[] ob = new char[2];
                ob[0] = Digit[(ib >> 4) & 0X0F];
                ob[1] = Digit[ib & 0X0F];
                _str = new String(ob);
            }
            catch (Exception)
            {
                new Exception("对不起有错。");
            }
            return _str;

        }
    }
    public static class String_
    {
        /// <summary>
        /// if InvokeRequired using BeginInvoke to set Control.Text
        /// </summary>
        /// <param name="control"></param>
        /// <param name="s"></param>
        public static void set_text(this Control control, string s)
        {
            if (control.InvokeRequired == true)
            {
                control.BeginInvoke((MethodInvoker)delegate
                {
                    control.Text = s;
                });
            }
            else
            {
                control.Text = s;
            }
        }
        /// <summary>
        /// if InvokeRequired using BeginInvoke to set Control.Text
        /// </summary>
        /// <param name="control"></param>
        /// <param name="s"></param>
        public static void append_text(this Control control, string s)
        {
            if (control.InvokeRequired == true)
            {
                control.BeginInvoke((MethodInvoker)delegate
                {
                    control.Text += s;
                });
            }
            else
            {
                control.Text += s;
            }
        }
    }
}
