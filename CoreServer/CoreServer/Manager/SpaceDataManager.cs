using CoreCommon;
using CoreServer.MMOModel;
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
    public class SpaceDataManager : Singleton<SpaceDataManager>
    {
        public Dictionary<int, SpaceDataModel> SpaceDic = new Dictionary<int, SpaceDataModel>();

        /// <summary>
        ///
        /// </summary>
        public void InitData()
        {
            SpaceDic = JsonDataManager.Instance.LoadingCurrentSpaceData<SpaceDataModel>(@"JsonData/SpaceDefine.json");
        }
    }
}