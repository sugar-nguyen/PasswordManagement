using LuuTruMatKhau.DataAccess;
using LuuTruMatKhau.Model;
using LuuTruMatKhau.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Services.Implements
{
    public class AccountRepository : IAccountRepository
    {
        private readonly SQLConnection _context;


        public AccountRepository(IConnectionFactory context)
        {
            _context = new SQLConnection(context);
        }

        public async Task<int> CreatePrivateKeyAsync(string userName, string key)
        {
            List<OutputModel> result = new List<OutputModel>();
            try
            {
                result = await _context.ExecuteNonQueryWithTransactionWithOutParamAsync("SP_InsertPrivateKey",
                    "@Key", key,
                    "@UserName", userName,
                    "@output|out", 0);

                if (result != null && result.Count > 0)
                {
                    return int.Parse(result.Where(m => m.Label == "@output").FirstOrDefault().Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Account>> GetMemberAccountAsync(string userName)
        {
            try
            {
                var obj = await _context.ExcuteSprocToListAsync<Account>("SP_GetMemberAccount", "@UserName", userName);

                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetPrivateKeyAsync(string userName)
        {

            try
            {
                var obj = await _context.ExcuteSprocToObjectAsync<string>("SP_GetPrivateKey", "@UserName", userName);

                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> InsertMemberAccount(Account account)
        {
            List<OutputModel> result = new List<OutputModel>();
            try
            {
                result = await _context.ExecuteNonQueryWithTransactionWithOutParamAsync("SP_InsertMemberAccount",
                    "@UserName", account.UserName,
                    "@Psw", account.Password,
                    "@CateID", account.CateID,
                    "@MemID", account.MemID,
                    "@output|out", 0);

                if (result != null && result.Count > 0)
                {
                    return int.Parse(result.Where(m => m.Label == "@output").FirstOrDefault().Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
