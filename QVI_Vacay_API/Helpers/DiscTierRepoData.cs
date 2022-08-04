using QVI_Vacay_API.Models;
using MySql.Data.MySqlClient;
using QVI_Vacay_API.Data;

namespace QVI_Vacay_API.Helpers
{
    public class DiscTierRepoData
    {
        string strconSSMS = ConfigSettings.SSMSconnection;
        string strconMysql = ConfigSettings.MySqlconnection;

        public void CreateNewTier(DiscTierRepo_Request obj)
        {
            using (MySqlConnection connection = new MySqlConnection(strconMysql))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("", connection))
                {
                    command.CommandText = "INSERT INTO discount_tier_repository (accounts_id, disc_tier_ID, purchase_date, expiry_date, status, order_no, item_no) " + "VALUES(@accounts_id, @disc_tier_ID, @purchase_date, @expiry_date, @status, @order_no, @item_no) ";

                    command.Parameters.AddWithValue("@accounts_id", obj.ir_id);
                    command.Parameters.AddWithValue("@disc_tier_ID", obj.disc_tier_ID);
                    command.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                    command.Parameters.AddWithValue("@expiry_date", obj.expiry_date);
                    command.Parameters.AddWithValue("@status", "active");
                    command.Parameters.AddWithValue("@order_no", obj.order_no);
                    command.Parameters.AddWithValue("@item_no", obj.item_no);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
