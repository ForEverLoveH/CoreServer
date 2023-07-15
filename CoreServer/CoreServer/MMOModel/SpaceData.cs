using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreCommon.NetCommon;
using Serilog;

namespace CoreServer.MMOModel
{
    /// <summary>
    /// 地图(场景）
    /// </summary>
    public class SpaceData
    {
        /// <summary>
        /// 标识
        /// </summary>
        public  int ID { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; } 
        /// <summary>
        /// 描述
        /// </summary>
        public  string Description { get; set; }
        /// <summary>
        /// 背景音
        /// </summary>
        public string Music { get; set; }
        /// <summary>
        /// 角色字典(当前场景中所有角色)
        /// </summary>
        private Dictionary<int, CharacterData> CharacterDataDic = new Dictionary<int, CharacterData>();
        /// <summary>
        /// 角色连接对象
        /// </summary>
        private Dictionary<Connection ,CharacterData>CharacterConnection = new Dictionary<Connection ,CharacterData>();
        /// <summary>
        /// 角色进入场景
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="character"></param>
        public  void CharacterJoin(Connection connection,CharacterData character)
        {
            Log.Information("角色进入场景:"+character.EntityID);
            character.connection=connection;
            connection.Set<CharacterData>(character);//将角色存到连接中
            connection.Set<SpaceData>(this);//场景存到连接
            character.SpceID=this.ID;
            CharacterDataDic[character.EntityID] = character;
            if (!CharacterConnection.ContainsKey(connection))
            {
                CharacterConnection[connection] = character;
            }
            //把新进入广播给当前场景中的所有玩家
            SpaceCharacterEnterResponse response = new SpaceCharacterEnterResponse();
            response.SpaceID=this.ID;//场景id 
            response.EntityList.Add(character.GetEntityData());
            foreach(var kv in CharacterDataDic)
            {
                //发送这个上线 消息给其它人不是自己
                if(kv.Value.connection!=connection)
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
        /// 广播 更新entity 的信息
        /// </summary>
        /// <param name="enitySync"></param>
        public void UpdataEntity(NEnitySync enitySync)
        {
            Log.Information("更新{0}", enitySync);
            foreach (var kv in CharacterDataDic)
            {
                //表示自己
                if(kv.Value.EntityID==enitySync.Enity.Id)
                {
                    kv.Value.SetEntityData(enitySync.Enity);
                }
                else
                {
                    EntitySyncResponse  response = new EntitySyncResponse();
                    response.EntitySync=enitySync;
                    kv.Value.connection.SendDataToClient(response);
                }
            }
        }
        /// <summary>
        /// 角色离开地图(离线)(切换地图)
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="character"></param>
        public void  CharacterLeave(Connection connection,CharacterData character)
        {
            Log.Information("角色离开场景:" + character.EntityID);
            connection.Set<SpaceData>(null);
            CharacterDataDic.Remove(character.EntityID);
            SpaceCharacterLeaveResponse response = new SpaceCharacterLeaveResponse(); 
            response.EntityID = character.EntityID;
            foreach(var kv in CharacterDataDic)
            {
                kv.Value.connection.SendDataToClient(response);
            }
        }
    
    }
}
