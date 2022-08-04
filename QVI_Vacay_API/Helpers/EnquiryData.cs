using QVI_Vacay_API.Models;
using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.Generic;
using QVI_Vacay_API.Data;

namespace QVI_Vacay_API.Helpers
{
    public class EnquiryData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public GetReservation_Response.GetReservation_ResponseModel RetrieveReservation(GetReservation_Request obj)
        {
            MySqlConnection Connection = new MySqlConnection();
            int i = 0;
            GetReservation_Response.GetReservation_ResponseModel response = new GetReservation_Response.GetReservation_ResponseModel();

            Connection.ConnectionString = strconMysql;
            Connection.Open();

            string sqlquery = "SELECT a.*, CONCAT(b.ai_first_name,' ',b.ai_last_name) as full_name, " + 
                              "CONCAT(c.ai_first_name,' ',c.ai_last_name) as refunded_by_name, " + 
                              "CONCAT(d.ai_first_name,' ',d.ai_last_name) as revised_by_name " + 
                              "FROM hotels_bookings AS a " + 
                              "INNER JOIN pt_accounts AS b ON b.accounts_id=a.booking_user_id " + 
                              "LEFT JOIN pt_accounts AS c ON c.accounts_id=a.refunded_by " + 
                              "LEFT JOIN pt_accounts AS d ON d.accounts_id=a.revised_by ";

            if (obj.BookingRefNo != "")
                sqlquery = sqlquery + " WHERE CONCAT(Convert(booking_ref_no, CHARACTER), '-', Convert(booking_id, CHARACTER)) = '" + obj.BookingRefNo + "' ";
            else
            {
                if (obj.DateType == 0)
                    sqlquery = sqlquery + "WHERE FROM_UNIXTIME(booking_date) BETWEEN CAST('" + obj.DateFrom.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) " + "AND CAST('" + obj.DateTo.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) ";
                else if (obj.DateType == 1)
                    sqlquery = sqlquery + "WHERE booking_checkin BETWEEN CAST('" + obj.DateFrom.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) " + "AND CAST('" + obj.DateTo.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) ";
                else if (obj.DateType == 2)
                    sqlquery = sqlquery + "WHERE booking_checkout BETWEEN CAST('" + obj.DateFrom.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) " + "AND CAST('" + obj.DateTo.ToString("yyyy-MM-dd HH:mm:ss.ff") + "' AS DATE) ";

                if (obj.BookingStatus != "")
                    sqlquery = sqlquery + "AND booking_status='" + obj.BookingStatus.ToString() + "' ";

                if (obj.PaymentStatus != "")
                    sqlquery = sqlquery + "AND booking_payment_status='" + obj.PaymentStatus.ToString() + "' ";

                if (obj.IRID != "")
                    sqlquery = sqlquery + "AND a.ir_id='" + obj.IRID.ToString() + "' ";

                if (obj.EmailAddress != "")
                    sqlquery = sqlquery + "AND b.accounts_email='" + obj.EmailAddress.ToString() + "' ";
            }

            MySqlDataReader data;
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand();

