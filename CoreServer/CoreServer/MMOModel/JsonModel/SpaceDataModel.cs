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
    }
}