using CoreCommon;
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.MMOModel;
using CoreServer.MMOModel.JsonModel;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.Manager
{
    /// <summary>
    /// 地图 json信息
    /// </summary>
    public class ConfigurationDataManager : Singleton<ConfigurationDataManager>
    {
        /// <summary>
        ///
        /// </summary>
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        ///
        /// </summary>
        public Dictionary<int, SpaceDataModel> SpaceDic = new Dictionary<int, SpaceDataModel>();

        /// <summary>
        ///
        /// </summary>
        private Dictionary<int, MonsterData> monsters = new Dictionary<int, MonsterData>();

        private Dictionary<int, NPCData> NPCDatas = new Dictionary<int, NPCData>();

        /// <summary>
        ///
        /// </summary>
        public void InitData()
        {
            SpaceDic = JsonDataManager.Instance.LoadingCurrentJsonData<SpaceDataModel>(@"JsonData/SpaceDefine.json");
            monsters = JsonDataManager.Instance.LoadingCurrentJsonData<MonsterData>(@"JsonData/MonsterData.json");
            NPCDatas = JsonDataManager.Instance.LoadingCurrentJsonData<NPCData>($"JsonData/NPCData.json");
            if (monsters != null && monsters.Count > 0)
                SaveMonsterDataToDataBase(monsters);
            if (NPCDatas != null && NPCDatas.Count > 0)
                SaveNPCDataToDataBase(NPCDatas);
        }

        /// <summary>
        /// 保存NPC信息
        /// </summary>
        /// <param name="nPCData"></param>
        private void SaveNPCDataToDataBase(Dictionary<int, NPCData> nPCData)
        {
            foreach (var npc in nPCData.Values)
            {
                if (npc != null)
                {
                    DBNPCData datas = new DBNPCData()
                    {
                        NPCID = npc.SID,
                        SpceID = npc.SpceID,
                        Type = npc.Type,
                        Name = npc.Name,
                        Description = npc.Description,
                        XPos = npc.XPos,
                        YPos = npc.YPos,
                        ZPos = npc.ZPos,
                        XRoutation = npc.XRoutation,
                        YRoutation = npc.YRoutation,
                        ZRoutation = npc.ZRoutation,
                    };
                    var sl = freeSql.Select<DBNPCData>().Where(x => x.SpceID == datas.SpceID && x.NPCID == datas.NPCID && x.Name == datas.Name).ToOne();
                    if (sl == null)
                        freeSql.InsertOrUpdate<DBNPCData>().SetSource(datas).IfExistsDoNothing().ExecuteAffrows();
                    else
                        continue;
                }
            }
        }

        /// <summary>
        ///保存怪物信息
        /// </summary>
        /// <param name="monsters"></param>
        private void SaveMonsterDataToDataBase(Dictionary<int, MonsterData> monsters)
        {
            foreach (var monster in monsters.Values)
            {
                if (monster != null)
                {
                    DBMonsterData dBMonsterData = new DBMonsterData()
                    {
                        MonsterID = monster.SID,
                        MonsterName = monster.Name,
                        HP = monster.HP,
                        Level = monster.Level,
                        SpaceID = monster.SpceID,
                        Type = monster.Type,
                    };
                    DBMonsterMap dBMonsterMap = new DBMonsterMap()
                    {
                        SpaceId = monster.SpceID,
                        Name = monster.Name,
                        XPos = monster.XPos,
                        Ypos = monster.YPos,
                        Zpos = monster.ZPos,
                        XRoutation = monster.ZRoutation,
                        YRoutation = monster.ZRoutation,
                        ZRoutation = monster.ZRoutation,
                    };
                    var sl = freeSql.Select<DBMonsterData>().Where(x => x.SpaceID == dBMonsterData.SpaceID && x.MonsterID == dBMonsterData.MonsterID && x.MonsterName == dBMonsterData.MonsterName).ToOne();
                    if (sl == null)
                    {
                        freeSql.InsertOrUpdate<DBMonsterData>().SetSource(dBMonsterData).IfExistsDoNothing().ExecuteAffrows();
                        var pl = freeSql.Select<DBMonsterMap>().Where(x => x.SpaceId == dBMonsterMap.SpaceId && x.Name == dBMonsterMap.Name).ToOne();
                        if (pl == null)
                        {
                            freeSql.InsertOrUpdate<DBMonsterMap>().SetSource(dBMonsterMap).IfExistsDoNothing().ExecuteAffrows();
                        }
                    }
                    else
                        continue;
                }
            }
        }
    }
}