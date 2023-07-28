using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    public class Monster : ActorData
    {
        public Monster(int entityID, Vector3Int pos, Vector3Int rotation) : base(entityID, pos, rotation)
        {
        }
    }
}