using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuuTruMatKhau.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; }

        public string Email { get; set; }
        public string ReturnUrl { get; set; }

    }
}
