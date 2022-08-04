using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QVI_Vacay_API.Models
{
    public class Login_Response
    {
        public bool AllowLogin { get; set; }
        public string Status { get; set; }
        public string ResultType { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
