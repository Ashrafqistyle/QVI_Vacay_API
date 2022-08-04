using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class RefundCredit_Request
    {
        [Required]
        public string ir_id { get; set; }

        [Required]
        public string account_id { get; set; }

        [Required]
        public string credit_value { get; set; }

        [Required]
        public string order_no { get; set; }

        [Required]
        public string item_no { get; set; }

        [Required]
        public string z_Validity_Date { get; set; }
    }
}
