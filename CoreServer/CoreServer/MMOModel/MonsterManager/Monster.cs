﻿using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CoreServer.FreeSqlService.Model;

namespace CoreServer.MMOModel
{
    public class Monster : ActorData
    {
        public Monster(Vector3Int pos, Vector3Int rotation) : base(pos, rotation)
        {
        }


        public DBMonsterMap character { get; set; }
    }
}