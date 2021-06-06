using System;
using System.Collections.Generic;
using System.Text;

namespace LuuTruMatKhau.Model
{
    public class ConfigMailModel
    {
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string Password { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string KeyDecrypt { get; set; }
    }
}
