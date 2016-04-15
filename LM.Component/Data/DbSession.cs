using System;
using System.Data;
using System.Data.SqlClient;

namespace LM.Component.Data
{
    /// <summary>
    ///  定义为可释放类型，使其可以使用 using 块。
    /// </summary>
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; private set; }

        public DbSession(string connString)
        {
            Connection = new SqlConnection(connString);
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
            // 注：connection 的 Dispose 方法会清空connectionString，但是不会是否 connection 内存，
            //          所以只能 Close，而不能 Dispose
            //Connection.Dispose();
        }

    }
}
