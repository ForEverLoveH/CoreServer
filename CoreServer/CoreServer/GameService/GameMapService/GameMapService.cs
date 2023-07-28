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
    /// <summary>
    /// 地图服务(移动位置同步)
    /// </summary>
    public class GameMapService : Singleton<GameMapService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        /// <summary>
        /// 地图字典
        /// </summary>
        private Dictionary<int, SpaceData> spaceDict = new Dictionary<int, SpaceData>();

        public void Start()
        {
            MessageRouter.Instance.OnMessage<SpaceCharactersEnterRequest>(_SpaceCharactersEnterRequest);
            MessageRouter.Instance.OnMessage<SpaceCharacterLeaveRequest>(_SpaceCharacterLeaveRequest);
            SpaceData space = new SpaceData();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _SpaceCharactersEnterRequest(Connection netConnection, SpaceCharactersEnterRequest messageData)
        {
            int spaceID = messageData.SpaceID;
            var spacedata = netConnection.Get<SpaceData>();
            var charaData = netConnection.Get<CharacterData>();
            if (spacedata != null)
            {
                spacedata.CharacterJoin(netConnection, charaData);
            }
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

        public void SetSpace(SpaceData spaceData)
        {
            if (!spaceDict.ContainsKey(spaceData.ID))
                spaceDict.Add(spaceData.ID, spaceData);
        }

        /// <summary>
        /// 设置当前选择的地图信息
        /// </summary>
        /// <param name="roles"></param>
        public DBCharacterMap SettingCurrentChooseMapData(DBCharacter roles, int msgID)
        {
            DBCharacterMap DBCharacterMap = new DBCharacterMap();
            DBCharacterMap = freeSql.Select<DBCharacterMap>().Where(a => a.SpaceId == roles.SpaceID && a.JobID == roles.JobID).ToOne();
            if (DBCharacterMap == null)
            {
                if (roles.Level == 0)
                {
                    if (roles.SpaceID != 0)
                    {
                        roles.SpaceID = 0;
                    }
                    Vector3Int vector3Int1 = Vector3Int.one;
                    Vector3Int vector3Int2 = Vector3Int.one;
                    DBCharacterMap = new DBCharacterMap()
                    {
                        JobID = roles.JobID,

                        Name = "新手村",
                        SpaceId = roles.SpaceID,
                        XPos = vector3Int1.x * 1000,
                        Ypos = vector3Int1.y * 1000,
                        Zpos = vector3Int1.z * 1000,
                        XRoutation = vector3Int2.x * 1000,
                        YRoutation = vector3Int2.y * 1000,
                        ZRoutation = vector3Int2.z * 1000,
                    };
                    freeSql.InsertOrUpdate<DBCharacterMap>().SetSource(DBCharacterMap).IfExistsDoNothing().ExecuteAffrows();
                }
            }
            return DBCharacterMap;
        }
    }
}