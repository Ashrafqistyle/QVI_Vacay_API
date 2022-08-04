
namespace QVI_Vacay_API.Helpers
{
    using Newtonsoft.Json;
    using QVI_Vacay_API.Data;
    using QVI_Vacay_API.Models;
    using System.IO;
    using System.Net;

    public class WebService
    {
        public string CallWebServiceMD5HashGenerator(object oData)
        {
            string oResponse = string.Empty;
            string errDetail = string.Empty;
            string url = ConfigSettings.ClientURLHash;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "text/plain";
                httpWebRequest.ContentType = "application/json";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(oData);
                    streamWriter.Write(json);
                    streamWriter.Close();
                }

                HttpWebResponse httpSubmit = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpSubmit.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    oResponse = result.Replace("\"", "");
                    streamReader.Close();
                }

                httpSubmit.Close();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    using (Stream responseStr = response.GetResponseStream())
                    {
                        errDetail = new StreamReader(responseStr).ReadToEnd();
                    }
                }
            }
            return oResponse;
        }

        public static Token_Response CallWebServiceToken(object oData)
        {
            Token_Response oResponse = new Token_Response();
            string errDetail = string.Empty;
            string url = ConfigSettings.ClientURLToken;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "text/plain";
                httpWebRequest.ContentType = "application/json";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(oData);
                    streamWriter.Write(json);
                    streamWriter.Close();
                }

                HttpWebResponse httpSubmit = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpSubmit.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    oResponse = JsonConvert.DeserializeObject<Token_Response>(result);
                    streamReader.Close();
                }

                httpSubmit.Close();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    using (Stream responseStr = response.GetResponseStream())
                    {
                        errDetail = new StreamReader(responseStr).ReadToEnd();
                    }
                }
            }
            return oResponse;
        }

        public static Signup_Response CallWebServiceSignup(object oData)
        {
            Signup_Response oResponse = new Signup_Response();
            string errDetail = string.Empty;
            string url = ConfigSettings.ClientURLSignup;

            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "text/plain";
                httpWebRequest.ContentType = "application/json";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(oData);
                    streamWriter.Write(json);
                    streamWriter.Close();
                }

                HttpWebResponse httpSubmit = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpSubmit.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    oResponse = JsonConvert.DeserializeObject<Signup_Response>(result);
                    streamReader.Close();
                }

                httpSubmit.Close();
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    using (Stream responseStr = response.GetResponseStream())
                    {
                        errDetail = new StreamReader(responseStr).ReadToEnd();
                    }
                }
            }

            return oResponse;
        }
    }
}
