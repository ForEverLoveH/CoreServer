using CoreCommon.GameLog;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.GameService.DBSqlService.MySqlHelper
{
    public class MySqlServiceHelper
    {
        private string mysqlConnectionStr = null;
        private string IPAddress { get; set; }
        private string Port { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string DataBase { get; set; }
        private MySqlConnection connection = null;

        public MySqlServiceHelper(string ipaddress, string port, string userName, string pass, string dataBase)
        {
            this.IPAddress = ipaddress;
            this.Port = port;
            this.UserName = userName;
            this.Password = pass;
            this.DataBase = dataBase;
            OpenMysqlConnection();
        }

        /// <summary>
        ///
        /// </summary>
        public void OpenMysqlConnection()
        {
            try
            {
                mysqlConnectionStr = $"server={IPAddress};port={Port};user={UserName};password={Password};database={DataBase}";
                connection = new MySqlConnection(mysqlConnectionStr);
                connection.Open();
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void EnsureMysqlConnection()
        {
            if (connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                OpenMysqlConnection();
            };
        }

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public T ExcuteSql<T>(string sql, Func<MySqlCommand, T> func)
        {
            try
            {
                EnsureMysqlConnection();
                using (MySqlCommand comm = new MySqlCommand(sql, connection))
                {
                    using (MySqlTransaction trans = connection.BeginTransaction())
                    {
                        T res = func.Invoke(comm);
                        trans.Commit();
                        return res;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                throw;
            }
        }
    }
}