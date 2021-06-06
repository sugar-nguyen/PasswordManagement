using Dapper;
using LuuTruMatKhau.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.DataAccess
{
    public class SQLConnection
    {
        IConnectionFactory context;

        public SQLConnection(IConnectionFactory connectionFactory)
        {
            context = connectionFactory;
        }

        static int _commandTimeout = 30;

        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }

            set
            {
                _commandTimeout = value;
            }
        }
        public int ExecuteNonQueryRowCount(string spname, params object[] parameters)
        {
            int result = 0;
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    param.Add(parameters[i].ToString(), parameters[i + 1]);
                }
                param.Add("RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                context.Database.Query<int>(query, param, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout);
                result = param.Get<int>("RowCount");
            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public int ExecuteNonQuery(string spname, params object[] parameters)
        {
            int result = 0;
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    param.Add(parameters[i].ToString(), parameters[i + 1]);
                }
                result = context.Database.Execute(query, param, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout);

            }
            catch (Exception ex)
            {
            }
            return result;
        }
        public int ExecuteNonQueryWithTransaction(string spname, params object[] parameters)
        {
            int result = 0;
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    param.Add(parameters[i].ToString(), parameters[i + 1]);
                }

                context.BeginTransaction();
                result = context.Database.Execute(query, param, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: _commandTimeout, transaction: context.Transaction);
                context.CommitTransaction();
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public List<T> ExcuteSprocToList<T>(string spname, params object[] parameters)
        {
            List<T> ReturnData = new List<T>();
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    param.Add(parameters[i].ToString(), parameters[i + 1]);
                }

                ReturnData = context.Database.Query<T>(query, param, commandType: System.Data.CommandType.StoredProcedure).ToList();

            }
            catch (Exception ex)
            {
            }
            return ReturnData;
        }
        public async Task<List<T>> ExcuteSprocToListAsync<T>(string spname, params object[] parameters)
        {
            List<T> ReturnData = new List<T>();
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    param.Add(parameters[i].ToString(), parameters[i + 1]);
                }

                ReturnData = (List<T>)await context.Database.QueryAsync<T>(query, param, commandType: System.Data.CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
            }
            return ReturnData;
        }

        public List<T> ExcuteCommandToList<T>(string spname, string dbConnect = "")
        {
            List<T> ReturnData = new List<T>();
            try
            {
                var query = spname;
                ReturnData = context.Database.Query<T>(query, "", commandType: System.Data.CommandType.Text).ToList();

            }
            catch (Exception ex)
            {
            }
            return ReturnData;
        }
        public int ExcuteCommand(string spname, string dbConnect = "")
        {
            int returnData = 0;
            try
            {
                var query = spname;
                returnData = context.Database.Query(query, null, commandType: System.Data.CommandType.Text).FirstOrDefault();

            }
            catch (Exception ex)
            {
            }
            return returnData;
        }
        public T ExcuteSprocToObject<T>(string spname, string dbConnect = "")
        {
            List<T> ReturnData = ExcuteCommandToList<T>(spname, dbConnect);
            T obj = ReturnData.FirstOrDefault();
            return obj;
        }
        public T ExcuteSprocToObject<T>(string spname, params object[] parameters)
        {
            List<T> ReturnData = ExcuteSprocToList<T>(spname, parameters);
            T obj = ReturnData.FirstOrDefault();
            return obj;
        }
        public async Task<T> ExcuteSprocToObjectAsync<T>(string spname, params object[] parameters)
        {
            List<T> ReturnData = await ExcuteSprocToListAsync<T>(spname, parameters);
            T obj = ReturnData.FirstOrDefault();
            return obj;
        }
        public List<OutputModel> ExecuteNonQueryWithOutParam(string spname, params object[] parameters)
        {
            List<OutputModel> model = new List<OutputModel>();
            try
            {
                var query = spname;
                var param = new DynamicParameters();
                for (int i = 0; i < parameters.Length - 1; i += 2)
                {
                    if (parameters[i].ToString().Contains("|out"))
                    {
                        param.Add(parameters[i].ToString().Replace("|out", ""), parameters[i + 1], direction: ParameterDirection.Output);
                        OutputModel item = new OutputModel
                        {
                            Label = parameters[i].ToString().Replace("|out", ""),
                            Value = ""
                        };
                        model.Add(item);
                    }
                    else
                    {
                        param.Add(parameters[i].ToString(), parameters[i + 1]);
                    }
                }
                context.Database.Execute(query, param, commandType: System.Data.CommandType.StoredProcedure);
                for (int i = 0; i < model.Count; i++)
                {
                    model[i].Value = (param.Get<dynamic>(model[i].Label)).ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }

    }
}