            try
            {
                command.CommandText = sqlquery;
                command.Connection = Connection;
                adapter.SelectCommand = command;
                data = command.ExecuteReader();

                response.Status = "success";
                response.Message = "success";

                var rcdList = new List<GetReservation_Response.ReservationList>();

                while (data.Read())
                {
                    // Label1.Text = data[1).ToString
                    GetReservation_Response.ReservationList rcd = new GetReservation_Response.ReservationList();
                    // command.Parameters.AddWithValue("booking_id", obj.BookingRefNo)
                    rcd.ID = (int)data["booking_id"];
                    rcd.RefNo = data["booking_ref_no"].ToString();
                    rcd.BookingRefNo = data["booking_ref_no"].ToString() + "-" + data["booking_id"].ToString();
                    rcd.HotelID = (int)data["hotel_id"];
                    rcd.HotelName = data["hotel_name"].ToString().ToUpper();
                    rcd.HotelStarRate = data["hotel_stars"].ToString();
                    rcd.City = data["hotel_loaction"].ToString().ToUpper();
                    rcd.CountryCode = data["country_code"].ToString().ToUpper();
                    rcd.HotelPhoneNo = data["hotel_phone"].ToString();
                    rcd.HotelEmailAddress = data["hotel_email"].ToString();
                    rcd.HotelWebsite = data["hotel_website"].ToString();
                    rcd.HotelLocation = data["hotel_loaction"].ToString();
                    rcd.HotelImg = data["hotel_img"].ToString();
                    rcd.HotelLang = data["lang"].ToString();
                    rcd.HotelLong = data["long"].ToString();
                    rcd.BookingStatus = data["booking_status"].ToString().ToUpper();
                    rcd.PaymentStatus = data["booking_payment_status"].ToString().ToUpper();
                    rcd.PaymentMethod = data["booking_payment_gateway"].ToString().ToUpper();
                    rcd.SupplierID = int.Parse(data["booking_supplier"].ToString());
                    rcd.SupplierName = data["supplier_name"].ToString().ToUpper();
                    //rcd.BookingDate = Convert.ToDateTime(data["booking_date"].ToString());
                    rcd.BookedByUserID = int.Parse(data["booking_user_id"].ToString());
                    rcd.BookedByUserName = data["full_name"].ToString();
                    rcd.CurrencyCode = data["booking_curr_code"].ToString().ToUpper();
                    rcd.ExchangeRate = Convert.ToDecimal(data["booking_ex_rate"]);
                    rcd.TotalPrice = Convert.ToDecimal(data["total_price"]);
                    rcd.ActualPrice = Convert.ToDecimal(data["actual_price"]);
                    rcd.AmountPaid = Convert.ToDecimal(data["booking_amount_paid"]);

                    if (data["booking_credit_used"] != DBNull.Value)
                        rcd.CreditUsedFlag = true;
                    else
                        rcd.CreditUsedFlag = false;

                    rcd.CreditPaid = Convert.ToDecimal(data["booking_credit_paid"]);
                    rcd.InternationalFee = Convert.ToDecimal(data["int_fee"]);
                    rcd.OutstandingAmount = Convert.ToDecimal(data["booking_remaining"]);
                    rcd.DepositAmount = Convert.ToDecimal(data["booking_deposit"]);
                    rcd.TaxAmount = Convert.ToDecimal(data["booking_tax"]);

                    rcd.TotalPrice_Base = (data["total_price_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["total_price_base"]);
                    rcd.ActualPrice_Base = (data["actual_price_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["actual_price_base"]);
                    rcd.AmountPaid_Base = (data["booking_amount_paid_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["booking_amount_paid_base"]);
                    rcd.CreditPaid_Base = (data["booking_credit_paid_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["booking_credit_paid_base"]);
                    rcd.InternationalFee_Base = (data["int_fee_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["int_fee_base"]);
                    rcd.OutstandingAmount_Base = (data["booking_remaining_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["booking_remaining_base"]);
                    rcd.DepositAmount_Base = (data["booking_deposit_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["booking_deposit_base"]);
                    rcd.TaxAmount_Base = (data["booking_tax_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["booking_tax_base"]);

                    rcd.CheckInDate = Convert.ToDateTime(data["booking_checkin"]);
                    rcd.CheckOutDate = Convert.ToDateTime(data["booking_checkout"]);
                    rcd.Nights = int.Parse(data["booking_nights"].ToString());
                    rcd.Rooms = int.Parse(data["booking_rooms"].ToString());
                    rcd.Adult = int.Parse(data["booking_adults"].ToString());
                    rcd.Children = int.Parse(data["booking_childs"].ToString());

                    if (data["booking_payment_date"] != DBNull.Value)
                        rcd.PaymentDate = Convert.ToDateTime(data["booking_payment_date"]);

                    if (data["booking_txn_id"] != DBNull.Value)
                        rcd.PaymentTrxID = data["booking_txn_id"].ToString();
                    else
                        rcd.PaymentTrxID = "";

                    rcd.InvoiceURL = data["invoice_url"].ToString();
                    rcd.BookerNationality = data["nationality"].ToString();
                    rcd.SupplierBookingID = data["supplier_booking_id"].ToString();
                    rcd.SupplierBookingCode = data["supplier_booking_code"].ToString();

                    if (data["cancellation_date"] != DBNull.Value)
                        rcd.CancellationDate = Convert.ToDateTime(data["cancellation_date"]);

                    if (data["cancellation_in_progress"] != DBNull.Value)
                        rcd.CancellationInProgress = true;
                    else
                        rcd.CancellationInProgress = false;

                    rcd.SupplierCancellationCharge = (data["supplier_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["supplier_cancellation_charge"]);
                    rcd.SupplierCancellationCharge_Base = (data["supplier_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["supplier_cancellation_charge_base"]);
                    rcd.AdminCancellationCharge = (data["admin_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["admin_cancellation_charge"]);
                    rcd.AdminCancellationCharge_Base = (data["admin_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["admin_cancellation_charge_base"]);
                    rcd.RefundAmount = (data["refund_amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["refund_amount"]);
                    rcd.RefundAmount_Base = (data["refund_amount_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["refund_amount_base"]);
                    rcd.RefundCredit = (data["refund_credit"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["refund_credit"]);
                    rcd.RefundCredit_Base = (data["refund_credit_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["refund_credit_base"]);
                  

                    if (data["refund_date"] != DBNull.Value)
                        rcd.RefundDate = Convert.ToDateTime(data["refund_date"]);

                    if (data["refunded_by"] != DBNull.Value)
                    {
                        rcd.RefundedBy = int.Parse(data["refunded_by"].ToString());
                        rcd.RefundedByName = data["refunded_by_name"].ToString();
                    }
                    else
                        rcd.RefundedByName = "";

                    if (data["revision_date"] != DBNull.Value)
                        rcd.RevisionDate = Convert.ToDateTime(data["revision_date"]);

                    if (data["revised_by"] != DBNull.Value)
                    {
                        rcd.RevisedBy = int.Parse(data["revised_by"].ToString());

                        if (data["revised_by_name"] != DBNull.Value)
                            rcd.RevisedByName = data["revised_by_name"].ToString();
                        else
                            rcd.RevisedByName = "";
                    }
                    else
                        rcd.RevisedByName = "";

                    rcd.RevertedSupplierCancellationCharge = (data["reverted_supplier_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["reverted_supplier_cancellation_charge"]);
                    rcd.RevertedSupplierCancellationCharge_Base = (data["reverted_supplier_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["reverted_supplier_cancellation_charge_base"]);
                    rcd.RevertedAdminCancellationCharge = (data["reverted_admin_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["reverted_admin_cancellation_charge"]);
                    rcd.RevertedAdminCancellationCharge_Base = (data["reverted_admin_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["reverted_admin_cancellation_charge_base"]);
                    rcd.FinalSupplierCancellationCharge = (data["final_supplier_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["final_supplier_cancellation_charge"]);
                    rcd.FinalSupplierCancellationCharge_Base = (data["final_supplier_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["final_supplier_cancellation_charge_base"]);
                    rcd.FinalAdminCancellationCharge = (data["final_admin_cancellation_charge"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["final_admin_cancellation_charge"]);
                    rcd.FinalAdminCancellationCharge_Base = (data["final_admin_cancellation_charge_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["final_admin_cancellation_charge_base"]);
                    rcd.RevisedRefundAmount = (data["revised_refund_amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["revised_refund_amount"]);
                    rcd.FinalAdminCancellationCharge_Base = (data["revised_refund_amount_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["revised_refund_amount_base"]);
                    rcd.RevisedRefundCredit = (data["revised_refund_credit"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["revised_refund_credit"]);
                    rcd.RevisedRefundCredit_Base = (data["revised_refund_credit_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["revised_refund_credit_base"]);
                    rcd.AdditionalRefundAmount = (data["additional_refund_amount"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["additional_refund_amount"]);
                    rcd.AdditionalRefundAmount_Base = (data["additional_refund_amount_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["additional_refund_amount_base"]);
                    rcd.AdditionalRefundCredit = (data["additional_refund_credit"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["additional_refund_credit"]);
                    rcd.FinalAdminCancellationCharge_Base = (data["additional_refund_credit_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["additional_refund_credit_base"]);
               
                    rcd.CancellationPolicy = data["booking_cancellation_policy"].ToString();
                    rcd.Session_Lang = data["session_lang"].ToString();

                    if (data["additional_refund_credit"] != DBNull.Value)
                        rcd.IR_ID = data["ir_id"].ToString();                               
                    else
                        rcd.IR_ID = "";

                    rcd.Primary_Guest_Title = data["primary_guest_title"].ToString();
                    rcd.Primary_Guest_First_Name = data["primary_guest_first_name"].ToString();
                    rcd.Primary_Guest_Last_Name = data["primary_guest_last_name"].ToString();
                    rcd.Primary_Guest_Email = data["primary_guest_email"].ToString();
                    rcd.Primary_Guest_Contact = data["primary_guest_contact"].ToString();

                    rcd.NightsRedeemed = (data["nights_redeemed"] == DBNull.Value) ? 0 : int.Parse(data["nights_redeemed"].ToString());
                    rcd.UsageFeeCollected = (data["usage_fee_collected"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["usage_fee_collected"]);
                    rcd.UsageFeeCollected_Base = (data["usage_fee_collected_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["usage_fee_collected_base"]);
                    rcd.PXFCollected = (data["pxf_collected"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["pxf_collected"]);
                    rcd.PXFCollected_Base = (data["pxf_collected_base"] == DBNull.Value) ? 0 : Convert.ToDecimal(data["pxf_collected_base"]);

                    rcdList.Add(rcd);
                }

                data.Close();
                response.Response = rcdList;
            }
            catch (Exception ex)
            {
                response.Message = "Error [RetrieveReservation] : " + ex.Message;
            }

            return response;
        }

    }
}
