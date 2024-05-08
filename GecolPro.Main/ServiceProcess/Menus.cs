using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ClassLibrary.Models.GecolModels;
using GecolPro.Main.Models.Cache;
using static ClassLibrary.Models.UssdModels.MultiResponseUSSD;

namespace GecolPro.Main.ServiceProcess
{
    public class Menus
    {
        public static async Task<(string UssdCont, string MessageCont)> CheckAsync(List<string> Arg, string Lang)
        {
            string ussdCont;

            string messageCont;

            switch (Lang)
            {
                case "En":
                    ussdCont = string.Format("The Meter: {0}\nCharged Amount: {1} LYD\n the Token : {2}\nSupportKey : {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    messageCont = string.Format("The Meter: {0}, Charged by  {1} LYD \n the Token :  {2} \n tKey :  {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    break;

                default:

                    ussdCont = string.Format("رقم العداد {0}\nالقيمة المشحونة {1}  دينار\nكرت {2}\n رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]); ;
                           
                    messageCont = string.Format("تم شحن العداد {0} بي {1} دينار \n  كرت الشحن  {2} \n   رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    break;
            }
            return (ussdCont, messageCont);
        }

        public static async Task<string> MeterNotExist(string Arg ,string Lang)
        {
            string ussdCont;

            //string messageCont = null;

            switch (Lang)
            {
                case "En":
                    ussdCont = string.Format("The Meter: {0} Not right or not exist", Arg);

                    break;

                default:

                    ussdCont = string.Format("رقم العداد {0} غير صحيح او غير موجود.", Arg); ;

                    break;
            }
            //return (ussdCont, messageCont);
            return ussdCont;
        }

        public static async Task<string> UnderMaintenance_Gecol(string Lang)
        {
            string ussdCont;

            switch (Lang)
            {
                case "En":
                    ussdCont = string.Format("The service under maintenance.");

                    break;

                default:

                    ussdCont = string.Format("الخدمة تحت الصيانة");

                    break;
            }
            //return (ussdCont, messageCont);
            return ussdCont;
        }

        public static async Task<string> UnderMaintenance_Billing(string FaultCode, string Lang)
        {
            string ussdCont;

            switch (FaultCode)
            {
                case "ns1:S-WS-00045":

                    switch (Lang)
                    {
                        case "En":
                            ussdCont = string.Format("The Billing system under maintenance.");

                            break;

                        default:

                            ussdCont = string.Format("نظام الفوترة تحت الصيانة");

                            break;
                    }

                    break;

                default:

                    switch (Lang)
                    {
                        case "En":
                            ussdCont = string.Format("The Billing system under maintenance.");

                            break;

                        default:

                            ussdCont = string.Format("نظام الفوترة تحت الصيانة");

                            break;
                    }

                    break;
            }




          
            //return (ussdCont, messageCont);
            return ussdCont;
        }
    }
}

