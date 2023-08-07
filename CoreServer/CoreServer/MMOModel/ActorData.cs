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
        public ActorData(Vector3Int pos, Vector3Int rotation) : base(pos, rotation)
        {
        }

        public SpaceData SpaceData { get; set; }
        public int id { get; set; }
        public NCharacter characterInfo { get; set; } = new NCharacter();
    }
}