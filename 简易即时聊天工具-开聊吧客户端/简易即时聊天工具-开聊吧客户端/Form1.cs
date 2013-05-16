using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace 简易即时聊天工具_开聊吧客户端
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //定义一个客户端socket
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //定义信息接收方法Receive_Message()
        private void Receive_Message()
        {
            while (true)//总是处于接收信息状态
            {
                try
                {
                    byte[] text = new byte[1024 * 1024];//存储服务器发来的信息
                    int n = client.Receive(text);       //记录信息长度
                    string s = Encoding.UTF8.GetString(text, 0, n);//将发来的信息转成字符串
                    richTextBox1.Text += "服务器:" + client.RemoteEndPoint.ToString() + "\r\n" + s +"\r\n";//显示服务器发来的信息
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress ip = IPAddress.Parse(textBox1.Text);              //服务器的ip地址
            IPEndPoint point = new IPEndPoint(ip, int.Parse(textBox2.Text));//服务器端口
            try
            {
                //尝试连接到服务器
                client.Connect(point);
                richTextBox1.Text += "连接成功！" + "\r\n";
                richTextBox1.Text += "服务器:" + client.RemoteEndPoint.ToString() + "\r\n";
                richTextBox1.Text += "客户端:" + client.LocalEndPoint.ToString() + "\r\n";
                //连接成功，开辟一个线程，接收服务器发送的信息
                Thread thread = new Thread(Receive_Message);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                try
                {
                    richTextBox1.Text += "客户端：" + client.LocalEndPoint.ToString() + "\r\n" + richTextBox2.Text +"\r\n";
                    byte[] buffer = Encoding.UTF8.GetBytes(richTextBox2.Text);
                    richTextBox2.Clear();
                    client.Send(buffer);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
