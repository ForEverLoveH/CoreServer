// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!")
using CoreCommon.GameLog;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.GameNet;
using CoreServer.GameService;

using Serilog;
using System.Net;
using System.Net.Sockets;

//Log.Level = 1;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Async(a => a.Console()).WriteTo.Async(a => a.File("logs\\Server-log.txt", rollingInterval: RollingInterval.Day)).CreateLogger();
ServiceManager.Instance.StartService();
Log.Information("位置同步服务启动完成");

//测试消息订阅 用户登录
//MessageRouter.Instance.OnMessage<UserLoginRequest>(LoginService.Instance.OnUserLoginRequest);
while (true)
{
    Thread.Sleep(100);
}

//Console.ReadKey();