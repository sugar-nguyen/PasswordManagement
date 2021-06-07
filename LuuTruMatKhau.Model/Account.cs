using System;
using System.Collections.Generic;
using System.Text;

namespace LuuTruMatKhau.Model
{
   public class Account
    {
        public int ID { get; set; }
        public int MemID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CateID { get; set; }
        public string CateName { get; set; }

    }
}
