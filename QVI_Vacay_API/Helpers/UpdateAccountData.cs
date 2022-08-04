using QVI_Vacay_API.Models;
using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using QVI_Vacay_API.Data;

namespace QVI_Vacay_API.Helpers
{
    public class UpdateAccountData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public bool UpdateAccount(UpdateAccount_Request obj)
        {
            string sqlquery = string.Empty;
            string query = string.Empty;
            string firstChar = string.Empty;

            sqlquery = "UPDATE pt_accounts " + "SET ";

            if (obj.ir_id != "")
                query = query + " ir_id = '" + obj.ir_id + "' ";

            if (obj.accounts_email != "")
                query = query + ", accounts_email = '" + obj.accounts_email + "' ";

            if (obj.ai_title != "")
                query = query + ", ai_title = '" + obj.ai_title + "' ";

            if (obj.ai_first_name != "")
                query = query + ", ai_first_name = '" + obj.ai_first_name + "' ";

            if (obj.ai_last_name != "")
                query = query + ", ai_last_name = '" + obj.ai_last_name + "' ";

            if (obj.ai_dob != "")
                query = query + ", ai_dob = '" + obj.ai_dob + "' ";

            if (obj.ai_nationality != "")
                query = query + ", ai_nationality = '" + obj.ai_nationality + "' ";

            if (obj.ai_country != "")
                query = query + ", ai_country = '" + obj.ai_country + "' ";

            if (obj.ai_state != "")
                query = query + ", ai_state = '" + obj.ai_state + "' ";

            if (obj.ai_city != "")
                query = query + ", ai_city = '" + obj.ai_city + "' ";

            if (obj.ai_address_1 != "")
                query = query + ", ai_address_1 = '" + obj.ai_address_1 + "' ";

            if (obj.ai_address_2 != "")
                query = query + ", ai_address_2 = '" + obj.ai_address_2 + "' ";

            if (obj.ai_mobile != "")
                query = query + ", ai_mobile = '" + obj.ai_mobile + "' ";

            if (obj.ai_fax != "")
                query = query + ", ai_fax = '" + obj.ai_fax + "' ";

            if (obj.ai_postal_code != "")
                query = query + ", ai_postal_code = '" + obj.ai_postal_code + "' ";

            if (obj.ai_passport != "")
                query = query + ", ai_passport = '" + obj.ai_passport + "' ";

            if (obj.facebook_id != "")
                query = query + ", facebook_id = '" + obj.facebook_id + "' ";

            if (obj.expiration_date != "")
                query = query + ", expiration_date = '" + obj.expiration_date + "' ";

            if (obj.ir_id != "")
                query = query + " WHERE  ir_id= '" + obj.ir_id + "' ";
            else if (obj.accounts_email != "")
                query = query + " WHERE  accounts_email= '" + obj.accounts_email + "' ";

            // If first char got comma, then remove it
            firstChar = query.Substring(0, 1);

            if (firstChar == ",")
                query = query.Remove(0, 1);

            sqlquery = sqlquery + query;

            using (MySqlConnection connection = new MySqlConnection(strconMysql))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("", connection))
                {
                    command.CommandText = sqlquery;
                    command.ExecuteNonQuery();
                }
            }
            return true;
        }

    }
}
