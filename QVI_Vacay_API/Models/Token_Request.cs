using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class Token_Request
    {
        public class Token
        {
            [Required]
            public string RequestFromSystem { get; set; }

            [Required]
            public string FromScreen { get; set; }

            [Required]
            public List<RequestsList> Request { get; set; }
        }

        public class RequestsList
        {
            [Required]
            public string ClientIdentifier { get; set; }

            [Required]
            public string Md5Hash { get; set; }
        }
    }
}
