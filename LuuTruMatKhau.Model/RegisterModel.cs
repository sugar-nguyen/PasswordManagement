using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuuTruMatKhau.Model
{
   public class RegisterModel
    {
        [Required(ErrorMessage ="Please enter user name.")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Please enter password.")]
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must contain uppercase, lowercase letters, numbers and special characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email is invalid.")]
        public string Email { get; set; }

        [Compare("Password",ErrorMessage ="Password do not match.")]
        [Required(ErrorMessage = "Please enter reapeat password.")]
        public string RepeatPassword { get; set; }


    }
}
