using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LuuTruMatKhau.DataAccess
{
    public class SqlConnectionFactory : IConnectionFactory
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private readonly IConfiguration _config;

        public SqlConnectionFactory(IConfiguration config)
        {
            _config = config;

            if (_connection == null)
                _connection = new  SqlConnection(_config.GetConnectionString("SqlConnectionString2"));
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        public IDbConnection Database
        {
            get
            {
                return _connection;
            }
        }

        public IDbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
        }

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new NullReferenceException("Null Transaction.");
            try
            {
                _transaction.Commit();
            }
            catch (Exception ex)
            {
                _transaction.Rollback();
                throw ex;
            }
            finally
            {
                _transaction.Dispose();
                _connection.Close();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~SqlConnectionFactory()
        {
            Dispose(false);
        }
    }
}
