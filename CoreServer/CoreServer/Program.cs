// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!")
using CoreCommon.GameLog;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.GameNet;
using CoreServer.GameService;
using CoreServer.Manager;
using Serilog;
using System.Net;
using System.Net.Sockets;

//Log.Level = 1;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Async(a => a.Console()).WriteTo.Async(a => a.File("logs\\Server-log.txt", rollingInterval: RollingInterval.Day)).CreateLogger();
//加载地图json配置文件
ConfigurationDataManager.Instance.InitData();

ServiceManager.Instance.StartService();

//测试消息订阅 用户登录
//MessageRouter.Instance.OnMessage<UserLoginRequest>(LoginService.Instance.OnUserLoginRequest);
while (true)
{
    Thread.Sleep(100);
}

//Console.ReadKey();