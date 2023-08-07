using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService.Model
{
    [Table(Name = "MonsterMap")]
    public class DBMonsterMap
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        /// <summary>
        /// 地图id
        /// </summary>

        public int SpaceId { get; set; }

        /// <summary>
        /// monster名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 在地图中的初始位置(X)
        /// </summary>
        public int XPos { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Ypos { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Zpos { get; set; }

        public int XRoutation { get; set; }
        public int YRoutation { get; set; }
        public int ZRoutation { get; set; }
    }
}