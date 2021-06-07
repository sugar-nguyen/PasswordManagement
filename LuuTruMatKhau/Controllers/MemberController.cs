using LuuTruMatKhau.Model;
using LuuTruMatKhau.Services.Interfaces;
using LuuTruMatKhau.Services.Lib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Controllers
{
    [Route("auth")]
    public class MemberController : BaseController
    {
        private readonly IMemberRepository _memberRepos;
        private readonly IOptions<ConfigMailModel> _configMailModel;
        private readonly IOptions<SystemConfig> _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MemberController(IMemberRepository memberRepos,
            IOptions<ConfigMailModel> option,
            IHttpContextAccessor httpContextAccessor,
            IOptions<SystemConfig> config)
        {
            _memberRepos = memberRepos;
            _configMailModel = option;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }

        [Route("login")]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }
        [Route("login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel login)
        {
            if (ModelState.IsValid)
            {
                var (isValid, user) = await _memberRepos.ValidateUserCredentialsAsync(login.UserName);
                if (isValid)
                {
                    var isMatchPsw = Common.VerifyPsw(login.Password, user.Password);
                    if (isMatchPsw)
                    {
                        await LoginAsync(user);
                        if (!string.IsNullOrEmpty(login.ReturnUrl))
                        {
                            return Redirect(login.ReturnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("ErrorMes", "Username or password is incorrect.");
            }
            return View();
        }

        private async Task LoginAsync(Member user)
        {
            var properties = new AuthenticationProperties
            {
                AllowRefresh = false,
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(_config.Value.LoginExpired)
            };

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.ID.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email,ClaimValueTypes.Email),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            //set cookie
            string cookieValueFromContext = _httpContextAccessor.HttpContext.Request.Cookies[_config.Value.LoginCookie];
            var jsonCookieInfo = JsonConvert.SerializeObject(user);
            if (cookieValueFromContext == null)
            {
                SetCookie(_config.Value.LoginCookie, jsonCookieInfo, _config.Value.LoginExpired);
            }
            else
            {
                //remove cookie hien co
                RemoveCookie(_config.Value.LoginCookie);
                //set cookie moi
                SetCookie(_config.Value.LoginCookie, jsonCookieInfo, _config.Value.LoginExpired);
            }
            await HttpContext.SignInAsync(principal, properties);
        }
        [Route("register")]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [Route("register")]
        [AutoValidateAntiforgeryToken]
        [HttpPost]
        public IActionResult Register(RegisterModel register)
        {
            if (ModelState.IsValid)
            {

                bool isUsing = (_memberRepos.GetMemberByEmail(register.Email) != null || _memberRepos.GetMemberByUserName(register.UserName) != null);

                if (isUsing)
                {
                    ModelState.AddModelError("ErrorMes", "Username or Email already in use ");
                    return View(register);
                }

                register.Password = Common.EncryptPsw(register.Password);
                int code = _memberRepos.MemberRegister(register);
                if (code != 0)
                {
                    _configMailModel.Value.Message = code.ToString();
                    _configMailModel.Value.MailTo = register.Email;

                    var isSendEmail = true;// MailServices.SendVerifyEmail(_configMailModel.Value);
                    if (isSendEmail)
                    {
                        //ViewBag.IsShowModalVerify = 1;
                        ViewBag.UserName = register.UserName;
                        return RedirectToAction("_Verifycation",new {us=register.UserName });
                    }
                    //else
                    //{
                    //    ViewBag.IsShowModalVerify = 0;
                    //}
                }
            }
            return View(register);
        }

        [Route("verify")]
        public IActionResult _Verifycation(string us)
        {
            return PartialView("_Verify");
        }

        [Route("active")]
        [HttpPost]
        public IActionResult ActiveMember(string code, string userName)
        {
            var result = _memberRepos.ActiveMember(userName, int.Parse(code));
            return Json(result);
        }


        [HttpPost]
        [Route("logout")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                RemoveCookie(_config.Value.LoginCookie);
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("login", "auth");
        }
    }
}
