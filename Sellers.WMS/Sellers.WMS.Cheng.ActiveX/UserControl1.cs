using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sellers.WMS.Cheng.ActiveX
{
    public partial class UserControl1 : UserControl, IObjectSafety
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private string value;

        public string Value
        {
            get { return textBox1.Text; }

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            serialPort1.PortName = "COM1";
            serialPort1.BaudRate = 9600;
            serialPort1.DataBits = 8;
            serialPort1.Parity = Parity.None;
            serialPort1.StopBits = StopBits.One;
            serialPort1.ReceivedBytesThreshold = 1;
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
            }

            //设置 DataReceived 事件发生前内部输入缓冲区中的字节数为8
            //serialPort1.ReceivedBytesThreshold = 8;
            //将事件处理方法添加到事件中去
            textChanged += new UpdateTextEventHandler(ChangeText);
            try
            {
                serialPort1.Open();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("未能打开端口，请检查是否已经连接串口.\n" + ex.Message);
            }
        }
        //定义委托
        private delegate void UpdateTextEventHandler(string text);
        //定义事件
        private event UpdateTextEventHandler textChanged;
        //事件处理方法
        private void ChangeText(string text)
        {
            if (textBox1.Text != text)
                textBox1.Text = text;
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);
            string text = serialPort1.ReadExisting();
            
            try
            {
                string s1 = text.Substring(1, 7);
                string s2 = text.Substring(8, 3);
                double w = Convert.ToDouble(s1.Trim());
                if(s2.Trim().ToLower()=="kg")
                {
                    w = w*1000;
                }
                this.Invoke(textChanged, new string[] { w.ToString() });
            }
            catch (Exception)
            {
               // MessageBox.Show(text);
            }
        }



        public void GetInterfacceSafyOptions(Int32 riid, out Int32 pdwSupportedOptions, out Int32 pdwEnabledOptions)
        {
            //TODO:添加 WebCamControl.GetInterfacceSafyOptions 实现 
            pdwSupportedOptions = 1;
            pdwEnabledOptions = 2;

        }

        public void SetInterfaceSafetyOptions(Int32 riid, Int32 dwOptionsSetMask, Int32 dwEnabledOptions)
        {
            //TODO:添加 WebCamControl.SetInterfaceSafetyOptions 实现             

        }
    }
}
