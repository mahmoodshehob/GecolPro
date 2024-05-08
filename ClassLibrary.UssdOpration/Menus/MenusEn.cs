//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web;

//namespace ClassLibrary.UssdOpration.Menus
//{
//    public class MenusEn
//    {

//        private static string resultDmc;

//        public async Task<object[]> CheckAsync(string CallingNumber, string MenuId,string UssdInput)
//        {
            
//            object[] ChosenResult;

//            switch  (MenuId)
//            {
//                case "m0":

//                    ChosenResult = new[] { "USSD Using Soap\n\n1.Ocs Services\n2.Dmc Services\n3.LTT\n4.LPTIC\n", "request", "0" };
//                    break;


//                //
//                //
//                //
//                case "m01":

//                    ChosenResult = new[] { "1.Query Balance\n2.Brand\n# back\n", "request", "0" };
//                    break;
        


//                case "m02":
//                        ChosenResult=  new[] { "DMC Services\n\n1.Brand\n2.IMSI\n3.IMEI\n# back\n", "request", "0" };
//                        break;
//                    //
//                    //
//                    //
//                case "m03":
//                        ChosenResult=  new[] { "LTT Team\n\n1.Seraj\n2.Moad\n# back\n", "request", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    case "m04":
//                        ChosenResult=  new[] { "\n1.Query Balance\n# back\n", "request", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    case "m012":
//                    case "m013":
//                    //case "m021":
//                    //case "m022":
//                    case "m031":
//                    case "m032":


//                        ChosenResult=  new[] { "1.ID\n2.Email\n3.MSISDN\n# back\n", "request", "0" };
//                        break;
//                    //
//                    case "m011":

//                    ChosenResult = new[] { "Here appare your roung", "end", "0" };


//                    //string resultQuery = await OcsOperation.LibyanaSoapFunction.QryUserBasicBal.BindReq(new QryUserBasicBalReqSoap()
//                    //{ 
//                    //    MSISDN = CallingNumber 
//                    //});

//                    ////string resultQuery = await OcsOperation.LibyanaServiceLogic.QueryBalanceProcess.QryUserBasicBalanceOrder(CallingNumber);

//                    //if (resultQuery != "Error")
//                    //{
//                    //    ChosenResult = new[] { "your Balance is :" + resultQuery, "end", "0" };
//                    //}
//                    //else
//                    //{
//                    //    ChosenResult = new[] { "Here appare your roung", "end", "0" };
//                    //}
//                    break;
//                //
//                case "m021":

//                    ChosenResult = new[] { "Here appare your roung", "end", "0" };

//                    //resultDmc = await  DmcCall.Crate(CallingNumber, "handset.brand");

//                    ////string resultQuery = await OcsOperation.LibyanaServiceLogic.QueryBalanceProcess.QryUserBasicBalanceOrder(CallingNumber);

//                    //if (resultDmc != "Error")
//                    //{
//                    //    ChosenResult = new[] { "your Brand :" + resultDmc, "end", "0" };
//                    //}
//                    //else
//                    //{
//                    //    ChosenResult = new[] { "Here appare your roung", "end", "0" };
//                    //}
//                    break;
//                //
//                case "m022":

//                     resultDmc = await DmcCall.Crate(CallingNumber, "sub.imsi");

//                    //string resultQuery = await OcsOperation.LibyanaServiceLogic.QueryBalanceProcess.QryUserBasicBalanceOrder(CallingNumber);

//                    if (resultDmc != "Error")
//                    {
//                        ChosenResult = new[] { "Your Imsi :" + resultDmc, "end", "0" };
//                    }
//                    else
//                    {
//                        ChosenResult = new[] { "Here appare your roung", "end", "0" };
//                    }
//                    break;
//                //
//                case "m023":

//                    resultDmc = await DmcCall.Crate(CallingNumber, "sub.imei");

//                    //string resultQuery = await OcsOperation.LibyanaServiceLogic.QueryBalanceProcess.QryUserBasicBalanceOrder(CallingNumber);

