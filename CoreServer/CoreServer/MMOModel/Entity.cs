using CoreCommon.NetCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MMOModel
{
    /// <summary>
    ///mmo地图中的实体类
    /// </summary>
    public class Entity
    {
        /*/// <summary>
        /// 唯一编号
        /// </summary>
        private int _entityId { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        private Vector3Int position {  get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        private Vector3Int rotation { get; set; }

        public int ID
        {
            get { return _entityId; }
        }

        public   Vector3Int     Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector3Int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }*/

        public NEntity GetEntityData()
        {
            var data = new NEntity();
            data.Id = this.EntityID;
            data.Position = new NVector3()
            {
                X = position.x,
                Y = position.y,
                Z = position.z
            };
            data.Direction = new NVector3
            {
                X = 0,
                Y = 0,
                Z = 0
            };
            return data;
        }

        private int spaceId;

        public int SpceID
        {
            get { return this.spaceId; }
            set
            {
                this.spaceId = value;
            }
        }

        private int id { get; set; }

        /// <summary>
        /// 唯一编号
        /// </summary>
        public int EntityID
        {
            get { return id; }
        }

        private string name { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private int level { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int EntityLevel
        {
            get { return level; }
            set { level = value; }
        }

        private int hp { get; set; }

        /// <summary>
        /// 血量
        /// </summary>
        public int HP
        {
            get
            {
                return hp;
            }
            set { hp = value; }
        }

        private int mp { get; set; }

        /// <summary>
        /// 魔量
        /// </summary>
        public int MP
        {
            get { return mp; }
            set { mp = value; }
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
            set { position = value; }
        }

        private Vector3Int rotation;

        public Vector3Int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Entity(int entityID, Vector3Int pos, Vector3Int rotation)
        {
            this.id = entityID;
            this.position = pos;
            this.Rotation = rotation;
        }

        public Entity(int entityID, string name, int level, int hp, int mp, Vector3Int pos, Vector3Int rotation)
        {
            this.id = entityID;
            this.name = name;
            this.level = level;
            this.hp = hp;
            this.mp = mp;
            this.position = new Vector3Int(pos.x, pos.y, pos.z);
            this.rotation = new Vector3Int(rotation.x, rotation.y, rotation.z);
        }

        public NEntity GetData()
        {
            var data = new NEntity();
            data.Id = this.id;
            data.Position = new NVector3
            {
                X = this.position.x,
                Y = this.position.y,
                Z = this.position.z
            };
            data.Direction = new NVector3
            {
                X = this.rotation.x,
                Y = this.rotation.y,
                Z = this.rotation.z
            };
            return data;
        }

        public void SetEntityData(NEntity enity)
        {
            position.x = enity.Position.X;
            position.y = enity.Position.Y;
            position.z = enity.Position.Z;
            rotation.x = enity.Direction.X;
            rotation.y = enity.Direction.Y;
            rotation.z = enity.Direction.Z;
        }
    }
}