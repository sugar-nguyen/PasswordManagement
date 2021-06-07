using LuuTruMatKhau.Model;
using LuuTruMatKhau.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LuuTruMatKhau.DataAccess;
using System.Data.SqlClient;
using System.Linq;

namespace LuuTruMatKhau.Services.Implements
{
    public class MemberRepository : IMemberRepository
    {
        private SQLConnection conn;
        public MemberRepository(IConnectionFactory context)
        {
            conn = new SQLConnection(context);
        }

        public int ActiveMember(string userName, int code)
        {
            List<OutputModel> result = new List<OutputModel>();
            try
            {
                result = conn.ExecuteNonQueryWithOutParam("SP_ActiveMember", "@Username", userName, "@Code", code, "@output|out", 0);
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

        public Member GetMemberByEmail(string email)
        {
            Member obj = new Member();
            try
            {
                obj = conn.ExcuteSprocToObject<Member>("SP_GetUserByEmail", "@Email", email);
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Member GetMemberByUserName(string userName)
        {
            Member obj = new Member();
            try
            {
                obj = conn.ExcuteSprocToObject<Member>("SP_GetUserByUserName", "@Username", userName);
                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<(bool, Member)> ValidateUserCredentialsAsync(string userName)
        {
            try
            {
                var obj = await conn.ExcuteSprocToObjectAsync<Member>("SP_MemberLogin", "@Username", userName);

                if (obj != null)
                {
                    return (true, obj);
                }

                return (false, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int MemberRegister(RegisterModel model)
        {
            int generateCode = 0;
            List<OutputModel> oModel = new List<OutputModel>();
            try
            {
                oModel = conn.ExecuteNonQueryWithOutParam("SP_RegisterMember",
                    "@Username", model.UserName,
                    "@Psw", model.Password,
                    "@Email", model.Email,
                    "@output|out", 0
                    );
                if (oModel != null && oModel.Count > 0)
                {
                    generateCode = int.Parse(oModel.Where(m => m.Label == "@output").FirstOrDefault().Value);
                }
                return generateCode;
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
