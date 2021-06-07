using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LuuTruMatKhau.Model
{
    public class Category
    {
        public int CateID { get; set; }

        public string CateName { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
