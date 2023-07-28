using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    public class SpaceDataModel
    {
        public int SID { get; set; }
        public string Name { get; set; } // 名称
        public string Resource { get; set; } // 资源
        public string Kind { get; set; } // 类型
        public int MinLevel { get; set; } // 要求玩家的最低等级
        public int MaxLevel { get; set; }// 最高等级
        public Vector3 InitPos { get; set; }// 玩家初始位置
        public Vector3 NPC1Pos { get; set; } // NPC1初始位置
        public Vector3 NPC2Pos { get; set; } // NPC2初始位置
        public Vector3 NPC3Pos { get; set; } // NPC3初始位置
        public Vector3 Monster1Pos { get; set; } // 怪物1的初始位置
        public Vector3 Monster2Pos { get; set; }// 怪物2的初始位置
        public Vector3 Monster3Pos { get; set; } // 怪物3的初始位置
    }
}