using System;
using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class Account_Request
    {
        [Key]
        [Required]
        public int seq_no { get; set; }

        [Required]
        public string ir_id { get; set; }

        [Required]
        public string accounts_email { get; set; }

        [Required]
        public string ai_first_name { get; set; }

        [Required]
        public string ai_last_name { get; set; }

        [Required]
        public string ai_address_1 { get; set; }

        [Required]
        public string ai_city { get; set; }

        [Required]
        public string ai_state { get; set; }

        [Required]
        public string ai_postal_code { get; set; }

        [Required]
        public string ai_country { get; set; }

        [Required]
        public string ai_nationality { get; set; }

        [Required]
        public string ai_mobile { get; set; }

        [Required]
        public string accounts_password { get; set; }

        [Required]
        public DateTime accounts_expiry { get; set; }
    }
}
