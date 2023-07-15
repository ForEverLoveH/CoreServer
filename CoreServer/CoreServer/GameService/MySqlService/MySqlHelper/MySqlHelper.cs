
using CoreCommon;
using CoreCommon.GameLog;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServer.MySqlService 
{
    public  class MySqlHelper:IDisposable
    {
        private string mysqlConnectionStr = null;
       
        private MySqlConnection mySqlConnection = null;

        public MySqlHelper()
        {

            mysqlConnectionStr = "server=127.0.0.1;port=3306;database=mmocore;username=root;password=root";
            if (!string.IsNullOrEmpty(mysqlConnectionStr))
            {
                OpenMysqlService();
            }

        }
       # region 连接
        /// <summary>
        /// 打开mysql
        /// </summary>
        private void OpenMysqlService()
        {
            try
            {
                mySqlConnection = new MySqlConnection(mysqlConnectionStr);
                mySqlConnection.Open();
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return;
            }
        }
        /// <summary>
        /// 保证mysql 的连接
        /// </summary>
        private void EnSureMySqlConnection()
        {
            if (mySqlConnection == null || mySqlConnection.State != System.Data.ConnectionState.Open)
                OpenMysqlService();
        }
        #endregion
        #region 事务
        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public MySqlTransaction BeginMySqlTransaction()
        {
            MySqlTransaction mySqlTransaction = null;
            try
            {
                mySqlTransaction = mySqlConnection.BeginTransaction();
            }
            catch(Exception e) { LoggerHelper.Debug(e); return null; }
            return mySqlTransaction;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="mySqlTransaction"></param>
        public void  CommMySqlTransaction(ref MySqlTransaction mySqlTransaction)
        {
            try
            {
                if (mySqlTransaction != null)
                {
                    mySqlTransaction.Commit();
                    mySqlTransaction = null;
                }
            }
            catch(Exception ex)
            {
                LoggerHelper.Debug(ex);
                mySqlTransaction = null;
                return;
            }
        }
        /// <summary>
        /// 准备命令操作参数
        /// </summary>
        /// <param name="mySqlCommand"></param>
        /// <param name="mySqlConnection"></param>
        /// <param name="cmdText"></param>
        /// <param name="data"></param>
        private void    PrepareCommand(MySqlCommand mySqlCommand,MySqlConnection mySqlConnection ,string cmdText, Dictionary<string, string> data)
        {
            try
            {
                EnSureMySqlConnection();
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Connection = mySqlConnection;
                mySqlCommand.CommandText = cmdText;
                mySqlCommand.CommandType = System.Data.CommandType.Text;
                mySqlCommand.CommandTimeout = 30;
                if (data == null) return;
                else  if(data != null || data.Count > 0)
                {
                    foreach(var ds in data)
                    {
                        mySqlCommand.Parameters.AddWithValue(ds.Key, ds.Value);
                    }
                }
               
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug (ex);
                return;
            }
        }
        #endregion
        #region 操作
        public DataSet ExcuteDataSet(string cmdText, Dictionary<string, string> data = null)
        {
            try
            {
                DataSet ds = new DataSet();
                MySqlCommand mySqlCommand = new MySqlCommand();
                PrepareCommand(mySqlCommand, mySqlConnection, cmdText, data);
                var datas = new MySqlDataAdapter(mySqlCommand);
                datas.Fill(ds);
                return ds;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ExcuteDataTable(string cmdText, Dictionary<string, string> data = null)
        {
            try
            {
                DataTable dataTable = new DataTable();
                MySqlCommand mySqlCommand = new MySqlCommand();
                PrepareCommand(mySqlCommand, mySqlConnection, cmdText, data);
                MySqlDataReader dataReader = mySqlCommand.ExecuteReader();
                dataTable.Load(dataReader);
                return dataTable;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataRow ExcuteDataRow(string cmdText, Dictionary<string, string> data = null)
        {
            try
            {
                DataSet ds = ExcuteDataSet(cmdText, data);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ExcuteNonQuery(string cmdText, Dictionary<string, string> data = null)
        {
            try
            {
                EnSureMySqlConnection();
                /* File.AppendAllText(@"./Log/db/" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log",
                     $"数据库操作:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n");
                 File.AppendAllText(@"./Log/db/" + DateTime.Now.ToString("yyyy-MM-dd-HH") + ".log", cmdText + "\n");*/
                int res = 0;
                MySqlCommand mySqlCommand = new MySqlCommand();
                PrepareCommand(mySqlCommand, mySqlConnection, cmdText, data);
                res = mySqlCommand.ExecuteNonQuery();
                return res;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return -1;
            }

        }
        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">Sql命令文本</param>
        /// <param name="data">传入的参数</param>
        /// <returns>SQLiteDataReader</returns>
        public MySqlDataReader ExecuteReader(string cmdText, Dictionary<string, string> data = null)
        {
            var command = new MySqlCommand();
            try
            {
                PrepareCommand(command, mySqlConnection, cmdText, data);
                var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception e)
            {
                command.Dispose();
                LoggerHelper.Debug(e);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> ExecuteReaderList(string cmdText)
        {
            try
            {
                List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
                var ds = ExecuteReader(cmdText);
                int columcount = ds.FieldCount;
                while (ds.Read())
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    for (int i = 0; i < columcount; i++)
                    {
                        object obj = ds.GetValue(i);
                        if (obj == null)
                        {
                            dic.Add(ds.GetName(i), "");
                        }
                        else
                        {
                            dic.Add(ds.GetName(i), obj.ToString());
                        }
                    }

                    list.Add(dic);
                }
                return list;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public Dictionary<string, string> ExecuteReaderOne(string cmdText)
        {
            try
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                var ds = ExecuteReader(cmdText);
                int columcount = ds.FieldCount;
                while (ds.Read())
                {
                    for (int i = 0; i < columcount; i++)
                    {
                        object obj = ds.GetValue(i);
                        if (obj == null)
                        {
                            dic.Add(ds.GetName(i), "");
                        }
                        else
                        {
                            dic.Add(ds.GetName(i), obj.ToString());
                        }
                    }
                    break;
                }
                return dic;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }
        }
        /// <summary>
        /// 返回结果集中的第一行第一列，忽略其他行或列
        /// </summary>
        /// <param name="cmdText">Sql命令文本</param>
        /// <param name="data">传入的参数</param>
        /// <returns>object</returns>
        public object ExecuteScalar(string cmdText, Dictionary<string, string> data = null)
        {
            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, mySqlConnection, cmdText, data);
            return cmd.ExecuteScalar();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageIndex">页牵引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="cmdText">Sql命令文本</param>
        /// <param name="countText">查询总记录数的Sql文本</param>
        /// <param name="data">命令参数</param>
        /// <returns>DataSet</returns>
        public DataSet ExecutePager(ref int recordCount, int pageIndex, int pageSize, string cmdText, string countText, Dictionary<string, string> data = null)
        {
            try
            {
                if (recordCount < 0)
                    recordCount = int.Parse(ExecuteScalar(countText, data).ToString());
                DataSet ds = new DataSet();
                MySqlCommand command = new MySqlCommand();
                PrepareCommand(command, mySqlConnection, cmdText, data);
                var da = new MySqlDataAdapter(command);
                da.Fill(ds, (pageIndex - 1) * pageSize, pageSize, "result");
                return ds;
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return null;
            }

        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public void CloseMySqlConnectionServer()
        {
            if (mySqlConnection.State == ConnectionState.Open)
                mySqlConnection.Close();
            mySqlConnection = null;
            mySqlConnection.Dispose();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            mySqlConnection?.Dispose();
        }
    }
}
