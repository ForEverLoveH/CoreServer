using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreCommon.NetCommon.NetServer;
using CoreCommon.Proto;
using CoreServer.MMOModel;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameNet
{
    /// <summary>
    /// 网络服务
    /// </summary>
    public  class NetService  
    {
        //TcpSocketListener listener;
        private TcpService tcpService;
        public NetService(string ip ,int port ,int block=100) 
        {
            tcpService = new TcpService( ip, port,block );
            tcpService.NewConnection += NewClientContion;
            tcpService.DisConnection += ClientDisContion;
           // tcpService.DataRecevied += DataRecieveData;
        }
        /// <summary>
        /// 记录链接对象的最后一次心跳时间
        /// </summary>
        private Dictionary<Connection,DateTime> HeartBeatPairs = new Dictionary<Connection,DateTime>();
        /*/// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
       private void DataRecieveData(Connection connection, IMessage data)
        {
           // Package package = ProtobufHelper. DeserializeProtoData<Package>(data);
            MessageRouter.Instance.AddMessageDataToQueue(connection, data);
        }*/
        /// <summary>
        /// 客户端断开
        /// </summary>
        /// <param name="connection"></param>
        private void ClientDisContion(Connection connection)
        {

            Log.Information("连接断开"+connection);
            var sapce=connection.Get<SpaceData>();
            if(sapce!=null )
            {
                var character= connection.Get<CharacterData>();
                sapce.CharacterLeave(connection, character);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        private void NewClientContion(Connection conn)
        {
            Console.WriteLine("客户端已接入");
            HeartBeatPairs[conn] = DateTime.Now;
            HeartBeatPairs.Remove(conn);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartService()
        {
            tcpService.StartService();
            MessageRouter.Instance.Start(10);
            MessageRouter.Instance.OnMessage<HeartBeatRequest>(_HeartBeatRequest);
            Timer timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void   TimerCallback(object state)
        {
            Log.Information("执行心跳检查");
            var  now = DateTime.Now;
            foreach(var pair in HeartBeatPairs)
            {
                var cha= now- pair.Value;
                if (cha.TotalMilliseconds > 20)
                {
                    //关闭超时链接
                    pair.Key.CloseNetConntion();
                    HeartBeatPairs.Remove(pair.Key);
                }
            }
        }
        /// <summary>
        /// 监听心跳包
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _HeartBeatRequest(Connection conn, HeartBeatRequest messageData)
        {
            Log.Information("收到心跳包" + conn);
            HeartBeatPairs[conn] = DateTime.Now;
           // Thread.Sleep(300);
            HeartBeatResponse message = new HeartBeatResponse();
            conn.SendDataToClient(message);
        }
    }
}
