namespace QVI_Vacay_API.Helpers
{
    using Microsoft.Data.SqlClient;
    using MySql.Data.MySqlClient;
    using QVI_Vacay_API.Data;
    using QVI_Vacay_API.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using static QVI_Vacay_API.Models.Client_Request;
    using static QVI_Vacay_API.Models.Token_Request;

    public class ClientData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public bool ValidateUser(Login_Request obj)
        {
            var ds = new DataSet();
            Int64 Count = 0;

            using (MySqlConnection conn = new MySqlConnection(strconMysql))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand($"SELECT COUNT(*) As Count " +
                                                               $"FROM pt_accounts " +
                                                               $"WHERE accounts_email = @accounts_email " +
                                                               $"AND ir_id = @ir_id ", conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@accounts_email", obj.AccountsEmail);
                        command.Parameters.AddWithValue("@ir_id", obj.ClientIdentifier);
                        using (var result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                Count = (Int64)result.GetValue("Count");
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

            if (Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckToken(string Token, string ClientID)
        {
            var ds = new DataSet();
            int Count = 0;

            using (SqlConnection conn = new SqlConnection(strconSSMS))
            {
                conn.Open();
                using (SqlCommand command = new SqlCommand($"SELECT COUNT(Token) As Count " +
                                                           $"FROM QLL_API_Authentication " +
                                                           $"WHERE Token = @Token " +
                                                           $"AND ClientID = @ClientID ", conn))
                {
                    try
                    {
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        using (var result = command.ExecuteReader())
                        {
                            while (result.Read())
                            {
                                Count = (int)result.GetValue("Count");
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

            if (Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ClientList GetClient(string ClientSystemName, string ClientScreenName)
        {
            var ds = new DataSet();
            var client = new ClientList();

            string strQuery = "SELECT ClientIdentifier, ClientKeySecret " + "FROM QLL_API_Clients " + "WHERE ClientSystemName = @ClientSystemName " + "AND ClientScreenName = @ClientScreenName ";
            using (var connection = new SqlConnection(strconSSMS))
            {
                connection.Open();
                using (var command = new SqlCommand(strQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientSystemName", ClientSystemName);
                    command.Parameters.AddWithValue("@ClientScreenName", ClientScreenName);
                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count == 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        client.ClientIdentifier = row["ClientIdentifier"].ToString();
                        client.ClientKeySecret = row["ClientKeySecret"].ToString();
                    }
                }
            }
            return client;
        }

        public string GetClientID(string ClientSystemName, string ClientScreenName)
        {
            var ds = new DataSet();
            string clientID = string.Empty;

            string strQuery = "SELECT ClientID " + "FROM QLL_API_Clients " + "WHERE ClientSystemName = @ClientSystemName " + "AND ClientScreenName = @ClientScreenName ";
            using (var connection = new SqlConnection(strconSSMS))
            {
                connection.Open();
                using (var command = new SqlCommand(strQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientSystemName", ClientSystemName);
                    command.Parameters.AddWithValue("@ClientScreenName", ClientScreenName);
                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count == 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                        clientID = row["ClientID"].ToString();
                }
            }
            return clientID;
        }

        public string GetClientIDLogin(Login_Request obj)
        {
            var ds = new DataSet();
            string clientID = string.Empty;

            string strQuery = "SELECT ClientID " + "FROM QLL_API_Clients " + "WHERE ClientIdentifier = @ClientIdentifier " + "AND ClientKeySecret = @ClientKeySecret ";
            using (var connection = new SqlConnection(strconSSMS))
            {
                connection.Open();
                using (var command = new SqlCommand(strQuery, connection))
                {
                    command.Parameters.AddWithValue("@ClientIdentifier", obj.ClientIdentifier);
                    command.Parameters.AddWithValue("@ClientKeySecret", obj.ClientKeySecret);
                    var da = new SqlDataAdapter(command);
                    da.Fill(ds);
                }
            }
            if (ds != null)
            {
                if (ds.Tables.Count == 1)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                        clientID = row["ClientID"].ToString();
                }
            }
            return clientID;
        }

        public string PassClientInfo(ClientList obj)
        {
            if (obj != null)
            {
                string MD5Hash = string.Empty;

                try
                {
                    MD5Hash = GetMD5Hash(obj);
                    return MD5Hash;
                }
                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
            else
            {
                return 1.ToString();
            }
        }

        public string GetMD5Hash(Client_Request.ClientList obj)
        {
            var WebService = new WebService();
            var client = new Clients();
            var clientList = new List<Client_Request.ClientList>();
            string MD5Hash = string.Empty;

            clientList.Add(obj);
            client.Client = clientList;

            MD5Hash = WebService.CallWebServiceMD5HashGenerator(client).ToString();
            return MD5Hash;

        }

        public Token_Response GetTokenInfo(Token obj)
        {
            string MD5Hash = string.Empty;
            var oResponse = new Token_Response();

            if (obj != null)
            {
                try
                {
                    oResponse = WebService.CallWebServiceToken(obj);
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

        public bool InsertNewClient(string ClientSystemName, Account_Request Account, Signup_Response Signup)
        {
            //string ClientScreenName = ConfigurationManager.AppSettings("ClientScreenNameLGN");
            string ClientScreenName = "Login";

            try
            {
                using (var connection = new SqlConnection(strconSSMS))
                {
                    connection.Open();
                    using (var command = new SqlCommand("", connection))
                    {
                        command.CommandText = "INSERT INTO QLL_API_Clients (ClientSystemName, ClientScreenName, ClientIdentifier, ClientKeySecret, ClientType, TokenUsageAllowed, ClientCreationDate) " + "VALUES(@ClientSystemName, @ClientScreenName, @ClientIdentifier, @ClientKeySecret, @ClientType, @TokenUsageAllowed, @ClientCreationDate) ";

                        command.Parameters.AddWithValue("@ClientSystemName", ClientSystemName);
                        command.Parameters.AddWithValue("@ClientScreenName", ClientScreenName);
                        command.Parameters.AddWithValue("@ClientIdentifier", Account.ir_id);
                        command.Parameters.AddWithValue("@ClientKeySecret", Signup.Error.Secret);
                        command.Parameters.AddWithValue("@ClientType", "API");
                        command.Parameters.AddWithValue("@TokenUsageAllowed", 1);
                        command.Parameters.AddWithValue("@ClientCreationDate", DateTime.Now);
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

        public bool InsertClientInfo(Token_Response TokenResponse, string ClientID)
        {
            try
            {
                using (var connection = new SqlConnection(strconSSMS))
                {
                    connection.Open();
                    using (var command = new SqlCommand("", connection))
                    {
                        command.CommandText = "INSERT INTO QLL_API_Authentication (ClientID, Token, TokenUsageAllowed, TokenUsedFlag, TokenExpiryDate, TokenCreationDate) " + "VALUES(@ClientID, @Token, @TokenUsageAllowed, @TokenUsedFlag, @TokenExpiryDate, @TokenCreationDate) ";

                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@Token", TokenResponse.AccessToken);
                        command.Parameters.AddWithValue("@TokenUsageAllowed", 1);
                        command.Parameters.AddWithValue("@TokenUsedFlag", 1);
                        command.Parameters.AddWithValue("@TokenExpiryDate", TokenResponse.TokenExpireUtcDate);
                        command.Parameters.AddWithValue("@TokenCreationDate", DateTime.Now);
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

        public bool UpdateClientInfo(string currentToken, string ClientID)
        {
            //int TokenExtendTime = ConfigurationManager.AppSettings("TokenExtendTime");
            int TokenExtendTime = int.Parse(ConfigSettings.TokenExtendTime);

            try
            {
                using (var connection = new SqlConnection(strconSSMS))
                {
                    connection.Open();
                    // utilities.WriteLog("Info", "Update Record- Connection OK")
                    using (var command = new SqlCommand("", connection))
                    {
                        command.CommandText = "UPDATE QLL_API_Authentication " + 
                                              "SET TokenExpiryDate = @TokenExpiryDate, " +
                                              "TokenUsedFlag = '1' " +
                                              "WHERE ClientID = @ClientID " + 
                                              "AND Token = @Token ";
                        command.Parameters.AddWithValue("@TokenExpiryDate", DateTime.Now.AddMinutes(TokenExtendTime));
                        command.Parameters.AddWithValue("@ClientID", ClientID);
                        command.Parameters.AddWithValue("@Token", currentToken);
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
