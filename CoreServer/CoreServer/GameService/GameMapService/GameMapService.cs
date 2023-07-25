using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.MMOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    /// <summary>
    /// 地图服务(移动位置同步)
    /// </summary>
    public class GameMapService : Singleton<GameMapService>
    {
        /// <summary>
        /// 地图字典
        /// </summary>
        private Dictionary<int, SpaceData> spaceDict = new Dictionary<int, SpaceData>();

        public void Start()
        {
            MessageRouter.Instance.OnMessage<EntitySyncRequest>(_EntitySyncRequest);
            MessageRouter.Instance.OnMessage<SpaceCharacterLeaveRequest>(_SpaceCharacterLeaveRequest);
            SpaceData space = new SpaceData();
            space.Name = "新手村";
            space.ID = 6;
            spaceDict[space.ID] = space;
        }

        /// <summary>
        /// 角色离开地图的请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _SpaceCharacterLeaveRequest(Connection netConnection, SpaceCharacterLeaveRequest messageData)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spaceId"></param>
        /// <returns></returns>
        public SpaceData GetSpace(int spaceId)
        {
            return spaceDict[spaceId];
        }

        /// <summary>
        /// 位置移动同步请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _EntitySyncRequest(Connection netConnection, EntitySyncRequest messageData)
        {
            //获取当前角色所在的地图
            var space = netConnection.Get<SpaceData>();
            if (space == null)
                return;
            space.UpdataEntity(messageData.EnitySync);
        }
    }
}