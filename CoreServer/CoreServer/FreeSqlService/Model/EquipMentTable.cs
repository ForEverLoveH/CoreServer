using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.FreeSqlService
{
    public class EquipMentTable
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }

        /// <summary>
        /// 玩家ID
        /// </summary>
        public int PlayerID { get; set; }

        /// <summary>
        /// 当前武器对应哪个角色的id
        /// </summary>
        public int RoleID { get; set; }

        /// <summary>
        /// 当前武器对应的ID号
        /// </summary>
        public int EquipMentID { get; set; }

        /// <summary>
        /// 武器的名字
        /// </summary>
        public string EquipMentName { get; set; }

        /// <summary>
        /// 武器类型
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 武器的等级
        /// </summary>
        public string EquipmentLevel { get; set; }

        /// <summary>
        /// 武器的描述
        /// </summary>
        public string EquipmentDescript { get; set; }

        /// <summary>
        /// 武器的物理伤害
        /// </summary>
        public string EquipmentWDamage { get; set; }

        /// <summary>
        /// 武器可造成的法术伤害
        /// </summary>
        public string EquipmentFDamage { get; set; }

        /// <summary>
        /// 武器增加的物抗
        /// </summary>
        public string EquipmentAddWDefensive { get; set; }

        /// <summary>
        /// 武器增加的法抗
        /// </summary>
        public string EquipmentAddFDefensive { get; set; }

        /// <summary>
        /// 武器增加的血量
        /// </summary>
        public string EquipmentAddHp { get; set; }

        /// <summary>
        /// 武器的售价
        /// </summary>
        public string EquipSGold { get; set; }

        /// <summary>
        /// 武器的买价
        /// </summary>
        public string EquipMGold { get; set; }
    }
}