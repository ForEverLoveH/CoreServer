using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService.Model
{
    /// <summary>
    /// 角色表
    /// </summary>
    public class RoleModelTable
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int ID { get; set; }
        /// <summary>
        /// 玩家id
        /// </summary>
        public int player_ID { get;set; }
        /// <summary>
        /// 玩家名字
        /// </summary>
        public string playerName { get;set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID { get; set; }
        /// <summary>
        /// 职业id
        /// </summary>
        public int TID { get;set; }
        /// <summary>
        /// 角色姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 角色等级
        /// </summary>
        public int roleLevel { get; set; }
        /// <summary>
        /// 角色血量
        /// </summary>
        public int roleHp { get;set; }
        /// <summary>
        /// 角色魔法值
        /// </summary>
        public int roleMp { get; set; }
        /// <summary>
        /// 角色经验值
        /// </summary>
        public int roleXp { get; set; }
        /// <summary>
        /// 所在空间id
        /// </summary>
        public int spaceId { get;set; }
        /// <summary>
        /// 位置信息
        /// </summary>
        public int Xposition {get;set;}
        public int Yposition { get;set;}
        public int Zposition { get;set;}
        public int XRoutation { get;set; }
        public int YRoutation { get; set; }
        public int ZRoutation { get; set; }

        /// <summary>
        /// 金币数
        /// </summary>
        public int goldNum { get; set; }
        /// <summary>
        /// 装备信息
        /// </summary>
        public string EquipmentData { get; set; }
                 
    }
}
