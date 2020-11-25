using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace StudentManage
{
    public  class SqlHelper
    {
        public static readonly string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;
        public static object ExecuteScalar(string sql,params SqlParameter[] paras)
        {
            object o = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                //创建command对象
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);

                //打开连接
                conn.Open();

                //执行命令
                o = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

                //关闭连接
                conn.Close();
            }
                
            return o;
        }

        /// <summary>
        /// 执行查询，并返回SqlDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string sql,params SqlParameter[]paras)
        {
            SqlConnection conn = new SqlConnection(connString);          
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dr;
            }
            catch(SqlException ex)
            {
                conn.Close();
                throw new Exception("执行出现异常", ex);                
            }
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sql,params SqlParameter[] paras)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                if (paras != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddRange(paras);
                }

                //conn.Open();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                
                da.Fill(dt);
                conn.Close();
            }

            return dt;
        }

        /// <summary>
        /// 返回受影响的行数  Insert  Update  Delete
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string sql,params SqlParameter[] paras)
        {
            int count = 0;
            using(SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(paras);

                conn.Open();
                count = cmd.ExecuteNonQuery();//执行T-SQL语句,返回受影响的行数
                cmd.Parameters.Clear();
                conn.Close();
            }
            return count;
        }
        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="comList"></param>
        /// <returns></returns>
        public static bool ExecuteTrans(List<CommandInfo>comList)
        {
            using(SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlTransaction trans = conn.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.Transaction = trans;
                try
                {
                    int count = 0;
                    for(int i = 0;i < comList.Count;i++)
                    {
                        cmd.CommandText = comList[i].CommandText;

                        if (comList[i].IsProc)
                            cmd.CommandType = CommandType.StoredProcedure;
                        else
                            cmd.CommandType = CommandType.Text;

                        if(comList[i].Parameters.Length > 0)
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(comList[i].Parameters);
                        }
                        count += cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                    }
                    trans.Commit();
                    return true;
                }
                catch(Exception ex)
                {
                    trans.Rollback();
                    throw new Exception("事务执行出现异常", ex);
                }
            }
        }
    }
}
