using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using Serilog;

namespace CoreServer.GameService
{
    public class CharacterService : Singleton<CharacterService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        public void Start()
        {
            MessageRouter.Instance.OnMessage<CharacterListRequest>(_CharacterListRequest);
            MessageRouter.Instance.OnMessage<CharacterCreateRequest>(_CharacterCreateRequest);
            MessageRouter.Instance.OnMessage<CharacterDeleteRequest>(_CharacterDeleteRequest);
        }

        /// <summary>
        /// 删除角色的请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _CharacterDeleteRequest(Connection netConnection, CharacterDeleteRequest messageData)
        {
            CharacterDeleteResponse response = new CharacterDeleteResponse();
            var play = netConnection.Get<DBPlayer>();
            if (play != null)
            {
                var res = freeSql.Delete<DBCharacter>().Where(t => t.Id == messageData.CharacterId && t.PlayerID == play.Id).ExecuteAffrows();
                if (res > 0)
                {
                    response.Success = true;
                    response.Message = "删除成功！！";
                }
                else
                {
                    response.Success = false;
                    response.Message = "删除失败！！";
                }
                netConnection.SendDataToClient(response);
            }
        }

        /// <summary>
        /// 创建角色的请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _CharacterCreateRequest(Connection netConnection, CharacterCreateRequest messageData)
        {
            Log.Information(string.Format("创建角色:{0}", messageData));

            var play = netConnection.Get<DBPlayer>();
            ChracterCreateResponse characterCreateResponse = new ChracterCreateResponse();
            if (play == null)
            {
                characterCreateResponse.Message = "未登录，无法创建角色！";
                characterCreateResponse.Success = false;
            }
            else
            {
                long count = freeSql.Select<DBCharacter>().Where(a => a.PlayerID.Equals(play.Id)).Count();
                if (count >= 4)
                {
                    characterCreateResponse.Message = "角色最多4个，无法创建";
                    characterCreateResponse.Success = false;
                }
                //判断角色名是否为空
                if (string.IsNullOrWhiteSpace(messageData.Name))
                {
                    characterCreateResponse.Message = "创建角色失败，角色名不能为空！！";
                    characterCreateResponse.Success = false;
                }
                string name = messageData.Name.Trim();
                if (name.Length > 7)
                {
                    characterCreateResponse.Message = "创建角色失败，角色名最大长度为7！！";
                    characterCreateResponse.Success = false;
                }
                if (freeSql.Select<DBCharacter>().Where(T => T.Name.Equals(name)).Count() > 0)
                {
                    characterCreateResponse.Message = "创建角色失败，角色名已存在！！";
                    characterCreateResponse.Success = false;
                }
                DBCharacter character = new DBCharacter()
                {
                    Name = name,
                    JobID = messageData.JobType,
                    Hp = 100,
                    Mp = 100,
                    Level = 0,
                    Exp = 0,
                    SpaceID = 0,
                    Gold = 0,
                    PlayerID = play.Id,
                };
                var res = freeSql.Insert(character).ExecuteAffrows();
                if (res > 0)
                {
                    characterCreateResponse.Message = "创建角色成功！！";
                    characterCreateResponse.Success = true;
                }
                netConnection.SendDataToClient(characterCreateResponse);
            }
        }

        /// <summary>
        ///查询角色列表的请求
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _CharacterListRequest(Connection netConnection, CharacterListRequest messageData)
        {
            var play = netConnection.Get<DBPlayer>();
            if (play != null)
            {
                var list = freeSql.Select<DBCharacter>().Where(a => a.PlayerID == play.Id).ToList();
                if (list.Count() > 0)
                {
                    CharacterListResponse characterListResponse = new CharacterListResponse();
                    foreach (var item in list)
                    {
                        characterListResponse.CharacterList.Add(new NCharacter()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            TypeId = item.JobID,
                            //EntityId
                            Level = item.Level,
                            Exp = item.Exp,
                            SpaceId = item.SpaceID,
                            Gold = item.Gold,
                            // Entity
                        });
                    }
                    netConnection.SendDataToClient(characterListResponse);
                }
            }
        }
    }
}