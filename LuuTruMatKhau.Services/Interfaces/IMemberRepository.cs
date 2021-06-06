using LuuTruMatKhau.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Services.Interfaces
{
    public interface IMemberRepository
    {
        Task<(bool,Member)> ValidateUserCredentialsAsync(string userName);
        int MemberRegister(RegisterModel model);
        int ActiveMember(string userName, int code);
        Member GetMemberByUserName(string userName);
        Member GetMemberByEmail(string email);
    }
}
