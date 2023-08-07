using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class MonsterService : Singleton<MonsterService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;
        /// <summary>
        ///
        /// </summary>
        public void StartService()
        {
            MessageRouter.Instance.OnMessage<MonsterCharacterRequest>(_MonsterCharacterRequest);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _MonsterCharacterRequest(Connection netConnection, MonsterCharacterRequest messageData)
        {
            var spaceID = messageData.SpaceID;
            MonsterManager.Instance.InitData(spaceID);
            MonsterCharacterResponse monsterCharacterResponse = new MonsterCharacterResponse();
            var datas = MonsterManager.Instance.GetMonsterDatas(spaceID);
            if (datas != null && datas.Count > 0)
            {
                foreach (var data in datas)
                {
                }
            }
        }
    }
}