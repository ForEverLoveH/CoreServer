using CoreCommon;
using CoreServer.GameNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class ServiceManager : Singleton<ServiceManager>
    {
        public void StartService()
        {
            StartNetService();
            StartUserService();
            StartGameMapService();
        }

        /// <summary>
        /// 开启地图服务
        /// </summary>
        private void StartGameMapService()
        {
            GameMapService gameMapService = GameMapService.Instance;
            gameMapService.Start();
        }

        /// <summary>
        /// 开启用户服务
        /// </summary>
        private void StartUserService()
        {
            UserService userService = UserService.Instance;
            userService.Start();
        }

        /// <summary>
        /// 开启网络服务
        /// </summary>
        private void StartNetService()
        {
            NetService netService = new NetService("0.0.0.0", 9666);
            netService.StartService();
        }
    }
}