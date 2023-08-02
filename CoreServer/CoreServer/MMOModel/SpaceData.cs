using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;
using Serilog;

namespace CoreServer.MMOModel
{
    /// <summary>
    /// 地图(场景）
    /// </summary>
    public class SpaceData
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;
        private SpaceDataModel SpaceDataMode { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 背景音
        /// </summary>
        public string Music { get; set; }

        public SpaceData()
        { }

        public SpaceData(SpaceDataModel data)
        {
            this.SpaceDataMode = data;
            this.ID = data.SID;
            this.Name = data.Name;
        }

        /// <summary>
        /// 角色字典(当前场景中所有角色)(角色id0
        /// </summary>
        private Dictionary<int, CharacterData> CharacterDataDic = new Dictionary<int, CharacterData>();

        /// <summary>
        /// 角色连接对象
        /// </summary>
        private Dictionary<Connection, CharacterData> CharacterConnection = new Dictionary<Connection, CharacterData>();

        /// <summary>
        /// 角色进入场景
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="character"></param>
        public void CharacterJoin(Connection connection, CharacterData character)
        {
            Log.Information("角色进入场景:" + character.id);
            character.connection = connection;

            character.SpaceData = this;
            CharacterDataDic[character.id] = character;
            if (!CharacterConnection.ContainsKey(connection))
            {
                CharacterConnection[connection] = character;
            }
            //把新进入广播给当前场景中的所有玩家
            SpaceCharactersEnterResponse response = new SpaceCharactersEnterResponse();
            response.SpaceId = this.ID;//场景id
            response.EntityList.Add(character.GetEntityData());
            foreach (var kv in CharacterDataDic)
            {
                //发送这个上线 消息给其它人不是自己
                if (kv.Value.connection != connection)
                {
                    kv.Value.connection.SendDataToClient(response);
                }
            }
            //新上线的角色需要获取当前地图的所有的角色信息
            foreach (var kv in CharacterDataDic)
            {
                if (kv.Value.connection == connection) continue;
                response.EntityList.Clear();
                response.EntityList.Add(kv.Value.GetEntityData());
                connection.SendDataToClient(response);
            }
        }

        /// <summary>
        ///  更新entity 的信息
        /// </summary>
        /// <param name="enitySync"></param>
        public void UpdataEntity(NEntitySync enitySync, SpaceData space, CharacterData player, Connection netConnection)
        {
            EntitySyncResponse response = new EntitySyncResponse();
            Log.Information("更新{0}", enitySync);
            Log.Information(CharacterDataDic.Count.ToString());
            if (CharacterDataDic.Count > 0)
            {
                foreach (var kv in CharacterDataDic)
                {
                    //表示自己
                    if (kv.Value.EntityID == enitySync.Entity.Id)
                    {
                        kv.Value.SetEntityData(enitySync.Entity);
                        int playerID = player.player_ID;
                        int jobID = player.characterInfo.TypeId;
                        int spaceID = space.ID;
                        DBCharacterMap map = freeSql.Select<DBCharacterMap>().Where(a => a.SpaceId == spaceID && a.JobID == jobID).ToOne();
                        if (map != null)
                        {
                            var res = freeSql.Update<DBCharacterMap>().Set(a => a.XPos == enitySync.Entity.Position.X && a.Ypos == enitySync.Entity.Position.Y && a.Zpos == enitySync.Entity.Position.Z &&
                            a.XRoutation == enitySync.Entity.Direction.X && a.YRoutation == enitySync.Entity.Direction.Y && a.ZRoutation == enitySync.Entity.Direction.Z).Where(a => a.Id == map.Id).ExecuteAffrows();
                            if (res > 0)
                            {
                                map = freeSql.Select<DBCharacterMap>().Where(a => a.SpaceId == spaceID && a.JobID == jobID).ToOne();
                                NVector3 pos = new NVector3()
                                {
                                    X = map.XPos,
                                    Y = map.Ypos,
                                    Z = map.Zpos
                                };
                                NVector3 dir = new NVector3()
                                {
                                    X = map.XRoutation,
                                    Y = map.YRoutation,
                                    Z = map.ZRoutation
                                };
                                NEntity nEntity = new NEntity()
                                {
                                    Id = enitySync.Entity.Id,
                                    Position = pos,
                                    Direction = dir,
                                };
                                response.EntityList.Add(nEntity);
                            }
                        }
                    }
                    else
                    {
                        response.EntityList.Add(new NEntity()
                        {
                            Direction = enitySync.Entity.Direction,
                            Position = enitySync.Entity.Position,
                            Id = enitySync.Entity.Id,
                        });
                    }
                    netConnection.SendDataToClient(response);
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 角色离开地图(离线)(切换地图)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="character"></param>
        public void CharacterLeave(Connection connection, CharacterData character)
        {
            Log.Information("角色离开场景:" + character.EntityID);
            connection.Set<SpaceData>(null);
            CharacterDataDic.Remove(character.id);
            SpaceCharacterLeaveResponse response = new SpaceCharacterLeaveResponse();
            response.EntityId = character.EntityID;
            foreach (var kv in CharacterDataDic)
            {
                kv.Value.connection.SendDataToClient(response);
            }
        }
    }
}