using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    /// <summary>
    /// 角色
    /// </summary>
    public class CharacterData: ActorData
    {
        /// <summary>
        /// 当前角色的客户端连接
        /// </summary>
        public Connection connection;
        public CharacterData(int entityID, Vector3Int pos, Vector3Int rotation) : base(entityID, pos, rotation)
        {
        }
        public CharacterData(int entityID, string name, int level, int hp, int mp, Vector3Int pos, Vector3Int rotation) : base(entityID, name, level, hp, mp, pos, rotation)
        {
        }
       
         
        public string playerName { get; set; }
        public int player_ID { get; set; }
        
    }
}
