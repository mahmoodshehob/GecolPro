using Microsoft.AspNetCore.Mvc;
using ZGecolPro.SmppClient.Models;
using ZGecolPro.SmppClient.Services.IServices;
using ZGecolPro.SmppClient.Services;
using System.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Text.Json;
using System.Net;
using System.Linq.Expressions;



namespace ZGecolPro.SmppClient.Controllers
{
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private ILoggers _loggers;
        private static DlrMessage dlrMessage = new DlrMessage();
        private KannelModel kannelModel;
        private IGuidService _msgID;



        public MessagesController(IConfiguration config, ILoggers loggers, IGuidService msgID)
        {
            kannelModel = new KannelModel();
            config.GetSection("KannelPara").Bind(kannelModel);
            _loggers = loggers;
            _msgID = msgID;
        }



        [HttpPost]
        [Route("[controller]/Submit")]
        public async Task<IActionResult> Post([FromBody] SmsToKannel message)
        {


            string? smsc_id = "";
            if (String.IsNullOrEmpty(message.Profile))
            {
                smsc_id = $"&smsc={message.Profile}";
            }

            dlrMessage.msgId = _msgID.GetGuid();


            string kannlSendSms = $"http://{kannelModel.HostName}:{kannelModel.Port}/cgi-bin/sendsms?username={kannelModel.UserName}&password={kannelModel.Password}{smsc_id}&from=%2B{message.Sender}&to=%2B{message.Receiver}&charset=UTF-8&coding=2&text={message.Message}";

            string kannlDlr = $"dlr-mask={kannelModel.DlrMask}&dlr-url={kannelModel.DlrEndPoint}/Messages/DLR/{dlrMessage.Phone}/{dlrMessage.msgId}/{dlrMessage.Status}/%t/";

            await _loggers.LogInfoAsync($"Req|sendsms:({kannlSendSms + "&" + kannlDlr})");


            Uri uri = new Uri(kannlSendSms + "&" + kannlDlr);

            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, uri);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();


                await _loggers.LogInfoAsync($"Rsq|Response:({await response.Content.ReadAsStringAsync()})");


