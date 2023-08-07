using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel.JsonModel
{
    public class MonsterData
    {
        public int SID { get; set; }// 怪物编号
        public string Name { get; set; }// 名称
        public int Type { get; set; } // 类型
        public int HP { get; set; }// 血量
        public int Level { get; set; }// 等级
        public int SpceID { get; set; } // 场景
        public int XPos { get; set; } // 位置(X)
        public int YPos { get; set; } // 位置(Y)
        public int ZPos { get; set; } // 位置(Z)
        public int XRoutation { get; set; }// 旋转(X)
        public int YRoutation { get; set; }// 旋转(Y)
        public int ZRoutation { get; set; } // 旋转(Z)
    }
}