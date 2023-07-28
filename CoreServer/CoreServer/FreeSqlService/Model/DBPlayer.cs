using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace CoreServer.FreeSqlService
{
    [Table(Name = "Player")]
    public class DBPlayer
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>

        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>

        public string TelPhone { get; set; }

        /// <summary>
        /// 金币数目
        /// </summary>
        public int Coin { get; set; }
    }
}