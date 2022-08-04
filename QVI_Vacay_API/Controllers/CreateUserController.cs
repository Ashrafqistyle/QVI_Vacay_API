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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace QVI_Vacay_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreateUserController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public string Index()
        {
            string connString = this.Configuration.GetConnectionString("QVI_Vacay_APIContext");
            return connString;
        }

        private readonly QVI_Vacay_APIContext _contect;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CreateUserController> _logger;

        public CreateUserController(ILogger<CreateUserController> logger)
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
        public Account_Response PostValue(Account_Request obj)
        {
            //Test
            var conn = ConfigSettings.SSMSconnection;
            var re = Request;
            var headers = re.Headers;
            string currentToken = string.Empty;
            Account_Response response = new Account_Response();
            Signup_Response signupResponse = new Signup_Response();

            if (obj != null)
            {
                AccountData data = new AccountData();
                ClientData clientData = new ClientData();
                Token_Response tokenResponse = new Token_Response();
                Token token = new Token();
                ClientList clientList;
                RequestsList Request = new RequestsList();
                var RequestList = new List<RequestsList>();
                bool checkTokenStatus = false;
                bool updateTokenStatus = false;
                bool insertNewClientTokenStatus = false;
                string val = string.Empty;
                string MD5Hash = string.Empty;
                string ClientID = string.Empty;
                string UserId = string.Empty;
                Signup_Response oResponse = new Signup_Response();
                string ClientSystemName = ConfigSettings.ClientSystemName;
                string ClientScreenName = ConfigSettings.ClientScreenNameCU;

                response.Status = "Error";
                response.ResultType = "Fail";
                response.Message = "Something wrong";
                response.UserId = "";

                string authHeader = HttpContext.Request.Headers["Authorization"];
                currentToken = authHeader.Replace("Bearer ", "");

                ClientID = clientData.GetClientID(ClientSystemName, ClientScreenName);
                checkTokenStatus = clientData.CheckToken(currentToken, ClientID);

                if (checkTokenStatus == true)
                {
                    updateTokenStatus = clientData.UpdateClientInfo(currentToken, ClientID);

                    if (updateTokenStatus == true)
                    {
                        oResponse = data.CreateNewAccount(obj);

                        if (oResponse.Response == true)
                        {
                            if (oResponse.Error.Secret.Length > 30)
                            {
                                UserId = data.GetNewAccount(obj);
                                UserId = "12";

                                if (UserId != "")
                                {
                                    insertNewClientTokenStatus = clientData.InsertNewClient(ClientSystemName, obj, oResponse);

                                    if (insertNewClientTokenStatus == true)
                                    {
                                        response.Status = "success";
                                        response.ResultType = "success";
                                        response.Message = "success";
                                        response.UserId = UserId;
                                    }
                                    else
                                        response.Message = "Fail to Insert New Client";

                                    return response;
                                }
                                else
                                    response.Message = "User with this Email and IrId not exist";

                                return response;
                            }
                            else
                            {
                                response.Message = "Secret Key not valid";
                                return response;
                            }
                        }
                        else
                        {
                            response.Message = oResponse.Error.Msg;
                            return response;
                        }  
                    }
                }
                else
                {
                    clientList = clientData.GetClient(ClientSystemName, ClientScreenName);

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
                                        oResponse = data.CreateNewAccount(obj);

                                        if (oResponse.Response == true)
                                        {
                                            if (oResponse.Error.Secret.Length > 30)
                                            {
                                                UserId = data.GetNewAccount(obj);
                                                UserId = "12";

                                                if (UserId != "")
                                                {
                                                    insertNewClientTokenStatus = clientData.InsertNewClient(ClientSystemName, obj, oResponse);

                                                    if (insertNewClientTokenStatus == true)
                                                    {
                                                        response.Status = "success";
                                                        response.ResultType = "success";
                                                        response.Message = "success";
                                                        response.UserId = UserId;
                                                    }
                                                    else
                                                        response.Message = "Fail to Insert New Client";

                                                    return response;
                                                }
                                                else
                                                    response.Message = "User with this Email and IrId not exist";

                                                return response;
                                            }
                                            else
                                            {
                                                response.Message = "Secret Key not valid";

                                                return response;
                                            }
                                        }
                                        else
                                        {
                                            response.Message = oResponse.Error.Msg;
                                            return response;
                                        }
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
                return response;
            }
            return response;
        }
    }
}