//                    if (resultDmc != "Error")
//                    {
//                        ChosenResult = new[] { "Your Imei :" + resultDmc, "end", "0" };
//                    }
//                    else
//                    {
//                        ChosenResult = new[] { "Here appare your roung", "end", "0" };
//                    }
//                    break;

//                //
//                //
//                //
//                case "m0111":
//                        ChosenResult=  new[] { "ID : 123132", "end", "0" };
//                        break;
//                    //
//                    case "m0112":
//                        ChosenResult=  new[] { "S.Alazabi@libyana@Ly", "end", "0" };
//                        break;
//                    //
//                    case "m0113":
//                        ChosenResult=  new[] { "MSISDN : 218947777128", "end", "0" };
//                        break;
//                    //
//                    case "m0114":
//                        ChosenResult=  new[] { "MSISDN : xxxxxxx", "end", "0" };
//                        break;
//                    //ChosenResult=  new[] { "Check Messages", "end", "0" };
//                    //
//                    //
//                    case "m0121":
//                        ChosenResult=  new[] { "ID : 1343242", "end", "0" };
//                        break;
//                    //
//                    case "m0122":
//                        ChosenResult=  new[] { "Email : alwindy@libyana.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0123":
//                        ChosenResult=  new[] { "MSISDN : 218947776212", "end", "0" };
//                        break;
//                    //
//                    //
//                    case "m0131":
//                        ChosenResult=  new[] { "ID : 9879869", "end", "0" };
//                        break;
//                    //
//                    case "m0132":
//                        ChosenResult=  new[] { "Email : basher@libyana.Ly", "end", "0" };
//                        break;
//                    //
//                    case "m0133":
//                        ChosenResult=  new[] { "MSISDN : 218947777164", "end", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    case "m0211":
//                        ChosenResult=  new[] { "ID : 987987", "end", "0" };
//                        break;
//                    //
//                    case "m0212":
//                        ChosenResult=  new[] { "Email : Omar@almadar.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0213":
//                        ChosenResult=  new[] { "MSISDN : 21891111", "end", "0" };
//                        break;
//                    //
//                    //
//                    case "m0221":
//                        ChosenResult=  new[] { "ID : 98798798", "end", "0" };
//                        break;
//                    //
//                    case "m0222":
//                        ChosenResult=  new[] { "Email : Akrem@Almadar.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0223":
//                        ChosenResult=  new[] { "MSISDN : 21891222", "end", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    case "m0311":
//                        ChosenResult=  new[] { "ID : 9598779", "end", "0" };
//                        break;
//                    //
//                    case "m0312":
//                        ChosenResult=  new[] { "Email : Seraj@LTT.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0313":
//                        ChosenResult=  new[] { "MSISDN : 218951111111", "end", "0" };
//                        break;
//                    //
//                    //
//                    case "m0321":
//                        ChosenResult=  new[] { "ID : 3453454098", "end", "0" };
//                        break;
//                    //
//                    case "m0322":
//                        ChosenResult=  new[] { "Email : Moad@LTT.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0323":
//                        ChosenResult=  new[] { "MSISDN : 21895432242", "end", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    case "m041":

//                    ChosenResult = new[] { "node end", "end", "0" };

//                    break;

//                    //
//                    //
//                    case "m0411":
//                        ChosenResult=  new[] { "ID : 9869697", "end", "0" };
//                        break;
//                    //
//                    case "m0412":
//                        ChosenResult=  new[] { "Email : Hatem Shaaban@LPTIC.ly", "end", "0" };
//                        break;
//                    //
//                    case "m0413":
//                        ChosenResult=  new[] { "MSISDN : 218925863786", "end", "0" };
//                        break;
//                    //
//                    //
//                    //
//                    default:
//                        //Missing mandatory parameter
//                        ChosenResult=  new[] { "Your option not exist", "end", "4001" };
//                        break;
                
//            }
//            //});
//            return  ChosenResult;
//        }
//    }
//}

