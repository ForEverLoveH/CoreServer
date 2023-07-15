using CoreCommon.NetCommon.ByteData;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Proto
{
    public class DataSerializer
    {
        public static byte[] Serialize(params object?[]? data)
        {
            using (MemoryStream me = new MemoryStream())
            {
                foreach (object obj in data)
                {

                    foreach (var ars in data)
                    {
                        if (ars is null) { me.WriteByte(0); }
                        else if (ars is sbyte)
                        {
                            me.WriteByte((byte)1);
                            var d = (sbyte)ars;
                            me.WriteByte((byte)d);
                        }
                        else if (ars is byte)
                        {
                            me.WriteByte((byte)2);
                            me.WriteByte((byte)ars);
                        }
                        else if (ars is short)
                        {
                            me.WriteByte((byte)3);
                            var d = ((short)ars);
                            byte[] arr = BitConverter.GetBytes((short)ars);
                            if (BitConverter.IsLittleEndian)
                                Array.Reverse(arr);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is ushort)
                        {
                            me.WriteByte((byte)4);
                            byte[] arr = BitConverter.GetBytes((ushort)ars);
                            if (BitConverter.IsLittleEndian)
                                Array.Reverse(arr);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is int)
                        {
                            me.WriteByte((byte)5);
                            byte[] arr = Varint.VarintEncode((ulong)(int)ars);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is uint)
                        {
                            me.WriteByte((byte)6);
                            var d = ((short)ars);
                            byte[] arr = Varint.VarintEncode((uint)ars);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is long)
                        {
                            me.WriteByte((byte)7);
                            var d = (long)ars;
                            byte[] arr = Varint.VarintEncode((ulong)d);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is ulong)
                        {
                            me.WriteByte((byte)8);
                            byte[] arr = Varint.VarintEncode((ulong)ars);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is float)
                        {
                            me.WriteByte((byte)9);
                            float d = 1000f * (float)ars;
                            byte[] arr = Varint.VarintEncode((ulong)(long)d);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if ((ars is double))
                        {
                            me.WriteByte((byte)10);
                            double d = 1000d * (double)ars;
                            byte[] arr = Varint.VarintEncode((ulong)(long)d);
                            me.WriteByte((byte)arr.Length);
                            me.Write(arr, 0, arr.Length);
                        }
                        else if (ars is bool)
                        {
                            bool d = (bool)ars;
                            me.WriteByte((byte)(d ? 11 : 12));
                        }
                        else if (ars is string)
                        {
                            string d = (string)ars;
                            byte[] datas = Encoding.UTF8.GetBytes(d);
                            byte[] lenbyte = BitConverter.GetBytes((int)datas.Length);
                            if (BitConverter.IsLittleEndian)
                                Array.Reverse(lenbyte);
                            me.WriteByte((byte)13);
                            me.Write(lenbyte, 0, lenbyte.Length);
                            me.Write(datas, 0, lenbyte.Length);
                            Log.Debug("lenbytes.len={0},datas.len={1}", lenbyte.Length, datas.Length);

                        }
                    }

                }
                return me.ToArray();
            }
            return null;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object[] Deserialize(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                var ds = new List<object>();
                while (ms.Position < ms.Length)
                {
                    byte lp = (byte)ms.ReadByte();
                    if (lp == 0) { ds.Add(null); }
                    else if (lp == 1)
                        ds.Add((sbyte)ms.ReadByte());
                    else if (lp == 2)
                        ds.Add((byte)ms.ReadByte());
                    else if(lp == 3)
                    {
                        byte[] arr = new byte[2];
                        ms.Read(arr, 0, arr.Length);
                        if(BitConverter.IsLittleEndian)
                           Array.Reverse(arr);
                        ds.Add(BitConverter.ToInt16(arr, 0));
                    }
                    else if (lp == 4) // ushort
                    {
                        byte[] arr = new byte[2];
                        ms.Read(arr, 0, 2);
                        if(BitConverter.IsLittleEndian) Array.Reverse(arr);
                        ds.Add(BitConverter.ToUInt16(arr, 0));
                    }else if (lp == 5) // int
                    {
                        int len = ms.ReadByte(); 
                        byte[] arr = new byte[len];
                        ms.Read(arr, 0, len);
                        ds.Add((int)Varint.VarintDecode(arr ));
                    }
                    else if (lp == 6) //uint
                    {
                        int len = ms.ReadByte();
                        byte[] arr = new byte[len];
                        ms.Read(arr,0,len);
                        ds.Add((uint)Varint.VarintDecode(arr ));
                    }
                    else if(lp == 7) // long
                    {
                        int len =ms.ReadByte();
                        byte[] arr = new byte[len];
                        ms.Read(arr,0,len);
                        ulong r = Varint.VarintDecode(arr);
                        ds.Add((long)r);
                    }
                    else if (lp == 8) //ulong
                    {
                        int len =ms.ReadByte();
                        byte[] arr = new byte[len];
                        ms.Read(arr,0,len);
                        ds.Add((ulong)Varint.VarintDecode(arr));
                    }
                    else if (lp == 9) // float
                    {
                        int len =ms.ReadByte();
                        byte[] arr = new byte[len];
                        ms.Read(arr,0,len);
                        float f = Varint.VarintDecode(arr)/1000.0f;
                        ds.Add(f);
                    }
                    else if(lp==10) // double
                    {
                        int  len = ms.ReadByte();
                        byte[] arr = new byte[len];
                        ms.Read(arr,0,len);
                        double val = Varint.VarintDecode(arr) / 1000.0d;
                        ds.Add(val);
                    }
                    else if(lp == 11)// bool - true
                    {
                        ds.Add(true);
                    } 
                    else if(lp==12)  // bool- false
                    {
                        ds.Add(false);
                    }
                    else if(lp==13)
                    {
                        byte[] len = new byte[4];
                        ms.Read(len,0,len.Length    );
                        if(BitConverter.IsLittleEndian) { Array.Reverse(len); }
                        int ls = BitConverter.ToInt32(len);
                        Log.Debug($"字符串的长度:{ls}");
                        byte[] bytes = new byte[ls];
                        ms.Read(bytes,0,bytes.Length    );
                        ds.Add(Encoding.UTF8.GetString(bytes));
                    }
                    else
                    {
                        throw new Exception("无法解析的数据类型"+lp);
                    }

                }
                return ds.ToArray();

            }


        }
    }
}
