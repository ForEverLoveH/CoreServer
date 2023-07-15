using CoreCommon;
using CoreCommon.GameLog;
using CoreCommon.NetCommon;
 
using CoreServer.FreeSqlService;
using CoreServer.FreeSqlService.Model;
using CoreServer.MMOModel;
using CoreServer.MySqlService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreServer.GameService.UserService
{
    public class LoginService:Singleton<LoginService>
    {
        IFreeSql freesql = FreeSqlHelper.mysql;
        
        Dictionary<Connection ,List<CharacterData>>characterConnection = new Dictionary<Connection ,List<CharacterData>>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="request"></param>
        public void StartLoginService(Connection connection ,UserLoginRequest request)
        {
            var acc = request.Username;
            var pass= request.Password;
            string sql = "";
            bool success=false;
            LoginTable res = null;
            if (RexGexTelPhone(acc))
            {
                 res =freesql.Select<LoginTable>().Where(a=>a.Telphone==acc&& a.Password ==pass).ToOne();
                if(res!=null)
                {
                    sql = "登录成功";
                    success=true;  
                }
                else
                    sql = "登录失败！！";
            }
            else
            {
                 res=freesql.Select<LoginTable>().Where(a=>a.Account==acc &&a.Password==pass).ToOne();
                if (res != null) { success = true; sql = "登录成功！！"; }
                else sql = "登录失败！！";
            }
            if (success)
            {
                var po = SelectCurrentRoleList(res.PlayerID, res.PlayerName);
                if (po.Count == 0)
                {
                    //当前没有角色
                }
                if (po.Count > 0)
                    characterConnection.Add(connection, po);
            }
            SendLoginResponseToClient(sql,connection );
            
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
            var sl= freesql.Select<RoleModelTable>().Where(a => a.playerName == playerName && a.player_ID == playerID).ToList();
            if(sl!=null&&sl.Count>0)
            {
                foreach (RoleModelTable role in sl) 
                {
                    CharacterData characterData = new CharacterData(role.player_ID,role.Name,role.roleLevel,role.roleHp,role.roleMp,new Vector3Int(role.Xposition,role.Yposition,role.Zposition),new Vector3Int(role.XRoutation,role.YRoutation,role.ZRoutation));
                    characterData.playerName = playerName;
                    characterData.player_ID= playerID;
                    characters.Add(characterData);
                }
            }
            return characters ;
            
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="connection"></param>
        private void SendLoginResponseToClient(string message,Connection connection)
        {
            UserLoginResponse response = new UserLoginResponse()
            {
                Code = 200,
                Message = message,
            };
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
            if(characterConnection.ContainsKey(netConnection))
            {
                characterConnection.TryGetValue(netConnection,out List<CharacterData> characts);
                if(characts != null)
                {
                    var sl = characts.FindAll(a => a.playerName == messageData.UserName);
                    if(sl !=null&&sl.Count > 0)
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
