using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace LuuTruMatKhau.Services.Lib
{
    public static class Common
    {
        private const string SALT = "_fhdhd39%$";
        public static string EncryptPsw(string psw)
        {
            psw += SALT;
            string _salt = BC.GenerateSalt(8);
            return BC.HashPassword(psw, _salt);
        }

        public static bool VerifyPsw(string psw, string hash)
        {
            psw += SALT;
            return BC.Verify(psw, hash);
        }


    }
}
