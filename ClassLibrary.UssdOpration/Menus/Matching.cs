using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassLibrary.Models.ModelOfUssd;

namespace ClassLibrary.UssdOpration.Menus
{        
public class Matching
{

        public string MatchingDB(string mSISDN, string nID)
        {
            Subscriber subscriber = new Subscriber()
            {
                MSISDN = mSISDN,
                NID = nID
            };

            if (subscriber.MSISDN == "218947776156" && subscriber.NID=="119941234")
            {
                return "Success";
            }
            if (subscriber.MSISDN == "218928525002" && subscriber.NID == "119945678")
            {
                return "Success";
            }
            if (subscriber.MSISDN == "218947777138" && subscriber.NID == "119845678")
            {
                return "Success";
            }
            if (subscriber.MSISDN == "218925555808" && subscriber.NID == "119841234")
            {
                return "Success";
            }

            if (subscriber.MSISDN == "218923184817" && subscriber.NID == "119951478")
            {
                return "Success";
            }

            return "NotRight";
        }
   


    }
}