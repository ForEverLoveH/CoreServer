﻿using CoreCommon;
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
        ///获取场景中的怪物
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _MonsterCharacterRequest(Connection netConnection, MonsterCharacterRequest messageData)
        {
            var spaceID = messageData.SpaceID;
            MonsterManager.Instance.InitData(spaceID);
            MonsterCharacterResponse monsterCharacterResponse = new MonsterCharacterResponse();
            List<Monster> monsters = MonsterManager.Instance.GetCurrentSpaceMonsterData(spaceID);
            foreach (Monster monster in monsters)
            {
                monsterCharacterResponse.MonsterList.Add(new NCharacter()
                {
                    Level = monster.Level,
                    Name = monster.Name,
                    Entity = monster.EntityData,
                    SpaceId = spaceID,
                    EntityId = monster.EntityID,
                    Hp = monster.HP,
                    Id = monster.id,
                });
            }
            netConnection.SendDataToClient(monsterCharacterResponse);
        }
    }
}