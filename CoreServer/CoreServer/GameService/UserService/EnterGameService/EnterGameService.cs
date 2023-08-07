using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;

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
    public class EnterGameService : Singleton<EnterGameService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        public void Start()
        {
            MessageRouter.Instance.OnMessage<GameEnterRequest>(_GameEnterRequest);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _GameEnterRequest(Connection netConnection, GameEnterRequest messageData)
        {
            //查找数据库根角色id  读取玩家的位置信息
            var msgID = messageData.CharacterId;

            Log.Information("有玩家进入游戏！！" + messageData.CharacterId);
            var play = netConnection.Get<DBPlayer>();
            var roles = freeSql.Select<DBCharacter>().Where(a => a.PlayerID == play.Id && a.Id == messageData.CharacterId).First();
            DBCharacterMap map = GameMapService.Instance.SettingCurrentChooseMapData(roles, msgID);
            CharacterData chara = null;
            if (map != null)
                chara = CharacterManager.Instance.CreateCharacterData(roles, map);
            SpaceData spaceData = new SpaceData()
            {
                Name = map.Name,
                ID = map.SpaceId,
            };
            chara.SpaceData = spaceData;
            netConnection.Set<CharacterData>(chara);
            GameEnterResponse response = new GameEnterResponse();
            response.Success = true;
            response.Entity = chara.EntityData;
            response.Character = chara.characterInfo;
            netConnection.SendDataToClient(response);
        }
    }
}