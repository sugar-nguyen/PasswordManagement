using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuuTruMatKhau.Models
{
    public class DataResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public DataResult(bool ss, string mes)
        {
            Success = ss;
            Message = mes;
        }
    }
}
