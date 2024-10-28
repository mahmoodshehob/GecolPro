//using GecolPro.Models.Gecol;
//using GecolPro.Models.Models;
//using GecolPro.WebApi.Interfaces;
//using Newtonsoft.Json;
//using System.Security.Cryptography;

//namespace GecolPro.WebApi.BusinessRules
//{
//    public class Menus : IMenus
//    {

//        public Menus() 
//        { 
        
//        }

//        private  MsgContent msgContent = new MsgContent();

//        public  async Task<MsgContent> SuccessResponseAsync(List<string> Arg, string Lang)
//        {          
//            switch (Lang)
//            {
//                case "En":
                    
//                    msgContent.UssdCont =
//                        $"Invoice: {Arg[0]}\n" +
//                        $"Token: {Arg[1]}\n" +
//                        $"Amount: {Arg[2]} LYD\n\n";

//                    msgContent.MessageCont =
//                                         $"Invoice: {Arg[0]}\n" +
//                        $"Token: {Arg[1]}\n" +
//                        $"Amount: {Arg[2]} LYD\n\n";

//                    break;
                    
//                default:

//                    //msgContent.UssdCont = string.Format("رقم العداد {0}\nالقيمة المشحونة {1}  دينار\nكرت {2}\n رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]); ;

//                    //msgContent.MessageCont = string.Format("تم شحن العداد {0} بي {1} دينار \n  كرت الشحن  {2} \n   رقم عملية الشحن  {3}", Arg[0], Arg[1], Arg[2], Arg[3]);

                    
//                    break;
//            }
//            return (msgContent);
//        }


//        public  async Task<MsgContent> BlockedResponseAsync(string Lang)
//        {

//            switch (Lang)
//            {
//                case "En":
//                    msgContent.UssdCont = string.Format("The Servise not allow for your number.");

//                    break;

//                default:

//                    msgContent.UssdCont = string.Format("الخدمة غير متاحة لرقمك."); ;

//                    break;
//            }
//            return (msgContent);
//        }

//        public  async Task<MsgContent> BlockedResponseAsync(string FaultCode, string Lang)
//        {

//            switch (Lang)
//            {
//                case "En":
//                    msgContent.UssdCont = string.Format("The Servise not allow for your number.");

//                    break;

//                default:

//                    msgContent.UssdCont = string.Format("الخدمة غير متاحة لرقمك."); ;

//                    break;
//            }
//            return (msgContent);
//        }

//        //public static async Task<MsgContent> MeterNotExist(string Arg ,string Lang)
//        //{
//        //    string ussdCont;

//        //    //string messageCont = null;

//        //    switch (Lang)
//        //    {
//        //        case "En":
//        //            msgContent.UssdCont = string.Format("The Meter: {0} Not right or not exist", Arg);

//        //            break;

//        //        default:

//        //            msgContent.UssdCont = string.Format("رقم العداد {0} غير صحيح او غير موجود.", Arg); ;

//        //            break;
//        //    }
//        //    //return (ussdCont, messageCont);
//        //    return msgContent;
//        //}

//        public  async Task<MsgContent> UnderMaintenance_Gecol(string FaultCode, string Lang)
//        {
//            switch (Lang)
//            {
//                case "En":

//                    switch (FaultCode)
//                    {
 

//                        case "caseFree1":
//                            msgContent.UssdCont = string.Format("The service under maintenance.");

//                            break;

//                        case "caseFree2":
//                            msgContent.UssdCont = string.Format("The service under maintenance.");

//                            break;

//                        case "caseFree3":
//                            msgContent.UssdCont = string.Format("The service under maintenance.");

//                            break;

//                        case "VD.13020116":
//                            msgContent.UssdCont = string.Format("The service under maintenance,Issue with Amount Value");

//                            break;

//                        case "timeout":
//                            msgContent.UssdCont = string.Format("The service timeouted.");

//                            break;

//                        case "VD.01010018":
//                            msgContent.UssdCont = string.Format("Customer does not exist or unbound meter");

//                            break;

//                        default:

//                            msgContent.UssdCont = string.Format("The service under maintenance.");

//                            break;
//                    }
//                    break;

//                default:
//                    switch (FaultCode)
//                    {

//                        case "caseFree1":
//                            msgContent.UssdCont = string.Format("الخدمة تحت الصيانة");

//                            break;

//                        case "caseFree2":
//                            msgContent.UssdCont = string.Format("الخدمة تحت الصيانة");

//                            break;
//                        case "caseFree3":
//                            msgContent.UssdCont = string.Format("الخدمة تحت الصيانة");

//                            break;

//                        case "VD.13020116":
//                            msgContent.UssdCont = string.Format("الخدمة تحت الصيانة,مشكلة في قيمة الرصيد.");

//                            break;

//                        case "timeout":
//                            msgContent.UssdCont = string.Format("مهلة الاتصال انتهت");

//                            break;

//                        case "VD.01010018":
//                            msgContent.UssdCont = string.Format("رقم العداد غير صحيح او غير موجود.");

//                            break;

//                        default:
//                            msgContent.UssdCont = string.Format("الخدمة تحت الصيانة");

//                            break;
//                    }
//                    break;

//            }

//            return msgContent;
//        }

//        public  async Task<MsgContent> UnderMaintenance_Billing(string FaultCode, string Lang)
//        {


//            switch (Lang)
//            {
//                case "En":
//                    switch (FaultCode)
//                    {

//                        case "ns1:S-WS-00045":
//                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

//                            break;

//                        case "S-ACT-00112":
//                            msgContent.UssdCont = string.Format("Not sufficient balance.");

//                            break;

//                        case "caseFree1":
//                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

//                            break;

//                        case "caseFree2":
//                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

//                            break;

//                        default:

//                            msgContent.UssdCont = string.Format("The Billing system under maintenance.");

//                            break;
//                    }
//                    break;
                
//                default:
//                    switch (FaultCode)
//                    {
//                        case "ns1:S-WS-00045":
//                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");

//                            break;

//                        case "S-ACT-00112":
//                            msgContent.UssdCont = string.Format("رصيدك غير كافي.");

//                            break;

//                        case "caseFree1":
//                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");

//                            break;

//                        case "caseFree2":
//                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");



//                            break;

//                        default:

//                            msgContent.UssdCont = string.Format("نظام الفوترة تحت الصيانة");

//                            break;
//                    }
//                    break;
//            }

//            return msgContent;
//        }
//    }
//}

