using System;
using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class DiscTierRepo_Request
    {
        [Required]
        public int seq_no { get; set; }

        [Required]
        public string ir_id { get; set; }

        [Required]
        public string disc_tier_ID { get; set; }

        [Required]
        public DateTime purchase_date { get; set; }

        [Required]
        public DateTime expiry_date { get; set; }

        [Required]
        public string order_no { get; set; }

        [Required]
        public string item_no { get; set; }
    }
}
