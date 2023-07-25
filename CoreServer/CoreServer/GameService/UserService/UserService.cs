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

namespace CoreServer.GameService
{
    /// <summary>
    /// 玩家服务类
    /// 注册 登录 创建角色 进入游戏等
    /// </summary>
    public class UserService : Singleton<UserService>
    {
        public void Start()
        {
            LoginService.Instance.Start();
            RegisterService.Instance.Start();
            CharacterService.Instance.Start();
            EnterGameService.Instance.Start();
        }
    }
}