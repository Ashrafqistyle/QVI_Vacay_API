using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class Client_Request
    {
        public class Clients
        {
            [Required]
            public List<ClientList> Client { get; set; }
        }

        public class ClientList
        {
            [Required]
            public string ClientIdentifier { get; set; }

            [Required]
            public string ClientKeySecret { get; set; }
        }
    }
}
