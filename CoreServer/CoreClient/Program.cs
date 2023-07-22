// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;
using CoreClient.GameSystem;
using Google.Protobuf;
using CoreCommon.NetCommon;
using CoreCommon.GameLog;
using Serilog;
using CoreCommon.Proto;

Console.WriteLine("Hello, World!");

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs\\Client-log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
string host = "127.0.0.1";
int port = 9666;
IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
socket.Connect(iPEndPoint);
Log.Debug("服务器连接成功！！");
Thread.Sleep(1000);
Connection connection = new Connection(socket);

var msg = new UserLoginRequest();
msg.Username = "123";
msg.Password = "123";
connection.SendDataToClient(msg);
Console.ReadKey();