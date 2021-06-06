using System;
using System.Collections.Generic;
using System.Text;

namespace LuuTruMatKhau.Model
{
    public class Member
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string DateCreated { get; set; }
        public bool IsCreatedKeyStep2 { get; set; }
    }
}
