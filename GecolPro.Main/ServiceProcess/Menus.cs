using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using GecolPro.Main.Models;

namespace GecolPro.Main.ServiceProcess
{
    public class Menus
    {
        private static MsgContent msgContent = new MsgContent();

        public static async Task<MsgContent> CheckAsync(List<string> Arg, string Lang)
        {
            string ussdCont;

            string messageCont;

            switch (Lang)
            {
                case "En":
                    msgContent.UssdCont = string.Format("The Meter: {0}\nCharged Amount: {1} LYD\n the Token : {2}\nSupportKey : {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    msgContent.MessageCont = string.Format("The Meter: {0}, Charged by  {1} LYD \n the Token :  {2} \n tKey :  {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    break;

                default:

                    ussdCont = string.Format("رقم العداد {0}\nالقيمة المشحونة {1}  دينار\nكرت {2}\n رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]); ;
                           
                    messageCont = string.Format("تم شحن العداد {0} بي {1} دينار \n  كرت الشحن  {2} \n   رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    break;
            }
            return (msgContent);
        }

        public static async Task<MsgContent> MeterNotExist(string Arg ,string Lang)
        {
            string ussdCont;

            //string messageCont = null;

            switch (Lang)
            {
                case "En":
                    msgContent.UssdCont = string.Format("The Meter: {0} Not right or not exist", Arg);

                    break;

                default:

                    msgContent.UssdCont = string.Format("رقم العداد {0} غير صحيح او غير موجود.", Arg); ;

                    break;
            }
            //return (ussdCont, messageCont);
            return msgContent;
        }

        public static async Task<MsgContent> UnderMaintenance_Gecol(string FaultCode, string Lang)
        {
            string ussdCont;

            switch (Lang)
            {
                case "En":
                    msgContent.UssdCont = string.Format("The service under maintenance.");

                    break;

                default:

                    msgContent.UssdCont = string.Format("الخدمة تحت الصيانة");

                    break;
            }
            //return (ussdCont, messageCont);
            return msgContent;
        }

        public static async Task<MsgContent> UnderMaintenance_Billing(string FaultCode, string Lang)
        {


            switch (Lang)
            {
                case "En":

                    switch (FaultCode)
                    {
                        case "ns1:S-WS-00045":
                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

                            break;

                        case "S-ACT-00112":
                            msgContent.UssdCont = string.Format("Not sufficient balance.");

                            break;

                        default:

                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

                            break;
                    }
                    break;
                
                default:
                    switch (FaultCode)
                    {
                        case "ns1:S-WS-00045":
                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");

                            break;

                        case "S-ACT-00112":
                            msgContent.UssdCont = string.Format("رصيدك غير كافي.");

                            break;

                        default:

                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");

                            break;
                    }
                    break;
            }

            return msgContent;
        }
    }
}

