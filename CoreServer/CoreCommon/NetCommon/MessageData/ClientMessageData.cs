
using CoreCommon.NetCommon;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.MessageData
{
    public class ClientMessageData
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        public  Connection    Connection { get; set; }
        /// <summary>
        /// 真正的消息数据
        /// </summary>
        public IMessage package { get; set; }

    }
}
