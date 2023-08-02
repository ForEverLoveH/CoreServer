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
                chara = SetCharacterData(roles, map);
            SpaceData spaceData = new SpaceData()
            {
                Name = map.Name,
                ID = map.SpaceId,
            };

            chara.SpaceData = spaceData;
            netConnection.Set<CharacterData>(chara);
            GameMapManager.Instance.SetSpace(spaceData);
            GameEnterResponse response = new GameEnterResponse();
            response.Success = true;
            response.Entity = chara.GetEntityData();
            response.Character = chara.characterInfo;
            netConnection.SendDataToClient(response);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="roles"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        private CharacterData SetCharacterData(DBCharacter roles, DBCharacterMap map)
        {
            int entityID = EntityManager.Instance.NewEntityId;
            CharacterData chara = new CharacterData(entityID, new Vector3Int(map.XPos, map.Ypos, map.Zpos), Vector3Int.one);
            chara.id = roles.Id;
            chara.Name = roles.Name;
            chara.Level = roles.Level;
            chara.player_ID = roles.PlayerID;
            chara.characterInfo.Id = roles.Id;
            chara.characterInfo.Name = roles.Name;
            chara.characterInfo.Level = roles.Level;
            chara.characterInfo.TypeId = roles.JobID;
            chara.characterInfo.Exp = roles.Exp;
            chara.characterInfo.SpaceId = roles.SpaceID;
            chara.characterInfo.Gold = roles.Gold;
            chara.characterInfo.Hp = roles.Hp;
            chara.characterInfo.Mp = roles.Mp;
            return chara;
        }
    }
}