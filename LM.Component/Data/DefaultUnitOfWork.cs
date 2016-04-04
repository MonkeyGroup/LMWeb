using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;

namespace LM.Component.Data
{
    public class DefaultUnitOfWork : IUnitOfWork
    {
        public IDbConnection Connection { get; private set; }

        private readonly Stack<IDbTransaction> _transactions;

        public DefaultUnitOfWork(string connectionString, SqlType sqlType = SqlType.SqlServer)
        {
            switch (sqlType)
            {
                case SqlType.MySql:
                    Connection = new MySqlConnection(connectionString); break;
                case SqlType.SqlServer:
                    Connection = new SqlConnection(connectionString); break;
                case SqlType.Oracle:
                    throw new NotImplementedException("Oracle数据库的扩展方法未实现！");
                default: throw new NotImplementedException("此数据库的扩展方法未实现！");
            }
            _transactions = new Stack<IDbTransaction>();
        }

        private void OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
            {
                Connection.Open();
            }
        }

        private void CloseConnection()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }

        public void BeginTran()
        {
            OpenConnection();
            _transactions.Push(Connection.BeginTransaction());
        }

        public void Commit()
        {
            if (_transactions.Any())
            {
                OpenConnection();
                _transactions.Pop().Commit();
                CloseConnection();
            }
        }

        public void Rollback()
        {
            if (_transactions.Any())
            {
                OpenConnection();
                _transactions.Pop().Rollback();
                CloseConnection();
            }
        }

        public IDbTransaction GetLastTransaction()
        {
            return _transactions.LastOrDefault();
        }

        public void Dispose()
        {
            CloseConnection();
            Connection.Dispose();
        }
    }

    public enum SqlType
    {
        MySql = 1,
        SqlServer = 2,
        Oracle = 3
    }
}
