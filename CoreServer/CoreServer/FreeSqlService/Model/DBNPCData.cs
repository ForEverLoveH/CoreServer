using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoreServer.FreeSqlService.Model
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    [Table(Name = "NPCData")]
    public class DBNPCData
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int NPCID { get; set; }

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