using CoreCommon;
using CoreCommon.Schedule;
using CoreServer.GameNet;
using Serilog;
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
            StartEntityService();
            StartMonsterService();
            StartScheduleService();
        }

        //启动中心计时器
        private void StartScheduleService()
        {
            ScheduleManager.Instance.Start();
            Log.Information("中心计时器启动完成！！");
        }

        /// <summary>
        ///
        /// </summary>
        private void StartMonsterService()
        {
            MonsterService.Instance.StartService();
        }

        /// <summary>
        /// 开启位置同步服务
        /// </summary>
        private void StartEntityService()
        {
            EntityService entity = new EntityService();
            entity.StartService();
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