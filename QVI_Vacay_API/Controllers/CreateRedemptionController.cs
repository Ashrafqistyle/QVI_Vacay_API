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
    public class CreateRedemptionController : ControllerBase
    {
        private readonly QVI_Vacay_APIContext _contect;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<CreateRedemptionController> _logger;

        public CreateRedemptionController(ILogger<CreateRedemptionController> logger)
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
        public CreateRedemption_Response PostValue(CreateRedemption_Request obj)
        {
            var re = Request;
            var headers = re.Headers;
            string currentToken = string.Empty;
            CreateRedemption_Response response = new CreateRedemption_Response();

            if (obj != null)
            {
                CreateRedemptionData data = new CreateRedemptionData();
                ClientData clientData = new ClientData();
                Token_Response tokenResponse = new Token_Response();
                Token token = new Token();
                ClientList clientList;
                RequestsList Request = new RequestsList();
                var RequestList = new List<RequestsList>();
                bool checkTokenStatus = false;
                bool updateTokenStatus = false;
                bool checkAccountIdStatus = false;
                bool CreateRedemptionStatus = false;
                bool CreateRedemptionRepoStatus = false;
                bool updateAccountBalanceStatus = false;
                string val = string.Empty;
                string MD5Hash = string.Empty;
                string ClientID = string.Empty;
                string UserId = string.Empty;
                string RedemptionMovementId = string.Empty;
                double NightBalance = 0;
                double PointBalance = 0;
                string ClientSystemName = ConfigSettings.ClientSystemName;
                string ClientScreenName = ConfigSettings.ClientScreenNameCC;
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
                        checkAccountIdStatus = data.CheckAccountId(obj.account_id);

                        if (checkAccountIdStatus == true)
                        {
                            CreateRedemptionStatus = data.CreateRedemptionMovement(obj);

                            if (CreateRedemptionStatus == true)
                            {
                                RedemptionMovementId = data.GetNewRedemptionMovement(obj);

                                if (RedemptionMovementId != "")
                                {
                                    CreateRedemptionRepoStatus = data.CreateRedemptionRepository(obj, RedemptionMovementId);

                                    if (CreateRedemptionRepoStatus == true)
                                    {
                                        NightBalance = data.GetAccountBalance(obj.account_id, "Night");
                                        PointBalance = data.GetAccountBalance(obj.account_id, "Point");

                                        updateAccountBalanceStatus = data.UpdateAccountBalance(obj.account_id, NightBalance, PointBalance);

                                        if (updateAccountBalanceStatus == true)
                                        {
                                            response.Status = "success";
                                            response.ResultType = "success";
                                            response.Message = "success";
                                        }
                                        else
                                            response.Message = "Fail to Update Account Balance";
                                    }
                                    else
                                        response.Message = "Fail to Create Redemption Repository";
                                }
                                else
                                    response.Message = "Redemption Movement Id Not Exist";
                            }
                            else
                                response.Message = "Fail to Create Redemption Movement";
                        }
                        else
                            response.Message = "Account Id Not Exist";

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
                                        checkAccountIdStatus = data.CheckAccountId(obj.account_id);

                                        if (checkAccountIdStatus == true)
                                        {
                                            CreateRedemptionStatus = data.CreateRedemptionMovement(obj);

                                            if (CreateRedemptionRepoStatus == true)
                                            {
                                                NightBalance = data.GetAccountBalance(obj.account_id, "Night");
                                                PointBalance = data.GetAccountBalance(obj.account_id, "Point");

                                                updateAccountBalanceStatus = data.UpdateAccountBalance(obj.account_id, NightBalance, PointBalance);

                                                if (updateAccountBalanceStatus == true)
                                                {
                                                    response.Status = "success";
                                                    response.ResultType = "success";
                                                    response.Message = "success";
                                                }
                                                else
                                                    response.Message = "Fail to Update Account Balance";
                                            }
                                            else
                                                response.Message = "Fail to Create Redemption Repository";
                                        }
                                        else
                                            response.Message = "Account Id Not Exist";

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
