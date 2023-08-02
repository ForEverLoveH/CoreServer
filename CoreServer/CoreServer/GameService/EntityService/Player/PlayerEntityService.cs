using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;

using CoreServer.MMOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class PlayerEntityService : Singleton<PlayerEntityService>
    {
        /// <summary>
        ///
        /// </summary>
        public void StartService()
        {
            MessageRouter.Instance.OnMessage<EntitySyncRequest>(_EntitySyncRequest);
        }

        /// <summary>
        /// 位置移动同步请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _EntitySyncRequest(Connection netConnection, EntitySyncRequest messageData)
        {
            //获取当前角色所在的地图
            var space = netConnection.Get<CharacterData>()?.SpaceData;
            var play = netConnection.Get<CharacterData>();
            if (space == null || play == null)
                return;
            space.UpdataEntity(messageData.EnitySync, space, play, netConnection);
        }
    }
}