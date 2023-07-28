using CoreCommon;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;
using CoreServer.FreeSqlService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class RegisterService : Singleton<RegisterService>
    {
        private IFreeSql freeSql = FreeSqlHelper.mysql;

        public void Start()
        {
            MessageRouter.Instance.OnMessage<UserRegisterRequest>(_UserRegisterRequest);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        private void _UserRegisterRequest(Connection netConnection, UserRegisterRequest messageData)
        {
            String acc = messageData.Username;
            string pass = messageData.Password;
            string tel = messageData.Telphone;
            UserRegisterResponse response = null;
            var sl = freeSql.Select<DBPlayer>().Where(a => a.UserName == acc && a.Password == pass && a.TelPhone == tel).ToOne();
            if (sl != null)
            {
                response = new UserRegisterResponse()
                {
                    Code = -1,
                    Message = "当前玩家角色信息已存在，无法注册！"
                };
            }
            else
            {
                DBPlayer dBPlayer = new DBPlayer()
                {
                    UserName = acc,
                    Password = pass,
                    Coin = 0,
                };
                int sy = freeSql.InsertOrUpdate<DBPlayer>().SetSource(dBPlayer).IfExistsDoNothing().ExecuteAffrows();
                if (sy == 1)
                {
                    response = new UserRegisterResponse()
                    {
                        Code = 200,
                        Message = "注册成功！！"
                    };
                }
                else
                {
                    response = new UserRegisterResponse()
                    {
                        Code = 0,
                        Message = "注册失败！！",
                    };
                }
            }
            netConnection.SendDataToClient(response);
        }
    }
}