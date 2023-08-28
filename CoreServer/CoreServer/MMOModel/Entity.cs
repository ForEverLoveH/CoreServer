using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    /// <summary>
    ///mmo地图中的实体类
    /// </summary>
    public class Entity
    {
        private string name { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int level { get; set; }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
            }
        }

        private NEntity Nentity;

        private int hp { get; set; }
        public int HP
        { get { return hp; } set { hp = value; } }

        private int mp { get; set; }
        public int MP
        { get { return mp; } set { mp = value; } }

        /// <summary>
        /// 唯一编号
        /// </summary>
        public int EntityID
        {
            get { return Nentity.Id; }
        }

        /// <summary>
        ///
        /// </summary>
        private Vector3Int position;

        /// <summary>
        ///
        /// </summary>
        public Vector3Int Position
        {
            get { return position; }
            set
            {
                position = value;
                Nentity.Position = value;
            }
        }

        private Vector3Int rotation;

        public Vector3Int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                Nentity.Direction = value;
            }
        }

        public Entity(Vector3Int pos, Vector3Int rotation)
        {
            Nentity = new NEntity();
            Position = pos;
            Rotation = rotation;
        }

        /// <summary>
        ///
        /// </summary>
        public NEntity EntityData
        {
            get
            {
                return Nentity;
            }
            set
            {
                Nentity = value;
                position = Nentity.Position;
                rotation = Nentity.Direction;
            }
        }
    }
}