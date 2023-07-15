using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.Manager;
using CoreServer.MMOModel;
using CoreServer.MySqlService;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService.UserService
{
    /// <summary>
    /// 玩家服务类 
    /// 注册 登录 创建角色 进入游戏等
    /// </summary>
    public  class UserService:Singleton<UserService>
    {
        public void Start()
        {
            MessageRouter.Instance.OnMessage<GameEnterRequest>(_GameEnterRequest);
            MessageRouter.Instance.OnMessage<UserLoginRequest>(_UserLoginGame);
            MessageRouter.Instance.OnMessage<UserRoleListRequest>(_UserRoleList);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _UserRoleList(Connection netConnection, UserRoleListRequest messageData)
        {
             LoginService.Instance.GetUserRoleList(netConnection, messageData);
        }

        /// <summary>
        /// 玩家登录方法
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _UserLoginGame(Connection netConnection, UserLoginRequest messageData)
        {
            LoginService.Instance.StartLoginService(netConnection, messageData);    
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _GameEnterRequest(Connection netConnection, GameEnterRequest messageData)
        {
            //查找数据库根角色id  读取玩家的位置信息
            var msgID = messageData.CharacterID;

            int entityID = EntityManager.Instance.NewEntityId;
            Log.Information("有玩家进入游戏！！");
           // Vector3Int pos = new Vector3Int(500,0,500);
            Random random = new Random();
            Vector3Int pos = new Vector3Int(31 + random.Next(-1, 1), 22 + random.Next(-1, 1), 54 + random.Next((int)-1, 1));
            pos *= 1000;
            
            //分配角色
             CharacterData characterData = new CharacterData(entityID,pos,Vector3Int.zero);
            ///通知登录成功
            GameEnterResponse response = new GameEnterResponse()
            {
                Enity = characterData.GetEntityData(),
                Success = true,
            };
            netConnection.SendDataToClient(response);
            var space = GameMapService.Instance.GetSpace(6);//新手村6 
            space.CharacterJoin(netConnection, characterData);

        
        }
    }
}
