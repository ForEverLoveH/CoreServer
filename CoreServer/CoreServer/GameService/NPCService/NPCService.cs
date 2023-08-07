using CoreCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreServer.FreeSqlService;
using CoreServer.MMOModel.JsonModel;

namespace CoreServer.GameService
{
    public class NPCService : Singleton<NPCService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;
        
        private Dictionary<int, List<NPCData>> NPCdata = new Dictionary<int, List< NPCData>>();

        /// <summary>
        ///
        /// </summary>
        public void InitNPCData(int spaceID)
        {
            var  datas=freeSql.Select<NPCData>().Where(a => a.SpceID == spaceID).ToList();
            if (datas != null && datas.Count>0)
            {
                if (!NPCdata.ContainsKey(spaceID))
                {
                    NPCdata.Add(spaceID,datas);
                }
                
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="spaceID"></param>
        /// <returns></returns>
        public List<NPCData> GetNpcDatas(int spaceID)
        {
            return NPCdata.ContainsKey(spaceID) == true ? NPCdata[spaceID] : null;
        }
        
    }
}