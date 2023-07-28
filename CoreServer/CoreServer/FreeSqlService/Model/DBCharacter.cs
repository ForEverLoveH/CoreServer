using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService
{
    /// <summary>
    /// 玩家角色
    /// </summary>
    [Table(Name = "Character")]
    public class DBCharacter
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        /// <summary>
        /// 职业id
        /// </summary>
        public int JobID { get; set; }

        /// <summary>
        ///角色名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Hp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Mp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Exp { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int SpaceID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public long Gold { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int PlayerID { get; set; }
    }
}