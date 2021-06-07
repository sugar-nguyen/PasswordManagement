using LuuTruMatKhau.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Services.Interfaces
{
    public interface ICategoryRepository
    {
        Task<int> InsertCategory(Category category,string userName);
        Task<List<Category>> GetCategoryAsync(string userName);
        Task<int> DeleteCategory(int cateID);
        Task<Category> GetCategoryByIDAsync(int cateID);
        Task<int> UpdateCategoryAsync(Category category);
        Task<SelectList> GetSelectListCategory(string userName);
    }
}
