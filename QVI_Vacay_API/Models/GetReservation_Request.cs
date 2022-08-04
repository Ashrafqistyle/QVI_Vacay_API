using System;
using System.ComponentModel.DataAnnotations;

namespace QVI_Vacay_API.Models
{
    public class GetReservation_Request
    {
        [Required]
        public int DateType { get; set; }

        [Required]
        public string BookingStatus { get; set; }

        [Required]
        public string PaymentStatus { get; set; }

        public string Status { get; set; }

        [Required]
        public DateTime DateFrom { get; set; }

        [Required]
        public DateTime DateTo { get; set; }

        [Required]
        public string BookingRefNo { get; set; }

        [Required]
        public string IRID { get; set; }

        [Required]
        public string EmailAddress { get; set; }
    }
}
