using System;
using System.Data;

namespace LM.Component.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }

        void BeginTran();

        void Commit();

        void Rollback();

        IDbTransaction GetLastTransaction();
    }
}
