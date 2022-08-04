using QVI_Vacay_API.Models;
using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace QVI_Vacay_API.Helpers
{
    public class Utilities
    {
        public string EnquiryParameters_v1_GetReservation(GetReservation_Request obj)
        {
            string parameter;
            parameter = " DateType : " + obj.DateType;
            parameter = parameter + " Booking Status: " + obj.BookingStatus;
            parameter = parameter + " Payment Status: " + obj.PaymentStatus;
            parameter = parameter + " DateFrom : " + obj.DateFrom;
            parameter = parameter + " DateTo : " + obj.DateTo;
            parameter = parameter + " BookingRefNo : " + obj.BookingRefNo;

            return parameter;
        }
    }
}
