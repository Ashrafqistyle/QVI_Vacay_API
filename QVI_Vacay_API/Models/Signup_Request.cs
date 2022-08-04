using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class Signup_Request
    {
        [Required]
        public string first_name { get; set; }

        [Required]
        public string last_name { get; set; }

        [Required]
        public string phone { get; set; }

        [Required]
        public string ir_id { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public string type { get; set; }

        [Required]
        public string account_status { get; set; }

        [Required]
        public string expiration_date { get; set; }

        [Required]
        public string ai_nationality { get; set; }

        [Required]
        public string ai_country { get; set; }

        [Required]
        public string ai_state { get; set; }

        [Required]
        public string ai_city { get; set; }
    }
}
