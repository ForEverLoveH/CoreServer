using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;
using CoreServer.Manager;
using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class CharacterData : ActorData
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        /// 当前角色的客户端连接
        /// </summary>
        public Connection connection;

        public CharacterData(int entityID, Vector3Int pos, Vector3Int rotation) : base(entityID, pos, rotation)
        {
        }

        public string playerName { get; set; }
        public int player_ID { get; set; }
    }
}