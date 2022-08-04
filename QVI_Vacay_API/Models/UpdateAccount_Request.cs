using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QVI_Vacay_API.Models
{
    public class UpdateAccount_Request
    {
        public string ir_id { get; set; }
        public string accounts_email { get; set; }
        public string ai_title { get; set; }
        public string ai_first_name { get; set; }
        public string ai_last_name { get; set; }
        public string ai_dob { get; set; }
        public string ai_nationality { get; set; }
        public string ai_country { get; set; }
        public string ai_state { get; set; }
        public string ai_city { get; set; }
        public string ai_address_1 { get; set; }
        public string ai_address_2 { get; set; }
        public string ai_mobile { get; set; }
        public string ai_fax { get; set; }
        public string ai_postal_code { get; set; }
        public string ai_passport { get; set; }
        public string facebook_id { get; set; }
        public string expiration_date { get; set; }
    }
}
