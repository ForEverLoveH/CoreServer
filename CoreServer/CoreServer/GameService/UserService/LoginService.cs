using CoreCommon;
using CoreCommon.GameLog;
using CoreCommon.NetCommon;

using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.MMOModel;
using CoreServer.MySqlService;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    public class LoginService : Singleton<LoginService>
    {
        private IFreeSql freesql = FreeSqlHelper.mysql;

        private Dictionary<Connection, List<CharacterData>> characterConnection = new Dictionary<Connection, List<CharacterData>>();

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
                    connection.Set<DBPlayer>(res);
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
            SendLoginResponseToClient(sql, connection, success);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerID"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        private List<CharacterData> SelectCurrentRoleList(int playerID, string playerName)
        {
            List<CharacterData> characters = new List<CharacterData>();
            var sl = freesql.Select<DBCharacter>().Where(a => a.PlayerID == playerID).ToList();
            if (sl != null && sl.Count > 0)
            {
            }
            return characters;
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
                Regex regex = new Regex("[0,9][11,11]");
                return regex.IsMatch(acc);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="messageData"></param>
        public void GetUserRoleList(Connection netConnection, UserRoleListRequest messageData)
        {
            if (characterConnection.ContainsKey(netConnection))
            {
                characterConnection.TryGetValue(netConnection, out List<CharacterData> characts);
                if (characts != null)
                {
                    var sl = characts.FindAll(a => a.playerName == messageData.UserName);
                    if (sl != null && sl.Count > 0)
                    {
                        SendUserRoleListResponseToClient(netConnection, sl);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="sl"></param>
        private void SendUserRoleListResponseToClient(Connection netConnection, List<CharacterData> sl)
        {
            UserRoleListResponse response = new UserRoleListResponse();
            foreach (CharacterData item in sl)
            {
                response.NEntity.Add(item.GetEntityData());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="netConnection"></param>
        /// <param name="charactID"></param>
        /// <returns></returns>
        public CharacterData GetCharacterData(Connection netConnection, int charactID)
        {
            if (characterConnection.ContainsKey(netConnection))
            {
                characterConnection.TryGetValue(netConnection, out List<CharacterData> characters);
                if (characters.Count > 0)
                {
                    CharacterData character = characters.Find(A => A.player_ID == charactID);
                    if (character != null)
                    {
                        return character;
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            else
                return null;
        }
    }
}