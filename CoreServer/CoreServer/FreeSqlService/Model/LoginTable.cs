using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService
{
    public class LoginTable
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
        /// <summary>
        /// 玩家id
        /// </summary>
        public int PlayerID { get;set; }
        /// <summary>
        /// 玩家姓名
        /// </summary>
        public string PlayerName { get;set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telphone { get;set; }
    }
}
