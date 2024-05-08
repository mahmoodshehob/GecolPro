using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.Models;

namespace ClassLibrary.UssdOpration.Menus
{
    public class MenusAr
    {
        public object[] Check(string CallingNumber, string ussdAction)
        {

            switch (ussdAction)
            {
                case ("m0"):
                    return new[] { "USSD *707# Using Soap\n1.Libyana\n2.Almadar\n3.LTT\n4.LPTIC\n", "request", "0" };
                //
                //
                //
                case "m01":
                    return new[] { "Libyana Team\n\n1.Sami Elazabi\n2.Mohamed Alwindy\n3.Basher\n# back\n", "request", "0" };
                //
                //
                //
                case "m02":
                    return new[] { "Almadar Team\n\n1.Omar\n2.Akrem\n# back\n", "request", "0" };
                //
                //
                //
                case "m03":
                    return new[] { "LTT Team\n\n1.Seraj\n2.Moad\n# back\n", "request", "0" };
                //
                //
                //
                case "m04":
                    return new[] { "LPTIC Team\n\n1.Hatem Shaaban\n# back\n", "request", "0" };
                //
                //
                //
                case "m012":
                case "m013":
                case "m021":
                case "m022":
                case "m031":
                case "m032":
                case "m041":


                    return new[] { "1.ID\n2.Email\n3.MSISDN\n# back\n", "request", "0" };
                //
                case "m011":


                    return new[] { "1.ID\n2.Email\n3.MSISDN\n4.Hlink\n# back\n", "request", "0" };
                //
                //
                //
                case "m0111":
                    return new[] { "ID : 123132", "end", "0" };
                //
                case "m0112":
                    return new[] { "S.Alazabi@libyana@Ly", "end", "0" };
                //
                case "m0113":
                    return new[] { "MSISDN : 218947777128", "end", "0" };
                //
                case "m0114":
                    return new[] { "MSISDN : xxxxxxxxx", "end", "0" };
                   // return new[] { "Check Messages", "end", "0" };
                //
                //
                case "m0121":
                    return new[] { "ID : 1343242", "end", "0" };
                //
                case "m0122":
                    return new[] { "Email : alwindy@libyana.ly", "end", "0" };
                //
                case "m0123":
                    return new[] { "MSISDN : 218947776212", "end", "0" };
                //
                //
                case "m0131":
                    return new[] { "ID : 9879869", "end", "0" };
                //
                case "m0132":
                    return new[] { "Email : basher@libyana.Ly", "end", "0" };
                //
                case "m0133":
                    return new[] { "MSISDN : 218947777164", "end", "0" };
                //
                //
                //
                case "m0211":
                    return new[] { "ID : 987987", "end", "0" };
                //
                case "m0212":
                    return new[] { "Email : Omar@almadar.ly", "end", "0" };
                //
                case "m0213":
                    return new[] { "MSISDN : 21891111", "end", "0" };
                //
                //
                case "m0221":
                    return new[] { "ID : 98798798", "end", "0" };
                //
                case "m0222":
                    return new[] { "Email : Akrem@Almadar.ly", "end", "0" };
                //
                case "m0223":
                    return new[] { "MSISDN : 21891222", "end", "0" };
                //
                //
                //
                case "m0311":
                    return new[] { "ID : 9598779", "end", "0" };
                //
                case "m0312":
                    return new[] { "Email : Seraj@LTT.ly", "end", "0" };
                //
                case "m0313":
                    return new[] { "MSISDN : 218951111111", "end", "0" };
                //
                //
                case "m0321":
                    return new[] { "ID : 3453454098", "end", "0" };
                //
                case "m0322":
                    return new[] { "Email : Moad@LTT.ly", "end", "0" };
                //
                case "m0323":
                    return new[] { "MSISDN : 21895432242", "end", "0" };
                //
                //
                //
                case "m0411":
                    return new[] { "ID : 9869697", "end", "0" };
                //
                case "m0412":
                    return new[] { "Email : Hatem Shaaban@LPTIC.ly", "end", "0" };
                //
                case "m0413":
                    return new[] { "MSISDN : 218925863786", "end", "0" };
                //
                //
                //
                default:
                    //Missing mandatory parameter
                    return new[] { "Your option not exist", "end", "4001" };
            }
        }
    }
}

