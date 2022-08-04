using QVI_Vacay_API.Models;
using System;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using QVI_Vacay_API.Data;

namespace QVI_Vacay_API.Helpers
{
    public class LoginData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public bool UpdateUserToken(Login_Request obj, string token)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    // utilities.WriteLog("Info", "Create New Record- Connection OK")
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText = "UPDATE pt_accounts " + 
                                              "SET access_token = @access_token " + 
                                              "WHERE accounts_email = @accounts_email " + 
                                              "AND ir_id = @ir_id ";

                        command.Parameters.AddWithValue("@access_token", token);
                        command.Parameters.AddWithValue("@accounts_email", obj.AccountsEmail);
                        command.Parameters.AddWithValue("@ir_id", obj.ClientIdentifier);
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
