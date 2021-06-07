using LuuTruMatKhau.DataAccess;
using LuuTruMatKhau.Model;
using LuuTruMatKhau.Services.Interfaces;
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

        public async Task<int> InsertCategory(Category category, string userName)
        {
            try
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
    }


}
