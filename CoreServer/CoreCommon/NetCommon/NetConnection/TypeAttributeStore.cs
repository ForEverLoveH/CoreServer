using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.NetCommon.NetConnection
{
    /// <summary>
    /// 通用的储存
    /// </summary>
    public class TypeAttributeStore
    {
        private Dictionary<string, object> _dict = new Dictionary<string, object>();
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        public void Set<T>(T obj)  
        {
            string key= typeof(T).FullName;
            _dict[key] = obj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            string key = typeof(T).FullName;
            object value;
            if(_dict.ContainsKey(key))
            {
                return (T)_dict[key];
            }
            return default(T);
        }
    }
}