                return Ok(await response.Content.ReadAsStringAsync());

            }
            catch (Exception ex)
            {
                await _loggers.LogInfoAsync($"Rsq|({ex.Message})");
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("[controller]/DLR/{phone}/{msgid}/{status}/{deliveryDate}")]
        public async Task<IActionResult> DLR(string phone, string msgid, string status, string deliveryDate)
        {
            try
            {
                phone = phone.Replace("+","");

                await _loggers.LogInfoAsync($"DLR|phone:{phone},msgid:{msgid},status:{status}, deliveryDate:{ deliveryDate}");

                // Validate parameters
                if (string.IsNullOrEmpty(msgid))
                {
                    return BadRequest("Message ID (msgid) is required.");
                }

                // Process the request (dummy response for demonstration)
                var response = new
                {
                    MessageId = msgid,
                    Status = status,
                    Timestamp = DateTime.UtcNow
                };

                return Ok(response); // Return the response as JSON
            }
            catch (Exception ex)
            {
                await _loggers.LogInfoAsync($"exp|{ex.Message}");

                return BadRequest(ex.Message);

            }
        }









        [HttpGet()]
        [Route("[controller]/Status")]
        public async Task<IActionResult> KannelStatus()
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, "http://172.16.31.118:8130/status?password=kannel");
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();


                if (response.IsSuccessStatusCode)
                {
                    string rrr = await response.Content.ReadAsStringAsync();

                    return Ok(JsonSerializer.Serialize(parcing(rrr))); // Return the response as JSON
                
                }




                return BadRequest();

            }
            catch (Exception ex)
            {
                await _loggers.LogInfoAsync($"exp|{ex.Message}");

                return BadRequest(ex.Message);

            }
        }




        private KannelStatus parcing(string bodystatus)
        {
            string inputText = bodystatus;

            // Corrected Box Connections Regex
            string boxConnectionsPattern = @"Box connections:\s+smsbox:\((.*?)\), IP ([0-9.]+) \((\d+) queued\), \(on-line (.*?)\)";
            MatchCollection boxMatches = Regex.Matches(inputText, boxConnectionsPattern);

            var boxConnections = new List<object>();

            foreach (Match match in boxMatches)
            {
                boxConnections.Add(new
                {
                    Type = "smsbox",
                    Details = new
                    {
                        Name = match.Groups[1].Value,
                        IP = match.Groups[2].Value,
                        Queued = int.Parse(match.Groups[3].Value),
                        OnlineTime = match.Groups[4].Value
                    }
                });
            }

            // SMSC Connections Regex (unchanged)
            string smscConnectionsPattern = @"\s*(.*?)\s*SMPP:(.*?):(.*?)/(.*?):(.*?):(.*?) \(online (\d+)s, rcvd: sms (\d+) \((.*?)\) / dlr (\d+) \((.*?)\), sent: sms (\d+) \((.*?)\) / dlr (\d+) \((.*?)\), failed (\d+), queued (\d+) msgs\)";
            MatchCollection smscMatches = Regex.Matches(inputText, smscConnectionsPattern);

            var smscConnections = new List<object>();

            foreach (Match smscMatch in smscMatches)
            {
  



                smscConnections.Add(new
                {
                    Name = smscMatch.Groups[1].Value.Trim(),
                    Protocol = "SMPP",
                    Address = $"{smscMatch.Groups[2].Value}:{smscMatch.Groups[3].Value}/{smscMatch.Groups[4].Value}",
                    ConnectionType = $"{smscMatch.Groups[3].Value}/{smscMatch.Groups[4].Value}",
                    //Account = $"{smscMatch.Groups[5].Value}:{smscMatch.Groups[6].Value}",
                    Account = $"{smscMatch.Groups[5].Value}",

                    Status = new
                    {
                        OnlineSeconds = int.Parse(smscMatch.Groups[7].Value),
                        Received = new
                        {
                            Sms = new
                            {
                                Count = int.Parse(smscMatch.Groups[8].Value),
                                Rate = smscMatch.Groups[9].Value.Split(',')
                            },
                            Dlr = new
                            {
                                Count = int.Parse(smscMatch.Groups[10].Value),
                                Rate = smscMatch.Groups[11].Value.Split(',')
                            }
                        },
                        Sent = new
                        {
                            Sms = new
                            {
                                Count = int.Parse(smscMatch.Groups[12].Value),
                                Rate = smscMatch.Groups[13].Value.Split(',')
                            },
                            Dlr = new
                            {
                                Count = int.Parse(smscMatch.Groups[14].Value),
                                Rate = smscMatch.Groups[15].Value.Split(',')
                            }
                        },
                        Failed = int.Parse(smscMatch.Groups[16].Value),
                        Queued = int.Parse(smscMatch.Groups[17].Value)
                    }
                });







            }

            // Combine results into a single object
            var result = new
            {
                BoxConnections = boxConnections,
                SMSCConnections = smscConnections
            };

            // Serialize to JSON
            string jsonResult = JsonSerializer.Serialize(result);
            var kannelStatus = JsonSerializer.Deserialize<KannelStatus>(jsonResult);

            if (kannelStatus.SMSCConnections.Count() >0)
            {
                for (int i = 0; i < (kannelStatus.SMSCConnections.Count() - 1); i++)
                {
                    switch ($"{kannelStatus.SMSCConnections[i].ConnectionType}")
                    {
                        case "5016/0":
                            kannelStatus.SMSCConnections[i].ConnectionType = "Transmitter";
                            break;
                        case "0/5016":
                            kannelStatus.SMSCConnections[i].ConnectionType = "Receiver";
                            break;
                        case "5016/5016":
                            kannelStatus.SMSCConnections[i].ConnectionType = "Transceiver";
                            break;
                        default:
                            kannelStatus.SMSCConnections[i].ConnectionType = "Transceiver";
                            break;
                    }
                }
            }

            return kannelStatus;
        }
    }
}
