using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections;


using ClassLibrary.UssdOpration.Response;
using ClassLibrary.UssdOpration.Menus;
using ClassLibrary.UssdOpration.Cache;
using ClassLibrary.Models.ModelOfUssd;
using ClassLibrary.Models.Cache;
using ClassLibrary.Models;



namespace ClassLibrary.UssdOpration.UssdProcess
{
    public class SoapArProcess : ApiController
    {
        //Constints 

        //static string UssdCode = "*707#";
        // private static Matching matching = new Matching();

        private static XmlResponses xmlResponses = new XmlResponses();
        private static MenusAr MenuD = new MenusAr();

        ///////////////////////////////////////////////////////////////

        // Conver XML to Varialbe
        //

        //private static List<ReqElement> XmlReqConverter(string xmlString)
        //{
        //    List<ReqElement> ReqContent = new List<ReqElement>();
        //    ReqElement reqElement = new ReqElement();
        //    XDocument doc = XDocument.Parse(xmlString);

        //    foreach (XContainer container in doc.Element("methodCall").Element("params").Element("param").Element("value").Element("struct").Descendants("member"))
        //    {
        //        reqElement.Name = container.Descendants("name").FirstOrDefault().LastNode.ToString();
        //        reqElement = container.Descendants("value").FirstOrDefault();

        //        ReqContent.Add(new ReqElement
        //        {
        //            Name = reqElement.Name,
        //            Value = reqElement
        //        });
        //    }
        //    return ReqContent;
        //}

        //

        /// ///////////////////////////////////////////////////////////////


        // Start

        public static async Task<HttpResponseMessage> Process(HttpRequestMessage req)
        {

            StockItems cacheMem = new StockItems();                     // Cache Memory Class
            VarCacheTrans varCache = new VarCacheTrans();               // Cache Object
            HttpResponseMessage response = new HttpResponseMessage();   // Response XML
            //MultiRequest multiRequest       = new MultiRequest();             // Request Object
            object[] MenuUssd;                                          // USSD Menu Object

            using (var contentStream = await req.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    string request = sr.ReadToEnd();


                    //List<ReqElement> XmlReqElement = XmlReqConverter(request);
                    MultiRequest multiRequest = await XmlConverterFaster.ConverterFaster(request);

                    // Items From XML Request

                    //multiRequest.USSDServiceCode    = XmlReqElement.SingleOrDefault(x => x.Name.Contains("USSDServiceCode"));
                    //multiRequest.TransactionId      = XmlReqElement.SingleOrDefault(x => x.Name.Contains("TransactionId"));
                    //multiRequest.USSDRequestString  = XmlReqElement.SingleOrDefault(x => x.Name.Contains("USSDRequestString"));
                    //multiRequest.MSISDN             = XmlReqElement.SingleOrDefault(x => x.Name.Contains("MSISDN"));

                    //Cache Part and Menu Option

                    varCache.TransID = multiRequest.TransactionId;
                    varCache.Input = multiRequest.USSDRequestString;

                    if (!String.IsNullOrWhiteSpace(multiRequest.USSDRequestString))
                    {
                        //
                        // Main Menu
                        //
                        // Condisions for input cases
                        //
                        //

                        if (multiRequest.USSDRequestString == "*" + multiRequest.USSDServiceCode + "#")
                        {
                            List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
                            MenuUssd = MenuD.Check(multiRequest.MSISDN, cacheStore[1]);
                            response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString(), MenuUssd[1].ToString()), Encoding.UTF8);

                        }
                        else
                        {
                            if (varCache.Input == "q")
                            {
                                response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
                            }

                            //
                            // Back to Previos Menu
                            //

                            if (varCache.Input == "#")
                            {
                                List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);

                                if (varCache.MenuId == "m")  // Close Call if back from main menu
                                {
                                    response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
                                }
                                else    // Action to Back to Previos Menu
                                {
                                    response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);
                                }
                            }
                            else        //Normal Process of USSD
                            {
                                List<string> cacheStore = (List<string>)cacheMem.GetAvailableStocks(varCache);
                                MenuUssd = MenuD.Check(multiRequest.MSISDN, cacheStore[1]);
                                response.Content = new StringContent(xmlResponses.Responce(multiRequest.TransactionId, multiRequest.MSISDN, MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[0].ToString(), MenuD.Check(multiRequest.MSISDN, varCache.MenuId)[1].ToString()), Encoding.UTF8);

                            }
                        }
                    }
                    else // this for empety or 
                    {
                        MenuUssd = MenuD.Check(multiRequest.MSISDN, multiRequest.USSDRequestString);
                        response.Content = new StringContent(xmlResponses.ResponceFuelts(multiRequest.TransactionId, multiRequest.MSISDN, MenuUssd[0].ToString()), Encoding.UTF8);
                    }

                    return response;
                }
            }
        }

    }
}