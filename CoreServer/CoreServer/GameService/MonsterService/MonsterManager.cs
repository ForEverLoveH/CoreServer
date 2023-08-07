using CoreCommon;
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class MonsterManager : Singleton<MonsterManager>
    {
        private Dictionary<int, List<DBMonsterData>> monsterData = new Dictionary<int, List<DBMonsterData>>();
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        ///
        /// </summary>
        public void InitData(int spaceID)
        {
            
            var monster = freeSql.Select<DBMonsterData>().Where(a => a.SpceID ==  spaceID).ToList();
            if (monster != null && monster.Count > 0)
            {
                if (!monsterData.ContainsKey(spaceID))
                {
                    monsterData.Add(spaceID,monster);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceID"></param>
        /// <returns></returns>
        public List<DBMonsterData> GetMonsterDatas(int spaceID)
        {
            return monsterData.ContainsKey(spaceID) == true ? monsterData[spaceID] : null;
        }
    }
}