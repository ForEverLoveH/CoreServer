using CoreCommon;
using CoreServer.Manager;
using CoreServer.MMOModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    /// <summary>
    /// 地图管理器
    /// </summary>
    public class GameMapManager : Singleton<GameMapManager>
    {
        /// <summary>
        /// 地图字典
        /// </summary>

        private Dictionary<int, SpaceData> mapDic = new Dictionary<int, SpaceData>();

        public void InitMapData()
        {
            foreach (var space in ConfigurationDataManager.Instance.SpaceDic)
            {
                mapDic[space.Key] = new SpaceData(space.Value);
                Log.Information("初始化地图:" + space.Value.Name);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        public SpaceData GetSpaceData(int spaceId)
        {
            return mapDic[spaceId];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceData"></param>
        public void SetSpace(SpaceData spaceData)
        {
            if (spaceData == null)
            {
                return;
            }
            else
            {
                if (!mapDic.ContainsValue(spaceData) && !mapDic.ContainsKey(spaceData.ID))
                {
                    mapDic.Add(spaceData.ID, spaceData);
                }
            }
        }
    }
}