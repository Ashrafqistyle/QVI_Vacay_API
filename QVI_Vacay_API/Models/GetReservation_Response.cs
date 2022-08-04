using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QVI_Vacay_API.Models
{
    public class GetReservation_Response
    {
        public enum BookingStatusFlag
        {
            ALL,
            PENDING,
            CONFIRMED,
            CANCELLED,
            ON_REQUEST,
            REJECTED,
            IN_PROCESS_CANCEL
        }

        public enum PaymentStatusFlag
        {
            ALL,
            UNPAID,
            PAID,
            REFUNDED,
            DISPUTED
        }

        public class GetReservation_ResponseModel
        {
            public string Status { get; set; }
            public string Message { get; set; }
            public List<ReservationList> Response { get; set; }
        }

        public class ReservationList
        {
            public int ID { get; set; }
            public string RefNo { get; set; }
            public string BookingRefNo { get; set; }
            public int HotelID { get; set; }
            public string HotelName { get; set; }
            public string HotelStarRate { get; set; }
            public string City { get; set; }
            public string CountryCode { get; set; }
            public string HotelPhoneNo { get; set; }
            public string HotelEmailAddress { get; set; }
            public string HotelWebsite { get; set; }
            public string HotelLocation { get; set; }
            public string HotelImg { get; set; }
            public string HotelLang { get; set; }
            public string HotelLong { get; set; }
            public string BookingStatus { get; set; }
            public string PaymentStatus { get; set; }
            public string PaymentMethod { get; set; }
            public int SupplierID { get; set; }
            public string SupplierName { get; set; }
            public DateTime BookingDate { get; set; }
            public int BookedByUserID { get; set; }
            public string BookedByUserName { get; set; }
            public decimal TotalPrice { get; set; }
            public decimal TotalPrice_Base { get; set; }
            public decimal ActualPrice { get; set; }
            public decimal ActualPrice_Base { get; set; }
            public decimal AmountPaid { get; set; }
            public decimal AmountPaid_Base { get; set; }
            public bool CreditUsedFlag { get; set; }
            public decimal CreditPaid { get; set; }
            public decimal CreditPaid_Base { get; set; }
            public decimal InternationalFee { get; set; }
            public decimal InternationalFee_Base { get; set; }
            public decimal OutstandingAmount { get; set; }
            public decimal OutstandingAmount_Base { get; set; }
            public DateTime CheckInDate { get; set; }
            public DateTime CheckOutDate { get; set; }
            public int Nights { get; set; }
            public int Rooms { get; set; }
            public int Adult { get; set; }
            public int Children { get; set; }
            public decimal DepositAmount { get; set; }
            public decimal DepositAmount_Base { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal TaxAmount_Base { get; set; }
            public string CurrencyCode { get; set; }
            public decimal ExchangeRate { get; set; }
            public DateTime? PaymentDate { get; set; }
            public string PaymentTrxID { get; set; }
            public string InvoiceURL { get; set; }
            public string BookerNationality { get; set; }
            public string SupplierBookingID { get; set; }
            public string SupplierBookingCode { get; set; }
            public DateTime? CancellationDate { get; set; }
            public decimal? SupplierCancellationCharge { get; set; }
            public decimal? SupplierCancellationCharge_Base { get; set; }
            public decimal? AdminCancellationCharge { get; set; }
            public decimal? AdminCancellationCharge_Base { get; set; }
            public decimal? RefundAmount { get; set; }
            public decimal? RefundAmount_Base { get; set; }
            public decimal? RefundCredit { get; set; }
            public decimal? RefundCredit_Base { get; set; }
            public DateTime? RefundDate { get; set; }
            public int? RefundedBy { get; set; }
            public string RefundedByName { get; set; }
            public decimal? RevertedSupplierCancellationCharge { get; set; }
            public decimal? RevertedSupplierCancellationCharge_Base { get; set; }
            public decimal? RevertedAdminCancellationCharge { get; set; }
            public decimal? RevertedAdminCancellationCharge_Base { get; set; }
            public decimal? FinalSupplierCancellationCharge { get; set; }
            public decimal? FinalSupplierCancellationCharge_Base { get; set; }
            public decimal? FinalAdminCancellationCharge { get; set; }
            public decimal? FinalAdminCancellationCharge_Base { get; set; }
            public decimal? RevisedRefundAmount { get; set; }
            public decimal? RevisedRefundAmount_Base { get; set; }
            public decimal? RevisedRefundCredit { get; set; }
            public decimal? RevisedRefundCredit_Base { get; set; }
            public decimal? AdditionalRefundAmount { get; set; }
            public decimal? AdditionalRefundAmount_Base { get; set; }
            public decimal? AdditionalRefundCredit { get; set; }
            public decimal? AdditionalRefundCredit_Base { get; set; }
            public DateTime? RevisionDate { get; set; }
            public int? RevisedBy { get; set; }
            public string RevisedByName { get; set; }
            public bool CancellationInProgress { get; set; }
            public string CancellationPolicy { get; set; }
            public string Session_Lang { get; set; }
            public string IR_ID { get; set; }
            public string Primary_Guest_Title { get; set; }
            public string Primary_Guest_First_Name { get; set; }
            public string Primary_Guest_Last_Name { get; set; }
            public string Primary_Guest_Email { get; set; }
            public string Primary_Guest_Contact { get; set; }
            public int NightsRedeemed { get; set; }
            public decimal? UsageFeeCollected { get; set; }
            public decimal? UsageFeeCollected_Base { get; set; }
            public decimal? PXFCollected { get; set; }
            public decimal? PXFCollected_Base { get; set; }
        }
    }
}
