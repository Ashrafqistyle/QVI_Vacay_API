using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class Login_Request
    {
        [Required]
        public string ClientIdentifier { get; set; }

        [Required]
        public string ClientKeySecret { get; set; }

        [Required]
        public string AccountsEmail { get; set; }
    }
}
