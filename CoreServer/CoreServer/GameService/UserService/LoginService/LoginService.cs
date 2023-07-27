using CoreCommon;
using CoreCommon.GameLog;
using CoreCommon.MessageData;
using CoreCommon.NetCommon;

using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.MMOModel;
using CoreServer.MySqlService;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class LoginService : Singleton<LoginService>
    {
        private IFreeSql freesql = FreeSqlHelper.mysql;

        public void Start()
        {
            MessageRouter.Instance.OnMessage<UserLoginRequest>(StartLoginService);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="request"></param>
        public void StartLoginService(Connection connection, UserLoginRequest request)
        {
            var acc = request.Username;
            var pass = request.Password;
            string sql = "";
            bool success = false;
            DBPlayer res = null;
            if (RexGexTelPhone(acc))
            {
                res = freesql.Select<DBPlayer>().Where(a => a.TelPhone == acc && a.Password == pass).ToOne();
                if (res != null)
                {
                    sql = "登录成功";
                    success = true;
                }
                else
                {
                    sql = "登录失败！！";
                    success = false;
                }
            }
            else
            {
                res = freesql.Select<DBPlayer>().Where(a => a.UserName == acc && a.Password == pass).ToOne();
                if (res != null) { success = true; sql = "登录成功！！"; }
                else { sql = "登录失败！！"; success = false; }
            }
            if (success) connection.Set(res);
            SendLoginResponseToClient(sql, connection, success);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connection"></param>
        private void SendLoginResponseToClient(string message, Connection connection, bool success)
        {
            UserLoginResponse response = null;
            if (success)
            {
                response = new UserLoginResponse()
                {
                    Code = 200,
                    Message = message,
                };
            }
            else
            {
                response = new UserLoginResponse()
                {
                    Code = 0,
                    Message = "用户名或者密码错误！！"
                };
            }
            connection.SendDataToClient(response);
        }

        /// <summary>
        /// 正则匹配电话号码
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public bool RexGexTelPhone(string acc)
        {
            if (acc == null) return false;
            else
            {
                return Regex.IsMatch(acc, @"^(1)\d{10}$");
            }
        }
    }
}