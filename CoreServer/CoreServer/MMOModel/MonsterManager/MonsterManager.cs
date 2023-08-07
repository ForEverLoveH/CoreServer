using CoreCommon;
using CoreCommon.NetCommon;
using CoreCommon.Schedule;
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.Manager;
using CoreServer.MMOModel.JsonModel;
using FreeSql;
using System.Collections.Concurrent;

namespace CoreServer.MMOModel
{
    public class MonsterManager : Singleton<MonsterManager>
    {
        private IBaseRepository<DBMonsterMap> rfes = null;

        public MonsterManager()
        {
            rfes = freeSql.GetRepository<DBMonsterMap>();
            ScheduleManager.Instance.AddTask(() =>
            {
                SaveMonsterDataToDataBase();
            }, 2f);
        }

        /// <summary>
        /// 游戏全部怪物(支持线程安全)
        /// </summary>
        private ConcurrentDictionary<int, Monster> Characters = new ConcurrentDictionary<int, Monster>();

        /// <summary>
        /// 当前场景中的怪物
        /// </summary>
        private Dictionary<int, List<Monster>> monster = new Dictionary<int, List<Monster>>();

        /// <summary>
        ///
        /// </summary>
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        ///
        /// </summary>
        public void InitData(int spaceID)
        {
            List<Monster> monsters = new List<Monster>();
            var monsterData = freeSql.Select<DBMonsterData>().Where(a => a.SpaceID == spaceID).ToList();
            if (monsterData != null && monsterData.Count > 0)
            {
                foreach (var mon in monsterData)
                {
                    var monsterMap = freeSql.Select<DBMonsterMap>().Where(a => a.SpaceId == spaceID && a.Name == mon.MonsterName).ToList();
                    if (monsterMap != null && monsterMap.Count > 0)
                    {
                        foreach (var map in monsterMap)
                        {
                            Monster monster = CreateMonsterData(mon, map);
                            if (!monsters.Contains(monster))
                                monsters.Add(monster);
                            else
                            {
                                continue;
                            }
                        }
                        if (!monster.ContainsKey(spaceID))
                        {
                            monster.Add(spaceID, monsters);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mon"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private Monster CreateMonsterData(DBMonsterData mon, DBMonsterMap map)
        {
            Monster chara = new Monster(new Vector3Int(map.XPos, map.Ypos, map.Zpos), Vector3Int.one);
            chara.id = mon.ID;
            chara.Name = mon.MonsterName;
            chara.Level = mon.Level;
            chara.characterInfo.Id = mon.ID;
            chara.characterInfo.Name = mon.MonsterName;
            chara.characterInfo.Level = mon.Level;
            // chara.characterInfo.Exp = roles.Exp;
            chara.characterInfo.SpaceId = mon.SpaceID;
            chara.characterInfo.Hp = mon.HP;
            // chara.characterInfo.Mp = mon.;
            chara.character = map;
            Characters.TryAdd(mon.ID, chara);
            EntityManager.Instance.AddEntity(mon.SpaceID, chara);
            return chara;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="monsterID"></param>
        public void RemoveCharacterData(int monsterID, int spaceID)
        {
            if (Characters.ContainsKey(monsterID) && monster.ContainsKey(spaceID))
            {
                Monster character = null;
                if (Characters.TryRemove(monsterID, out character))
                {
                    monster.Remove(spaceID);
                    EntityManager.Instance.RemoveEntity(character.characterInfo.SpaceId, character);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceID"></param>
        /// <returns></returns>
        public List<Monster> GetCurrentSpaceMonsterData(int spaceID)
        {
            if (monster.ContainsKey(spaceID))
            {
                return monster[spaceID];
            }
            else
                return null;
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearAllMonster()
        {
            Characters.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveMonsterDataToDataBase()
        {
            foreach (var chara in Characters.Values)
            {
                rfes?.UpdateAsync(chara.character);
            }
        }
    }
}