using LuuTruMatKhau.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Services.Interfaces
{
    public interface IAccountRepository
    {
        Task<int> CreatePrivateKeyAsync(string userName, string key);
        Task<List<Account>> GetMemberAccountAsync(string userName);
        Task<string> GetPrivateKeyAsync(string userName);
        Task<int> InsertMemberAccount(Account account);
    }
}
