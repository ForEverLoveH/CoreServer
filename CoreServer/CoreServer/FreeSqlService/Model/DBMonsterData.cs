using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService.Model
{
    [Table(Name = "MonsterData")]
    public class DBMonsterData
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int MonsterID { get; set; }

        /// <summary>
        /// 怪物名
        /// </summary>
        public string MonsterName { get; set; }

        /// <summary>
        /// 怪物类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 血量
        /// </summary>
        public int HP { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 场景id
        /// </summary>
        public int SpaceID { get; set; }
    }
}