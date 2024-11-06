//using GecolPro.Models.DCB;
//using GecolPro.Models.Gecol;
//using GecolPro.Models.Models;
//using GecolPro.Models.SMPP;
//using GecolPro.WebApi.Interfaces;
//using System.Text;
//using System.Transactions;

//namespace GecolPro.WebApi.BusinessRules
//{
//    public class DatabaseAPIs : IDatabaseAPIs
//    {
//        private readonly DbApiConnection _dbApiPara;
//        private HttpClient client;


//        public DatabaseAPIs(IConfiguration config)
//        {
//            config.GetSection("DbApiConnection").Bind(_dbApiPara);

//            client = new HttpClient
//            {
//                Timeout = TimeSpan.FromMilliseconds(5000)
//            };
//        }


//        public async Task<bool> IsMeterExist(string meter)
//        {
//            try
//            {
//                string endpoint = "IsMeterExist/";

//                string uri = _dbApiPara + endpoint + meter;

//                HttpResponseMessage response = await GetRequest(uri);


//                if (response.IsSuccessStatusCode)
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            catch (Exception ex) 
//            {
//                return false;
//            }
//        }

//        public async Task<bool> CreateNewMeter(string meter , string at, string tt)
//        {
//            string endpoint = "CreateNewMeter/";

//            string uri = _dbApiPara + endpoint + meter +"/"+ at + "/" + tt;

//            HttpResponseMessage response = await GetRequest(uri);


//            if (response.IsSuccessStatusCode)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public async Task<bool> SaveDcblRequest(string MSISDN, string Amount, string Status, string TransactionId)
//        {
//            string endpoint = "CreateNewMeter/";

//            string uri = _dbApiPara + endpoint + MSISDN + "/" + Amount + "/" + Status + "/" + TransactionId;

//            HttpResponseMessage response = await GetRequest(uri);


//            if (response.IsSuccessStatusCode)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public async Task<bool> SaveGecolRequest(string MSISDN, string Amount, string Status, string Token, string UniqueNumber, string TotalTax)
//        {
//            string endpoint = "CreateNewMeter/";

//            string uri = _dbApiPara + endpoint + MSISDN + "/" + Amount + "/" + Status + "/" + Token + "/" + UniqueNumber + "/" + TotalTax;

//            HttpResponseMessage response = await GetRequest(uri);


//            if (response.IsSuccessStatusCode)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private async Task<HttpResponseMessage> GetRequest(string _url)
//        {
//            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, _url);

//            request.Headers.Add("accept", "*/*");

//            var response = await client.SendAsync(request);

//            response.EnsureSuccessStatusCode();

//            return response;
//        }
//    }
//}
