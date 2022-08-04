using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QVI_Vacay_API.Models
{
    public class Product
    {
        [XmlElement("Id")]
        public string Id { get; set; }
        [XmlElement("ProductCode")]
        public string ProductCode { get; set; }
        [XmlElement("ProductName")]
        public string ProductName { get; set; }
        [XmlElement("ValidityInMonths")]
        public int ValidityInMonths { get; set; }
        [XmlElement("SavingsDollars")]
        public int SavingsDollars { get; set; }
        [XmlElement("TravelCredits")]
        public int? TravelCredits { get; set; }


      //  private static AppSettingsReader _settings = new AppSettingsReader();
      //  // Private Shared strcon As String = _settings.GetValue("SSMSconnection", GetType(String))
      //  private static string strcon = ConfigurationManager.ConnectionStrings("SSMSconnection").ConnectionString;

      //  public static List<Product> GetProducts()
      //  {
      //      string strQuery = @"SELECT Id, Name, Code, Platform, ValidityMonths, DiscTierID, RedemptionNights, CreditTopUpValue 
						//FROM QviVacay_Products";
      //      var ds = new DataSet();
      //      List<Product> products = null;
      //      using (var connection = new SqlConnection(strcon))
      //      {
      //          connection.Open();
      //          using (var command = new SqlCommand(strQuery, connection))
      //          {
      //              // command.CommandText = "SELECT Id, Name, Code, Validity, SavingsDollars, TravelCredits FROM QviVacay_Products"
      //              var da = new SqlDataAdapter(command);
      //              da.Fill(ds);
      //          }
      //      }
      //      if (ds is null)
      //      {
      //          if (ds.Tables.Count == 1)
      //          {
      //              products = new List<Product>();
      //              foreach (DataRow row in ds.Tables(0).Rows)
      //              {
      //                  var product = new Product();
      //                  product.Id = row("Id").ToString();
      //                  product.ProductCode = row("Code");
      //                  product.ProductName = row("Name");
      //                  product.SavingsDollars = row("RedemptionNights");
      //                  product.ValidityInMonths = row("ValidityMonths");
      //                  product.TravelCredits = row("CreditTopUpValue");
      //                  products.Add(product);
      //              }
      //          }
      //      }
      //      return products;
      //  }
    }
}
