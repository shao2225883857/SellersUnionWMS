using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Sellers.Cheng.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private byte[] receivedData = new byte[8];
       

        private void Form1_Load(object sender, EventArgs e)
        {
            string text = "+  0.128 kg";


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
                this.Invoke(textChanged, new string[] { s1 + s2 });
            }
            catch (Exception)
            {
                MessageBox.Show(text);
            }


        }
    }
}
