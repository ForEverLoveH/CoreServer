using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel.JsonModel
{
    public class NPCData
    {
        /// <summary>
        ///
        /// </summary>
        public int SID { get; set; }// NPC编号

        /// <summary>
        ///
        /// </summary>
        public string Name { get; set; } // 名称

        /// <summary>
        ///
        /// </summary>
        public int Type { get; set; } // 类型

        /// <summary>
        ///
        /// </summary>
        public string Description { get; set; }// 描述

        /// <summary>
        ///
        /// </summary>
        public int SpceID { get; set; } // 场景

        /// <summary>
        ///
        /// </summary>
        public int XPos { get; set; } // 位置(X)

        /// <summary>
        ///
        /// </summary>
        public int YPos { get; set; } // 位置(Y)

        /// <summary>
        ///
        /// </summary>
        public int ZPos { get; set; } // 位置(Z)

        /// <summary>
        ///
        /// </summary>
        public int XRoutation { get; set; }// 旋转(X)

        /// <summary>
        ///
        /// </summary>
        public int YRoutation { get; set; } // 旋转(Y)

        /// <summary>
        ///
        /// </summary>
        public int ZRoutation { get; set; } // 旋转(Z)
    }
}