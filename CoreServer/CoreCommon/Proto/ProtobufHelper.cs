using Google.Protobuf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Emit;
using System.Reflection;
using Google.Protobuf.Reflection;
using Serilog;
 

namespace CoreCommon.Proto
{
    public class ProtobufHelper  
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] SerializeProtoData(IMessage message)
        {
            using (var ms = new MemoryStream())
            {
                message.WriteTo(ms);
                byte[] da = ms.ToArray();
                return da;
            }
        }
        /// <summary>
        /// 解析
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeProtoData<T>(byte[] data) where T : IMessage, new() {
            T msg = new T();
            msg = (T)msg.Descriptor.Parser.ParseFrom(data);
            return msg;

        }
        private  static Dictionary<string, Type> GetAllTypes = new Dictionary<string, Type>();
        /// <summary>
        /// 根据序号获取类型
        /// </summary>
        private  static Dictionary<int,Type>MDic1= new Dictionary<int,Type>();
        /// <summary>
        /// 根据类型获取序号
        /// </summary>
        private static Dictionary<Type,int> MDic2 = new Dictionary<Type,int>();

        static ProtobufHelper()
        {
            List<string> list = new List<string>();
            var lp = from t in Assembly.GetExecutingAssembly().GetTypes() select t;
            //扫描所有的 proto 的消息类型
            lp.ToList().ForEach(t =>
            {
                if (typeof(IMessage).IsAssignableFrom(t))
                {
                    var dst= t.GetProperty("Descriptor").GetValue(t) as MessageDescriptor;
                    // GetAllTypes.Add()
                    GetAllTypes.Add(dst.FullName, t);
                    Log.Information("类型注册" + dst.FullName);
                    list.Add(dst.FullName);
                }
            });
            list.Sort((x, y) =>
            {
                if (x.Length != y.Length)
                {
                    return x.Length - y.Length;
                }
                //长度相同
                return string.Compare(x, y, StringComparison.Ordinal);
            });
            for (int i = 0; i < list.Count; i++)
            {
                var fname = list[i];
                var tl = GetAllTypes[fname];
                Log.Debug("Proto类型注册:{0}-1",i, fname);
                MDic1[i] = tl;
                MDic2[tl] = i;
            }

        }
        /// <summary>
        /// 根据消息的类型去序号
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static  int SeqCode(Type type)
        {
            return MDic2[type];
        }
        /// <summary>
        /// 根据序号找到类型
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Type SeqType(int index)
        {
            return MDic1[index];
        }


        /// <summary>
        /// 将普通消息打包成 package
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Package Pack(IMessage message)
        {
            Package package = new Package();
            package.FullName=message.Descriptor.FullName;
            package.Data=message.ToByteString();
            return package;
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static  IMessage UnPack(Package package)
        {
            string fullName = package.FullName;
            if (GetAllTypes.ContainsKey(fullName))
            {
                Type ts = GetAllTypes[fullName];
                var desc = ts.GetProperty("Descriptor").GetValue(ts) as MessageDescriptor;
                return desc.Parser.ParseFrom(package.Data);
            }
            return null;
        }
        /// <summary>
        /// 根据消息编码进行解析
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static    IMessage ParseFromData(int index, byte[] data  ,int offset,int len)
        {
            Type tp = ProtobufHelper.SeqType(index);
            var desc = tp.GetProperty("Descriptor").GetValue(tp) as MessageDescriptor;
            var msg = desc.Parser.ParseFrom(data, 2, data.Length - 2);
                Log.Information("解析消息：code={0}-{1}", index, msg);
            return msg;
        }
        
    }
}
