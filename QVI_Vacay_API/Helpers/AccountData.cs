using QVI_Vacay_API.Models;
using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using QVI_Vacay_API.Data;

namespace QVI_Vacay_API.Helpers
{
    public class AccountData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public Signup_Response CreateNewAccount(Account_Request obj)
        {
            string MD5Hash = string.Empty;
            var oRequest = new Signup_Request();
            var oResponse = new Signup_Response();

            if(obj != null)
            {
                try
                {
                    oRequest.first_name = obj.ai_first_name;
                    oRequest.last_name = obj.ai_last_name;
                    oRequest.phone = obj.ai_mobile;
                    oRequest.ir_id = obj.ir_id;
                    oRequest.email = obj.accounts_email;
                    oRequest.password = obj.accounts_password;
                    oRequest.address = obj.ai_address_1;
                    oRequest.type = "customers";
                    oRequest.account_status = "Active";
                    oRequest.expiration_date = obj.accounts_expiry.ToString("yyyy-MM-dd HH:mm:ss");
                    oRequest.ai_nationality = obj.ai_nationality;
                    oRequest.ai_country = obj.ai_country;
                    oRequest.ai_state = obj.ai_state;
                    oRequest.ai_city = obj.ai_city;
                    oResponse = WebService.CallWebServiceSignup(oRequest);
                    return oResponse;
                }
                catch (Exception ex)
                {
                    return oResponse;
                }
            }
            else
            {
                return oResponse;
            }

        }

        public string GetNewAccount(Account_Request obj)
        {
            string UserId = string.Empty;

            using (MySqlConnection conn = new MySqlConnection(strconMysql))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT accounts_id " +
                                                               $"FROM pt_accounts " +
                                                               $"WHERE ir_id = @ir_id " +
                                                               $"AND accounts_email = @accounts_email " +
                                                               $"ORDER by accounts_id Desc LIMIT 1 ", conn))
                {
                    try
                    {
                        using (var result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                UserId = (string)result.GetValue("Name");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return UserId;
        }
    }
}
