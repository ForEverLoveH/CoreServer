 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.NetCommon
{
    public class JsonData:Singleton<JsonData>
    {
        /// <summary>
        /// 反列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T DeserializeJsonData<T>(string data) where T:class 
        {
            return string.IsNullOrEmpty(data)? null : Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public  string SerializeJsonData<T>(T  data) where T:class
        {
            return  Newtonsoft.Json.JsonConvert.SerializeObject(data);
        }
    }
}
