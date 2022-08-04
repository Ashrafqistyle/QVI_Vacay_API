using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QVI_Vacay_API.Models
{
    public class Token_Response
    {
        public string AllowLogin { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset TokenExpireUtcDate { get; set; }
    }
}
