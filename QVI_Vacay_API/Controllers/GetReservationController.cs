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
    public class GetReservationController : ControllerBase
    {
        private readonly QVI_Vacay_APIContext _contect;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<GetReservationController> _logger;

        public GetReservationController(ILogger<GetReservationController> logger)
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
        public GetReservation_Response.GetReservation_ResponseModel PostValue(GetReservation_Request obj)
        {
            DateTime Startime = DateTime.Now;
            GetReservation_Response.GetReservation_ResponseModel response = new GetReservation_Response.GetReservation_ResponseModel();
            HttpResponseMessage content = new HttpResponseMessage();
            Utilities InputParameters = new Utilities();
            var re = Request;
            var headers = re.Headers;
            string currentToken = string.Empty;

            if (obj != null)
            {
                EnquiryData data = new EnquiryData();
                string val;
                string inputStr = string.Empty;
                ClientData clientData = new ClientData();
                Token_Response tokenResponse = new Token_Response();
                Token token = new Token();
                ClientList clientList;
                RequestsList Request = new RequestsList();
                var RequestList = new List<RequestsList>();
                bool checkTokenStatus = false;
                bool updateTokenStatus = false;
                string MD5Hash = string.Empty;
                string ClientID = string.Empty;
                string UserId = string.Empty;
                string ClientSystemName = ConfigSettings.ClientSystemName;
                string ClientScreenName = ConfigSettings.ClientScreenNameGR;

                response.Status = "Error";
                response.Message = "Something wrong";

                string authHeader = HttpContext.Request.Headers["Authorization"];
                currentToken = authHeader.Replace("Bearer ", "");

                ClientID = clientData.GetClientID(ClientSystemName, ClientScreenName);
                checkTokenStatus = clientData.CheckToken(currentToken, ClientID);

                if (checkTokenStatus == true)
                {
                    updateTokenStatus = clientData.UpdateClientInfo(currentToken, ClientID);

                    if (updateTokenStatus == true)
                    {
                        try
                        {
                            inputStr = InputParameters.EnquiryParameters_v1_GetReservation(obj);
                            response = data.RetrieveReservation(obj);
                            return response;
                        }
                        catch (Exception ex)
                        {
                            response.Message = ex.Message.ToString();
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
                                        try
                                        {
                                            response = data.RetrieveReservation(obj);
                                            return response;
                                        }
                                        catch (Exception ex)
                                        {
                                            response.Message = ex.Message.ToString();
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
