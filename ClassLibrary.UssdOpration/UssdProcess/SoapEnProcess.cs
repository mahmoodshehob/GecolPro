//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Net.Http;
//using System.Threading.Tasks;
//using ClassLibrary.Models;
//using ClassLibrary.UssdOpration.Response;

//namespace ClassLibrary.UssdOpration.UssdProcess
//{
//    public class SoapEnProcess //: ApiController
//    {

//        private static XmlResponses xmlResponses = new XmlResponses();
//        private static MenusEn MenuD = new MenusEn();
//        private static Logger logger = new Logger();

//        ///////////////////////////////////////////////////////////////


//        public static async Task<HttpResponseMessage> Process(HttpRequestMessage req)
//        {
//            using (var contentStream = await req.Content.ReadAsStreamAsync())
//            {
//                contentStream.Seek(0, SeekOrigin.Begin);
//                using (var sr = new StreamReader(contentStream))
//                {
//                    string request = sr.ReadToEnd();

//                    return await Processing(request);
//                }
//            }
//        }




//        private static async Task<HttpResponseMessage> Processing(string Xmlreq)
//        {
//            HttpResponseMessage response = new HttpResponseMessage();   // Response XML

//            StockItems cacheMem = new StockItems();                     // Cache Memory Class
//            VarCacheTrans varCache = new VarCacheTrans();               // Cache Object
//            object[] MenuUssd;

//            //List<ReqElement> XmlReqElement = XmlReqConverter(request);

//            MultiRequest multiRequest = await XmlConverterFaster.ConverterFaster(Xmlreq);

//          //  string MainUssdCode = multiRequest.






//            try
//            {
//                logger.LogWrite(multiRequest.TransactionId, ("ServiceCode:" + multiRequest.USSDServiceCode + "|TransactionTime:" + multiRequest.TransactionTime + "|CallingNumber:" + multiRequest.MSISDN + "|Option:" + multiRequest.USSDRequestString).ToString());
//            }
//            catch (Exception ex)
//            {
//            }

//            varCache.TransID = multiRequest.TransactionId;
//            varCache.Input = multiRequest.USSDRequestString;

//            if (!String.IsNullOrWhiteSpace(multiRequest.USSDRequestString))
//            {
//                //
//                // Main Menu
//                //
//                // Condisions for input cases
//                //
//                //

//                if (multiRequest.USSDRequestString == "*" + multiRequest.USSDServiceCode + "#")
//                {
//                    List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
//                    MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, cacheStore[1], multiRequest.USSDRequestString);
//                    response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);

//                }
//                else
//                {
//                    if (varCache.Input == "q")
//                    {
//                        MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, default, multiRequest.USSDRequestString);
//                        response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);
//                    }

//                    //
//                    // Back to Previos Menu
//                    //

//                    if (varCache.Input == "#")
//                    {
//                        List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);

//                        if (varCache.MenuId == "m")  // Close Call if back from main menu
//                        {
//                            MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, cacheStore[1], multiRequest.USSDRequestString);
//                            response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);
//                        }
//                        else    // Action to Back to Previos Menu
//                        {
//                            MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, cacheStore[1], multiRequest.USSDRequestString);
//                            response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);
//                        }
//                    }
//                    else        //Normal Process of USSD
//                    {
//                        List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
//                        MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, cacheStore[1], multiRequest.USSDRequestString);
//                        response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);

//                    }
//                }
//            }
//            else // this for empety or 
//            {
//                MenuUssd = await MenuD.CheckAsync(multiRequest.MSISDN, multiRequest.USSDRequestString, multiRequest.USSDRequestString);
//                response.Content = new StringContent(xmlResponses.ResponceFuelts(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString()), Encoding.UTF8);
//            }




//            if (MenuUssd[1] == "end")
//            {
//                try
//                {
//                    logger.LogWrite(multiRequest.TransactionId, ("ServiceCode:" + multiRequest.USSDServiceCode + "|TransactionTime:" + multiRequest.TransactionTime + "|CallingNumber:" + multiRequest.MSISDN + "|Resutl:" + MenuUssd[0]).ToString());
//                }
//                catch (Exception ex)
//                {
//                }
//            }


//            return response;// USSD Menu Object
//        }
//    }
//}