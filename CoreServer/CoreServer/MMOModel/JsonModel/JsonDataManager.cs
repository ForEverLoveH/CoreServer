using CoreCommon;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    public class JsonDataManager : Singleton<JsonDataManager>
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public T DeserializeObject<T>(string data) where T : class
        {
            return string.IsNullOrEmpty(data) == true ? null : JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SerializeObject<T>(T data) where T : class
        {
            return data == null ? null : JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 加载当前地图信息
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public Dictionary<int, T> LoadingCurrentSpaceData<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            else
            {
                string content = File.ReadAllText(path);
                if (!string.IsNullOrEmpty(content))
                {
                    return JsonDataManager.Instance.DeserializeObject<Dictionary<int, T>>(content);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}