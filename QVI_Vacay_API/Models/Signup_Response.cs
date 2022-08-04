using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QVI_Vacay_API.Models
{
    public class Signup_Response
    {
        public bool Response { get; set; }
        public Errors Error { get; set; }

        public class Errors
        {
            public bool Status { get; set; }
            public string Secret { get; set; }
            public string Msg { get; set; }
        }
    }
}
