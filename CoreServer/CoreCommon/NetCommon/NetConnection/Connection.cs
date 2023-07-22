using CoreCommon;
using CoreCommon.GameLog;
using CoreCommon.NetCommon;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CoreCommon.Proto;
using Serilog;
using Google.Protobuf.Reflection;
using CoreCommon.NetCommon.DataStream;
using CoreCommon.MessageData;
using CoreCommon.NetCommon.NetConnection;

namespace CoreCommon.NetCommon
{
    /// <summary>
    /// 网络连接对象
    /// </summary>
    public class Connection : TypeAttributeStore
    {
        private Socket netSocket;

        public Socket M_netSocket
        {
            get { return netSocket; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        public delegate void DisconnectConnectionCallBack(Connection sender);

        public DisconnectConnectionCallBack disconnectCallBack;

        public delegate void DataRecieveCallBack(Connection sender, IMessage message);

        public DataRecieveCallBack dataRecieveCallBack;

        /// <summary>
        ///
        /// </summary>
        /// <param name="netScock"></param>
        /// <param name="dataRecieveCallBack"></param>
        /// <param name="disconnectConnectionCallBack"></param>
        public Connection(Socket netScock)
        {
            this.netSocket = netScock;

            //创建解码器
            LengthFieldDecoder lfd = new LengthFieldDecoder(netSocket, 64 * 1024, 0, 4, 0, 4);
            lfd.DataReceived += HandleClientDataRecieved;
            //if(disconnectCallBack!=null)
            lfd.DisConnection += () => disconnectCallBack?.Invoke(this);
            lfd.Start();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleClientDataRecieved(object? sender, byte[] buffer)
        {
            var code = GetUshort(buffer, 0);
            var msg = ProtobufHelper.ParseFromData(code, buffer, 2, buffer.Length - 2);
            if (MessageRouter.Instance.Running)
            {
                MessageRouter.Instance.AddMessageDataToQueue(this, msg);
            }

            /* string txt = Encoding.UTF8.GetString(buffer);
            Log.Info(txt);*/
            // 解包 得到的package UnPack
            /*var pass= Package.Parser.ParseFrom(buffer);
            var message= ProtobufHelper.UnPack(pass);
            dataRecieveCallBack?.Invoke(this, message);*/
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data">大端数据</param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public ushort GetUshort(byte[] data, int offset)
        {
            if (BitConverter.IsLittleEndian)
                return (ushort)((data[offset] << 8) | data[offset + 1]);
            else
                return (ushort)((data[offset + 1] << 8) | data[offset]);
        }

        /// <summary>
        /// 断开网络
        /// </summary>
        public void CloseNetConntion()
        {
            try
            {
                netSocket.Close();
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }

        #region 发送网络数据包

        /// <summary>
        /// 发送消息往客户端
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void SocketSendDataToClient(byte[] data, int offset, int length)
        {
            lock (this)
            {
                if (netSocket.Connected)
                {
                    netSocket.BeginSend(data, offset, data.Length, SocketFlags.None, new AsyncCallback(SendDataCallBack), netSocket);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="package"></param>
        public void SendDataToClient(IMessage message)
        {
            /* using (var buf = DataStream.DataStream.Allocate())
             {
                 int code = ProtobufHelper.SeqCode(message.GetType());
                 buf.WriteUShort((ushort)code);
                 buf.Write(message.ToByteArray());
                 this.SendDataToClient(buf.ToArray());
             }*/
            /*  // 把 message 打包成 Package
                    Package pack = ProtobufHelper.Pack(message);

                    byte[] data = null;
                    using (var stream = new MemoryStream())
                    {
                       pack.WriteTo(stream);//zhuancheng byte shuzhu
                        // 编码
                        int len = (int)stream.Length;
                        data = new byte[4 + len];
                        byte[] lenbyte =BitConverter.GetBytes(len);
                        if (BitConverter.IsLittleEndian)
                        {
                            //小端平台
                            Array.Reverse(lenbyte);
                        }
                        //数据拼装
                        Array.Copy(lenbyte, 0, data, 0, 4);
                        Array.Copy(stream.GetBuffer(), 0, data, 4, (int)stream.Length);
                    }
                    SendDataToClient(data, 0, data.Length);*/
            using (var ds = DataStream.DataStream.Allocate())
            {
                int code = ProtobufHelper.SeqCode(message.GetType());
                ds.WriteInt(message.CalculateSize() + 2);
                ds.WriteUShort((ushort)code);
                message.WriteTo(ds);
                this.SendDataToClient(ds.ToArray());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        public void SendDataToClient(byte[] data, int offset, int length)
        {
            lock (this)
            {
                if (netSocket.Connected)
                {
                    Log.Debug("发送消息：len={0}", data.Length);
                    byte[] buffer = new byte[4 + length];
                    byte[] lenb = BitConverter.GetBytes(length);
                    if (BitConverter.IsLittleEndian) Array.Reverse(lenb);
                    Array.Copy(lenb, 0, buffer, 0, 4);
                    Array.Copy(data, offset, buffer, 4, length);
                    netSocket.BeginSend(buffer, offset, buffer.Length, SocketFlags.None, new AsyncCallback(SendDataCallBack), netSocket);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        private void SendDataToClient(byte[] data)
        {
            this.SocketSendDataToClient(data, 0, data.Length);
        }

        /// <summary>
        /// 发送消息是否成功
        /// </summary>
        /// <param name="ar"></param>
        private void SendDataCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            int len = netSocket.EndSend(ar);
        }

        #endregion 发送网络数据包

    }
}