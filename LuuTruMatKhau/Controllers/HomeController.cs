using LuuTruMatKhau.Model;
using LuuTruMatKhau.Models;
using LuuTruMatKhau.Services.Interfaces;
using LuuTruMatKhau.Services.Lib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAccountRepository _accountRepository;

        public HomeController(IMemberRepository memberRepository, ICategoryRepository categoryRepository, IAccountRepository accountRepository)
        {
            _memberRepository = memberRepository;
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }



        #region Category
        public async Task<IActionResult> CategoryManagement()
        {
            return View(await GetCategory());
        }
        private async Task<List<Category>> GetCategory()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userName = User.Identity.Name;
                var model = await _categoryRepository.GetCategoryAsync(userName);
                return model;
            }

            return new List<Category>();
        }

        public async Task<IActionResult> _GetGetCategoryViewModel()
        {
            return View(await GetCategory());
        }

        public async Task<IActionResult> _GetViewInsertCategory(int? cateID)
        {
            if (cateID != null)
            {
                var model = await _categoryRepository.GetCategoryByIDAsync((int)cateID);
                return PartialView(model);
            }
            return PartialView(new Category());
        }

        [HttpPost]
        public async Task<IActionResult> InsertCategory(string cateName, string description)
        {
            if (User.Identity.IsAuthenticated)
            {
                Category category = new Category
                {
                    CateName = cateName,
                    Description = description
                };

                int result = await _categoryRepository.InsertCategory(category, User.Identity.Name);
                if (result > 0)
                {
                    return Json(new Models.DataResult(true, "Insert succesfull."));
                }

                return Json(new Models.DataResult(false, "Insert category fail, please try later."));
            }

            return Json(new Models.DataResult(false, "You are timeout"));

        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int cateID, string cateName, string description)
        {
            if (User.Identity.IsAuthenticated)
            {
                Category category = new Category
                {
                    CateID = cateID,
                    CateName = cateName,
                    Description = description
                };

                int result = await _categoryRepository.UpdateCategoryAsync(category);
                if (result > 0)
                {
                    return Json(new Models.DataResult(true, "Update succesfull."));
                }

                return Json(new Models.DataResult(false, "Update category fail, please try later."));
            }

            return Json(new Models.DataResult(false, "You are timeout"));

        }

        [HttpPost]
        public async Task<IActionResult> DelCategory(int cateID)
        {
            var result = await _categoryRepository.DeleteCategory(cateID);
            if (result > 0)
            {
                return Json(new DataResult(true, "Delete successfull."));
            }
            return Json(new DataResult(false, "Delete fail."));
        }
        #endregion

        #region account
        public async Task<IActionResult> AccountManagement()
        {
            var member = _memberRepository.GetMemberByUserName(User.Identity.Name);
            ViewBag.AccountMember = await _accountRepository.GetMemberAccountAsync(User.Identity.Name);
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrivateKey()
        {
            string key = PasswordServices.CreateRandomPrivateKey(32);
            int result = 0;
            if (!string.IsNullOrEmpty(key))
            {
                result = await _accountRepository.CreatePrivateKeyAsync(User.Identity.Name, key);
                if (result > 0)
                {
                    return Json(new DataResult(true, "Create private key successfull."));
                }
            }
            return Json(new DataResult(false, "Create private key fail."));
        }

        public async Task<IActionResult> _GetAccountViewModel()
        {
            var model = await _accountRepository.GetMemberAccountAsync(User.Identity.Name);
            return View(model);
        }
        public async Task<IActionResult> _GetViewInsertMemberAccount()
        {
            ViewBag.SelectCate = await _categoryRepository.GetSelectListCategory(User.Identity.Name);
            return PartialView(new Account());
        }
        [HttpPost]
        public async Task<IActionResult> InsertMemberAccount(int cateID, string userName, string psw, string memPsw)
        {
            string usName = User.Identity.Name;
            var (valid, member) = await _memberRepository.ValidateUserCredentialsAsync(usName);
            bool isMathPsw = Common.VerifyPsw(memPsw, member.Password);
            if (isMathPsw)
            {
                var privateKey = await _accountRepository.GetPrivateKeyAsync(usName);
                if (!string.IsNullOrEmpty(privateKey))
                {
                    Regex reg = new Regex("[*'\",._\\-&#^@!]");
                    string memberPsw = reg.Replace(memPsw, string.Empty);

                    string keyEncrypt = privateKey.Substring(0,privateKey.Length - memberPsw.Length) + memberPsw;
                    int a = keyEncrypt.Length;

                    string encryptPsw = PasswordServices.EncryptString(keyEncrypt, psw);

                    var account = new Account
                    {
                        CateID = cateID,
                        MemID = member.ID,
                        UserName = userName,
                        Password = encryptPsw,
                    };

                    int result = await _accountRepository.InsertMemberAccount(account);
                    if (result > 0)
                    {
                        return Json(new DataResult(true, "Insert successfull."));
                    }
                }
            }
            else
            {
                return Json(new DataResult(false, "Your password is incorrect."));
            }

            return Json(new DataResult(false, "Insert fail."));
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
