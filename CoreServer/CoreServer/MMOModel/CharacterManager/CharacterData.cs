using CoreCommon.NetCommon;
using CoreCommon.Schedule;
using CoreServer.FreeSqlService;
using CoreServer.Manager;
using Org.BouncyCastle.Bcpg.Sig;
using Serilog;
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
        public CharacterData(Vector3Int pos, Vector3Int rotation) : base(pos, rotation)
        {
        }

        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        /// 当前角色的客户端连接
        /// </summary>
        public Connection connection;

        public string playerName { get; set; }
        public int player_ID { get; set; }

        /// <summary>
        /// 当前角色对应的数据库对象
        /// </summary>
        public DBCharacterMap character { get; set; }

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveCharacterData()
        {
            Log.Information("保存信息");
        }
    }
}