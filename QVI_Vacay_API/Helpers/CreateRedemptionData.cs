namespace QVI_Vacay_API.Helpers
{
    using MySql.Data.MySqlClient;
    using QVI_Vacay_API.Data;
    using QVI_Vacay_API.Models;
    using System;
    using System.Data;


    public class CreateRedemptionData
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

        public bool CreateRedemptionMovement(CreateRedemption_Request obj)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText = "INSERT INTO redemption_movement (type, redemption_type, creation_date, user_id, booking_id, " +
                                              " redemption_nights_value, redemption_points_value, description, internal_description, status) " +
                                              "VALUES(@type, @redemption_type, @creation_date, @user_id, @booking_id, " +
                                              " @redemption_nights_value, @redemption_points_value, @description, @internal_description, @status) ";

                        command.Parameters.AddWithValue("@type", "Deposit");
                        command.Parameters.AddWithValue("@redemption_type", "Nights");
                        command.Parameters.AddWithValue("@creation_date", DateTime.Now);
                        command.Parameters.AddWithValue("@user_id", obj.account_id);
                        command.Parameters.AddWithValue("@booking_id", "");
                        command.Parameters.AddWithValue("@redemption_nights_value", "");
                        command.Parameters.AddWithValue("@redemption_points_value", "");
                        command.Parameters.AddWithValue("@description", "tripsavr EAZE(Order No: " + obj.order_no + " Item No: " + obj.item_no + ")");
                        command.Parameters.AddWithValue("@internal_description", "");
                        command.Parameters.AddWithValue("@status", "Active");
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

        public string GetNewRedemptionMovement(CreateRedemption_Request obj)
        {
            DateTime Startime = DateTime.Now;
            int count = 0;
            DataSet ds = new DataSet();
            string RedemptionMovementId = string.Empty;

            using (MySqlConnection connection = new MySqlConnection(strconMysql))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("", connection))
                {
                    command.CommandText = "SELECT id " + 
                                          "FROM redemption_movement " + 
                                          "WHERE type = @type " + 
                                          "AND user_id = @user_id " + 
                                          "AND booking_id = @booking_id " +
                                          "AND redemption_nights_value = @redemption_nights_value " +
                                          "AND redemption_points_value = @redemption_points_value " +
                                          "ORDER by creation_date Desc " + 
                                          "LIMIT 1 ";
                    command.Parameters.AddWithValue("@type", "Cash In");
                    command.Parameters.AddWithValue("@user_id", obj.account_id);
                    command.Parameters.AddWithValue("@booking_id", 0);
                    command.Parameters.AddWithValue("@redemption_nights_value", obj.redemption_nights_value);
                    command.Parameters.AddWithValue("@redemption_points_value", obj.redemption_points_value);

                    MySqlDataAdapter dAdapter = new MySqlDataAdapter(command);
                    dAdapter.Fill(ds);

                    if (ds != null)
                    {
                        if (ds.Tables.Count == 1)
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                                RedemptionMovementId = row["id"].ToString();
                        }
                    }
                }
            }
            return RedemptionMovementId;
        }

        public bool CreateRedemptionRepository(CreateRedemption_Request obj, string Redemption_movement_id)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText = "INSERT INTO redemption_repository (user_id, redemption_type, redemption_movement_id, nights_original_value, nights_balance_value, " +
                                                " points_original_value, points_balance_value, expiration_date, nights_usage_info, points_usage_info, " +
                                                " creation_date, modified_date, order_no, item_no) " +
                                                "VALUES(@user_id, @redemption_type, @redemption_movement_id, @nights_original_value, @nights_balance_value, " +
                                                " @points_original_value, @points_balance_value, @expiration_date, @nights_usage_info, @points_usage_info, " +
                                                " @creation_date, @modified_date, @order_no, @item_no) ";

                        command.Parameters.AddWithValue("@user_id", obj.account_id);
                        command.Parameters.AddWithValue("@redemption_type", "Nights");
                        command.Parameters.AddWithValue("@redemption_movement_id", Redemption_movement_id);
                        command.Parameters.AddWithValue("@nights_original_value", "");
                        command.Parameters.AddWithValue("@nights_balance_value", "");
                        command.Parameters.AddWithValue("@points_original_value", "");
                        command.Parameters.AddWithValue("@points_balance_value", "");
                        command.Parameters.AddWithValue("@expiration_date", obj.z_Validity_Date);
                        command.Parameters.AddWithValue("@nights_usage_info", "");
                        command.Parameters.AddWithValue("@points_usage_info", "");
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

        public double GetAccountBalance(string accounts_id, string balance_type)
        {
            double AccountBalance = 0;
            DataSet ds = new DataSet();
            string BalanceField = string.Empty;

            if (balance_type == "Night")
            {
                BalanceField = "nights_balance_value";
            }
            else
            if (balance_type == "Point")
            {
                BalanceField = "points_balance_value";
            }

            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText = "SELECT SUM(@BalanceField) As AccountBalance " + 
                                                "FROM redemption_repository " + 
                                                "WHERE user_id = @user_id " + 
                                                "AND expiration_date > @CurrentDateTime ";
                        command.Parameters.AddWithValue("@BalanceField", BalanceField);
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

        public bool UpdateAccountBalance(string accounts_id, double nights_balance, double points_balance)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(strconMysql))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand("", connection))
                    {
                        command.CommandText =   "UPDATE pt_accounts " +
                                                " SET redemption_nights_balance = @redemption_nights_balance, " +
                                                " redemption_points_balance = @redemption_points_balance " +
                                                " WHERE accounts_id  = @accounts_id ";
                        command.Parameters.AddWithValue("@redemption_nights_balance", nights_balance);
                        command.Parameters.AddWithValue("@redemption_points_balance", points_balance);
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
