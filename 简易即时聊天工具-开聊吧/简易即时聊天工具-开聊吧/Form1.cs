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

namespace 简易即时聊天工具_开聊吧
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        ////定义一个服务器类，用于服务器的初始化及其他相关服务
        //class Server
        //{
        //    IPAddress ip;          //服务器的IP地址
        //    IPEndPoint point;      //服务器的端点号
        //    string Client_Ip;      //记录客户端端点
        //    Socket socket_Listen;  //监听socket
        //    Socket socket_Trans;   //通信socket
        //    public Dictionary<string, Socket> dic = new Dictionary<string, Socket>();//记录已创建连接的客户端通信Socket


        //    //构造函数：用于初始化服务器ip地址和端点号
        //   public Server(string strip,string strport,string strcilent)
        //    {
        //        this.ip = IPAddress.Parse(strip);
        //        this.point = new IPEndPoint(this.ip, int.Parse(strport));
        //        this.Client_Ip = strcilent;
        //    }

        //    //服务器的启动Server_Start()
        //    public bool Server_Start()
        //    {
        //        //初始化一个监听socket，使用IPv4地址，流式socket，TCP协议传送数据
        //        this.socket_Listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //        return true;
        //    }

        //    //监听客户端的连接请求
        //    public void Connection_Listen()
        //    {
        //        try
        //        {
        //            //绑定监听端点
        //            this.socket_Listen.Bind(this.point);
        //            this.socket_Listen.Listen(10);//监听队列长度为10
        //            //为监听到的每个发送请求的客户端创建一个线程
        //            Thread thread = new Thread(Accept_Request);
        //            thread.IsBackground = true;
        //            thread.Start(this.socket_Listen);
        //        }
        //        catch (Exception)
        //        {
                    
        //            throw;
        //        }
        //    }

        //    //接受客户端请求
        //    public void Accept_Request(object o)
        //    {
        //        this.socket_Trans = o as Socket;
        //        while (true)
        //        {
        //            try
        //            {
        //                Socket s = this.socket_Trans.Accept();
        //                string point = s.RemoteEndPoint.ToString();
        //                dic.Add(point, s);
        //            }
        //            catch (Exception)
        //            {

        //                throw;
        //            }
        //        }
        //    }

        //    //接收客户端发来的信息
        //    public void Receive_Message(object o)
        //    {
        //        Socket client = o as Socket;
        //        while (true)
        //        {
        //            try
        //            {
        //                byte[] text = new byte[1024 * 1024];//存储从客户端发来的信息
        //                int n = client.Receive(text);       //记录信息的长度
        //                string s = Encoding.UTF8.GetString(text, 0, n);//将信息字节流转成字符串
        //            }
        //            catch (Exception)
        //            {
                        
        //                throw;
        //            }
        //        }
        //    }

        //    //给客户端发送信息
        //    public void Send_Message(string txtMsg)
        //    {
        //        byte[] buffer = Encoding.UTF8.GetBytes(txtMsg);
        //        dic[this.Client_Ip].Send(buffer);
        //    }
        //}


        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        //ip地址
        IPAddress ip;         //服务器的ip地址
        IPEndPoint point;     //服务器端口
        //定义一个容器，存储监听到的客户端端点
        Dictionary<string, Socket> dic = new Dictionary<string, Socket>();

        //定义一个接收客户端信息的方法Receive_Message
        void Receive_Message(object o)
        {
            Socket client = o as Socket;
            while (true)
            {
                try
                {
                    //接收从客户端发来的信息
                    byte[] buffer = new byte[1024 * 1024];//存储发送来的信息
                    int n = client.Receive(buffer);       //记录信息长度
                    String s = Encoding.UTF8.GetString(buffer, 0, n);      //将接收到的信息转成字符串
                    richTextBox1.Text += "客户端:" + client.RemoteEndPoint.ToString() + "\r\n" + s +"\r\n";//显示客户端发来的信息
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }
        //定义一个接受客户端请求的方法Accept_Request
        void Accept_Request(object o)
        {
            Socket s = o as Socket;
            while (true)
            {
                try
                {
                    //创建一个通信用的soket，服务器接受监听到的客户端的请求
                    Socket tSocket = s.Accept();
                    string point = tSocket.RemoteEndPoint.ToString();
                    richTextBox1.Text += "连接服务器成功。。。" + "\r\n";
                    dic.Add(point, tSocket);
                    comboBox1.Items.Add(point);
                    //开辟一个新的线程，负责接收服务器发来的信息
                    Thread thread = new Thread(Receive_Message);
                    thread.IsBackground = true;
                    thread.Start(tSocket);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }
            }
        }

        //监听
        //Server server;
        private void button1_Click(object sender, EventArgs e)
        {
            ////实例化一个服务器
            //server = new Server(textBox1.Text,textBox2.Text,textBox3.Text);
            ////启动服务器
            //server.Server_Start();
            //richTextBox1.Text += "服务器启动。。。" + "\r\n";
            ////开始监听
            //server.Connection_Listen();
            //richTextBox1.Text += "开始监听。。。" + "\r\n";
            //if (server.dic.Count != 0)
            //{
            //    richTextBox1.Text += "连接客户端成功！";
            //}
            ////接受客户端发来的信息
            //server.Receive_Message(server.dic.Keys);

            ip = IPAddress.Parse(textBox1.Text);              //服务器的ip地址
            point = new IPEndPoint(ip, int.Parse(textBox2.Text));//服务器端口
            //创建监听用的Scoket
            Socket Socket_Listen = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //绑定监听Socket
                Socket_Listen.Bind(point);
                //开始监听
                Socket_Listen.Listen(10);
                richTextBox1.Text += "服务器开始监听。。。" + "\r\n";
                //开辟一个新的线程，接受提出连接请求的客户端
                Thread thread = new Thread(Accept_Request);
                thread.IsBackground = true;
                thread.Start(Socket_Listen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //发送信息给客户端
        private void button2_Click(object sender, EventArgs e)
        {
            //server.Send_Message(richTextBox2.Text);
            try
            {
                richTextBox1.Text += "服务器：" + point + "\r\n" + richTextBox2.Text +"\r\n";
                byte[] buffer = Encoding.UTF8.GetBytes(richTextBox2.Text);
                string ip = comboBox1.Text;
                richTextBox2.Clear();
                dic[ip].Send(buffer);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
