using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QVI_Vacay_API.Data;
using QVI_Vacay_API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static QVI_Vacay_API.Models.Client_Request;
using static QVI_Vacay_API.Models.Token_Request;
using QVI_Vacay_API.Helpers;

namespace QVI_Vacay_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly QVI_Vacay_APIContext _contect;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Account_Request> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Account_Request
            {
                ir_id = "1",
                ai_first_name = "1",
                ai_last_name = "2"
            })
            .ToArray();
        }

        [HttpPost]
        public Login_Response PostValue(Login_Request obj)
        {
            var re = Request;
            var headers = re.Headers;
            string currentToken = string.Empty;
            Login_Response response = new Login_Response();
            HttpResponseMessage content = new HttpResponseMessage();

            if (obj != null)
            {
                LoginData data = new LoginData();
                ClientData clientData = new ClientData();
                Token_Response tokenResponse = new Token_Response();
                Token token = new Token();
                ClientList clientList = new ClientList();
                RequestsList Request = new RequestsList();
                var RequestList = new List<RequestsList>();
                bool checkTokenStatus = false;
                bool updateTokenStatus = false;
                bool checkValidationStatus = false;
                bool UpdateUserTokenStatus = false;
                string val = string.Empty;
                string MD5Hash = string.Empty;
                string ClientID = string.Empty;
                string UserId = string.Empty;
                string ClientSystemName = ConfigSettings.ClientSystemName;
                string ClientScreenName = ConfigSettings.ClientScreenNameLGN;
                string authHeader = HttpContext.Request.Headers["Authorization"];

                if (authHeader != null)
                {
                    currentToken = authHeader.Replace("Bearer ", "");
                }
                else
                {
                    currentToken = "";
                }

                response.Status = "Error";
                response.ResultType = "Fail";
                response.Message = "Something wrong";
                response.Token = "";
                response.AllowLogin = false;

                checkValidationStatus = clientData.ValidateUser(obj);

                if (checkValidationStatus == true)
                {
                    ClientID = clientData.GetClientIDLogin(obj);

                    if (ClientID != "")
                    {
                        checkTokenStatus = clientData.CheckToken(currentToken, ClientID);

                        if (checkTokenStatus == true)
                        {
                            updateTokenStatus = clientData.UpdateClientInfo(currentToken, ClientID);

                            if (updateTokenStatus == true)
                            {
                                response.Status = "success";
                                response.ResultType = "success";
                                response.Message = "success";
                                response.Token = currentToken;
                                response.AllowLogin = true;
                                return response;
                            }
                        }
                        else
                        {
                            {
                                var withBlock = clientList;
                                withBlock.ClientIdentifier = obj.ClientIdentifier;
                                withBlock.ClientKeySecret = obj.ClientKeySecret;
                            }

                            if (clientList.ClientIdentifier != "")
                            {
                                try
                                {
                                    MD5Hash = clientData.PassClientInfo(clientList);

                                    if (MD5Hash != "")
                                    {
                                        try
                                        {
                                            token.RequestFromSystem = ClientSystemName;
                                            token.FromScreen = ClientScreenName;

                                            {
                                                var withBlock = Request;
                                                withBlock.ClientIdentifier = clientList.ClientIdentifier;
                                                withBlock.Md5Hash = MD5Hash;
                                            }

                                            RequestList.Add(Request);

                                            token.Request = RequestList;
                                            tokenResponse = clientData.GetTokenInfo(token);

                                            if (tokenResponse.AllowLogin == "true")
                                            {
                                                UpdateUserTokenStatus = data.UpdateUserToken(obj, tokenResponse.AccessToken);

                                                if (UpdateUserTokenStatus == true)
                                                {
                                                    response.Status = "success";
                                                    response.ResultType = "success";
                                                    response.Message = "success";
                                                    response.Token = tokenResponse.AccessToken;
                                                    response.AllowLogin = true;
                                                }
                                                else
                                                    response.Message = "Fail To Update User Token Status";

                                                return response;
                                            }
                                            else
                                            {
                                                response.Message = "Token Not found";
                                                return response;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            response.Message = ex.Message.ToString();
                                            return response;
                                        }
                                    }
                                    else
                                    {
                                        response.Message = "Hash Not Found";
                                        return response;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    response.Message = ex.Message.ToString();
                                    return response;
                                }
                            }
                            else
                            {
                                response.Message = "ClientIdentifier & ClientKeySecret Not Found";
                                return response;
                            }
                        }
                    }
                    else
                    {
                        response.Message = "ClientID Not Exist";
                        return response;
                    }
                }
                else
                {
                    response.Message = "This account is not valid";
                    return response;
                }
            }
            else
            {
                return response;
            }
            return response;
        }
    }
}
