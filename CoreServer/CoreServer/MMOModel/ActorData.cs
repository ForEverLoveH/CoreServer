using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{

    public class ActorData : Entity
    {
        public ActorData(int entityID, Vector3Int pos, Vector3Int rotation) : base(entityID, pos, rotation)
        {
        }
        public ActorData(int entityID, string name, int level, int hp, int mp, Vector3Int pos, Vector3Int rotation) : base(entityID, name, level, hp, mp, pos, rotation)
        {
        }
        private  int attrack { get;set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attrack { get { return attrack; } set { attrack = value; } }
        private int defense { get;set; }
        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense { get { return defense; } set { defense = value; } }

    }
}
