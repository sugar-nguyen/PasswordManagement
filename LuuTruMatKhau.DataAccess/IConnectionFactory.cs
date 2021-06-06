using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LuuTruMatKhau.DataAccess
{
    public interface IConnectionFactory : IDisposable
    {
        IDbConnection Database { get; }
        IDbTransaction Transaction { get; }
        void BeginTransaction();
        void CommitTransaction();
    }
}
