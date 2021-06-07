using LuuTruMatKhau.Model;
using LuuTruMatKhau.Models;
using LuuTruMatKhau.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly IMemberRepository _memberRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IMemberRepository memberRepository, ICategoryRepository categoryRepository)
        {
            _memberRepository = memberRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccountManagement()
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

        public IActionResult _GetViewInsertCategory()
        {
            return PartialView();
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
