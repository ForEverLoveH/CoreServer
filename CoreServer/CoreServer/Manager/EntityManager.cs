using CoreCommon;
using CoreCommon.NetCommon;
using CoreServer.MMOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.Manager
{
    /// <summary>
    /// Entity管理器(角色 ，怪物 npc， 陷阱)
    /// </summary>
    public class EntityManager : Singleton<EntityManager>
    {
        private int index = 1;
        private Dictionary<int, Entity> AllEntities = new Dictionary<int, Entity>();

        /// <summary>
        /// 场景中的entity 列表
        /// </summary>
        private Dictionary<int, List<Entity>> spaceEntities = new Dictionary<int, List<Entity>>();

        /// <summary>
        ///
        /// </summary>
        public int NewEntityId
        {
            get
            {
                lock (this)
                {
                    return index++;
                }
            }
        }

        /// <summary>
        /// 统一管理的对象分配id
        /// </summary>
        /// <param name="spaceID"></param>
        /// <param name="entity"></param>
        public void AddEntity(int spaceID, Entity entity)
        {
            lock (this)
            {
                if (!AllEntities.ContainsKey(spaceID))
                {
                    entity.EntityData.Id = NewEntityId;
                    AllEntities[entity.EntityID] = entity;
                    if (!spaceEntities.ContainsKey(spaceID))
                    {
                        spaceEntities[spaceID] = new List<Entity>();
                    }
                    spaceEntities[spaceID].Add(entity);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceID"></param>
        /// <param name="entity"></param>
        public void RemoveEntity(int spaceID, Entity entity)
        {
            lock (this)
            {
                spaceEntities[spaceID].Remove(entity);
                AllEntities.Remove(entity.EntityID);
            }
        }

        public Entity GetEntity(int entityID)
        {
            return AllEntities.GetValueOrDefault(entityID, null);
        }
    }
}