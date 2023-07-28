using CoreCommon;
using CoreCommon.NetCommon;
using CoreServer.MMOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.Manager
{
    /// <summary>
    /// Entity管理器
    /// </summary>
    public class EntityManager : Singleton<EntityManager>
    {
        private int index = 1;

        /// <summary>
        ///
        /// </summary>
        public int NewEntityId
        {
            get
            {
                lock (this)
                {
                    return index++;
                }
            }
        }
    }
}