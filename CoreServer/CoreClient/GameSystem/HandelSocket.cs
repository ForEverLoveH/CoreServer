using CoreCommon;
using CoreCommon.NetCommon;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CoreClient.GameSystem
{
    public class HandelSocket:Singleton<HandelSocket>
    {
         
        /// <summary>
        /// 发送数据到服务器
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="body"></param>
        public void SendMessageToServer(Socket socket, byte[] body)
        {
            int len = body.Length;
            byte[] buffers = BitConverter.GetBytes(len);
            socket.Send(buffers);
            socket.Send(body);
        }

        /// <summary>
        /// 处理从服务器接收到的数据
        /// </summary>
        /// <param name="socket"></param>
        public void HandleServerMeassage(Socket socket)
        {
           
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /*public void SendRequest(IMessage message,Connection conn)
        {
            var pa = new Package()
            {
                Request = new Request(),
            };
            foreach (var p in pa.Request.GetType().GetProperties())
            {
               // Console.WriteLine(p.PropertyType);
                if (p.PropertyType == message.GetType())
                {
                    p.SetValue(pa.Request, message);
                }
            }
            conn.SendDataToClient(message);
        }*/
    }
}
