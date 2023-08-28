using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using CoreCommon.GameLog;
using Serilog;
using Google.Protobuf;

namespace CoreCommon.NetCommon.NetServer
{
    /// <summary>
    /// 负责tcp 网络端口，异步接收
    ///
    /// </summary>
    public class TcpService
    {
        private IPEndPoint endPoint;
        private Socket serverSocket;
        private int balckLog = 100;

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        public delegate void NewConnectionedCallBack(Connection connection);

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        public delegate void DataReceviedCallback(Connection connection, IMessage data);

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        public delegate void DisConnectionCallBack(Connection connection);

        /// <summary>
        /// 客户端接入事件
        /// </summary>
        public event EventHandler<Socket> _socketConnected;

        /// <summary>
        /// 事件委托 新建连接
        /// </summary>
        public event NewConnectionedCallBack NewConnection;

        /// <summary>
        /// 事件委托断开连接
        /// </summary>
        public event DisConnectionCallBack DisConnection;

        /// <summary>
        /// 收到消息的委托
        /// </summary>
        public event DataReceviedCallback DataRecevied;

        /// <summary>
        ///
        /// </summary>
        public bool IsRunning
        { get { return serverSocket != null; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public TcpService(string host, int port)
        {
            endPoint = new IPEndPoint(IPAddress.Parse(host), port);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="block"></param>
        public TcpService(string host, int port, int block) : this(host, port)
        {
            this.balckLog = block;
        }

        /// <summary>
        ///
        /// </summary>
        public void StartService()
        {
            lock (this)
            {
                if (IsRunning == false)
                {
                    serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    serverSocket.Bind(endPoint);
                    serverSocket.Listen(balckLog);
                    Log.Information("开始监听端口: " + endPoint.Port);
                    SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                    args.Completed += HandleConnection;
                    serverSocket.AcceptAsync(args);
                }
                else
                {
                    Log.Information("tcp Service 已经连接");
                }
            }
        }

        /// <summary>
        /// 处理连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleConnection(object? sender, SocketAsyncEventArgs e)
        {
            Socket client = e.AcceptSocket as Socket;
            e.AcceptSocket = null;
            serverSocket.AcceptAsync(e);
            if (e.SocketError == SocketError.Success)
            {
                if (client != null)
                {
                    HandleNewConnection(client);
                }
            }
        }

        /// <summary>
        /// 新socket接入
        /// </summary>
        /// <param name="socket"></param>
        private void HandleNewConnection(Socket socket)
        {
            _socketConnected?.Invoke(this, socket);
            Connection connection = new Connection(socket);
            connection.dataRecieveCallBack += (connection, data) => DataRecevied(connection, data);
            connection.disconnectCallBack += (connection) => DisConnection?.Invoke(connection);
            NewConnection?.Invoke(connection);
        }

        /// <summary>
        ///
        /// </summary>
        public void StopService()
        {
            lock (this)
            {
                if (serverSocket == null) { return; }
                serverSocket.Close();
                serverSocket = null;
                serverSocket.Dispose();
            }
        }
    }
}