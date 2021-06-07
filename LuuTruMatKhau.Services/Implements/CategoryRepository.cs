using LuuTruMatKhau.DataAccess;
using LuuTruMatKhau.Model;
using LuuTruMatKhau.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Services.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SQLConnection _context;
        public CategoryRepository(IConnectionFactory context)
        {
            _context = new SQLConnection(context);
        }

        public async Task<SelectList> GetSelectListCategory(string userName)
        {
            var cate = await GetCategoryAsync(userName);
            var select = new SelectList(cate.Select(x => new {Value = x.CateID, Text = x.CateName }),"Value", "Text");
            return select;
        }

        public async Task<int> InsertCategory(Category category, string userName)
        {
            List<OutputModel> result = new List<OutputModel>();
            try
            {
                result = await _context.ExecuteNonQueryWithOutParamAsync("SP_InsertCategory",
                    "@CateName", category.CateName,
                    "@UserName", userName,
                    "@Desciption", category.Description,
                    "@output|out", 0);
                if (result != null && result.Count > 0)
                {
                    return int.Parse(result.Where(m => m.Label == "@output").FirstOrDefault().Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Category>> GetCategoryAsync(string userName)
        {
            try
            {
                var obj = await _context.ExcuteSprocToListAsync<Category>("SP_GetCategory", "@Username", userName);

                return obj;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteCategory(int cateID)
        {
            int result = 0;
            try
            {
                result = await _context.ExecuteNonQueryAsync("SP_DelCategory", "@CateID", cateID);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Category> GetCategoryByIDAsync(int cateID)
        {
            Category category = new Category();

            try
            {
                category = await _context.ExcuteSprocToObjectAsync<Category>("SP_GetCategoryByID", "@CateID", cateID);
                return category;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateCategoryAsync(Category category)
        {
            List<OutputModel> result = new List<OutputModel>();
            try
            {
                result = await _context.ExecuteNonQueryWithOutParamAsync("SP_UpdateCategory",
                    "@CateID", category.CateID,
                    "@CateName", category.CateName,
                    "@Desciption", category.Description,
                    "@output|out", 0);

                if (result != null && result.Count > 0)
                {
                    return int.Parse(result.Where(m => m.Label == "@output").FirstOrDefault().Value);
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }


}
