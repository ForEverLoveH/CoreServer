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
    public class EntityManager:Singleton<EntityManager>
    {
        private int index = 1;
        /// <summary>
        /// 创建
        /// </summary>
        /// <returns></returns>
        private Entity CreateEntity(string name,int level,int hp,int mp,Vector3 pos,Vector3 roation )
        {
            lock (this)
            {
                return new Entity(index++,name,level,hp ,mp,new Vector3Int(int.Parse(pos.X.ToString()),int.Parse(pos.Y.ToString()),int.Parse(pos.Z.ToString())), new Vector3Int(int.Parse(roation.X.ToString()), int.Parse(roation.Y.ToString()), int.Parse(roation.Z.ToString())));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NewEntityId
        {
            get {
                lock (this)
                {
                    return index++;
                }
            }
        }

    }
}
