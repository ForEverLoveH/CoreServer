using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;
using CoreServer.Manager;
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="roles"></param>
        public static implicit operator CharacterData(DBCharacter roles)
        {
            int entityID = EntityManager.Instance.NewEntityId;
            CharacterData chara = new CharacterData(entityID, new Vector3Int(roles.XPos, roles.YPos, roles.ZPos), Vector3Int.zero);
            chara.id = roles.Id;
            chara.Name = roles.Name;
            chara.Level = roles.Level;
            chara.characterInfo.Id = roles.Id;
            chara.characterInfo.Name = roles.Name;
            chara.characterInfo.Level = roles.Level;
            chara.characterInfo.TypeId = roles.JobID;
            chara.characterInfo.Exp = roles.Exp;
            chara.characterInfo.SpaceId = roles.SpaceID;
            chara.characterInfo.Gold = roles.Gold;
            chara.characterInfo.Hp = roles.Hp;
            chara.characterInfo.Mp = roles.Mp;
            return chara;
        }
    }
}