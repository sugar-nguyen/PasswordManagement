using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Controllers
{
    public class BaseController : Controller
    {
        public void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(1);

            Response.Cookies.Append(key, value, option);
        }
        public void RemoveCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
