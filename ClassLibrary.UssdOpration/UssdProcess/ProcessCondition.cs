//using ClassLibrary.Models.Cache;
//using ClassLibrary.Models.ModelOfUssd;
//using ClassLibrary.UssdOpration.Cache;
//using ClassLibrary.UssdOpration.Menus;
//using ClassLibrary.UssdOpration.Response;
//using System.Text;

//namespace ClassLibrary.UssdOpration.UssdProcess
//{
//    public class ProcessCondition
//    {
//        //private static XmlResponses xmlResponses = new XmlResponses();

//        ////private static HttpResponseMessage response = new HttpResponseMessage();  // Response XML
//        //private static StockItems cacheMem = new StockItems();                     // Cache Memory Class
//        //private static VarCacheTrans varCache = new VarCacheTrans();               // Cache Object
//        //private static object[] MenuUssd;

//        //private static void MenusX(string Lang)
//        //{
//        //    if (Lang == "Ar")
//        //    {
//        //        MenusAr MenuD = new MenusAr();
//        //    }
//        //    else
//        //    {
//        //        MenusEn MenuD = new MenusEn();
//        //    }

//        //}




//        //public static async Task<HttpResponseMessage> Processing(MultiRequest multiReqElement, string Lang)
//        //{
//        //    HttpResponseMessage response = new HttpResponseMessage();

//        //    await Task.Run(() => {
               
//        //        MenusAr MenuD = new MenusAr();

//        //        //Cache Part and Menu Option
//        //        varCache = new VarCacheTrans()
//        //        {
//        //            TransID = multiReqElement.TransactionId,
//        //            Input = multiReqElement.USSDRequestString
//        //        };


//        //        if (!String.IsNullOrWhiteSpace(multiReqElement.USSDRequestString))
//        //        {
//        //            //
//        //            // Main Menu
//        //            //
//        //            // Condisions for input cases
//        //            //
//        //            //

//        //            if (multiReqElement.USSDRequestString == "*" + multiReqElement.USSDServiceCode + "#")
//        //            {
//        //                List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
//        //                // MenuUssd = MenuD.Check(multiReqElement.MSISDN,cacheStore[1]);
//        //                //response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);



//        //                response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuD.Check(multiReqElement.MSISDN, cacheStore[1])[0].ToString(), MenuD.Check(multiReqElement.MSISDN, cacheStore[1])[1].ToString()), Encoding.UTF8);

//        //            }
//        //            else
//        //            {
//        //                if (varCache.Input == "q")
//        //                {
//        //                    response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
//        //                }

//        //                //
//        //                // Back to Previos Menu
//        //                //

//        //                if (varCache.Input == "#")
//        //                {
//        //                    List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);

//        //                    if (varCache.MenuId == "m")  // Close Call if back from main menu
//        //                    {
//        //                        response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
//        //                    }
//        //                    else    // Action to Back to Previos Menu
//        //                    {
//        //                        response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
//        //                    }
//        //                }
//        //                else        //Normal Process of USSD
//        //                {
//        //                    List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
//        //                    MenuUssd = MenuD.Check(multiReqElement.MSISDN, cacheStore[1]);
//        //                    response.Content = new StringContent(xmlResponses.Responce(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiReqElement.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);

//        //                }
//        //            }
//        //        }
//        //        else // this for empety or 
//        //        {
//        //            MenuUssd = MenuD.Check(multiReqElement.MSISDN, multiReqElement.USSDRequestString);
//        //            response.Content = new StringContent(xmlResponses.ResponceFuelts(multiReqElement.TransactionId, multiReqElement.MSISDN, MenuUssd[0].ToString()), Encoding.UTF8);
//        //        }
              
//        //    }).ConfigureAwait(false);
//        //    return response;

//        //}
        
//    }
//}
