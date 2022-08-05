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
using System.Text;

namespace QVI_Vacay_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpdateAccountController : ControllerBase
    {
        private readonly QVI_Vacay_APIContext _contect;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<UpdateAccountController> _logger;

        public UpdateAccountController(ILogger<UpdateAccountController> logger)
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
        public UpdateAccount_Response PostValue(UpdateAccount_Request obj)
        {
            Utilities InputParameters = new Utilities();
            var re = Request;
            var headers = re.Headers;
            string currentToken = string.Empty;
            UpdateAccount_Response response = new UpdateAccount_Response();

            if (obj != null)
            {
                UpdateAccountData data = new UpdateAccountData();
                ClientData clientData = new ClientData();
                Token_Response tokenResponse = new Token_Response();
                Token token = new Token();
                ClientList clientList;
                RequestsList Request = new RequestsList();
                var RequestList = new List<RequestsList>();
                bool checkTokenStatus = false;
                bool updateTokenStatus = false;
                bool updateAccountStatus = false;
                string val = string.Empty;
                string MD5Hash = string.Empty;
                string ClientID = string.Empty;
                string UserId = string.Empty;
                string ClientSystemName = ConfigSettings.ClientSystemName;
                string ClientScreenName = ConfigSettings.ClientScreenNameUA;
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

                ClientID = clientData.GetClientID(ClientSystemName, ClientScreenName);
                checkTokenStatus = clientData.CheckToken(currentToken, ClientID);

                if (checkTokenStatus == true)
                {
                    updateTokenStatus = clientData.UpdateClientInfo(currentToken, ClientID);

                    if (updateTokenStatus == true)
                    {
                        updateAccountStatus = data.UpdateAccount(obj);

                        if (updateAccountStatus == true)
                        {
                            response.Status = "success";
                            response.ResultType = "success";
                            response.Message = "success";
                        }
                        else
                            response.Message = "Fail to Update Account";

                        return response;
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
                                        updateAccountStatus = data.UpdateAccount(obj);

                                        if (updateAccountStatus == true)
                                        {
                                            response.Status = "success";
                                            response.ResultType = "success";
                                            response.Message = "success";
                                        }
                                        else
                                            response.Message = "Fail to Update Account";

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
                return response;
            }
            return response;
        }

    }
}
