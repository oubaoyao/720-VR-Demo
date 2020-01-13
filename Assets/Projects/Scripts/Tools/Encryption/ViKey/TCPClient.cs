using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
namespace MTFrame. Tool.ViKey
{
    /// <summary>
    /// TCP客户端
    /// </summary>
    public class TCPClient:IDisposable
    {
        int port = 0226;
        string host = "127.0.0.1";

        
        public Socket tcp;

        /// <summary>
        /// 接收回调事件
        /// </summary>
        public System.Action<string> OnReceiveCall;

        private Thread ConnectThread;
        private Thread ReceiveThread;
        private Thread PingThread;

        private bool isThread = true;

        //构造函数
        public TCPClient()
        {
            tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            ConnectThread = new Thread(new ThreadStart(Connect));
            ConnectThread.Start();
        }

        //连接
        private void Connect()
        {
            while (true)
            {
                if (!isThread)
                    return;
                try
                {
                    tcp.Connect(IPAddress.Parse(host), port);
                    if (tcp.Connected)
                    {
                        Debug.Log("连接成功");

                        ReceiveThread = new Thread(new ThreadStart(Receive));
                        ReceiveThread.Start();
                        PingThread = new Thread(new ThreadStart(ping));
                        PingThread.Start();

                        return;
                    }
                }
                catch
                {
                    tcp.Close();
                    tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    Debug.Log("连接失败");
                }
                Thread.Sleep(1000);
            }

        }

        private void ping()
        {
            while (true)
            {
                if (!isThread)
                    return;
                if (tcp != null)
                {
                    if (tcp.Connected)
                    {
                        tcp.Send(ASCIIEncoding.UTF8.GetBytes("Ping"));
                    }
                    else
                    {
                        Debug.Log("连接断开");

                        ConnectThread = new Thread(new ThreadStart(Connect));
                        ConnectThread.Start();

                        return;
                    }
                }
                else
                {
                    Debug.Log("tcp不存在");
                    return;
                }
                Thread.Sleep(1000);
            }
        }
      
        /// <summary>
        /// 接收
        /// </summary>
        private void Receive()
        {
            while (true)
            {
                if (!isThread)
                    return;
                try
                {
                    if (tcp.Connected)
                    {
                        byte[] buffer = new byte[4096];
                        int lis = tcp.Receive(buffer);
                        string data = ASCIIEncoding.UTF8.GetString(buffer, 0, lis);

                        OnReceiveCall?.Invoke(data);
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("监听服务器信息错误=" + e.Message);
                }
            }
        }

        /// <summary>
        ///关闭
        /// </summary>
        public void Close()
        {
            isThread = false;
            if (ConnectThread != null)
            {
                ConnectThread.Abort();
                ConnectThread = null;
            }
            if (PingThread != null)
            {
                PingThread.Abort();
                PingThread = null;
            }
            if (ReceiveThread != null)
            {
                ReceiveThread.Abort();
                ReceiveThread = null;
            }

        }

        public void Dispose()
        {
            Close();
        }
    }
}