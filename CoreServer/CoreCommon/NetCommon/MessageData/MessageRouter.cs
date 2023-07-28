using CoreCommon;
using CoreCommon.GameLog;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using System.Reflection;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Google.Protobuf.WellKnownTypes;
using Serilog;

namespace CoreCommon.MessageData
{
    /// <summary>
    /// 消息队列
    /// </summary>
    public class MessageRouter : Singleton<MessageRouter>
    {
        private int threadCount = 1;//默认工作线程数目
        private int workCount = 0;//当前线程数

        /// <summary>
        /// 协调多个线程的通信
        /// </summary>
        private AutoResetEvent AutoResetEvent = new AutoResetEvent(true);

        /// <summary>
        /// 消息队列
        /// </summary>
        private Queue<ClientMessageData> messageQueue = new Queue<ClientMessageData>();

        /// <summary>
        /// 消息处理器
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        public delegate void MessageHandler<T>(Connection netConnection, T messageData);

        /// <summary>
        /// 频道字典(订阅记录)
        /// </summary>
        private Dictionary<string, Delegate> Handler = new Dictionary<string, Delegate>();

        /// <summary>
        /// 将消息传入到消息对列中
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        public void AddMessageDataToQueue(Connection netConnection, IMessage message)
        {
            lock (messageQueue)
            {
                messageQueue.Enqueue(new ClientMessageData()
                {
                    Connection = netConnection,
                    package = message
                });
            }
            if (messageQueue.Count > 0)
            {
                AutoResetEvent.Set();//唤醒1等待线程
            }
        }

        #region 订阅

        public void OnMessage<T>(MessageHandler<T> handler) where T : IMessage
        {
            try
            {
                string key = typeof(T).FullName;
                if (!Handler.ContainsKey(key))
                {
                    Handler[key] = null;
                }
                Handler[key] = (Handler[key] as MessageHandler<T>) + handler;
                Console.Write(Handler[key].GetInvocationList().Length);
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }

        #endregion 订阅

        #region 退订

        public void OffMessage<T>(MessageHandler<T> handler) where T : IMessage
        {
            try
            {
                string key = typeof(T).FullName;
                if (!Handler.ContainsKey(key))
                {
                    Handler[key] = null;
                }
                Handler[key] = Handler[key] as MessageHandler<T> - handler;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return;
            }
        }

        #endregion 退订

        /// <summary>
        /// 消息分发器是否在运行
        /// </summary>
        private bool _isRunning = false;

        public bool Running
        { get { return _isRunning; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="threadCount"></param>
        public void Start(int threadCount)
        {
            if (_isRunning) return;
            _isRunning = true;
            this.threadCount = threadCount;
            this.threadCount = Math.Min(Math.Max(threadCount, 1), 200);
            for (int i = 0; i < this.threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MessageWork));
            }
            while (workCount < this.threadCount)
            {
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 线程工作
        /// </summary>
        /// <param name="state"></param>
        private void MessageWork(object? state)
        {
            try
            {
                workCount = Interlocked.Increment(ref workCount);
                ///一直处理
                while (_isRunning)
                {
                    if (messageQueue.Count > 0)
                    {
                        ClientMessageData messageData;
                        lock (messageQueue)
                        {
                            //存在可能有多个线程，但是只有一个消息
                            if (messageQueue.Count == 0) continue;
                            messageData = messageQueue.Dequeue();
                        }
                        if (messageData != null)
                        {
                            var packMessage = messageData.package;
                            if (packMessage != null)
                            {
                                ExcuteLoopMessage(packMessage, messageData.Connection);
                            }
                        }
                    }
                    else
                    {
                        AutoResetEvent.WaitOne(); ///休眠等待，可使用set唤醒 存在前一个线程已经拿走了你当前的消息数据(也就是当前线程要俩消息数）
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Information(ex.StackTrace);
            }
            finally
            {
                workCount = Interlocked.Decrement(ref workCount);
            }
        }

        /// <summary>
        /// 递归处理消息(分发)
        /// </summary>
        /// <param name="message"></param>
        public void ExcuteLoopMessage(IMessage message, Connection connection)
        {
            //触发订阅
            var fireMethod = this.GetType().GetMethod("FireMessageData", BindingFlags.NonPublic | BindingFlags.Instance);
            var met = fireMethod.MakeGenericMethod(message.GetType());
            met.Invoke(this, new object[] { connection, message });
            var t = message.GetType();
            foreach (var p in t.GetProperties())
            {
                // Log.Information($"{p.Name}");
                if (p.Name == "Parser" || p.Name == "Descriptor")
                    continue;
                //只要发现消息就可以订阅 递归思路实现
                var value = p.GetValue(message);
                if (value != null)
                {
                    //发现消息是否需要进一步递归 触发订阅
                    if (typeof(IMessage).IsAssignableFrom(value.GetType()))
                    {
                        //发现消息是否需要进一步递归 触发订阅
                        //继续递归
                        ExcuteLoopMessage((IMessage)value, connection);
                    }
                }
            }
        }

        /* /// <summary>
         /// 根据反射对消息进行分发
         /// </summary>
         /// <param name="connection"></param>
         /// <param name="obj"></param>
         private void ExcuteAuto(NetConnection connection,object obj)
         {
             var fireMethod = this.GetType().GetMethod("FireMessageData", BindingFlags.NonPublic | BindingFlags.Instance);
             //Console.WriteLine("fire="+fireMethod);
             Type t = obj.GetType();
             foreach (var pl in t.GetProperties())
             {
                 if (pl.Name == "Parser" || pl.Name == "Descriptor")
                     continue;
                 var value = pl.GetValue(obj);
                 if (value != null)
                 {
                     var met = fireMethod.MakeGenericMethod(value.GetType());
                     met.Invoke(this, new object[] { connection, value });
                 }
             }
         }*/

        /// <summary>
        /// 触发消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void FireMessageData<T>(Connection netConnection, T messageData)
        {
            string type = typeof(T).FullName;
            if (Handler.ContainsKey(type))
            {
                MessageHandler<T> handler = (MessageHandler<T>)Handler[type];
                try
                {
                    handler?.Invoke(netConnection, messageData);
                }
                catch (Exception ex)
                {
                    LoggerHelper.Debug(ex);
                    //打印错误日志
                    Log.Information("messageRouter is error" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 关闭消息分发器
        /// </summary>
        public void Stop()
        {
            _isRunning = false;
            messageQueue.Clear();
            while (workCount > 0)
            {
                AutoResetEvent.Set();
            }
            Thread.Sleep(50);//考虑多线程，数据不一定同步
        }
    }
}