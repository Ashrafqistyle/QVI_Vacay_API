namespace QVI_Vacay_API.Helpers
{
    using MySql.Data.MySqlClient;
    using QVI_Vacay_API.Data;
    using QVI_Vacay_API.Models;
    using System;
    using System.Data;


    public class RefundCreditData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public bool CheckAccountId(string accounts_id)
        {
            DataSet ds = new DataSet();
            Int64 Count = 0;

            try
            {
                var strQuery = "SELECT COUNT(accounts_id) As Count " + 
                               "FROM pt_accounts " + 
                               "WHERE accounts_id = @accounts_id ";

                using (MySqlConnection conn = new MySqlConnection(strconMysql))
                {
                    conn.Open();
                    using (MySqlCommand command = new MySqlCommand($"SELECT COUNT(accounts_id) As Count " +
                                                                   $"FROM pt_accounts " +
                                                                   $"WHERE accounts_id = @accounts_id", conn))
                    {
                        try
                        {
                            command.Parameters.AddWithValue("@accounts_id", accounts_id);
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

            }
            catch (Exception ex)
            {
                return false;
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

        public bool CreateCreditMovement(CreateCredit_Request obj)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText = "INSERT INTO credit_movement (type, creation_date, user_id, ir_id, booking_id, " + 
                                              " status, credit_value, description, internal_description) " + 
                                              "VALUES(@type, @creation_date, @user_id, @ir_id, @booking_id, " + 
                                              " @status, @credit_value, @description, @internal_description) ";

                        command.Parameters.AddWithValue("@type", "Cash In");
                        command.Parameters.AddWithValue("@creation_date", DateTime.Now);
                        command.Parameters.AddWithValue("@user_id", obj.account_id);
                        command.Parameters.AddWithValue("@ir_id", obj.ir_id);
                        command.Parameters.AddWithValue("@booking_id", "");
                        command.Parameters.AddWithValue("@status", "Active");
                        command.Parameters.AddWithValue("@credit_value", obj.credit_value);
                        command.Parameters.AddWithValue("@description", "tripsavr EAZE(Order No: " + obj.order_no + " Item No: " + obj.item_no + ")");
                        command.Parameters.AddWithValue("@internal_description", "");
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

        public string GetNewCreditMovement(CreateCredit_Request obj)
        {
            DateTime Startime = DateTime.Now;
            int count = 0;
            DataSet ds = new DataSet();
            string creditMovementId = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(strconMysql))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("", connection))
                {
                    command.CommandText = "SELECT id " + 
                                          "FROM credit_movement " + 
                                          "WHERE type = @type " + 
                                          "AND user_id = @user_id " + "AND ir_id = @ir_id " + 
                                          "AND booking_id = @booking_id " + 
                                          "AND credit_value = @credit_value " + 
                                          "ORDER by creation_date Desc " + 
                                          "LIMIT 1 ";
                    command.Parameters.AddWithValue("@type", "Cash In");
                    command.Parameters.AddWithValue("@user_id", obj.account_id);
                    command.Parameters.AddWithValue("@ir_id", obj.ir_id);
                    command.Parameters.AddWithValue("@booking_id", 0);
                    command.Parameters.AddWithValue("@credit_value", obj.credit_value);

                    MySqlDataAdapter dAdapter = new MySqlDataAdapter(command);
                    dAdapter.Fill(ds);

                    if (ds != null)
                    {
                        if (ds.Tables.Count == 1)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                                creditMovementId = row["id"].ToString();
                        }
                    }
                }
            }
            return creditMovementId;
        }

        public bool CreateCreditRepository(CreateCredit_Request obj, string credit_movement_id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText =   "INSERT INTO credit_repository (user_id, credit_movement_id, credit_original_value, credit_balance_value, expiration_date, " + 
                                                " credit_usage_info, creation_date, modified_date, order_no, item_no) " + 
                                                "VALUES(@user_id, @credit_movement_id, @credit_original_value, @credit_balance_value, @expiration_date, " + 
                                                " @credit_usage_info, @creation_date, @modified_date, @order_no, @item_no) ";

                        command.Parameters.AddWithValue("@user_id", obj.account_id);
                        command.Parameters.AddWithValue("@credit_movement_id", credit_movement_id);
                        command.Parameters.AddWithValue("@credit_original_value", obj.credit_value);
                        command.Parameters.AddWithValue("@credit_balance_value", obj.credit_value);
                        command.Parameters.AddWithValue("@expiration_date", obj.z_Validity_Date);
                        command.Parameters.AddWithValue("@credit_usage_info", "");
                        command.Parameters.AddWithValue("@creation_date", DateTime.Now);
                        command.Parameters.AddWithValue("@modified_date", DateTime.Now);
                        command.Parameters.AddWithValue("@order_no", obj.order_no);
                        command.Parameters.AddWithValue("@item_no", obj.item_no);
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

        public double GetAccountBalance(string accounts_id)
        {
            double AccountBalance = 0;
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText =   "SELECT SUM(credit_balance_value) As AccountBalance " + 
                                                "FROM credit_repository " + 
                                                "WHERE user_id = @user_id " + 
                                                "AND expiration_date > @CurrentDateTime ";
                        command.Parameters.AddWithValue("@user_id", accounts_id);
                        command.Parameters.AddWithValue("@CurrentDateTime", DateTime.Now);

                        MySqlDataAdapter dAdapter = new MySqlDataAdapter(command);
                        dAdapter.Fill(ds);

                        if (ds != null)
                        {
                            if (ds.Tables.Count == 1)
                            {
                                foreach (DataRow row in ds.Tables[0].Rows)
                                    AccountBalance = (double)row["AccountBalance"];
                            }
                        }
                    }
                }
                return AccountBalance;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public bool UpdateAccountBalance(string accounts_id, double balance)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText =   "UPDATE pt_accounts " + 
                                                " SET balance = @balance " + 
                                                " WHERE accounts_id  = @accounts_id ";
                        command.Parameters.AddWithValue("@balance", balance);
                        command.Parameters.AddWithValue("@accounts_id", accounts_id);
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
