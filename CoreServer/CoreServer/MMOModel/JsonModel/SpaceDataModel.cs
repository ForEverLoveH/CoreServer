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

        public int InitPosX { get; set; }// 玩家初始位置
        public int InitPosY { get; set; }// 玩家初始位置
        public int InitPosZ { get; set; }// 玩家初始位置
        public int NPC1PosX { get; set; } // NPC1初始位置
        public int NPC1PosY { get; set; } // NPC1初始位置
        public int NPC1PosZ { get; set; } // NPC1初始位置
        public int NPC2PosX { get; set; } // NPC1初始位置
        public int NPC2PosY { get; set; } // NPC1初始位置
        public int NPC2PosZ { get; set; } // NPC1初始位置
        public int NPC3PosX { get; set; } // NPC1初始位置
        public int NPC3PosY { get; set; } // NPC1初始位置
        public int NPC3PosZ { get; set; } // NPC1初始位置
        public int Mouster1PosX { get; set; } // NPC1初始位置
        public int Mouster1PosY { get; set; } // NPC1初始位置
        public int Mouster1PosZ { get; set; } // NPC1初始位置
        public int Mouster2PosX { get; set; } // NPC1初始位置
        public int Mouster2PosY { get; set; } // NPC1初始位置
        public int Mouster2PosZ { get; set; } // NPC1初始位置
        public int Mouster3PosX { get; set; } // NPC1初始位置
        public int Mouster3PosY { get; set; } // NPC1初始位置
        public int Mouster3PosZ { get; set; } // NPC1初始位置 // 怪物3的初始位置
    }
}